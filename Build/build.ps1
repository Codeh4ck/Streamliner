Set-Location ../
dotnet build ./Streamliner.sln --configuration Release
dotnet pack ./Streamliner.sln --configuration Release

Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');