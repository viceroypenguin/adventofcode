using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Runner;

[HtmlExporter, MarkdownExporter]
[MemoryDiagnoser(false)]
public class PuzzleBenchmarkRunner<TPuzzle, TParsed>
	where TPuzzle : IPuzzle<TParsed>, new()
{
	private readonly TPuzzle _puzzle = new();
	private PuzzleInput? _rawInput;
	private TParsed? _parsed;

	[GlobalSetup]
	public void Setup()
	{
		var attribute = typeof(TPuzzle).GetCustomAttribute<PuzzleAttribute>()!;
		_rawInput = PuzzleInputProvider.Instance
			.GetRawInput(attribute.Year, attribute.Day);
		_parsed = _puzzle.Parse(_rawInput);
	}

	[Benchmark, BenchmarkCategory("Parse")]
	public TParsed Parse() => _puzzle.Parse(_rawInput!);

	[Benchmark, BenchmarkCategory("Part1")]
	public string Part1() => _puzzle.Part1(_parsed!);

	[Benchmark, BenchmarkCategory("Part2")]
	public string Part2() => _puzzle.Part2(_parsed!);
}
