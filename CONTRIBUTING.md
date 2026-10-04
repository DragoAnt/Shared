# Contributing

Issues and pull requests are welcome. For anything larger than a small fix, open an issue first so the approach can be agreed before you write the code.

## Build and test

You need the .NET SDK pinned in [global.json](./global.json), plus the .NET 8 and .NET 9 runtimes so the tests run for every target framework. Then run the same steps as CI:

```sh
dotnet restore src/DragoAnt.Shared.slnx
dotnet build src/DragoAnt.Shared.slnx -c Release --no-restore
dotnet test --solution src/DragoAnt.Shared.slnx -c Release --no-build
```

Tests use xUnit v3 on Microsoft.Testing.Platform v2, with FluentAssertions 7.

## Build kit

The shared MSBuild settings come from [DragoAnt.MSBuildKit](https://github.com/DragoAnt/MSBuildKit), committed under `src/.toolkit/`; don't edit it by hand. Move to another kit release with its update script and commit the result:

```sh
sh src/.toolkit/update.sh --root src --version 0.2.0      # or: pwsh src/.toolkit/update.ps1 -Root src -Version 0.2.0
```

Repository settings live in `src/Directory.Build.props` (target frameworks, copyright), `src/Directory.Version.props` (the next release's `VersionPrefix`) and `src/Directory.Packages.props` (package versions the kit does not provide).

## Pull requests

- Branch from `main` and target `main`.
- Add or update tests for every behavior change.
- Keep the build warning-free: warnings are treated as errors.
- The `ci` workflow must pass on the pull request.

Releases are published to nuget.org by the maintainers from a GitHub release.
