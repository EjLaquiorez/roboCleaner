namespace RobotCleaner
{
	/// <summary>
	/// Breadth-first search utilities for target discovery and shortest pathing on the grid.
	/// </summary>
	public static class Pathfinding
	{
		/// <summary>
		/// Finds the nearest cell satisfying <paramref name="predicate"/> from a start point using BFS.
		/// Returns null when no such cell is reachable.
		/// </summary>
		public static Point? FindNearest(Map map, Point start, System.Func<int,int,bool> predicate)
		{
			var queue = new System.Collections.Generic.Queue<Point>();
			var visited = new System.Collections.Generic.HashSet<(int,int)>();
			queue.Enqueue(start);
			visited.Add((start.X, start.Y));
			while (queue.Count > 0)
			{
				var cur = queue.Dequeue();
				if (predicate(cur.X, cur.Y))
				{
					return cur;
				}
				foreach (var n in GridUtils.Neighbors(map, cur))
				{
					var key = (n.X, n.Y);
					if (!visited.Contains(key))
					{
						visited.Add(key);
						queue.Enqueue(n);
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Computes a shortest path between two points using BFS on passable cells.
		/// Returns null if the goal is unreachable.
		/// </summary>
		public static System.Collections.Generic.List<Point> ShortestPath(Map map, Point start, Point goal)
		{
			var queue = new System.Collections.Generic.Queue<Point>();
			var cameFrom = new System.Collections.Generic.Dictionary<(int,int),(int,int)>();
			var visited = new System.Collections.Generic.HashSet<(int,int)>();
			queue.Enqueue(start);
			visited.Add((start.X, start.Y));
			bool found = false;
			while (queue.Count > 0)
			{
				var cur = queue.Dequeue();
				if (cur.X == goal.X && cur.Y == goal.Y)
				{
					found = true;
					break;
				}
				foreach (var n in GridUtils.Neighbors(map, cur))
				{
					var key = (n.X, n.Y);
					if (!visited.Contains(key))
					{
						visited.Add(key);
						cameFrom[key] = (cur.X, cur.Y);
						queue.Enqueue(n);
					}
				}
			}
			if (!found) return null;
			var path = new System.Collections.Generic.List<Point>();
			var curKey = (goal.X, goal.Y);
			path.Add(new Point(curKey.Item1, curKey.Item2));
			while (!(curKey.Item1 == start.X && curKey.Item2 == start.Y))
			{
				curKey = cameFrom[curKey];
				path.Add(new Point(curKey.Item1, curKey.Item2));
			}
			path.Reverse();
			return path;
		}
	}
}


