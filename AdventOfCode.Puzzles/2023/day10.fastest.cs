using System.Buffers;
using System.Runtime.Intrinsics.X86;
using CommunityToolkit.HighPerformance;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 10, CodeType.Fastest)]
public sealed partial class Day_10_Fastest : IPuzzle
{
	private static readonly SearchValues<byte> s_nextInterest =
		SearchValues.Create("|F7\n"u8);

	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		var loopLength = span.Length.DivRoundUp(64);
		Span<ulong> loop = stackalloc ulong[loopLength];

		static void SetIsLoop(Span<ulong> loop, int position)
		{
			var idx = position / 64;
			var bit = 1ul << (position % 64);
			loop[idx] |= bit;
		}

		var width = span.IndexOf((byte)'\n') + 1;
		var startIndex = input.Span.IndexOf((byte)'S');
		SetIsLoop(loop, startIndex);

		var (p, pp) =
			span[startIndex + 1] is (byte)'-' or (byte)'J' or (byte)'7'
				? (startIndex + 1, '├') :
			span[startIndex + width] is (byte)'|' or (byte)'J' or (byte)'L'
				? (startIndex + width, '┬') :
				(startIndex - 1, '┤');

		var part1 = 1;
		while (p != startIndex)
		{
			SetIsLoop(loop, p);
			part1++;
			(p, pp) = ((char)span[p], pp) switch
			{
				('-', '├') => (p + 1, '├'),
				('-', '┤') => (p - 1, '┤'),
				('|', '┬') => (p + width, '┬'),
				('|', '┴') => (p - width, '┴'),
				('J', '├') => (p - width, '┴'),
				('J', '┬') => (p - 1, '┤'),
				('F', '┤') => (p + width, '┬'),
				('F', '┴') => (p + 1, '├'),
				('7', '├') => (p + width, '┬'),
				('7', '┴') => (p - 1, '┤'),
				('L', '┬') => (p + 1, '├'),
				('L', '┤') => (p - width, '┴'),
				_ => throw new InvalidOperationException($"('{(char)span[p]}', '{pp}')"),
			};
		}

		part1 /= 2;

		static bool GetIsLoop(Span<ulong> loop, int position)
		{
			var idx = position / 64;
			var bit = 1ul << (position % 64);
			return (loop[idx] & bit) != 0;
		}

		static int GetIsLoopPopCnt(Span<ulong> loop, int start, int length)
		{
			if (length == 0) return 0;

			var cnt = 0ul;
			var idx = start / 64;

			{
				start %= 64;
				var firstLength = Math.Min(64 - start, length);
				length -= firstLength;
				cnt += Popcnt.X64.PopCount(
					Bmi1.X64.BitFieldExtract(~loop[idx], (byte)start, (byte)firstLength));
				idx++;
			}

			while (length > 64)
			{
				cnt += Popcnt.X64.PopCount(
					Bmi1.X64.BitFieldExtract(~loop[idx], 0, 64));
				length -= 64;
				idx++;
			}

			if (length > 0)
			{
				cnt += Popcnt.X64.PopCount(
					Bmi1.X64.BitFieldExtract(~loop[idx], 0, (byte)length));
			}

			return (int)cnt;
		}

		var part2 = 0;
		var isInLoop = false;
		for (p = 0; p < span.Length;)
		{
			var n = span[p..].IndexOfAny(s_nextInterest);

			if (isInLoop)
				part2 += GetIsLoopPopCnt(loop, p, n);

			p += n;

			if (!GetIsLoop(loop, p))
			{
				if (isInLoop && span[p] != '\n')
					part2++;
			}
			else
			{
				isInLoop = !isInLoop;
			}

			p++;

		}

		return (part1.ToString(), part2.ToString());
	}
}
