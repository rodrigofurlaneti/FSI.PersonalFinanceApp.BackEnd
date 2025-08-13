param(
  [string] $SolutionDir = "C:\Users\AMD\Source\Repos\FSI.PersonalFinanceApp.BackEnd",   # pasta raiz do seu .sln
  [string] $ApiProj     = "src\FSI.PersonalFinanceApp.Api\FSI.PersonalFinanceApp.Api.csproj",
  [string] $WorkerProj  = "src\FSI.PersonalFinanceApp.Worker\FSI.PersonalFinanceApp.Worker.csproj",
  [string] $Configuration = "Release",

  # Versão única para toda a release (opcional)
  [string] $Version = "1.0.0",

  # Worker: FDD por padrão. Se precisar EXE sem runtime no servidor, use -SelfContainedWorker $true
  [bool]   $SelfContainedWorker = $false,
  [string] $WorkerRid = "win-x64",        # RID para self-contained
  [bool]   $PublishSingleFileWorker = $false,  # opcional: empacotar em um único arquivo

  # Saída esperada pelo deploy.ps1
  [string] $OutRoot = "C:\_artefatos"
)

$ErrorActionPreference = "Stop"
$ApiOut    = Join-Path $OutRoot "api"
$WorkerOut = Join-Path $OutRoot "worker"

Write-Host "== Limpeza de artefatos ==" -ForegroundColor Cyan
if (Test-Path $ApiOut)    { Remove-Item $ApiOut -Recurse -Force }
if (Test-Path $WorkerOut) { Remove-Item $WorkerOut -Recurse -Force }
New-Item -ItemType Directory -Force -Path $ApiOut,$WorkerOut | Out-Null

# Normaliza versão para AssemblyVersion (quatro partes)
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

  Write-Host "== Testes (opcional) =="
  # Se não tiver projetos de teste, comente a linha abaixo
  # dotnet test --configuration $Configuration --no-build

  Write-Host "== Publish API (FDD/IIS) ==" -ForegroundColor Cyan
  dotnet publish $ApiProj `
    -c $Configuration `
    -o $ApiOut `
    /p:Version=$Version `
    /p:FileVersion=$Version `
    /p:AssemblyVersion=$AssemblyVersion

  Write-Host "== Publish Worker ==" -ForegroundColor Cyan

  $workerArgs = @(
    $WorkerProj,
    "-c", $Configuration,
    "-o", $WorkerOut,
    "/p:Version=$Version",
    "/p:FileVersion=$Version",
    "/p:AssemblyVersion=$AssemblyVersion"
  )

  if ($SelfContainedWorker) {
    $workerArgs += @("-r", $WorkerRid, "--self-contained", "true")
    if ($PublishSingleFileWorker) {
      $workerArgs += @("/p:PublishSingleFile=true", "/p:IncludeNativeLibrariesForSelfExtract=true")
    }
  }

  dotnet publish @workerArgs

  # Metadados úteis
  $commit = ""
  try { $commit = (git rev-parse --short HEAD) 2>$null } catch {}
  $stamp = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
  @"
Build Info
----------
Solution : $SolutionDir
Version  : $Version
Assembly : $AssemblyVersion
Config   : $Configuration
Commit   : $commit
Date     : $stamp
"@ | Out-File (Join-Path $OutRoot "BUILDINFO.txt") -Encoding utf8

  Write-Host "`nBuild finalizado. Artefatos em:`n - $ApiOut`n - $WorkerOut" -ForegroundColor Green
}
finally {
  Pop-Location
}
