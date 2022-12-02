using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Running;

namespace AdventOfCode.Runner;

public class PuzzleRunner
{
	private readonly IReadOnlyList<PuzzleModel> _puzzles;
	private readonly MethodInfo _runMethod;
	private readonly MethodInfo _benchmarkMethod;

	private static readonly Assembly[] assemblies =
	{
		Assembly.GetAssembly(typeof(Puzzles._2022.Day_01_Original))!,
	};

	public PuzzleRunner()
	{
		_puzzles = GetAllPuzzles();
		_runMethod = GetType().GetMethod(nameof(RunPuzzle), BindingFlags.Static | BindingFlags.NonPublic)!;
		_benchmarkMethod = GetType().GetMethod(nameof(RunBenchmark), BindingFlags.Static | BindingFlags.NonPublic)!;
	}

	private static IReadOnlyList<PuzzleModel> GetAllPuzzles()
	{
		var c = assemblies
			.SelectMany(
				assembly => assembly!.GetTypes(),
				(assembly, type) => new
				{
					Type = type,
					PuzzleAttribute = type.GetCustomAttribute<PuzzleAttribute>()!,
				})
			.Where(x => x.PuzzleAttribute != null);

		return c
			.Select(x => new PuzzleModel(
				x.PuzzleAttribute.Name,
				x.PuzzleAttribute.Year,
				x.PuzzleAttribute.Day,
				x.Type,
				x.Type.GetInterfaces()[0].GenericTypeArguments[0]))
			.ToList();
	}

	private static PuzzleResult RunPuzzle<TPuzzle, TParsed>(PuzzleModel puzzleInfo)
		where TPuzzle : IPuzzle<TParsed>, new()
	{
		var puzzle = new TPuzzle();

		var rawInput = PuzzleInputProvider.Instance
			.GetRawInput(puzzleInfo.Year, puzzleInfo.Day);

		var sw = Stopwatch.StartNew();
		var parsed = puzzle.Parse(rawInput);
		sw.Stop();
		var elapsedParse = sw.Elapsed;

		sw.Restart();
		var part1 = puzzle.Part1(parsed);
		sw.Stop();
		var elapsedPart1 = sw.Elapsed;

		sw.Restart();
		var part2 = puzzle.Part2(parsed);
		sw.Stop();
		var elapsedPart2 = sw.Elapsed;

		return new PuzzleResult(puzzleInfo, part1, part2, elapsedParse, elapsedPart1, elapsedPart2);
	}

	private static void RunBenchmark<TPuzzle, TParsed>()
		where TPuzzle : IPuzzle<TParsed>, new()
	{
		var summary = BenchmarkRunner.Run<PuzzleBenchmarkRunner<TPuzzle, TParsed>>();
		Console.WriteLine(summary.ToString());
	}
}
