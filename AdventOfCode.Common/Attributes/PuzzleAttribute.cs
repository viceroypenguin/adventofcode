namespace AdventOfCode.Common.Attributes;

public enum CodeType
{
	Original,
	Fastest,
}

[AttributeUsage(AttributeTargets.Class)]
public class PuzzleAttribute : Attribute
{
	public string? Name { get; }
	public int Year { get; }
	public int Day { get; }
	public CodeType CodeType { get; }

	public PuzzleAttribute(int year, int day, CodeType codeType, string? name = null)
	{
		Year = year;
		Day = day;
		CodeType = codeType;
		Name = name;
	}
}
