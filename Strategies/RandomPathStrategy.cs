using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RobotCleaner
{
	public class RandomPathStrategy : ICleaningStrategy
	{
		/// <summary>
		/// Randomized full-coverage: prefer unvisited random neighbors, otherwise
		/// jump via shortest path to the nearest unvisited reachable cell. Finally
		/// return to the origin (0,0). Cleans along the way.
		/// </summary>
		public void Clean(Robot robot, Map map)
		{
			CleanInternal(robot, map, null);
		}

		public void Clean(Robot robot, Map map, System.Threading.CancellationToken token)
		{
			CleanInternal(robot, map, token);
		}

		private void CleanInternal(Robot robot, Map map, System.Threading.CancellationToken? token)
		{
			var visited = new HashSet<(int,int)>();
			var rng = new Random();
			bool IsCancelled() => token.HasValue && token.Value.IsCancellationRequested;
			void MarkVisitedCurrent()
			{
				visited.Add((robot.X, robot.Y));
			}
			IEnumerable<Point> ShuffledNeighbors()
			{
				var list = GridUtils.Neighbors(map, new Point(robot.X, robot.Y)).ToList();
				for (int i = list.Count - 1; i > 0; i--)
				{
					int j = rng.Next(i + 1);
					var tmp = list[i];
					list[i] = list[j];
					list[j] = tmp;
				}
				return list;
			}
			void FollowPath(IEnumerable<Point> path)
			{
				foreach (var p in path)
				{
					if (IsCancelled()) return;
					if (p.X == robot.X && p.Y == robot.Y) continue;
					if (robot.Move(p.X, p.Y))
					{
						robot.CleanCurrentSpot();
						MarkVisitedCurrent();
					}
					else
					{
						break;
					}
				}
			}

			robot.CleanCurrentSpot();
			MarkVisitedCurrent();

			while (true)
			{
				if (IsCancelled()) return;
				bool movedRandomly = false;
				foreach (var n in ShuffledNeighbors())
				{
					if (!visited.Contains((n.X, n.Y)))
					{
						if (robot.Move(n.X, n.Y))
						{
							robot.CleanCurrentSpot();
							MarkVisitedCurrent();
							movedRandomly = true;
							break;
						}
					}
				}
				if (movedRandomly) continue;

				var start = new Point(robot.X, robot.Y);
				Point? target = Pathfinding.FindNearest(
					map,
					start,
					(x,y) => map.IsInBounds(x,y) && !map.IsObstacle(x,y) && !visited.Contains((x,y))
				);
				if (target == null)
				{
					break; // no more reachable unvisited cells
				}
				var path = Pathfinding.ShortestPath(map, start, target.Value);
				if (path == null || path.Count == 0)
				{
					break;
				}
				FollowPath(path);
			}

			// Return to origin (0,0)
			if (!IsCancelled())
			{
				var backPath = Pathfinding.ShortestPath(map, new Point(robot.X, robot.Y), new Point(0,0));
				if (backPath != null)
				{
					FollowPath(backPath);
				}
				robot.CleanCurrentSpot();
			}
		}
	}
}

