namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 06, CodeType.Fastest)]
public partial class Day_06_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var lines = input.Lines;
		var operations = lines[^1];

		var part1 = 0L;
		var part2 = 0L;

		Span<int> byRow = stackalloc int[4];
		Span<int> byCol = stackalloc int[4];

		var byRowIndex = lines.Length - 1;
		var byColIndex = 0;

		var operation = '*';

		for (var i = 0; i < operations.Length; i++)
		{
			if (operations[i] is not ' ')
				operation = operations[i];

			var flag = false;
			var colNum = 0;

			for (var j = 0; j < byRowIndex; j++)
			{
				if (lines[j][i] is ' ')
					continue;

				flag = true;
				var num = lines[j][i] - '0';

				byRow[j] = (byRow[j] * 10) + num;
				colNum = (colNum * 10) + num;
			}

			if (!flag)
			{
				ProcessOperation(ref part1, ref part2, byRow, byCol, byRowIndex, byColIndex, operation);

				byCol.Clear();
				byRow.Clear();
				byColIndex = 0;
			}
			else
			{
				byCol[byColIndex++] = colNum;
			}
		}

		ProcessOperation(ref part1, ref part2, byRow, byCol, byRowIndex, byColIndex, operation);

		return (part1.ToString(), part2.ToString());
	}

	private static void ProcessOperation(ref long part1, ref long part2, Span<int> byRow, Span<int> byCol, int byRowIndex, int byColIndex, char operation)
	{
		if (operation is '*')
		{
			var num = 1L;
			for (var j = 0; j < byRowIndex; j++)
				num *= byRow[j];
			part1 += num;

			num = 1L;
			for (var j = 0; j < byColIndex; j++)
				num *= byCol[j];
			part2 += num;
		}
		else
		{
			var num = 0L;
			for (var j = 0; j < byRowIndex; j++)
				num += byRow[j];
			part1 += num;

			num = 0L;
			for (var j = 0; j < byColIndex; j++)
				num += byCol[j];
			part2 += num;
		}
	}
}
