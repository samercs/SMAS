
$rootDirectory = Resolve-Path .\
$solutionName = (Get-Item $rootDirectory).Name


$solutionFile = Get-ChildItem -Path $rootDirectory | where {$_.extension -eq ".sln"} | Select-Object -First 1
$sourceFiles = Get-ChildItem -Path $rootDirectory -Include @("*.sln", "*.suo", "*.xproj", "*.cproj", "*.csproj", "*.nuspec", "*.cs", "*.config", "*.asax", "*.cshtml","*.json") -Recurse -Force
$projectFiles = Get-ChildItem -Path $rootDirectory -Include @("*.csproj") -Recurse -Force
$originalNamespace = $solutionFile.BaseName

# Replace Root Namespace in Files

Write-Host "Root Directory: $rootDirectory"
Write-Host "Solution Name: $solutionName"
Write-Host "Original Namespace: $originalNamespace"

Write-Host "Updating namespaces..."

foreach ($sourceFile in $sourceFiles)
{
   Write-Host "Updating $sourceFile..."
   (Get-Content $sourceFile) | Foreach-Object { $_ -Replace $originalNamespace, $solutionName } | Set-Content $sourceFile
}

# Rename Project Files

Write-Host "Renaming project files..."

foreach ($projectFile in $projectFiles)
{
    $projectFileName = $projectFile.Name
    $newProjectFileName = $projectFileName.Replace($originalNamespace, $solutionName)

    Write-Host "Renaming file $projectFileName to $newProjectFileName..."

    if ($projectFileName -ne $newProjectFileName) 
    {
        Rename-Item $projectFile $newProjectFileName
        Write-Host $projectFile
    }
}

# Rename Project Directories

Write-Host "Renaming project directories..."

foreach ($projectFile in $projectFiles)
{
    $projectDirectory = $projectFile.Directory
    $projectDirectoryName = $projectFile.Directory.Name
    $newProjectDirectoryName = $projectDirectoryName.Replace($originalNamespace, $solutionName)

    if ($projectDirectoryName -ne $newProjectDirectoryName) 
    {
        Write-Host "Renaming directory $projectDirectoryName to $newProjectDirectoryName..."

        Rename-Item $projectDirectory $newProjectDirectoryName
    }    
}

# Rename Solution File

Write-Host "Renaming solution file..."

$solutionFile | Rename-Item -NewName { $_.Name -replace $originalNamespace, $solutionName }
