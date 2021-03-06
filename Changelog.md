# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- Supported processing of `.csproj` files without Visual Studio Tools for Unity (i.e. when using VS Code or Rider).

### Changed
- Settings are now stored in json format in project settings.
- StyleCop.Analyzers package version updated to 1.1.118.

## [0.1.1] - 2019-01-09
### Fixed
- Fixed compilation error when Visual Studio Tools for Unity are disabled.

## [0.1.0] - 2018-12-31
### Added
- Added support to add StyleCop.Analyzers package to project when assembly contains `stylecop.json` file in root directory.
- Added support to cleanup `.csproj` files.

[Unreleased]: https://github.com/Si1ver/C-Sharp-Project-Tools-for-Unity/compare/v0.1.1...HEAD
[0.1.1]: https://github.com/Si1ver/C-Sharp-Project-Tools-for-Unity/releases/tag/v0.1.1
[0.1.0]: https://github.com/Si1ver/C-Sharp-Project-Tools-for-Unity/releases/tag/v0.1.0
