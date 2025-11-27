#!/usr/bin/env pwsh
param(
    # npm version bump type: patch | minor | major
    [ValidateSet("patch", "minor", "major", "none")]
    [string]$VersionType = "patch",

    # How Orval should get the OpenAPI spec:
    # Http      -> temporarily change orval.config.js to use HTTP URL
    # Download  -> download swagger.json to openapi.json, keep config unchanged
    [ValidateSet("Http", "Download")]
    [string]$OrvalMode = "Download",

    # whether npm package should be published after build
    [bool]$Publish = $true
)

$ErrorActionPreference = "Stop"

Write-Host "=== Build & Publish events-api-sdk ===" -ForegroundColor Cyan

# Repo root
$RepoRoot = $PSScriptRoot
Set-Location $RepoRoot

$SwaggerUrl = "http://localhost:5121/swagger/v1/swagger.json"
$ConfigPath = "$RepoRoot/orval.config.js"
$LocalSpec = Join-Path $RepoRoot "..\..\..\openapi.json"
$BackupPath = "$ConfigPath.bak"

# Ask for backend ready
Write-Host "Ensure API is running at $SwaggerUrl"
$answer = Read-Host "Is it running? (y/n)"
if ($answer -notin @("y", "Y", "yes", "Yes")) { exit 1 }

# --- ORVAL SECTION ---
switch ($OrvalMode) {

    'Http' {
        Write-Host "Using HTTP URL temporarily in Orval config..."
        $original = Get-Content $ConfigPath -Raw
        Set-Content $BackupPath $original

        $localLine = "      target: '../../../openapi.json',"
        $httpLine = "      target: 'http://localhost:5121/swagger/v1/swagger.json',"

        $updated = $original -replace [Regex]::Escape($localLine), $httpLine
        Set-Content $ConfigPath $updated

        try {
            orval --config $ConfigPath
        }
        finally {
            Set-Content $ConfigPath $original
            Write-Host "Orval config restored."
        }
    }

    'Download' {
        Write-Host "Downloading OpenAPI spec to $LocalSpec..."
        Invoke-WebRequest -Uri $SwaggerUrl -OutFile $LocalSpec

        Write-Host "Running Orval with original config..."
        orval --config $ConfigPath
    }
}

# --- NPM + PUBLISH SECTION ---
if (-not $env:GITHUB_NODE_AUTH_TOKEN) {
    Write-Error "GITHUB_NODE_AUTH_TOKEN not set."
    exit 1
}

# TEMPORARY switch to /src
$SrcDir = Join-Path $RepoRoot "src"

Push-Location $SrcDir
try {
    if ($VersionType -ne "none") {
        npm version $VersionType
    }
    npm run build
    if ($Publish) {
        npm publish
    }
    else {
        Write-Host "Publish skipped (use -Publish to enable)" -ForegroundColor Yellow
    }
}
finally {
    Pop-Location
}

Write-Host "✅ Done!"
