namespace AdventOfCode.Common.Models;

public record PuzzleInput(byte[] Bytes, string Text, string[] Lines)
{
	public ReadOnlySpan<byte> Span => Bytes;
}
