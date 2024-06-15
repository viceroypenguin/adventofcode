namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 13, CodeType.Original)]
public class Day_13_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Text
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		var screenOffset = 639;
		var screenHeight = Math.Max(instructions[604], instructions[605]);
		var screenSize = Math.Max(instructions[620], instructions[621]);
		var screenWidth = screenSize / screenHeight;

		var scoreOffset = instructions[632];
		var magicA = Math.Max(instructions[612], instructions[613]);
		var magicB = Math.Max(instructions[616], instructions[617]);

		long numBlocks = 0, score = 0;
		for (var y = 0; y < screenHeight; y++)
		{
			for (var x = 0; x < screenWidth; x++)
			{
				if (instructions[screenOffset + (y * screenWidth) + x] == 2)
				{
					numBlocks++;
					score += instructions[scoreOffset + (((((x * screenHeight) + y) * magicA) + magicB) % screenSize)];
				}
			}
		}

		return (
			numBlocks.ToString(),
			score.ToString());
	}
}
