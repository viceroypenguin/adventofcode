namespace AdventOfCode.Puzzles._2018;

[Puzzle(2018, 25, CodeType.Original)]
public class Day_25_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var stars = input.Lines
			.Select(l => l.Split(','))
			.Select(l => (
				x: Convert.ToInt32(l[0]),
				y: Convert.ToInt32(l[1]),
				z: Convert.ToInt32(l[2]),
				t: Convert.ToInt32(l[3])))
			.ToArray();

		var constellations =
			stars.Select((_, i) => -i - 1).ToArray();
		var constellationNumber = 0;

		int ManhattanDistance((int x, int y, int z, int t) a, (int x, int y, int z, int t) b) =>
			Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) + Math.Abs(a.t - b.t);

		for (int i = 0; i < stars.Length; i++)
		{
			for (int j = 0; j < stars.Length; j++)
			{
				if (i == j) continue;
				if (ManhattanDistance(stars[i], stars[j]) <= 3)
				{
					if (constellations[i] < 0)
					{
						if (constellations[j] < 0)
						{
							constellations[i] = constellationNumber;
							constellations[j] = constellationNumber;
							constellationNumber++;
						}
						else
							constellations[i] = constellations[j];
					}
					else
					{
						if (constellations[j] < 0)
							constellations[j] = constellations[i];
						else if (constellations[i] != constellations[j])
						{
							var oldId = constellations[j];
							var newId = constellations[i];
							for (int k = 0; k < stars.Length; k++)
								if (constellations[k] == oldId)
									constellations[k] = newId;
						}
					}
				}
			}
		}

		return (
			constellations
				.Distinct()
				.Count()
				.ToString(),
			string.Empty);
	}
}
