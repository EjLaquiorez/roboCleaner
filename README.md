# RoboCleaner 🧹🤖

A simple C# console-based **robot vacuum simulation** that demonstrates pathfinding, map handling, and cleaning strategies. The simulation uses a grid-based environment populated with dirt, obstacles, and empty cells, where a robot navigates intelligently (or randomly) to clean the map.

---

## Features

* **Grid-based environment** (`Map`)

  * Supports dirt, obstacles, empty, and cleaned cells.
  * Random map generation with configurable obstacle/dirt ratios.
  * Console visualization of the robot’s movement and cleaning progress.

* **Robot functionality** (`Robot`)

  * Moves step by step across the grid.
  * Cleans dirt upon visiting a dirty cell.
  * Displays map state after each action.

* **Cleaning strategies** (`ICleaningStrategy`)

  * **S-Pattern Strategy** – Moves in a snake-like (row by row) pattern for systematic coverage.
  * **Random Path Strategy** – Moves randomly across the grid while cleaning.
  * **Nearest Dirt Strategy** – Finds and moves to the closest dirt using BFS pathfinding.
  * **Complete Coverage Strategy** – Ensures all reachable tiles are visited and cleaned.

* **Pathfinding utilities**

  * BFS-based shortest pathfinding (`Pathfinding.ShortestPath`).
  * Nearest target search (`Pathfinding.FindNearest`).
  * Neighbor exploration via cardinal directions.

---

## Requirements

* .NET 6.0 or later (or compatible runtime).
* Console window (supports ASCII grid rendering).

---

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/RoboCleaner.git
   cd RoboCleaner
   ```

2. Build the project:

   ```bash
   dotnet build
   ```

3. Run the simulation:

   ```bash
   dotnet run
   ```

---

## Example Output

```
Vacuum cleaner robot simulation
--------------------------------
Legends: #=Obstacles, D=Dirt, .=Empty, R=Robot, C=Cleaned

. . . . # . . D . .
. D . . . . # . D .
R . . # . . . . . .
. # . . D . . . # .
. . . D . . # . . .
```

As the robot moves, it updates the grid in real-time, showing cleaned cells (`C`) and its current position (`R`).

---

## Customization

* Change **map size**:

  ```csharp
  Map map = new Map(15, 10); // width=15, height=10
  ```

* Adjust **random generation ratios**:

  ```csharp
  map.PopulateRandom(0.10, 0.30); // 10% obstacles, 30% dirt
  ```

* Switch **cleaning strategy**:

  ```csharp
  robot.SetStrategy(new SPatternStrategy());
  robot.StartCleaning();
  ```

---

## Educational Value

This project is useful for:

* Practicing **object-oriented design** in C#.
* Learning about **interfaces and strategy patterns**.
* Implementing **pathfinding algorithms** (BFS).
* Visualizing algorithms in a **grid-based environment**.

---

## License

This project is open-source and available under the **MIT License**.
