using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using AdventOfCode.Common.Interfaces;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace AdventOfCode.Runner;

public class PuzzleRunner
{
	private readonly IReadOnlyList<PuzzleModel> _puzzles;
	private readonly MethodInfo _runMethod;
	private readonly Type _benchmarkClass;

	private static readonly Assembly[] assemblies =
	{
		Assembly.GetAssembly(typeof(Puzzles._2022.Day_01_Original))!,
	};

	public PuzzleRunner()
	{
		_puzzles = GetAllPuzzles();
		_runMethod = GetType().GetMethod(nameof(RunPuzzle), BindingFlags.Static | BindingFlags.NonPublic)!;
		_benchmarkClass = typeof(PuzzleBenchmarkRunner<,>);
	}

	public IReadOnlyCollection<PuzzleModel> GetPuzzles() => _puzzles;

	public IEnumerable<PuzzleResult> RunPuzzles(IEnumerable<PuzzleModel> puzzles) =>
		puzzles
			.Select(puzzle =>
			{
				var method = _runMethod.MakeGenericMethod(puzzle.PuzzleType, puzzle.ParsedType);
				return (PuzzleResult)method.Invoke(null, new object[] { puzzle })!;
			});

	public Summary BenchmarkPuzzles(IEnumerable<PuzzleModel> puzzles)
	{
		var types = puzzles
			.Select(p => _benchmarkClass.MakeGenericType(p.PuzzleType, p.ParsedType))
			.ToArray();

		return BenchmarkSwitcher.FromTypes(types)
			.RunAllJoined(
				DefaultConfig.Instance
					.WithOrderer(new TypeOrderer()));
	}

	private class TypeOrderer : DefaultOrderer, IOrderer
	{
		public override IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary) =>
			benchmarksCases
				.OrderBy(c => c.Descriptor.Type.FullName)
				.ThenBy(c => c.Descriptor.MethodIndex);
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
}
