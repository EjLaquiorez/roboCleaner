namespace RobotCleaner
{
	public class NearestDirtStrategy : ICleaningStrategy
	{
		/// <summary>
		/// Repeatedly finds the nearest dirt using BFS and routes there with a shortest path
		/// until no dirt remains. Skips unreachable dirt by marking it cleaned to avoid loops.
		/// </summary>
		public void Clean(Robot robot, Map map)
		{
			while (true)
			{
				// Ensure we clean where we stand to avoid looping when starting on dirt
				robot.CleanCurrentSpot();
				var from = new Point(robot.X, robot.Y);
				var target = Pathfinding.FindNearest(map, from, (x,y) => map.IsDirt(x,y));
				if (target == null)
				{
					break; // no more dirt
				}
				var path = Pathfinding.ShortestPath(map, from, target.Value);
				if (path == null)
				{
					// unreachable dirt (should be rare with our generator). Mark as cleaned skip to avoid infinite loop
					map.Clean(target.Value.X, target.Value.Y);
					continue;
				}
				robot.MoveAlongPath(path);
				// Clean after arrival as well (in case path did not move)
				robot.CleanCurrentSpot();
			}
		}

		public void Clean(Robot robot, Map map, System.Threading.CancellationToken token)
		{
			while (true)
			{
				if (token.IsCancellationRequested) return;
				robot.CleanCurrentSpot();
				var from = new Point(robot.X, robot.Y);
				var target = Pathfinding.FindNearest(map, from, (x,y) => map.IsDirt(x,y));
				if (target == null)
				{
					break;
				}
				var path = Pathfinding.ShortestPath(map, from, target.Value);
				if (path == null)
				{
					map.Clean(target.Value.X, target.Value.Y);
					continue;
				}
				robot.MoveAlongPath(path);
				robot.CleanCurrentSpot();
			}
		}
	}
}


