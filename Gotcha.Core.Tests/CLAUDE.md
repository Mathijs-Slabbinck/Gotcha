# Gotcha.Core.Tests Conventions

## Run Tests

```bash
dotnet test Gotcha.Core.Tests
```

## Project Setup

- xUnit test project, references `Gotcha.Core` with `InternalsVisibleTo`
- Test structure mirrors Core: `Services/` and `Entities/` folders

## Test Conventions

- **Naming**: `MethodName_Scenario_ExpectedResult`
- **Pattern**: Comment above each test explains what to check + expected result, body is `throw new NotImplementedException()` (user writes the implementation)
- **Auto-create stubs**: When new entities, services, or methods are added elsewhere that need tests, automatically create/update test files with `throw new NotImplementedException()` stubs and inform the user in the console
