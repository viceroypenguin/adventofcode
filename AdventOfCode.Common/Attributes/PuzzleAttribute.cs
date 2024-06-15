namespace AdventOfCode.Common.Attributes;

public enum CodeType
{
	Original,
	Fastest,
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class PuzzleAttribute(int year, int day, CodeType codeType, string? name = null) : Attribute
{
	public string? Name { get; } = name;
	public int Year { get; } = year;
	public int Day { get; } = day;
	public CodeType CodeType { get; } = codeType;
}
