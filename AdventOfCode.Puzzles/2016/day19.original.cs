namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 19, CodeType.Original)]
public class Day_19_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		static int nextPowerOfTwo(int n)
		{
			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;

			return n + 1;
		}

		static int partAElfHoldingPresents(int n) =>
			(n * 2 % nextPowerOfTwo(n)) + 1;

		static int nextPowerOfThree(int n)
		{
			var x = 3;
			for (; x < n; x *= 3)
				;
			return x;
		}

		static int partBElfHoldingPresents(int n)
		{
			var roundUp = nextPowerOfThree(n);
			var roundDown = roundUp / 3;
			return n <= roundDown * 2 ? n - roundDown :
				n == roundUp ? n :
				n * 2 % roundUp;
		}

		var num = Convert.ToInt32(input.Text);

		return (
			partAElfHoldingPresents(num).ToString(),
			partBElfHoldingPresents(num).ToString());
	}
}
