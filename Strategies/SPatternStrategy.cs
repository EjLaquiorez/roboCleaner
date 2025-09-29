using System.Collections.Generic;
using System.Linq;

namespace RobotCleaner
{
	public class SPatternStrategy : ICleaningStrategy
	{
		/// <summary>
		/// Sweep the map row-by-row in an "S"/boustrophedon pattern for coverage.
		/// Avoids obstacles by pathing around them to the next reachable passable segment.
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
			bool IsCancelled() => token.HasValue && token.Value.IsCancellationRequested;
			bool IsPassable(int x, int y) => map.IsInBounds(x, y) && !map.IsObstacle(x, y);
			IEnumerable<(int startX, int endX, int step)> BuildSegmentsForRow(int y, int direction)
			{
				int start = direction == 1 ? 0 : map.Width - 1;
				int endExclusive = direction == 1 ? map.Width : -1;
				int step = direction;
				int? segStart = null;
				for (int x = start; x != endExclusive; x += step)
				{
					if (IsPassable(x, y))
					{
						if (!segStart.HasValue) segStart = x;
					}
					else
					{
						if (segStart.HasValue)
						{
							yield return (segStart.Value, x - step, step);
							segStart = null;
						}
					}
				}
				if (segStart.HasValue)
				{
					int last = direction == 1 ? map.Width - 1 : 0;
					yield return (segStart.Value, last, step);
				}
			}

			// Start by cleaning current spot
			robot.CleanCurrentSpot();

			int direction = 1; // 1 = right, -1 = left
			for (int y = 0; y < map.Height; y++)
			{
				if (IsCancelled()) return;
				// Build contiguous passable segments for this row in traversal order
				foreach (var seg in BuildSegmentsForRow(y, direction))
				{
					if (IsCancelled()) return;
					int entryX = seg.startX; // already in traversal order for this row
					var startPoint = new Point(robot.X, robot.Y);
					var goalPoint = new Point(entryX, y);
					var pathToSegment = Pathfinding.ShortestPath(map, startPoint, goalPoint);
					if (pathToSegment == null)
					{
						// Segment entry unreachable; skip this segment
						continue;
					}
					robot.MoveAlongPath(pathToSegment);
					// Sweep the segment directly (adjacent moves)
					int x = seg.startX;
					while (true)
					{
						if (IsCancelled()) return;
						robot.Move(x, y);
						robot.CleanCurrentSpot();
						if (x == seg.endX) break;
						x += seg.step;
					}
				}
				direction *= -1; // Reverse direction for next row
			}
		}
	}
}


