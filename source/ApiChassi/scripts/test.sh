dotnet test --collect:"XPlat Code Coverage" ApiChassi.sln
dotnet reportgenerator "-reports:ApiChassi.Test.*/TestResults/*/coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
open coveragereport/index.html