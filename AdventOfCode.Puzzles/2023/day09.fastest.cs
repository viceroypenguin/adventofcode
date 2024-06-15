using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 09, CodeType.Fastest)]
public sealed partial class Day_09_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;
		var part1 = 0;
		var part2 = 0;

		Span<int> array = stackalloc int[32];

		foreach (var l in span.EnumerateLines())
		{
			if (l.Length == 0)
				break;

			var j = 0;
			var line = l;
			while (true)
			{
				(array[j++], var n) = line.AtoI();
				if (n == line.Length) break;
				line = line[(n + 1)..];
			}

			var ints = array[..j];
			part1 += ints[j - 1];
			part2 += ints[0];
			var isNeg = true;
			while (true)
			{
				ref var intsRef = ref MemoryMarshal.GetReference(ints);
				ref var intsOneRef = ref MemoryMarshal.GetReference(ints[1..]);

				nuint vectorEnd = (uint)(ints.Length - 1);
				if (vectorEnd >= (uint)(Vector128<int>.Count + 1))
				{
					vectorEnd -= vectorEnd % (uint)Vector128<int>.Count;

					for (nuint k = 0; k < vectorEnd; k += (uint)Vector128<int>.Count)
					{
						var result = Vector128.LoadUnsafe(ref intsOneRef, k)
							- Vector128.LoadUnsafe(ref intsRef, k);
						result.StoreUnsafe(ref intsRef, k);
					}
				}
				else
				{
					vectorEnd = 0;
				}

				for (var k = (int)vectorEnd; k < ints.Length - 1; k++)
					ints[k] = ints[k + 1] - ints[k];

				ints = ints[..^1];

				part1 += ints[^1];
				part2 += isNeg ? -ints[0] : +ints[0];
				isNeg = !isNeg;

				if (ints[0] == ints[^1])
					break;
			}
		}

		return (part1.ToString(), part2.ToString());
	}
}
