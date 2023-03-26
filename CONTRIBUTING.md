# Solution Layout
1. **Fundamentals**: low-level stuff
   * **Util**: extension methods, converters.
   * **Shared**: configuration, logging, MVC conventions.
2. **Security**
   * **Identity**: extensions on ASP.NET Core Identity
   * **OAuth**: our own OAuth server (WIP) based on Identity.
3. **Imaging**
   * **Imaging**: reading image info, resizing them, saving to disk.
   * **EntityFramework**: saving image metadata through EF.
4. **Tests**
   * _TODO_

# Code Style
* 0 warnings
* No `this`, but `_`.
* One tab becomes four spaces
* No unused imports, references, variables

# Pull Requests
PRs welcome, as long as they follow the ideology (see: [README.md](README.md)) and solution layout.
