namespace RobotCleaner
{
	/// <summary>
	/// Helper utilities for grid movement and neighborhood traversal.
	/// </summary>
	public static class GridUtils
	{
		/// <summary>Four-directional movement vectors: Right, Left, Down, Up.</summary>
		public static readonly Point[] CardinalDirections = new []
		{
			new Point(1,0), new Point(-1,0), new Point(0,1), new Point(0,-1)
		};

		/// <summary>
		/// Enumerates passable neighbor cells around a point (no obstacles, in-bounds).
		/// </summary>
		public static System.Collections.Generic.IEnumerable<Point> Neighbors(Map map, Point p)
		{
			foreach (var d in CardinalDirections)
			{
				int nx = p.X + d.X;
				int ny = p.Y + d.Y;
				if (map.IsInBounds(nx, ny) && !map.IsObstacle(nx, ny))
				{
					yield return new Point(nx, ny);
				}
			}
		}
	}
}


