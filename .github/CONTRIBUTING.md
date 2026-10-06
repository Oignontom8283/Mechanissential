
# Contributing to Mechanissential

## Utilities

### Converting an assembly to a reference assembly

To compile the project, we need the game's DLLs, UnityEngine, BepInEx, and any other code used by the project.

These components are required for linking during compilation, as well as for autocompletion and static code analysis in IDEs.

However, for licensing reasons, the Mechanica, UnityEngine, BepInEx, and Harmony DLLs must be converted into reference assemblies so they can be stored in the repository without including code that is not freely redistributable.

Therefore, before storing them in the repository, we must first strip them of their executable logic, retaining only the information required for compilation and project interoperability.

> [!CAUTION]
> Contributions containing proprietary or non-freely redistributable files are strictly prohibited, **including temporarily in Git history**.
> This includes, in particular, the original Mechanica and UnityEngine DLLs,
> as well as images and other files without a freely redistributable license. The **original BepInEx and Harmony DLLs are also prohibited** for consistency.

To do this, we use JetBrains' [Refasmer CLI Tool 2.0.3](https://github.com/JetBrains/RefAsmer).

To facilitate the use of this tool, a PowerShell command is provided below. It:
- Installs RefAsmer in a temporary directory;
- Converts the DLLs into reference assemblies;
- Verifies that the output files were generated and are not empty;
- Deletes the original DLLs if generation succeeds;
- Deletes the temporary directory.

To use it, place the `.dll` file to be added to the project in the `Libraries/` directory, then run the following PowerShell command from the project root.

```powershell
$t="$env:TEMP\refasmer-$([guid]::NewGuid())"; $ok=$false; try { New-Item -ItemType Directory -Path $t -Force -ErrorAction Stop | Out-Null; dotnet tool install --tool-path $t jetbrains.refasmer.clitool --version 2.0.3 --no-cache; if ($LASTEXITCODE -ne 0) { throw "RefAsmer installation failed (code $LASTEXITCODE)" }; $r=Join-Path $t "refasmer.exe"; if (-not (Test-Path -LiteralPath $r -PathType Leaf)) { throw "RefAsmer not found after installation" }; $dlls=@(Get-ChildItem -LiteralPath ".\Libraries" -Filter "*.dll" -File -ErrorAction Stop | Where-Object { $_.Name -notlike "*.ref.dll" }); $outputs=@(foreach ($dll in $dlls) { $out=Join-Path $dll.DirectoryName "$($dll.BaseName).ref.dll"; & $r -r --all -o $out $dll.FullName; $code=$LASTEXITCODE; if ($code -ne 0) { throw "RefAsmer failed for '$($dll.Name)' (code $code)" }; if (-not (Test-Path -LiteralPath $out -PathType Leaf)) { throw "Output file missing for '$($dll.Name)'" }; if ((Get-Item -LiteralPath $out -ErrorAction Stop).Length -eq 0) { throw "Output file is empty for '$($dll.Name)'" }; [PSCustomObject]@{ Source=$dll.FullName; Output=$out } }); foreach ($x in $outputs) { Remove-Item -LiteralPath $x.Source -Force -ErrorAction Stop; Write-Host "✓ $([IO.Path]::GetFileName($x.Source)) -> $([IO.Path]::GetFileName($x.Output))" }; $ok=$true } finally { Remove-Item -LiteralPath $t -Recurse -Force -ErrorAction SilentlyContinue }; if (-not $ok) { throw "Generation failed: the original DLLs have been preserved" }
```

Then add references to the generated DLLs in the `.csproj` project file, following the pattern of the references already present.

> [!WARNING]
> Contributions must not directly add DLLs. If adding a DLL is necessary,
> please request it from a maintainer beforehand so they can add it.
> (Use a local copy of the DLL until it is officially added.)
> A contribution that adds an assembly reference or DLL itself will be rejected until the file or files are removed.
