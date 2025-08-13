<# ============================
  CONFIG — ajuste aqui
============================ #>

# Caminhos dos artefatos já publicados (dotnet publish) — locais/temporários
$ApiSource    = "C:\_artefatos\api"           # ex: dotnet publish ... -o C:\_artefatos\api
$WorkerSource = "C:\_artefatos\worker"        # ex: dotnet publish ... -o C:\_artefatos\worker

# Destinos em produção (conforme você informou)
$ApiTarget    = "C:\sites\PersonalFinanceApp"
$WorkerTarget = "C:\service\PersonalFinanceApp"

# Nomes de infraestrutura
$WorkerServiceName = "PersonalFinanceApp.Worker"  # nome do serviço Windows
$IisAppPool        = "PersonalFinanceApp"         # nome do AppPool
$ApiWarmupUrl      = "http://localhost/health"    # ajuste para sua rota de health

# Itens que NÃO devem ser apagados sobrescritos (logs, configs sensíveis, etc.)
$ExcludeDirs = @("logs")
$ExcludeFiles = @("appsettings.Production.json")  # ajuste se guardar secretos no servidor

# Backups
$BackupRoot = "C:\_deploy_backups\PersonalFinanceApp"
$stamp = (Get-Date).ToString("yyyyMMdd-HHmmss")

# ============ FIM CONFIG ============ #

$ErrorActionPreference = "Stop"
New-Item -ItemType Directory -Force -Path $BackupRoot | Out-Null

function Backup-Folder($src, $name) {
    if (Test-Path $src) {
        $zip = Join-Path $BackupRoot "$($name)-$stamp.zip"
        Write-Host "Backup: $src -> $zip"
        Compress-Archive -Path (Join-Path $src "*") -DestinationPath $zip -Force
    }
}

function Copy-Content($from, $to) {
    if (!(Test-Path $to)) { New-Item -ItemType Directory -Force -Path $to | Out-Null }

    # Monta parâmetros do Robocopy
    $xd = @(); foreach($d in $ExcludeDirs){ $xd += @("/XD", (Join-Path $to $d)) }
    $xf = @(); foreach($f in $ExcludeFiles){ $xf += @("/XF", (Join-Path $to $f)) }

    $args = @("$from", "$to", "/MIR", "/R:2", "/W:2") + $xd + $xf
    $rc = (Start-Process -FilePath "robocopy.exe" -ArgumentList $args -PassThru -Wait).ExitCode

    # Códigos 0/1 do Robocopy são sucesso
    if ($rc -gt 1) { throw "Robocopy falhou ($rc) ao copiar $from -> $to" }
}

function Ensure-Service-Runs($name) {
    $svc = Get-Service -Name $name -ErrorAction Stop
    if ($svc.Status -ne 'Running') {
        Start-Service $name
        $svc.WaitForStatus('Running','00:00:25')
    }
}

function Stop-Service-Safe($name) {
    if (Get-Service -Name $name -ErrorAction SilentlyContinue) {
        if ((Get-Service $name).Status -ne 'Stopped') {
            Stop-Service $name -Force
            (Get-Service $name).WaitForStatus('Stopped','00:00:25')
        }
    }
}

#------------------------------
# 1) WORKER PRIMEIRO
#------------------------------
Write-Host "`n=== DEPLOY WORKER ===" -ForegroundColor Cyan
if (!(Test-Path $WorkerSource)) { throw "Pasta de artefatos do Worker não encontrada: $WorkerSource" }

# 1.1 Parar serviço
Write-Host "Parando serviço $WorkerServiceName..."
Stop-Service-Safe $WorkerServiceName

# 1.2 Backup
Backup-Folder $WorkerTarget "worker"

# 1.3 Copiar (preservando logs/configs listados)
Write-Host "Copiando Worker..."
Copy-Content $WorkerSource $WorkerTarget

# 1.4 Subir serviço e validar
Write-Host "Iniciando serviço $WorkerServiceName..."
Ensure-Service-Runs $WorkerServiceName
Write-Host "Worker em execução."

# Validação simples (opcional): aguarda 5s e checa novamente
Start-Sleep -Seconds 5
if ((Get-Service $WorkerServiceName).Status -ne 'Running') {
    throw "Worker não permaneceu em execução após start."
}

#------------------------------
# 2) API DEPOIS DO WORKER
#------------------------------
Write-Host "`n=== DEPLOY API ===" -ForegroundColor Cyan
if (!(Test-Path $ApiSource)) { throw "Pasta de artefatos da API não encontrada: $ApiSource" }

Import-Module WebAdministration

# 2.1 app_offline.htm para descarregar a app com segurança (evita lock de arquivos)
$appOffline = Join-Path $ApiTarget "app_offline.htm"
"Atualizando... $stamp" | Out-File $appOffline -Encoding utf8

# 2.2 Parar App Pool
if (Test-Path IIS:\AppPools\$IisAppPool) {
    Write-Host "Parando AppPool $IisAppPool..."
    Stop-WebAppPool -Name $IisAppPool
}

# 2.3 Backup
Backup-Folder $ApiTarget "api"

# 2.4 Copiar (preservando logs/configs listados)
Write-Host "Copiando API..."
Copy-Content $ApiSource $ApiTarget

# 2.5 Remover app_offline, subir AppPool e aquecer endpoint
Remove-Item $appOffline -ErrorAction SilentlyContinue

if (Test-Path IIS:\AppPools\$IisAppPool) {
    Write-Host "Iniciando AppPool $IisAppPool..."
    Start-WebAppPool -Name $IisAppPool
}

# 2.6 Warm-up/Health
if ($ApiWarmupUrl) {
    Write-Host "Aquecendo API em $ApiWarmupUrl ..."
    try {
        $resp = Invoke-WebRequest -Uri $ApiWarmupUrl -UseBasicParsing -TimeoutSec 20
        if ($resp.StatusCode -ge 400) { throw "Health retornou $($resp.StatusCode)" }
        Write-Host "API saudável."
    } catch {
        throw "Falha no health-check da API: $($_.Exception.Message)"
    }
}

Write-Host "`nDeploy concluído com sucesso (Worker -> API) em $stamp." -ForegroundColor Green
