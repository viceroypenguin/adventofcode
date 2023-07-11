using System.Collections.Immutable;
using System;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 19, CodeType.Original)]
public partial class Day_19_Original : IPuzzle
{
	[GeneratedRegex(@"^(((\w+) => (\w+))|(\w+))$")]
	private static partial Regex TransformationRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = TransformationRegex();

		var matches = input.Lines
			.Select(s => regex.Match(s))
			.ToList();

		var transformations = matches
			.Where(m => m.Groups[2].Success)
			.Select(m => new Transformation
			{
				Source = m.Groups[3].Value,
				Result = m.Groups[4].Value,
			})
			.ToList();

		var molecule = matches
			.Where(m => m.Groups[5].Success)
			.Select(m => m.Groups[5].Value)
			.Single();

		var partA =
			transformations
				.SelectMany(t => Regex.Matches(molecule, t.Source)
					.OfType<Match>()
					.Select(m => string.Concat(molecule.AsSpan()[..m.Index], t.Result, molecule.AsSpan(m.Index + m.Length))))
				.Distinct()
				.Count();

		var partB =
			GetTransformations(
				molecule,
				transformations,
				ImmutableStack<TransformationResult>.Empty
					.Push(new TransformationResult()
					{
						Source = "",
						Result = "",
						Molecule = molecule,
					}))
				.Count() - 1;

		return (partA.ToString(), partB.ToString());
	}

	private sealed class Transformation
	{
		public string Source { get; set; }
		public string Result { get; set; }
	}

	private sealed class TransformationResult
	{
		public string Source { get; set; }
		public string Result { get; set; }
		public string Molecule { get; set; }
	}

	private static ImmutableStack<TransformationResult> GetTransformations(string molecule, List<Transformation> language, ImmutableStack<TransformationResult> stack)
	{
		foreach (var opt in language)
		{
			var idx = molecule.IndexOf(opt.Result);
			if (idx < 0)
				continue;

			var newMolecule = string.Concat(molecule.AsSpan()[..idx], opt.Source, molecule.AsSpan(idx + opt.Result.Length));
			var newStack = stack.Push(new TransformationResult
			{
				Source = opt.Source,
				Result = opt.Result,
				Molecule = newMolecule,
			});
			if (newMolecule == "e")
				return newStack;

			newStack = GetTransformations(newMolecule, language, newStack);
			if (newStack != null)
				return newStack;
		}

		return null;
	}
}
