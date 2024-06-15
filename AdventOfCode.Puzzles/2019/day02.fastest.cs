using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 2, CodeType.Fastest)]
public class Day_02_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		Span<int> nums = stackalloc int[input.Bytes.Length / 2];
		int numCount = 0, n = 0;
		foreach (var c in input.Bytes)
		{
			if (c == ',')
			{
				nums[numCount++] = n;
				n = 0;
			}
			else if (c >= '0')
			{
				n = (n * 10) + c - '0';
			}
		}

		nums = nums[..numCount];
		Span<int> copy = stackalloc int[numCount];
		nums.CopyTo(copy);
		copy[1] = 12;
		copy[2] = 2;

		RunProgram(copy, numCount);
		var part1 = copy[0].ToString();

		nums.CopyTo(copy);
		copy[1] = 0;
		copy[2] = 0;

		RunProgram(copy, numCount);
		var zero = copy[0];

		nums.CopyTo(copy);
		copy[1] = 1;
		copy[2] = 0;

		RunProgram(copy, numCount);
		var one = copy[0];

		var perNoun = one - zero;
		var noun = (19690720 - zero) / perNoun;
		var verb = 19690720 - (zero + (perNoun * noun));

		var part2 = ((noun * 100) + verb).ToString();
		return (part1, part2);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void RunProgram(Span<int> instructions, int instructionCount)
	{
		var ip = 0;
		while (ip < instructionCount && instructions[ip] != 99)
		{
			var num1 = instructions[instructions[ip + 1]];
			var num2 = instructions[instructions[ip + 2]];
			var res = instructions[ip] switch
			{
				1 => num1 + num2,
				2 => num1 * num2,
				_ => 0,
			};
			instructions[instructions[ip + 3]] = res;

			ip += 4;
		}
	}
}
