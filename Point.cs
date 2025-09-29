using System;

namespace RobotCleaner
{
	/// <summary>
	/// Immutable 2D grid coordinate used for map and pathfinding operations.
	/// </summary>
	public readonly struct Point
	{
		/// <summary>Zero-based column index.</summary>
		public int X { get; }
		/// <summary>Zero-based row index.</summary>
		public int Y { get; }
		/// <summary>Create a new point at (<paramref name="x"/>, <paramref name="y"/>).</summary>
		/// <param name="x">Column index.</param>
		/// <param name="y">Row index.</param>
		public Point(int x, int y) { X = x; Y = y; }
		/// <summary>Deconstructs the point for tuple-like assignment.</summary>
		public void Deconstruct(out int x, out int y) { x = X; y = Y; }
		public override string ToString() => $"({X},{Y})";
	}
}


