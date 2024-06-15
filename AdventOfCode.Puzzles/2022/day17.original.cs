namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 17, CodeType.Original)]
public partial class Day_17_Original : IPuzzle
{
	private static readonly (int maxX, int maxY, HashSet<(int x, int y)>)[] s_rocks =
	[
		(3, 0, [(0, 0), (1, 0), (2, 0), (3, 0),]),
		(2, 2, [(1, 0), (0, 1), (1, 1), (1, 2), (2, 1),]),
		(2, 2, [(0, 0), (1, 0), (2, 0), (2, 1), (2, 2),]),
		(0, 3, [(0, 0), (0, 1), (0, 2), (0, 3),]),
		(1, 1, [(0, 0), (0, 1), (1, 0), (1, 1),]),
	];

	private sealed class State : IEquatable<State>
	{
		public int CurrentRockIndex { get; set; }
		public int LastLandedX { get; set; }
		public int JetPatternIndex { get; set; }

		public int MaximumMapRow => FallenRocks.Count;
		public List<byte[]> FallenRocks { get; } = [];
		public string BuildMap() =>
			string.Join(Environment.NewLine, FallenRocks
				.Select(l =>
					string.Join("", l.Select(c => (char)c)))
				.Reverse());

		public byte[] JetPattern { get; set; } = [];

		public bool Equals(State? other) =>
			other != null
			&& CurrentRockIndex == other.CurrentRockIndex
			&& LastLandedX == other.LastLandedX
			&& JetPatternIndex == other.JetPatternIndex;

		public override bool Equals(object? obj) =>
			Equals(obj as State);

		public override int GetHashCode() =>
			HashCode.Combine(
				CurrentRockIndex,
				LastLandedX,
				JetPatternIndex);

		public void DropRock()
		{
			var (x, y) = (2, MaximumMapRow + 3);

			var (rockWidth, _, rock) = s_rocks[CurrentRockIndex++];
			if (CurrentRockIndex >= s_rocks.Length)
				CurrentRockIndex = 0;

			while (true)
			{
				var action = JetPattern[JetPatternIndex++];
				if (JetPatternIndex >= JetPattern.Length)
					JetPatternIndex = 0;

				var newX = action == '<' ? x - 1 : x + 1;
				if (newX < 0 || (newX + rockWidth) >= 7
					|| Intersection(rock, newX, y))
				{
					newX = x;
				}

				var newY = y - 1;
				if (newY == -1 || Intersection(rock, newX, newY))
				{
					LastLandedX = newX;
					PlaceRock(rock, newX, y);
					return;
				}

				(x, y) = (newX, newY);
			}
		}

		private bool Intersection(HashSet<(int x, int y)> rock, int x, int y)
		{
			if (y >= MaximumMapRow)
				return false;

			foreach (var (rx, ry) in rock)
			{
				if (y + ry >= MaximumMapRow)
					continue;

				if (FallenRocks[y + ry][x + rx] == '#')
					return true;
			}

			return false;
		}

		private void PlaceRock(HashSet<(int x, int y)> rock, int x, int y)
		{
			foreach (var (rx, ry) in rock)
			{
				while (y + ry >= MaximumMapRow)
					FallenRocks.Add(Enumerable.Repeat((byte)'.', 7).ToArray());
				FallenRocks[y + ry][x + rx] = (byte)'#';
			}
		}
	}

	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var jetPattern = input.Bytes[..^1];

		var state1 = new State
		{
			CurrentRockIndex = 0,
			JetPatternIndex = 0,
			JetPattern = jetPattern,
		};
		var state2 = new State
		{
			CurrentRockIndex = 0,
			JetPatternIndex = 0,
			JetPattern = jetPattern,
		};

		var i = 0;
		for (; i < 2022; i++)
		{
			state1.DropRock();
			state2.DropRock();
			state2.DropRock();
		}

		var part1 = state1.MaximumMapRow.ToString();

		for (; !state1.Equals(state2); i++)
		{
			state1.DropRock();
			state2.DropRock();
			state2.DropRock();
		}

		const long RockCount = 1_000_000_000_000;
		var loopCount = (RockCount / i) - 1;
		var loopSum = loopCount * (state2.MaximumMapRow - state1.MaximumMapRow);
		loopSum += state1.MaximumMapRow;

		var curHeight = state1.MaximumMapRow;
		var remainder = RockCount % i;
		for (i = 0; i < remainder; i++)
		{
			state1.DropRock();
		}

		loopSum += state1.MaximumMapRow - curHeight;

		var part2 = loopSum.ToString();
		return (part1, part2);
	}
}
