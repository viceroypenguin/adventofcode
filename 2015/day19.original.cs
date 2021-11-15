using System.Collections.Immutable;

namespace AdventOfCode;

public class Day_2015_19_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 19;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		var regex = new Regex(@"^(((\w+) => (\w+))|(\w+))$");

		var matches = input.GetLines()
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

		Dump('A',
			transformations
				.SelectMany(t => Regex.Matches(molecule, t.Source)
					.OfType<Match>()
					.Select(m => molecule.Substring(0, m.Index) + t.Result + molecule.Substring(m.Index + m.Length)))
				.Distinct()
				.Count());

		Dump('B',
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
				.Count());

	}

	class Transformation
	{
		public string Source { get; set; }
		public string Result { get; set; }
	}

	class TransformationResult
	{
		public string Source { get; set; }
		public string Result { get; set; }
		public string Molecule { get; set; }
	}

	ImmutableStack<TransformationResult> GetTransformations(string molecule, List<Transformation> language, ImmutableStack<TransformationResult> stack)
	{
		foreach (var opt in language)
		{
			var idx = molecule.IndexOf(opt.Result);
			if (idx < 0)
				continue;

			var newMolecule = molecule.Substring(0, idx) + opt.Source + molecule.Substring(idx + opt.Result.Length);
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
