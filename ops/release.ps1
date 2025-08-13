# ============================
# release.ps1  (BUILD + DEPLOY)
# ============================

param(
  # ===== BUILD =====
  [string] $SolutionDir = "C:\Users\AMD\Source\Repos\FSI.PersonalFinanceApp.BackEnd",
  [string] $ApiProj     = "src\FSI.PersonalFinanceApp.Api\FSI.PersonalFinanceApp.Api.csproj",
  [string] $WorkerProj  = "src\FSI.PersonalFinanceApp.Worker\FSI.PersonalFinanceApp.Worker.csproj",
  [string] $Configuration = "Release",
  [string] $Version = "1.0.0",

  # Worker self-contained? (default: FDD)
  [bool]   $SelfContainedWorker = $false,
  [string] $WorkerRid = "win-x64",
  [bool]   $PublishSingleFileWorker = $false,

  # Saída do build
  [string] $OutRoot = "C:\_artefatos",

  # ===== DEPLOY =====
  [string] $ApiTarget    = "C:\sites\PersonalFinanceApp",
  [string] $WorkerTarget = "C:\service\PersonalFinanceApp",
  [string] $IisAppPool   = "PersonalFinanceApp",
  [string] $ApiWarmupUrl = "http://localhost/health",

  # Backups
  [string] $BackupRoot = "C:\_deploy_backups\PersonalFinanceApp"
)

$ErrorActionPreference = "Stop"

# ======== BUILD ========
$ApiOut    = Join-Path $OutRoot "api"
$WorkerOut = Join-Path $OutRoot "worker"

Write-Host "== Limpeza de artefatos ==" -ForegroundColor Cyan
if (Test-Path $ApiOut)    { Remove-Item $ApiOut -Recurse -Force }
if (Test-Path $WorkerOut) { Remove-Item $WorkerOut -Recurse -Force }
New-Item -ItemType Directory -Force -Path $ApiOut,$WorkerOut | Out-Null

function Normalize-AssemblyVersion([string]$v) {
  if ($v -match '^\d+\.\d+\.\d+\.\d+$') { return $v }
  elseif ($v -match '^\d+\.\d+\.\d+$')  { return "$v.0" }
  else { return "1.0.0.0" }
}
$AssemblyVersion = Normalize-AssemblyVersion $Version

Push-Location $SolutionDir
try {
  Write-Host "== Restore ==" -ForegroundColor Cyan
  dotnet restore

  Write-Host "== Publish API ==" -ForegroundColor Cyan
  dotnet publish $ApiProj -c $Configuration -o $ApiOut `
    /p:Version=$Version /p:FileVersion=$Version /p:AssemblyVersion=$AssemblyVersion

  Write-Host "== Publish Worker ==" -ForegroundColor Cyan
  $workerArgs = @(
    $WorkerProj, "-c", $Configuration, "-o", $WorkerOut,
    "/p:Version=$Version", "/p:FileVersion=$Version", "/p:AssemblyVersion=$AssemblyVersion"
  )
  if ($SelfContainedWorker) {
    $workerArgs += @("-r", $WorkerRid, "--self-contained", "true")
    if ($PublishSingleFileWorker) {
      $workerArgs += @("/p:PublishSingleFile=true", "/p:IncludeNativeLibrariesForSelfExtract=true")
    }
  }
  dotnet publish @workerArgs

  $commit = ""; try { $commit = (git rev-parse --short HEAD) 2>$null } catch {}
  $stampBuild = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
  @"
Build Info
----------
Solution : $SolutionDir
Version  : $Version
Assembly : $AssemblyVersion
Config   : $Configuration
Commit   : $commit
Date     : $stampBuild
"@ | Out-File (Join-Path $OutRoot "BUILDINFO.txt") -Encoding utf8

  Write-Host ("`nBuild finalizado. Artefatos em:`n - {0}`n - {1}" -f $ApiOut, $WorkerOut) -ForegroundColor Green
}
finally { Pop-Location }

# ======== DEPLOY ========
$ApiSource    = $ApiOut
$WorkerSource = $WorkerOut

