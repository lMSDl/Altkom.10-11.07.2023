```
dotnet tool install -g dotnet-reportgenerator-globaltool

dotnet test --collect:"XPLAT Code Coverage"

reportgenerator -reports:"C:\Path\To\coverage.cobertura.xml" -targetdir:"TestReport"
```
