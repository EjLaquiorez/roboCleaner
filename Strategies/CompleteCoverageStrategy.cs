namespace RobotCleaner
{
	public class CompleteCoverageStrategy : ICleaningStrategy
	{
		/// <summary>
		/// Depth-first style coverage of all reachable cells, backtracking when dead-ends are hit.
		/// Guarantees full reachable coverage without prior map knowledge.
		/// </summary>
		public void Clean(Robot robot, Map map)
		{
			bool[,] visited = new bool[map.Width, map.Height];
			var stack = new System.Collections.Generic.Stack<Point>();
			var dirs = new [] { new Point(1,0), new Point(0,1), new Point(-1,0), new Point(0,-1) }; // R, D, L, U

			void Visit()
			{
				if (!visited[robot.X, robot.Y])
				{
					visited[robot.X, robot.Y] = true;
					robot.CleanCurrentSpot();
				}
			}

			Visit();

			while (true)
			{
				bool advanced = false;
				foreach (var d in dirs)
				{
					int nx = robot.X + d.X;
					int ny = robot.Y + d.Y;
					if (map.IsInBounds(nx, ny) && !map.IsObstacle(nx, ny) && !visited[nx, ny])
					{
						stack.Push(new Point(robot.X, robot.Y));
						robot.Move(nx, ny);
						Visit();
						advanced = true;
						break;
					}
				}
				if (advanced)
				{
					continue;
				}
				if (stack.Count == 0)
				{
					break; // all reachable cells have been visited
				}
				var back = stack.Pop();
				robot.Move(back.X, back.Y);
				// no need to clean again; continue searching for new neighbors
			}
		}

		public void Clean(Robot robot, Map map, System.Threading.CancellationToken token)
		{
			bool[,] visited = new bool[map.Width, map.Height];
			var stack = new System.Collections.Generic.Stack<Point>();
			var dirs = new [] { new Point(1,0), new Point(0,1), new Point(-1,0), new Point(0,-1) };

			void Visit()
			{
				if (!visited[robot.X, robot.Y])
				{
					visited[robot.X, robot.Y] = true;
					robot.CleanCurrentSpot();
				}
			}

			Visit();

			while (!token.IsCancellationRequested)
			{
				bool advanced = false;
				foreach (var d in dirs)
				{
					if (token.IsCancellationRequested) return;
					int nx = robot.X + d.X;
					int ny = robot.Y + d.Y;
					if (map.IsInBounds(nx, ny) && !map.IsObstacle(nx, ny) && !visited[nx, ny])
					{
						stack.Push(new Point(robot.X, robot.Y));
						robot.Move(nx, ny);
						Visit();
						advanced = true;
						break;
					}
				}
				if (advanced)
				{
					continue;
				}
				if (stack.Count == 0)
				{
					break;
				}
				var back = stack.Pop();
				robot.Move(back.X, back.Y);
			}
		}
	}
}