$WorkerExeName = "FSI.PersonalFinanceApp.Worker.exe"
$WorkerDllName = "FSI.PersonalFinanceApp.Worker.dll"

New-Item -ItemType Directory -Force -Path $BackupRoot,$ApiTarget,$WorkerTarget | Out-Null
$stamp = Get-Date -Format "yyyyMMdd-HHmmss"

function Backup-Folder($src,$name){
  if(Test-Path $src){
    $zip = Join-Path $BackupRoot ("{0}-{1}.zip" -f $name,$stamp)
    Write-Host ("Backup: {0} -> {1}" -f $src,$zip)
    Compress-Archive -Path (Join-Path $src "*") -DestinationPath $zip -Force
  }
}

# Robocopy com stash/restauração de arquivos do DESTINO e exit codes corretos (compatível PS 5.1)
$ExcludeDirs  = @("logs")
$ExcludeFiles = @("appsettings.Production.json")

function Copy-Content($from,$to){
  if(!(Test-Path $from)){ throw ("Origem não existe: {0}" -f $from) }
  if(!(Test-Path $to)){ New-Item -ItemType Directory -Force -Path $to | Out-Null }

  # 1) stash: salva arquivos a preservar (no destino)
  $stash = Join-Path $env:TEMP ("deploy-preserve-{0}" -f ([guid]::NewGuid()))
  New-Item -ItemType Directory -Force -Path $stash | Out-Null
  foreach($f in $ExcludeFiles){
    $destFile = Join-Path $to $f
    if(Test-Path $destFile){
      $destDir = Split-Path -Parent (Join-Path $stash $f)
      if(!(Test-Path $destDir)){ New-Item -ItemType Directory -Force -Path $destDir | Out-Null }
      Copy-Item $destFile -Destination (Join-Path $stash $f) -Force
    }
  }

  # 2) excluir diretórios (tanto no src quanto no dest)
  $xd=@()
  foreach($d in $ExcludeDirs){
    $xd += @("/XD",(Join-Path $from $d),(Join-Path $to $d))
  }

  # 3) espelhar
  $args = @("$from","$to","/MIR","/R:2","/W:2") + $xd
  $rc = (Start-Process -FilePath "robocopy.exe" -ArgumentList $args -PassThru -Wait).ExitCode

  # Robocopy: 0..7 = sucesso/atenção; 8+ = erro
  if ($rc -ge 8) {
    throw ("Robocopy falhou ({0}) ao copiar {1} -> {2}" -f $rc,$from,$to)
  } else {
    $map = @{
      0="Sem alterações"; 1="Arquivos copiados"; 2="Extras/deletados"; 3="Copiados+extras";
      4="Falhas recuperadas"; 5="Copiados+recuperadas"; 6="Extras+recuperadas"; 7="Tudo+recuperadas"
    }
    $desc = if ($map.ContainsKey($rc)) { $map[$rc] } else { "OK" }
    Write-Host ("Robocopy concluído (código {0}: {1})" -f $rc, $desc)
  }

  # 4) restaurar arquivos preservados
  foreach($f in $ExcludeFiles){
    $stashFile = Join-Path $stash $f
    if(Test-Path $stashFile){
      $destDir = Split-Path -Parent (Join-Path $to $f)
      if(!(Test-Path $destDir)){ New-Item -ItemType Directory -Force -Path $destDir | Out-Null }
      Copy-Item $stashFile -Destination (Join-Path $to $f) -Force
    }
  }
  Remove-Item $stash -Recurse -Force -ErrorAction SilentlyContinue
}

function Get-WorkerPaths {
  $exe = Join-Path $WorkerTarget $WorkerExeName
  $dll = Join-Path $WorkerTarget $WorkerDllName
  return [pscustomobject]@{ Exe=$exe; Dll=$dll }
}

