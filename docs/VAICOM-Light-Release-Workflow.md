# VAICOM Full + Light Release Workflow

## Repository Layout
- `VAICOM/` - Full plugin project.
- `VAICOM.Light/` - Standalone Light Windows app.
- `docs/VAICOM-Light-Feature-Matrix.md` - Light scope and tab matrix.

## Versioning
- Use one shared semantic version tag for both deliverables.
- Example: `v3.2.0` produces Full and Light artifacts.

## Build/Artifacts
- Workflow: `.github/workflows/build-full-light.yml`
- Artifacts:
  - `VAICOM-Full`
  - `VAICOM-Light`

## Packaging Rules
- Do not bundle Light into Full installer/zip flow.
- Ship Light as a separate downloadable package.

## Update Channels
- Full app/plugin uses Full update metadata.
- Light app uses Light-only feed endpoint (`LightUpdateChannel`).
- Keep feed manifests independent while sharing version numbers.

## Maintenance Rules
- Keep bridge/runtime abstractions provider-driven so Full and Light can host the same core code.
- Add new shared behavior in reusable components first, then wire host-specific startup/lifecycle.
- Validate both projects build successfully before release.
