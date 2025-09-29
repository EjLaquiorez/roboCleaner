using System;
using System.Threading;

namespace RobotCleaner
{
	/// <summary>
	/// Represents the 2D environment the robot operates in. Cells can be empty, dirt, obstacles, or cleaned.
	/// Provides helper methods for querying and mutating cell state and for visualizing the grid.
	/// </summary>
	public class Map
	{
		private enum CellType { Empty, Dirt, Obstacle, Cleaned };
		private CellType[,] _grid;
		/// <summary>Number of columns in the grid.</summary>
		public int Width {get; private set;}
		/// <summary>Number of rows in the grid.</summary>
		public int Height {get; private set;}
		
		/// <summary>Create a new empty map of the given size.</summary>
		/// <param name="width">Number of columns.</param>
		/// <param name="height">Number of rows.</param>
		public Map(int width, int height)
		{
			this.Width = width;
			this.Height = height;
			_grid = new CellType[width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++ )
				{
					_grid[x,y] = CellType.Empty;
				}
			}
		}
		
		/// <summary>Returns true if the coordinate lies within the map bounds.</summary>
		public bool IsInBounds(int x, int y)
		{
			return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
		}
		
		/// <summary>Returns true if the cell currently contains dirt.</summary>
		public bool IsDirt(int x, int y){
			return IsInBounds(x,y) && _grid[x,y] == CellType.Dirt;
		}
		
		/// <summary>Returns true if the cell is an obstacle.</summary>
		public bool IsObstacle(int x, int y){
			return IsInBounds(x,y) && _grid[x,y] == CellType.Obstacle;
		}
		
		/// <summary>Place an obstacle at a coordinate.</summary>
		public void AddObstacle(int x, int y)
		{
			_grid[x, y] = CellType.Obstacle;
		}
		/// <summary>Place dirt at a coordinate.</summary>
		public void AddDirt(int x, int y)
		{
			_grid[x, y] = CellType.Dirt;
		}
		
		/// <summary>Mark a cell as cleaned if it is within bounds.</summary>
		public void Clean(int x, int y)
		{
			if( IsInBounds(x,y))
			{
				_grid[x, y] = CellType.Cleaned;
			}
		}
		/// <summary>
		/// Clears the console and prints the grid, highlighting the robot location.
		/// A small delay is introduced to make movement visible.
		/// </summary>
		public void Display(int robotX, int robotY)
		{
			// display the 2d grid, it accepts the location of the robot in x and y
			Console.Clear();
			Console.WriteLine("Vacuum cleaner robot simulation");
			Console.WriteLine("--------------------------------");
			Console.WriteLine("Legends: #=Obstacles, D=Dirt, .=Empty, R=Robot, C=Cleaned");
			
			//display the grid using loop
			for (int y = 0; y < this.Height; y++)
			{
				for (int x = 0; x < this.Width; x++)
				{
					if( x==robotX && y == robotY)
					{
						Console.Write("R ");
					}
					else
					{
						switch(_grid[x,y])
						{
							case CellType.Empty: Console.Write(". "); break;
							case CellType.Dirt: Console.Write("D "); break;
							case CellType.Obstacle: Console.Write("# "); break;
							case CellType.Cleaned: Console.Write("C "); break;
						}
					}
				}
				Console.WriteLine();
			} //outer for loop
			// persistent on-screen menu options
			Console.WriteLine();
			Console.WriteLine("Menu: [1] Complete Coverage  [2] S-Pattern  [3] Random  [4] Nearest Dirt  [R] Reset Map  [Q] Quit");
			// add delay
			Thread.Sleep(200);
		} // display method

		/// <summary>
		/// Populate the map with random obstacles and dirt according to ratios.
		/// Guarantees at least one passable cell per column and keeps (0,0) empty.
		/// </summary>
		/// <param name="obstacleRatio">Probability per cell to become an obstacle.</param>
		/// <param name="dirtRatio">Probability per cell (after obstacles) to become dirt.</param>
		/// <param name="seed">Optional seed for determinism.</param>
		public void PopulateRandom(double obstacleRatio, double dirtRatio, int? seed = null)
		{
			if (obstacleRatio < 0 || dirtRatio < 0 || obstacleRatio + dirtRatio > 0.8)
			{
				throw new ArgumentException("Invalid ratios. Ensure non-negative and total <= 0.8 to avoid blocking.");
			}
			Random rnd = seed.HasValue ? new Random(seed.Value) : new Random();
			for (int x = 0; x < this.Width; x++)
			{
				// ensure at least one non-obstacle per row to prevent full blocking
				int guaranteedEmptyY = rnd.Next(0, this.Height);
				for (int y = 0; y < this.Height; y++)
				{
					if ((x == 0 && y == 0) || y == guaranteedEmptyY)
					{
						_grid[x,y] = CellType.Empty;
						continue;
					}
					double r = rnd.NextDouble();
					if (r < obstacleRatio)
					{
						_grid[x,y] = CellType.Obstacle;
					}
					else if (r < obstacleRatio + dirtRatio)
					{
						_grid[x,y] = CellType.Dirt;
					}
					else
					{
						_grid[x,y] = CellType.Empty;
					}
				}
			}
		}
	}//class map
}


