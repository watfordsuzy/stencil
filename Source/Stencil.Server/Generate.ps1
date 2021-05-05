[cmdletbinding()]
Param(
    [string]$Configuration = "$($PSScriptRoot)\..\..\Generation\Tools\CodeGeneratorPrefs.xml",
    [string]$CodeGenerationToolPath = "$($PSScriptRoot)\..\..\Generation\Tools\CodeGenerator.exe"
)

If (-not (Test-Path -PathType Leaf $Configuration)) {
    Write-Error "Could not find configuration file: $($Configuration)"
}
ElseIf (-not (Test-Path -PathType Leaf $CodeGenerationToolPath)) {
    Write-Error "Could not find CodeGenerator: $($CodeGenerationToolPath)"
}
Else {
    Push-Location (Split-Path (Resolve-Path $Configuration))
    Try {
        Write-Output "Running: & $(Resolve-Path $CodeGenerationToolPath) $(Resolve-Path $Configuration)"
        & "$(Resolve-Path $CodeGenerationToolPath)" "$(Resolve-Path $Configuration)"
    }
    Finally {
        Pop-Location
    }
}
