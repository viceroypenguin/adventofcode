namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 19, CodeType.Original)]
public class Day_19_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		static int NextPowerOfTwo(int n)
		{
			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;

			return n + 1;
		}

		static int PartAElfHoldingPresents(int n) =>
			(n * 2 % NextPowerOfTwo(n)) + 1;

		static int NextPowerOfThree(int n)
		{
			var x = 3;
			while (x < n)
				x *= 3;
			return x;
		}

		static int PartBElfHoldingPresents(int n)
		{
			var roundUp = NextPowerOfThree(n);
			var roundDown = roundUp / 3;
			return n <= roundDown * 2 ? n - roundDown :
				n == roundUp ? n :
				n * 2 % roundUp;
		}

		var num = Convert.ToInt32(input.Text);

		return (
			PartAElfHoldingPresents(num).ToString(),
			PartBElfHoldingPresents(num).ToString());
	}
}
