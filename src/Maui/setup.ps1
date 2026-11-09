# Подготовка решения «Конь на доске».
# Создаёт временный проект из шаблона maui и переносит из него платформенные файлы
# (Platforms, Properties, иконку, заставку, шрифты), которых ещё нет в Calabonga.HorseOnTheGrid.Mobile.
# Свои файлы проекта скрипт не перезаписывает.

$ErrorActionPreference = 'Stop'

$root = $PSScriptRoot
$project = Join-Path $root 'src\Calabonga.HorseOnTheGrid.Mobile'
$temp = Join-Path ([System.IO.Path]::GetTempPath()) ('HorseOnTheGrid_' + [guid]::NewGuid().ToString('N'))

Write-Host 'Создаю проект из шаблона maui во временной папке...'
Write-Host '(ошибка NETSDK1147 при восстановлении временного проекта ожидаема и не мешает: нужны только его файлы)'
dotnet new maui --name Calabonga.HorseOnTheGrid.Mobile --output $temp
if ($LASTEXITCODE -ne 0) {
    throw 'Не удалось создать проект из шаблона maui. Проверьте, что установлена рабочая нагрузка .NET MAUI (dotnet workload list).'
}

try {
    $folders = @('Platforms', 'Properties', 'Resources\AppIcon', 'Resources\Splash', 'Resources\Fonts')
    foreach ($folder in $folders) {
        $source = Join-Path $temp $folder
        if (-not (Test-Path $source)) { continue }

        Get-ChildItem -Path $source -Recurse -File | ForEach-Object {
            $relative = $_.FullName.Substring($temp.Length).TrimStart('\', '/')
            $target = Join-Path $project $relative
            if (-not (Test-Path $target)) {
                New-Item -ItemType Directory -Force -Path (Split-Path $target -Parent) | Out-Null
                Copy-Item -Path $_.FullName -Destination $target
                Write-Host "  добавлен $relative"
            }
        }
    }
}
finally {
    Remove-Item -Path $temp -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host 'Восстанавливаю пакеты NuGet...'
dotnet restore (Join-Path $root 'Calabonga.HorseOnTheGrid.slnx')
if ($LASTEXITCODE -ne 0) { throw 'Ошибка восстановления пакетов.' }

Write-Host 'Готово. Дальше: dotnet test и dotnet build (см. README.md).'
