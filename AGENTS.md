# Guidelines for Contributors

This repository contains a .NET backend.
Follow these rules when making changes.

## Testing
- Run `dotnet test` from `Source/Backend` whenever backend code or libraries change.

## Coding Style
- **C#** files use 4 spaces for indentation.
- Write comments and documentation in English.
- Keep lines under 120 characters when practical.
- Use PascalCase for public types and methods.
- Use camelCase for private fields and parameters.
- Use `var` for local variable declarations when the type is obvious.
- Use `nameof` for member names in exceptions and logging.

## Commits and Pull Requests
- Use a short summary (<72 characters) as the first line of commit messages.
- Use the imperative mood in commit messages (e.g., "Fix bug" instead of "Fixed bug").
- Use the body of the commit message to explain the "why" behind the change, if necessary.
- Use English for commit messages.
- Prefix commits with a [semantic type](https://www.conventionalcommits.org/) such as `feat`, `fix`, `docs`, `style`, `refactor`, `test`, or `chore`.
- Include a PR description summarizing the change and the test results.
- Use English when naming branches for pull requests.
