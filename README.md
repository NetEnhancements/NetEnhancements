<div align="center">

# NetEnhancements  
A modular toolkit for building .NET 7+ applications with speed and clarity

[![Docs](https://img.shields.io/badge/docs-netenhancements.github.io-blue)](https://netenhancements.github.io/)
[![Build](https://img.shields.io/github/actions/workflow/status/NetEnhancements/NetEnhancements/Continuous-Integration.yml?branch=main)](https://github.com/NetEnhancements/NetEnhancements/actions)
[![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Shared.svg)](https://www.nuget.org/packages/NetEnhancements.Shared/)

</div>

# [Documentation](https://netenhancements.github.io/)
See https://netenhancements.github.io/.


## 📦 Features

- `Util`: Extension methods for dates, strings, enums, collections, randomness, and hashing.  
- `Shared`: Centralized app configuration (`IConfiguration`), environment detection, and structured logging support.
- `AspNet`: Conventions for routing, security headers, model validation, exception handling, and minimal API helpers.
- `EntityFramework`: EF Core extensions for `DateOnly`/`TimeOnly`, auditing (`CreatedOn`, `ModifiedOn`), soft deletes, and GUID-based primary keys.
  - `EntityFramework.Data`: Adds repository and unit of work patterns, query projections, paging, and service registration helpers.
- `ClosedXML`: Helpers to export/import Excel files with automatic column detection, styling, and typing.
- `Identity`: Plug-and-play ASP.NET Core Identity setup with strongly typed roles and claims, based on GUIDs.
  - `Identity.Data`: EntityFramework support for custom users/roles, including seeding and audit fields.
- `Imaging`: Image resizing, MIME detection, aspect ratio handling, and file/stream conversion.
  - `Imaging.EntityFramework`: Store image metadata in EF Core models (dimensions, format, orientation, etc).
- `Services`: Hosted services with scoped execution, graceful shutdown, queue processing, and delay scheduling.
- `Business`: Combine the above in clean, reusable service base classes with validation, `Result<T>` types, and orchestration support.


## 💿 Installation

All packages are available on NuGet and follow [Semantic Versioning](https://semver.org/). Pick the parts you need:


| Package | NuGet | Downloads |
| ------- | ----- | --------- |
| [NetEnhancements.Util](https://www.nuget.org/packages/NetEnhancements.Util/)<br><small>Utility classes and helpers.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Util.svg)](https://www.nuget.org/packages/NetEnhancements.Util) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.Util.svg)](https://www.nuget.org/packages/NetEnhancements.Util) |
| [NetEnhancements.Shared](https://www.nuget.org/packages/NetEnhancements.Shared/)<br><small>Shared abstractions and contracts.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Shared.svg)](https://www.nuget.org/packages/NetEnhancements.Shared) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.Shared.svg)](https://www.nuget.org/packages/NetEnhancements.Shared) |
| [NetEnhancements.AspNet](https://www.nuget.org/packages/NetEnhancements.AspNet/)<br><small>Enhancements for ASP.NET applications.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.AspNet.svg)](https://www.nuget.org/packages/NetEnhancements.AspNet) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.AspNet.svg)](https://www.nuget.org/packages/NetEnhancements.AspNet) |
| [NetEnhancements.ClosedXML](https://www.nuget.org/packages/NetEnhancements.ClosedXML/)<br><small>Helpers and wrappers around ClosedXML for Excel export/import.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.ClosedXML.svg)](https://www.nuget.org/packages/NetEnhancements.ClosedXML) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.ClosedXML.svg)](https://www.nuget.org/packages/NetEnhancements.ClosedXML) |
| [NetEnhancements.EntityFramework](https://www.nuget.org/packages/NetEnhancements.EntityFramework/)<br><small>EF Core enhancements for querying and persistence.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.EntityFramework.svg)](https://www.nuget.org/packages/NetEnhancements.EntityFramework) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.EntityFramework.svg)](https://www.nuget.org/packages/NetEnhancements.EntityFramework) |
| [NetEnhancements.EntityFramework.Data](https://www.nuget.org/packages/NetEnhancements.EntityFramework.Data/)<br><small>Repository and unit-of-work support for EF Core.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.EntityFramework.Data.svg)](https://www.nuget.org/packages/NetEnhancements.EntityFramework.Data) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.EntityFramework.Data.svg)](https://www.nuget.org/packages/NetEnhancements.EntityFramework.Data) |
| [NetEnhancements.Business](https://www.nuget.org/packages/NetEnhancements.Business/)<br><small>Business layer patterns and service base implementations.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Business.svg)](https://www.nuget.org/packages/NetEnhancements.Business) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.Business.svg)](https://www.nuget.org/packages/NetEnhancements.Business) |
| [NetEnhancements.Services](https://www.nuget.org/packages/NetEnhancements.Services/)<br><small>Service registration and dependency injection helpers.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Services.svg)](https://www.nuget.org/packages/NetEnhancements.Services) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.Services.svg)](https://www.nuget.org/packages/NetEnhancements.Services) |
| [NetEnhancements.Imaging](https://www.nuget.org/packages/NetEnhancements.Imaging/)<br><small>Image processing utilities and helpers.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Imaging.svg)](https://www.nuget.org/packages/NetEnhancements.Imaging) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.Imaging.svg)](https://www.nuget.org/packages/NetEnhancements.Imaging) |
| [NetEnhancements.Identity](https://www.nuget.org/packages/NetEnhancements.Identity/)<br><small>Extensions for ASP.NET Core Identity.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Identity.svg)](https://www.nuget.org/packages/NetEnhancements.Identity) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.Identity.svg)](https://www.nuget.org/packages/NetEnhancements.Identity) |
| [NetEnhancements.Imaging.EntityFramework](https://www.nuget.org/packages/NetEnhancements.Imaging.EntityFramework/)<br><small>EF Core integration for image storage and metadata.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Imaging.EntityFramework.svg)](https://www.nuget.org/packages/NetEnhancements.Imaging.EntityFramework) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.Imaging.EntityFramework.svg)](https://www.nuget.org/packages/NetEnhancements.Imaging.EntityFramework) |
| [NetEnhancements.Identity.Data](https://www.nuget.org/packages/NetEnhancements.Identity.Data/)<br><small>EntityFramework support for Identity enhancements.</small> | [![NuGet](https://img.shields.io/nuget/v/NetEnhancements.Identity.Data.svg)](https://www.nuget.org/packages/NetEnhancements.Identity.Data) | [![NuGet](https://img.shields.io/nuget/dt/NetEnhancements.Identity.Data.svg)](https://www.nuget.org/packages/NetEnhancements.Identity.Data) |


## 🧭 Philosophy

- We start at **version 0.x** — things may change.
- Once stable, we’ll release `v1.0.0` and follow clear versioning.
- No tests + no docs = no support.
- All packages are modular and DI-friendly.
