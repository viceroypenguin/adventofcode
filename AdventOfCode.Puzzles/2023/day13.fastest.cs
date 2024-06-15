using System.Numerics;
using System.Runtime.Intrinsics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 13, CodeType.Fastest)]
public sealed partial class Day_13_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		var part1 = 0;
		var part2 = 0;
		while (span.Length > 0)
		{
			var n = span.IndexOf("\n\n"u8);
			var pattern = n > 0 ? span[..n] : span[..^1];
			span = n > 0 ? span[(n + 2)..] : [];

			var (p1, p2) = FindReflections(pattern);
			part1 += p1;
			part2 += p2;
		}

		return (part1.ToString(), part2.ToString());
	}

	private static (int part1, int part2) FindReflections(ReadOnlySpan<byte> pattern)
	{
		Span<uint> rows = stackalloc uint[24];
		Span<uint> cols = stackalloc uint[24];

		var width = pattern.IndexOf((byte)'\n');
		var height = pattern.Count((byte)'\n') + 1;

		ExtractRocks(pattern, rows, cols, width);
		rows = rows[..height];
		cols = cols[..width];

		var p1 = FindReflection(cols);
		if (p1 < 0)
			p1 = FindReflection(rows) * 100;

		var p2 = FindSmudgeReflection(cols);
		if (p2 < 0)
			p2 = FindSmudgeReflection(rows) * 100;

		return (p1, p2);
	}

	private static void ExtractRocks(ReadOnlySpan<byte> pattern, Span<uint> rows, Span<uint> cols, int width)
	{
		var row = 0;
		foreach (var l in pattern.EnumerateLines(width))
		{
			// technically _really_ unsafe; could pull from past end of `input.Bytes`
			// _shouldn't_ matter unless `input.Bytes` ends too close to end of a page
			// definitely do _NOT_ do this in a real application
			var vec = Vector256.LoadUnsafe(in l[0]);
			var tr = Vector256.Equals(vec, Vector256.Create((byte)'#'))
				.ExtractMostSignificantBits();
			tr &= (uint)(1 << width) - 1;

			rows[row] = tr;

			var bit = 1u << row;
			while (tr != 0)
			{
				cols[BitOperations.TrailingZeroCount(tr)] |= bit;
				tr ^= tr & (uint)-tr;
			}

			row++;
		}
	}

	private static int FindReflection(Span<uint> data)
	{
		var cnt = (uint)data.Length;
		var prev = data[0];
		for (var i = 1; i < cnt; i++)
		{
			var cur = data[i];
			if (cur == prev
				&& VerifyReflection(data, i))
			{
				return i;
			}

			prev = cur;
		}

		return -1;
	}

	private static bool VerifyReflection(Span<uint> data, int i)
	{
		var max = Math.Min(i, data.Length - i);
		for (var j = 1; j < max; j++)
		{
			if (data[i - j - 1] != data[i + j])
				return false;
		}

		return true;
	}

	private static int FindSmudgeReflection(Span<uint> data)
	{
		var cnt = (uint)data.Length;
		var prev = data[0];
		for (var i = 1; i < cnt; i++)
		{
			var cur = data[i];
			var xor = cur ^ prev;
			if ((xor == 0 || BitOperations.IsPow2(xor))
				&& VerifySmudgeReflection(data, i, xor != 0))
			{
				return i;
			}

			prev = cur;
		}

		return -1;
	}

	private static bool VerifySmudgeReflection(Span<uint> data, int i, bool smudge)
	{
		var max = Math.Min(i, data.Length - i);
		for (var j = 1; j < max; j++)
		{
			var xor = data[i - j - 1] ^ data[i + j];
			if (xor != 0)
			{
				if (smudge || !BitOperations.IsPow2(xor))
					return false;

				smudge = true;
			}
		}

		return smudge;
	}
}