function Stop-Worker-If-Running {
  # tenta matar pelo PID salvo (preciso e evita matar outro dotnet)
  $pidFile = Join-Path $WorkerTarget "worker.pid"
  if (Test-Path $pidFile) {
    $savedPid = (Get-Content $pidFile -ErrorAction SilentlyContinue | Select-Object -First 1).Trim()
    if ($savedPid -and $savedPid -match '^\d+$') {
      $proc = Get-Process -Id ([int]$savedPid) -ErrorAction SilentlyContinue
      if ($proc) {
        Write-Host ("Matando PID {0} (via worker.pid)" -f $proc.Id)
        Stop-Process -Id $proc.Id -Force -ErrorAction SilentlyContinue
      }
    }
    Remove-Item $pidFile -Force -ErrorAction SilentlyContinue
  }

  # fallback: por caminho do exe, se existir
  $paths = Get-WorkerPaths
  try {
    $procs = Get-CimInstance Win32_Process | Where-Object { $_.ExecutablePath -ieq $paths.Exe }
    foreach($p in $procs){
      Write-Host ("Matando PID {0}" -f $p.ProcessId)
      Stop-Process -Id $p.ProcessId -Force -ErrorAction SilentlyContinue
    }
  } catch { }
}

function Start-Worker {
  $paths = Get-WorkerPaths
  $dotnet = "C:\Program Files\dotnet\dotnet.exe"
  $p = $null

  if (Test-Path $paths.Exe) {
    Write-Host ("Iniciando {0} ..." -f $paths.Exe)
    $p = Start-Process -FilePath $paths.Exe -WorkingDirectory $WorkerTarget -WindowStyle Hidden -PassThru
  }
  elseif (Test-Path $paths.Dll -and (Test-Path $dotnet)) {
    Write-Host ("Iniciando dotnet {0} ..." -f $paths.Dll)
    $p = Start-Process -FilePath $dotnet -ArgumentList ('"{0}"' -f $paths.Dll) -WorkingDirectory $WorkerTarget -WindowStyle Hidden -PassThru
  }
  else {
    throw ("Worker não encontrado (.exe/.dll) em {0}" -f $WorkerTarget)
  }

  Start-Sleep -Seconds 2
  if(!(Get-Process -Id $p.Id -ErrorAction SilentlyContinue)){
    throw "Worker saiu logo após iniciar. Verifique logs/conexões (RabbitMQ, config, etc.)."
  }
  Set-Content -Path (Join-Path $WorkerTarget "worker.pid") -Value $p.Id -Encoding ascii
  Write-Host ("Worker rodando. PID: {0}" -f $p.Id)
}

# ====== DEPLOY WORKER ======
Write-Host "`n=== DEPLOY WORKER ===" -ForegroundColor Cyan
Stop-Worker-If-Running
Backup-Folder $WorkerTarget "worker"
Copy-Content  $WorkerSource $WorkerTarget
Start-Worker

# ====== DEPLOY API (IIS) ======
Write-Host "`n=== DEPLOY API (IIS) ===" -ForegroundColor Cyan
Import-Module WebAdministration -ErrorAction Stop

$appOffline = Join-Path $ApiTarget "app_offline.htm"
"Atualizando... $stamp" | Out-File $appOffline -Encoding utf8

$appPoolPath = Join-Path "IIS:\AppPools" $IisAppPool
if (Test-Path $appPoolPath) {
  Write-Host ("Parando AppPool {0}..." -f $IisAppPool)
  Stop-WebAppPool -Name $IisAppPool
}

Backup-Folder $ApiTarget "api"
Copy-Content  $ApiSource $ApiTarget

Remove-Item $appOffline -ErrorAction SilentlyContinue

if (Test-Path $appPoolPath) {
  Write-Host ("Iniciando AppPool {0}..." -f $IisAppPool)
  Start-WebAppPool -Name $IisAppPool
}

if ($ApiWarmupUrl) {
  Write-Host ("Aquecendo API em {0}..." -f $ApiWarmupUrl)
  $resp = Invoke-WebRequest -Uri $ApiWarmupUrl -UseBasicParsing -TimeoutSec 20
  if ($resp.StatusCode -ge 400) { throw ("Health da API falhou: {0}" -f $resp.StatusCode) }
  Write-Host "API saudável."
}

Write-Host "`nDeploy finalizado com sucesso (Worker -> API/IIS)." -ForegroundColor Green
