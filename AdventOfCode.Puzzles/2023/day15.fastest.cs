using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using CommunityToolkit.HighPerformance;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 15, CodeType.Fastest)]
public sealed partial class Day_15_Fastest : IPuzzle
{
	private const uint KeyMask = (1u << 28) - 1;

	public (string, string) Solve(PuzzleInput input)
	{
		Span<uint> lenses =
			stackalloc uint[256 * 8];

		var span = input.Span;
		var part1 = 0;
		foreach (var step in span[..^1].Tokenize((byte)','))
		{
			var hash = 0u;
			var key = 0u;

			static uint Hash(uint hash, byte b) =>
				(hash + b) * 17;

			for (var i = 0; i < step.Length; i++)
			{
				var box = (int)(hash % 256);
				hash = Hash(hash, step[i]);

				static int FindKey(Span<uint> lenses, int @base, uint key)
				{
					var vec = Vector256.LoadUnsafe(ref lenses[@base]);
					vec = Vector256.BitwiseAnd(vec, Vector256.Create(KeyMask));
					vec = Vector256.Equals(vec, Vector256.Create(key));

					return BitOperations.TrailingZeroCount(
						vec.ExtractMostSignificantBits());
				}

				if (step[i] is (byte)'-')
				{
					var @base = box * 8;

					var idx = FindKey(lenses, @base, key);

					if (idx < 32)
					{
						var len = 8 - (idx + 1);
						lenses.Slice(@base + idx + 1, len)
							.CopyTo(lenses[(@base + idx)..]);
					}

					break;
				}
				else if (step[i] is (byte)'=')
				{
					hash = Hash(hash, step[i + 1]);
					var length = (uint)(step[i + 1] - '0') << 28;

					var @base = box * 8;

					var idx = FindKey(lenses, @base, key);

					if (idx == 32)
					{
						idx = FindKey(lenses, @base, 0);
					}

					lenses[@base + idx] = key | length;
					break;
				}
				else
				{
					key = (key << 4) + (uint)(step[i] - 'a');
				}
			}

			part1 += (int)(hash % 256);
		}

		var part2 = Vector256<int>.Zero;
		var slotAdd = Vector256.Create(1, 2, 3, 4, 5, 6, 7, 8);
		var slotVec = slotAdd;
		var lensesVec = MemoryMarshal.Cast<uint, Vector256<uint>>(lenses);
		foreach (var l in lensesVec)
		{
			part2 += Vector256.Multiply(
				Vector256.ShiftRightLogical(l, 28).AsInt32(),
				slotVec);
			slotVec += slotAdd;
		}

		return (part1.ToString(), Vector256.Sum(part2).ToString());
	}
}
