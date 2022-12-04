using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Runner;

[MemoryDiagnoser(false)]
public class PuzzleBenchmarkRunner<TPuzzle>
	where TPuzzle : IPuzzle, new()
{
	private readonly TPuzzle _puzzle = new();
	private PuzzleInput? _rawInput;

	[GlobalSetup]
	public void Setup()
	{
		var attribute = typeof(TPuzzle).GetCustomAttribute<PuzzleAttribute>()!;
		_rawInput = BenchmarkInputProvider.GetRawInput(attribute.Year, attribute.Day);
	}

	[Benchmark]
	public (string, string) Solve() => _puzzle.Solve(_rawInput!);
}
