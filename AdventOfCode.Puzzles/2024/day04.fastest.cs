using System.Numerics;
using System.Runtime.Intrinsics;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 04, CodeType.Fastest)]
public partial class Day_04_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var arr = new byte[input.Bytes.Length + (Vector256<byte>.Count * 2)];
		input.Bytes.CopyTo(arr, 0);
		ReadOnlySpan<byte> span = arr;

		var width = span.IndexOf((byte)'\n') + 1;
		var i = 0;
		var part1 = 0;
		var part2 = 0;

		while (i < span.Length)
		{
			var vec = Vector256.LoadUnsafe(in span[i]);

			var isM = Vector256.Equals(vec, Vector256.Create((byte)'X'));
			var isS = Vector256.Equals(vec, Vector256.Create((byte)'S'));

			// right
			part1 += CheckXmas(span, i, 1, isM, isS);
			// down
			part1 += CheckXmas(span, i, width, isM, isS);
			// down-left
			part1 += CheckXmas(span, i, width - 1, isM, isS);
			// down-right
			part1 += CheckXmas(span, i, width + 1, isM, isS);

			i += Vector256<byte>.Count;
		}

		i = width + 1;
		while (i < span.Length)
		{
			part2 += CheckXMas(span, i, width);
			i += Vector256<byte>.Count;
		}

		return (part1.ToString(), part2.ToString());
	}

	private static int CheckXmas(ReadOnlySpan<byte> span, int i, int stride, Vector256<byte> forward, Vector256<byte> backward)
	{
		if (i + stride + stride + stride + Vector256<byte>.Count - 1 >= span.Length)
			return 0;

		i += stride;
		var vec = Vector256.LoadUnsafe(in span[i]);
		forward &= Vector256.Equals(vec, Vector256.Create((byte)'M'));
		backward &= Vector256.Equals(vec, Vector256.Create((byte)'A'));

		i += stride;
		vec = Vector256.LoadUnsafe(in span[i]);
		forward &= Vector256.Equals(vec, Vector256.Create((byte)'A'));
		backward &= Vector256.Equals(vec, Vector256.Create((byte)'M'));

		i += stride;
		vec = Vector256.LoadUnsafe(in span[i]);
		forward &= Vector256.Equals(vec, Vector256.Create((byte)'S'));
		backward &= Vector256.Equals(vec, Vector256.Create((byte)'X'));

		return BitOperations.PopCount(forward.ExtractMostSignificantBits())
			+ BitOperations.PopCount(backward.ExtractMostSignificantBits());
	}

	private static int CheckXMas(ReadOnlySpan<byte> span, int i, int stride)
	{
		if (i < stride + 1 || i + stride + 1 + Vector256<byte>.Count - 1 > span.Length)
			return 0;

		var isA = Vector256.Equals(Vector256.LoadUnsafe(in span[i]), Vector256.Create((byte)'A'));

		var upLeft = Vector256.LoadUnsafe(in span[i - stride - 1]);
		var upRight = Vector256.LoadUnsafe(in span[i - stride + 1]);
		var downLeft = Vector256.LoadUnsafe(in span[i + stride - 1]);
		var downRight = Vector256.LoadUnsafe(in span[i + stride + 1]);

		var forward =
			(Vector256.Equals(upLeft, Vector256.Create((byte)'M')) & Vector256.Equals(downRight, Vector256.Create((byte)'S')))
			| (Vector256.Equals(upLeft, Vector256.Create((byte)'S')) & Vector256.Equals(downRight, Vector256.Create((byte)'M')));
		var backward =
			(Vector256.Equals(upRight, Vector256.Create((byte)'M')) & Vector256.Equals(downLeft, Vector256.Create((byte)'S')))
			| (Vector256.Equals(upRight, Vector256.Create((byte)'S')) & Vector256.Equals(downLeft, Vector256.Create((byte)'M')));

		var valid = isA & forward & backward;
		return BitOperations.PopCount(valid.ExtractMostSignificantBits());
	}
}
