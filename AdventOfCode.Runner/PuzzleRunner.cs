using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
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

	private static readonly Assembly[] s_assemblies =
	[
		Assembly.GetAssembly(typeof(Puzzles._2015.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2016.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2017.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2018.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2019.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2020.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2021.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2022.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2023.Day_01_Original))!,
		Assembly.GetAssembly(typeof(Puzzles._2024.Day_01_Original))!,
	];

	public PuzzleRunner()
	{
		_puzzles = GetAllPuzzles();
		_runMethod = GetType().GetMethod(nameof(RunPuzzle), BindingFlags.Static | BindingFlags.NonPublic)!;
		_benchmarkClass = typeof(PuzzleBenchmarkRunner<>);
	}

	public IReadOnlyCollection<PuzzleModel> Puzzles => _puzzles;

	public IEnumerable<PuzzleResult> RunPuzzles(IEnumerable<PuzzleModel> puzzles) =>
		puzzles
			.Select(puzzle =>
			{
				var method = _runMethod.MakeGenericMethod(puzzle.PuzzleType);
				return (PuzzleResult)method.Invoke(null, [puzzle])!;
			});

	public Summary BenchmarkPuzzles(IEnumerable<PuzzleModel> puzzles)
	{
		var types = puzzles
			.Select(p => _benchmarkClass.MakeGenericType(p.PuzzleType))
			.ToArray();

		return BenchmarkSwitcher.FromTypes(types)
			.RunAllJoined(
				DefaultConfig.Instance
					.WithOrderer(new TypeOrderer()));
	}

	private sealed class TypeOrderer : DefaultOrderer, IOrderer
	{
		public override IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary) =>
			benchmarksCases
				.OrderBy(c => c.Descriptor.Type.FullName)
				.ThenBy(c => c.Descriptor.MethodIndex);
	}

	private static List<PuzzleModel> GetAllPuzzles()
	{
		var c = s_assemblies
			.SelectMany(
				assembly => assembly.GetTypes(),
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
				x.PuzzleAttribute.CodeType,
				x.Type))
			.ToList();
	}

	private static PuzzleResult RunPuzzle<TPuzzle>(PuzzleModel puzzleInfo)
		where TPuzzle : IPuzzle, new()
	{
		var puzzle = new TPuzzle();

		var rawInput = PuzzleInputProvider.Instance
			.GetRawInput(puzzleInfo.Year, puzzleInfo.Day);

		var sw = Stopwatch.StartNew();
		var (part1, part2) = puzzle.Solve(rawInput);
		sw.Stop();
		var elapsed = sw.Elapsed;

		// run twice to get better timings
		if (elapsed < TimeSpan.FromMilliseconds(500))
		{
			sw.Restart();
			_ = puzzle.Solve(rawInput);
			sw.Stop();
			elapsed = sw.Elapsed;
		}

		return new PuzzleResult(puzzleInfo, part1, part2, elapsed);
	}
}
