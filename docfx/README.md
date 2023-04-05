# Building documentation
Using [docfx](https://dotnet.github.io/docfx/) with [.NET API Docs](https://dotnet.github.io/docfx/docs/dotnet-api-docs.html):

1. `dotnet tool update -g docfx`
2. In solution root: `docfx docfx/docfx.json --serve`
3. Go to http://localhost:8080/api/ to see the generated docs.
4. In solution root: `docfx docfx/docfx.json` to regenerate while serving, or Ctrl+C step 2 and re-run it.
