# Dina Game Engine

> 🇫🇷 [Français](#français) | 🇬🇧 [English](#english)

---

## Français

### Description

**Dina Game Engine** est un éditeur visuel WPF permettant de créer et gérer des jeux 2D basés sur [MonoGame](https://monogame.net/) et le framework [DinaCSharp](https://github.com/Asthegor/DinaCSharp).

Il s'inspire des moteurs de jeu modernes comme Unity ou Unreal Engine, tout en restant volontairement simple et accessible. L'éditeur génère automatiquement la structure complète d'un projet Visual Studio prêt à compiler.

> ⚠️ **Projet en développement actif** — des fonctionnalités sont ajoutées régulièrement. L'API et la structure peuvent évoluer.

---

### Technologies

| Composant | Technologie |
|---|---|
| Langage | C# (.NET 10) |
| Interface | WPF, architecture MVVM |
| Runtime de jeu | MonoGame + DinaCSharp |
| Format de solution | `.slnx` (Visual Studio) |

---

### Fonctionnalités actuelles

- ✅ **Fenêtre de démarrage** — liste des projets récents avec épinglage, groupement par date
- ✅ **Création de projet** — assistant en 3 étapes (nom, emplacement, validation des marqueurs)
- ✅ **Génération du template** — structure complète de solution Visual Studio générée automatiquement
- ✅ **Marqueurs personnalisables** — nom de la solution, nom du projet, namespace racine
- ✅ **Localisation** — interface disponible en français et en anglais
- ✅ **Logs** — traçabilité complète des opérations dans un fichier de log
- ✅ **Mise à jour** — mécanisme de mise à jour prévu via GitHub Releases

---

### Captures d'écran

> 📸 Captures à venir

---

### Roadmap

- 🔲 `MainWindow` — fenêtre principale de l'éditeur
- 🔲 `UpdateService` — vérification et installation des mises à jour
- 🔲 `WhatsNewWindow` — fenêtre des nouveautés après mise à jour
- 🔲 `DinaGameEngine.Updater` — utilitaire de mise à jour autonome
- 🔲 Gestion des scènes depuis l'éditeur
- 🔲 Éditeur visuel de scènes

---

### Prérequis

- [Visual Studio 2022](https://visualstudio.microsoft.com/) v17.13 ou supérieur
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [MonoGame](https://monogame.net/) 3.8.4 ou supérieur

---

### Structure de la solution

```
DinaGameEngine.sln
├── DinaGameEngine.Common        ← ObservableObject, RelayCommand, utilitaires partagés
├── DinaGameEngine.Abstractions  ← Interfaces des services
├── DinaGameEngine.Models        ← Modèles de données
├── DinaGameEngine.Services      ← Implémentations des services
├── DinaGameEngine.Templates     ← Templates de projet embarqués
├── DinaGameEngine.Updater       ← Utilitaire de mise à jour (WPF autonome)
└── DinaGameEngine               ← Application WPF principale
```

---

### Licence

Ce projet est sous licence MIT.

---

---

## English

### Description

**Dina Game Engine** is a WPF visual editor for creating and managing 2D games built on [MonoGame](https://monogame.net/) and the [DinaCSharp](https://github.com/Asthegor/DinaCSharp) framework.

It draws inspiration from modern game engines such as Unity or Unreal Engine, while remaining intentionally simple and accessible. The editor automatically generates a complete, ready-to-compile Visual Studio project structure.

> ⚠️ **Actively developed** — features are being added regularly. The API and structure may change.

---

### Technologies

| Component | Technology |
|---|---|
| Language | C# (.NET 10) |
| UI | WPF, MVVM architecture |
| Game runtime | MonoGame + DinaCSharp |
| Solution format | `.slnx` (Visual Studio) |

---

### Current Features

- ✅ **Startup window** — recent projects list with pinning and date-based grouping
- ✅ **Project creation** — 3-step wizard (name, location, marker validation)
- ✅ **Template generation** — full Visual Studio solution structure generated automatically
- ✅ **Customizable markers** — solution name, project name, root namespace
- ✅ **Localization** — UI available in French and English
- ✅ **Logging** — full operation traceability in a log file
- ✅ **Updates** — update mechanism planned via GitHub Releases

---

### Screenshots

> 📸 Coming soon

---

### Roadmap

- 🔲 `MainWindow` — main editor window
- 🔲 `UpdateService` — update check and installation
- 🔲 `WhatsNewWindow` — what's new window after updates
- 🔲 `DinaGameEngine.Updater` — standalone update utility
- 🔲 Scene management from the editor
- 🔲 Visual scene editor

---

### Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/) v17.13 or later
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [MonoGame](https://monogame.net/) 3.8.4 or later

---

### Solution Structure

```
DinaGameEngine.sln
├── DinaGameEngine.Common        ← ObservableObject, RelayCommand, shared utilities
├── DinaGameEngine.Abstractions  ← Service interfaces
├── DinaGameEngine.Models        ← Data models
├── DinaGameEngine.Services      ← Service implementations
├── DinaGameEngine.Templates     ← Embedded project templates
├── DinaGameEngine.Updater       ← Standalone update utility (WPF)
└── DinaGameEngine               ← Main WPF application
```

---

### License

This project is licensed under the MIT License.
