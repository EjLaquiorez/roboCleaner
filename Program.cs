using System;

namespace RobotCleaner
{
	/// <summary>
	/// Demo entry point. Builds a random map and runs a selected cleaning strategy.
	/// Swap the strategy instance below to compare behaviors.
	/// </summary>
	public class Program
	{
		public static void Main(string[] args){
			Console.WriteLine("Initialize robot");

			Map map = new Map(10, 5);
			map.PopulateRandom(0.12, 0.25);
			map.Display(0,0);

			ICleaningStrategy strategy = new CompleteCoverageStrategy();
			Robot robot = new Robot(map, strategy);

			var cts = new System.Threading.CancellationTokenSource();

			// run cleaning in a background task so we can accept user input
			System.Threading.Tasks.Task cleaningTask = System.Threading.Tasks.Task.Run(() => robot.StartCleaning(cts.Token));

			while (true)
			{
				Console.WriteLine();
				Console.WriteLine("Menu: [1] Complete Coverage  [2] S-Pattern  [3] Random  [4] Nearest Dirt  [R] Reset Map  [Q] Quit");
				Console.Write("Select: ");
				string input = Console.ReadLine()?.Trim().ToUpperInvariant() ?? string.Empty;
				switch (input)
				{
					case "1":
						cts.Cancel(); cleaningTask.Wait();
						strategy = new CompleteCoverageStrategy(); robot.SetStrategy(strategy);
						cts = new System.Threading.CancellationTokenSource();
						cleaningTask = System.Threading.Tasks.Task.Run(() => robot.StartCleaning(cts.Token));
						break;
					case "2":
						cts.Cancel(); cleaningTask.Wait();
						strategy = new SPatternStrategy(); robot.SetStrategy(strategy);
						cts = new System.Threading.CancellationTokenSource();
						cleaningTask = System.Threading.Tasks.Task.Run(() => robot.StartCleaning(cts.Token));
						break;
					case "3":
						cts.Cancel(); cleaningTask.Wait();
						strategy = new RandomPathStrategy(); robot.SetStrategy(strategy);
						cts = new System.Threading.CancellationTokenSource();
						cleaningTask = System.Threading.Tasks.Task.Run(() => robot.StartCleaning(cts.Token));
						break;
					case "4":
						cts.Cancel(); cleaningTask.Wait();
						strategy = new NearestDirtStrategy(); robot.SetStrategy(strategy);
						cts = new System.Threading.CancellationTokenSource();
						cleaningTask = System.Threading.Tasks.Task.Run(() => robot.StartCleaning(cts.Token));
						break;
					case "R":
						cts.Cancel(); cleaningTask.Wait();
						map.PopulateRandom(0.12, 0.25);
						map.Display(robot.X, robot.Y);
						cts = new System.Threading.CancellationTokenSource();
						cleaningTask = System.Threading.Tasks.Task.Run(() => robot.StartCleaning(cts.Token));
						break;
					case "Q":
						cts.Cancel(); cleaningTask.Wait();
						Console.WriteLine("Goodbye.");
						return;
					default:
						Console.WriteLine("Invalid selection.");
						break;
				}
			}
		}
	}
}
