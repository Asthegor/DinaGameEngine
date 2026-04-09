# Dina Game Engine

![Version](https://img.shields.io/github/v/tag/Asthegor/DinaGameEngine?label=version)
![License](https://img.shields.io/badge/license-MIT-blue)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)
![.NET](https://img.shields.io/badge/.NET-10-purple)
![MonoGame](https://img.shields.io/badge/MonoGame-3.8-red)

A visual WPF editor for creating 2D games with [MonoGame](https://monogame.net/) and the [DinaCSharp](https://dinacsharp.lacombedominique.com) framework.

> **Dina Game Engine does not lock you in.** Every generated project is a standard Visual Studio C# solution — fully editable, fully yours.

---

## What is Dina Game Engine?

Dina Game Engine is a desktop editor that generates complete, ready-to-compile Visual Studio solutions for 2D MonoGame games. It handles the boilerplate — project structure, scene scaffolding, component registration, asset pipelines — so you can focus on your game.

The editor targets developers who want a head start without losing control. Generated code follows a clean partial-class pattern: engine-managed files (`*.Designer.cs`) coexist with user files where you write your own logic, and the two never conflict.

---

## Features

### Project Wizard
- Create a new game project from a single dialog
- Generates a full 6-project Visual Studio solution automatically
- Configures DinaCSharp as a DLL reference — no source dependency
- Supports multilingual projects from the start (14 languages available)

### Recent Projects Panel
- Grouped history (today, yesterday, this week, older…)
- Pin/unpin projects for quick access
- Right-click context menu to remove or unpin entries

### Scene Management
- Add and organize scenes visually
- Define the startup scene with a clear badge indicator
- Each scene generates its own `Designer.cs` (engine-managed) and `.cs` (user-managed) partial class pair

### Color System
- Named color palette per project (`PaletteColors` class)
- Visual color picker with RGBA sliders
- Full C# code generation — add a color in the editor, use it in code instantly

### Font Management
- Add TrueType fonts to your project
- Automatic SpriteFont generation at 5 resolutions (720p → 2160p) with proportional scaling
- `FontKeys` class generated for type-safe references in code

### Component System
The editor supports adding components to scenes with live property editing and immediate code generation.

**Text component**
- Font, content, color selection
- Position, alignment, z-order, visibility
- Full Designer.cs generation (field, load, update, draw)

**MenuManager component**
- Complete menu system with items and titles
- Per-item properties: font, content, color, alignment, z-order, visibility, icon support
- Hierarchical display in the properties panel (collapsible groups)
- Partial methods generated for selection/deselection callbacks

### Code Generation
- Surgical file modification via zone markers for project-level files — scene Designer files are fully regenerated to guarantee ordering consistency
- Partial class system separates generated code from user code cleanly
- Strategy pattern (`IComponentGenerator`) makes adding new component types straightforward
- `SectionParser` ensures safe, idempotent updates to generated files

---

## Generated Project Structure

Each project Dina creates is a standard Visual Studio solution:

```
MyGame/
├── MyGame.sln
├── DinaCSharp.dll
├── DinaCSharp.xml
├── Core/               # Keys, enums, shared constants
├── Fonts/              # SpriteFont definitions (5 resolutions)
├── Audio/              # Audio assets
├── Assets/             # Textures and sprites
├── Scenes/             # Scene classes (Designer.cs + user .cs)
└── MyGame/             # Main game project
```

User-editable partial methods are scaffolded and ready:

```csharp
// MyScene.cs  — yours to edit
partial void OnLoad() { }
partial void OnUpdate(GameTime gameTime) { }
partial void OnDraw(SpriteBatch spriteBatch) { }
```

---

## Requirements

- Windows 10 or later
- [.NET 10 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (to open and build generated projects)
- [MonoGame 3.8](https://monogame.net/download/) (Content Pipeline tools)

---

## Installation

A first release is coming. In the meantime, clone the repository and build from source in Visual Studio 2022.

---

## Related Project

**DinaCSharp** is the MonoGame-based framework that powers the generated games.
It is developed in parallel and distributed as a DLL bundled with the editor.

- Site: [dinacsharp.lacombedominique.com](https://dinacsharp.lacombedominique.com)

---

## Roadmap

- [ ] Image component
- [ ] Sound component
- [ ] Transition system
- [ ] GitHub wiki (documentation)
- [ ] Additional built-in component types

---

## License

MIT — see [LICENSE](LICENSE) for details.

---

## Author

Dominique Lacombe — solo developer, Montreal.
Coding since age 9. Building tools I wish had existed when I started making games.
