# Building documentation
Using [docfx](https://dotnet.github.io/docfx/) with [.NET API Docs](https://dotnet.github.io/docfx/docs/dotnet-api-docs.html).

Initial setup to install the tools:
```cmd
dotnet tool update -g docfx
dotnet tool update -g DocFxTocGenerator
```

Then in the project directory, after checkout and after adding articles:

1. `DocFxTocGenerator --docfolder articles --sequence --override` to generate `toc.yml`
2. `docfx docfx.json --serve` to host the site.
3. Go to http://localhost:8080/api/ to see the generated docs.
4. `docfx docfx.json` to regenerate while serving, or press Ctrl+C in the terminal from step 2 and re-run it.
