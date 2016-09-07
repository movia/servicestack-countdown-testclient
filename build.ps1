Param(
  [string]$configuration = "Production"
)

# Add MSBuild to path:
($env:PATH) += ";C:\Program Files (x86)\MSBuild\14.0\Bin"

$solutionName = "ServiceStack.Countdown.TestClient"
$srcPath = "src"
$distPath = "dist"

# Build
msbuild "$srcPath/$solutionName.sln" /p:Configuration=$configuration /p:Platform="Any CPU"

if (-not $?) { exit(-1) }

# Ensure distPath
If (-not (Test-Path $distPath)) { mkdir $distPath }

# Copy 
cp "$srcPath/$solutionName/bin/$configuration/*.dll" $distPath
cp "$srcPath/$solutionName/bin/$configuration/*.exe" $distPath
If (-not (Test-Path "$distPath/$solutionName.exe.config")) {
    cp "$srcPath/$solutionName/bin/$configuration/$solutionName.exe.config" $distPath
}