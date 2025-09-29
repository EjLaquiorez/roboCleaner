namespace RobotCleaner
{
	/// <summary>
	/// The autonomous vacuum robot. Holds position, a reference to the map, and a swappable
	/// <see cref="ICleaningStrategy"/> that decides how it moves and cleans.
	/// </summary>
	public class Robot
	{
		private readonly Map _map;
		private ICleaningStrategy _strategy;
		
		/// <summary>Current column of the robot.</summary>
		public int X {get; set;}
		/// <summary>Current row of the robot.</summary>
		public int Y {get; set;}
		
		/// <summary>The map the robot operates on.</summary>
		public Map Map { get { return _map;}}
		
		/// <summary>Create a robot at (0,0) with the provided strategy.</summary>
		public Robot(Map map, ICleaningStrategy strategy)
		{
			_map = map;
			_strategy = strategy;
			X = 0;
			Y = 0;
		}
		
		/// <summary>
		/// Attempt to move to a new coordinate if it is within bounds and not an obstacle.
		/// Returns true if the move occurred. Also updates the display.
		/// </summary>
		public bool Move(int newX, int newY)
		{
			if( _map.IsInBounds(newX, newY) && !_map.IsObstacle(newX, newY) )
			{
				// set the new location
				X = newX;
				Y = newY;
				// display the map with the robot in its location in the grid
				_map.Display(X, Y);
					return true;
			}
			// it cannot move
			return false;
		}// Move method
		
		/// <summary>
		/// If standing on dirt, mark it as cleaned and refresh the display.
		/// </summary>
		public void CleanCurrentSpot()
		{
			if(_map.IsDirt(X, Y))
			{
				_map.Clean(X, Y);
				_map.Display(X, Y);
			}
		}

		/// <summary>
		/// Follow a sequence of waypoints, cleaning along the way. Stops early if a step is invalid.
		/// </summary>
		public void MoveAlongPath(System.Collections.Generic.IEnumerable<Point> path)
		{
			foreach (var p in path)
			{
				if (p.X == X && p.Y == Y) { continue; }
				if (Move(p.X, p.Y))
				{
					CleanCurrentSpot();
				}
				else
				{
					break;
				}
			}
		}
		
		/// <summary>
		/// Invoke the current <see cref="ICleaningStrategy"/> to perform cleaning.
		/// </summary>
		public void StartCleaning()
		{
			_strategy.Clean(this, _map);
		}

		/// <summary>
		/// Invoke the current strategy with cancellation support.
		/// </summary>
		public void StartCleaning(System.Threading.CancellationToken token)
		{
			_strategy.Clean(this, _map, token);
		}

		/// <summary>Swap to a new cleaning strategy at runtime.</summary>
		public void SetStrategy(ICleaningStrategy strategy)
		{
			_strategy = strategy;
		}
	}
}


