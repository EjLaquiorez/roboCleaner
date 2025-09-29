namespace RobotCleaner
{
	/// <summary>
	/// Strategy interface representing a pluggable cleaning algorithm for a <see cref="Robot"/>.
	/// Implementations may choose different exploration/cleaning behaviors.
	/// </summary>
	public interface ICleaningStrategy
	{
		/// <summary>
		/// Execute the strategy's cleaning routine. Implementations should drive the robot
		/// via its movement helpers and call <see cref="Robot.CleanCurrentSpot"/> as needed.
		/// </summary>
		void Clean(Robot robot, Map map);

		/// <summary>
		/// Execute the cleaning routine cooperatively with cancellation support.
		/// Implementations should periodically check <paramref name="token"/> and stop when requested.
		/// </summary>
		void Clean(Robot robot, Map map, System.Threading.CancellationToken token);
	}
}


