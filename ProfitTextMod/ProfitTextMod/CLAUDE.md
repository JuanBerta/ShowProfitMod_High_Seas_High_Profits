# CLAUDE Guidance for ProfitTextMod

This document serves as a high-level guide for future Claude Code instances working within the ProfitTextMod codebase. It is designed to provide context on the architecture and standard development practices to maximize productivity.

## 1. Development Commands

For standard development workflows, the following commands are expected:

*   **Build:** Build the Unity project (e.g., via Unity Editor or build scripts).
*   **Lint:** Run code analysis/linting tools (e.g., Roslyn analyzers, Unity Linter).
*   **Run Tests:** Execute unit or integration tests defined within the project structure.
*   **Run Single Test:** Execute a specific test case to verify functionality.

## 2. High-Level Code Architecture

The codebase is structured around C# scripts, core DLLs, and Unity assets, which suggests a modular, component-based architecture.

*   **Core Logic:** Primary game/mod logic resides within the C# source files (e.g., `*.cs` files) located in the project folders.
*   **Assembly Structure:** Core functionality is exposed through compiled DLLs (e.g., those found in `bin\Debug\`). These assemblies contain the compiled logic for features like Harmony patching, asset ripping, and core game systems.
*   **Asset Management:** Mod assets, configurations, and loaded data are managed via specific XML/asset files (e.g., `MelonLoader.xml`) alongside compiled code.
*   **Modularity:** The structure emphasizes separation between core engine components and mod-specific implementations, allowing for targeted modifications.

## 3. Productivity Notes

Focus on modifying logic within source code files first, and use the compiled DLLs for understanding the runtime structure. Avoid repeating information found in existing documentation files like `README.md`, `.cursor/rules/`, or `.github/copilot-instructions.md`.
