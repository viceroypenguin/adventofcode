using System.Runtime.CompilerServices;
using AdventOfCode.Runner;
using DocoptNet;
using Spectre.Console;

var runner = new PuzzleRunner();

if (args.Length > 0)
{
	static int ShowHelp(string help) { Console.WriteLine(help); return 0; }

	static int OnError(string error) { Console.Error.WriteLine(error); return 1; }

	return ProgramArguments.CreateParser()
		.Parse(args) switch
	{
		IArgumentsResult<ProgramArguments> { Arguments: var arguments } => Main(arguments),
		IHelpResult { Help: var help } => ShowHelp(help),
		IInputErrorResult { Error: var error } => OnError(error),
		var result => throw new SwitchExpressionException(result),
	};

	int Main(ProgramArguments arguments)
	{
		var puzzles = runner.Puzzles.AsEnumerable();

		if (arguments.OptOriginal)
			puzzles = puzzles.Where(p => p.CodeType == CodeType.Original);
		else if (arguments.OptFastest)
			puzzles = puzzles.Where(p => p.CodeType == CodeType.Fastest);

		if (arguments.OptYear != null)
		{
			puzzles = puzzles.Where(p => p.Year == Convert.ToInt32(arguments.OptYear));
			if (arguments.OptDay != null)
				puzzles = puzzles.Where(p => p.Day == Convert.ToInt32(arguments.OptDay));
		}

		if (arguments.OptBenchmark)
		{
			_ = runner.BenchmarkPuzzles(puzzles);
		}
		else
		{
			RunPuzzles(
				puzzles
					.OrderBy(p => p.Year)
					.ThenBy(p => p.Day)
					.ThenBy(p => p.CodeType == CodeType.Original ? 0 : 1)
					.ToList(),
				arguments.OptSilent
			);
		}

		return 0;
	}
}
else
{
	var console = AnsiConsole.Create(new AnsiConsoleSettings());
	var font = FigletFont.Default;
	var f = new FigletText(font, "Advent of Code")
	{
		Color = ConsoleColor.Green,
	};

	console.Write(f);

	var puzzles = runner.Puzzles;

	var years = puzzles.Select(x => x.Year).Distinct().Order().ToList();

	if (years.Count == 0)
	{
		console.Markup("Could not find any puzzles. Exiting.");
		return 0;
	}

	(var year, puzzles) = PickYear(puzzles, years);

	console.MarkupLineInterpolated($"Running year [red]{year}[/].");

	puzzles = PickPuzzles(puzzles, year);

	console.MarkupLineInterpolated($"Running puzzle(s) [red]{string.Join(", ", puzzles.Select(x => x.Day))}[/].");

	var doBenchmark = console.Confirm("Do you want to benchmark?", false);

	if (doBenchmark)
	{
		runner.BenchmarkPuzzles(puzzles);
	}
	else
	{
		RunPuzzles(puzzles, silent: false);
	}

	(int, IReadOnlyCollection<PuzzleModel>) PickYear(IReadOnlyCollection<PuzzleModel> puzzles, IReadOnlyList<int> years)
	{
		var year = years[^1];

		if (years.Count > 1)
		{
			year = console.Prompt(
				new SelectionPrompt<int>()
					.Title("Which [green]year[/] do you want to execute?")
					.PageSize(20)
					.AddChoices(years));
		}

		return (year, puzzles.Where(x => x.Year == year).ToList());
	}

	IReadOnlyCollection<PuzzleModel> PickPuzzles(IReadOnlyCollection<PuzzleModel> puzzles, int year)
	{
		var selectedDays = console.Prompt(
			new MultiSelectionPrompt<int>()
				.Title("Which [green]year[/] do you want to execute?")
				.PageSize(20)
				.MoreChoicesText("[grey](Move up and down to reveal more days)[/]")
				.InstructionsText(
					"[grey](Press [blue]<space>[/] to toggle a day, " +
					"[green]<enter>[/] to accept)[/]")
				.AddChoiceGroup(
					year,
					puzzles.Select(x => x.Day).Distinct().Order()));

		return puzzles
			.Where(p => selectedDays.Contains(p.Day))
			.OrderBy(p => p.Day)
			.ThenBy(p => p.CodeType == CodeType.Original ? 0 : 1)
			.ToList();
	}

	return 0;
}

void RunPuzzles(IReadOnlyCollection<PuzzleModel> puzzles, bool silent)
{
	if (silent)
	{
		runner.RunPuzzles(puzzles).Consume();
		return;
	}

	var console = AnsiConsole.Create(new AnsiConsoleSettings());
	var rootTable = new Table().Expand()
		.AddColumn("Dummy")
		.HideHeaders()
		.NoBorder();

	var outputTable = new Table().Expand()
		.HorizontalBorder()
		.AddColumn("Problem")
		.AddColumn("Part 1")
		.AddColumn("Part 2")
		.AddColumn("Solve Time", tc => tc.RightAligned())
		.ShowFooters();

	_ = rootTable.AddRow(outputTable);

	console.Live(rootTable)
		.Start(ctx =>
		{
			var totalSolve = TimeSpan.Zero;
			foreach (var r in runner.RunPuzzles(puzzles))
			{
				_ = outputTable.AddRow(
					new Markup($"{r.Puzzle.Year} - {r.Puzzle.Day} - {r.Puzzle.CodeType}"),
					new Markup($"[red]{r.Part1}[/]"),
					new Markup($"[red]{r.Part2}[/]"),
					new Markup($"[blue]{r.Elapsed.TotalMicroseconds:N0}[/]μs"));

				totalSolve += r.Elapsed;
				outputTable.Columns[3].Footer = new Markup($"[blue]{totalSolve.TotalMicroseconds:N0}[/]μs");

				ctx.Refresh();
			}
		});
}
