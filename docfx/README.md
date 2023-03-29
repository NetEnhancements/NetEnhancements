# Building documentation
Using [docfx](https://dotnet.github.io/docfx/) with [.NET API Docs](https://dotnet.github.io/docfx/docs/dotnet-api-docs.html):

1. `dotnet tool update -g docfx`
2. In solution root: `docfx docfx/docfx.json --serve`
3. Go to http://localhost:8080/api/ to see the generated docs.
