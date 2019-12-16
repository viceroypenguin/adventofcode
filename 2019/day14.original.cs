using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2019_14_Original : Day
	{
		private const long OneTrillion = 1_000_000_000_000L;

		public override int Year => 2019;
		public override int DayNumber => 14;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var regex = new Regex(@"^((?<in>\d+ \w+),? )+=> (?<out>\d+ \w+)$", RegexOptions.ExplicitCapture);
			var materialRegex = new Regex(@"(\d+) (\w+)");

			(int amt, string mat) ParseMaterialDefinition(string definition)
			{
				var match = materialRegex.Match(definition);
				return (amt: int.Parse(match.Groups[1].Value), mat: match.Groups[2].Value);
			}

			var recipes = input.GetLines()
				.Select(r => regex.Match(r))
				.Select(m =>
				{
					var output = ParseMaterialDefinition(m.Groups["out"].Value);

					var inp = m.Groups["in"]
						.Captures
						.OfType<Capture>()
						.Select(c => ParseMaterialDefinition(c.Value))
						.ToList();
					return (inp, output);
				})
				.ToDictionary(x => x.output.mat);

			var ore = CalculateOreRequirement(recipes, (1, "FUEL"));
			PartA = ore.ToString();

			var guess = OneTrillion / ore;
			while (true)
			{
				ore = CalculateOreRequirement(recipes, (guess, "FUEL"));
				var newGuess = guess + guess * (OneTrillion - ore) / OneTrillion;
				if (newGuess == guess)
					break;
				guess = newGuess;
			}

			PartB = guess.ToString();
		}

		private static long CalculateOreRequirement(
			Dictionary<string, (List<(int amt, string mat)> inp, (int amt, string mat) output)> recipes, 
			(long, string) requirement)
		{
			var materials = new Queue<(long amt, string mat)>();
			materials.Enqueue(requirement);

			var excess = new Dictionary<string, long>();
			var ore = 0L;

			while (materials.Any())
			{
				var (amt, mat) = materials.Dequeue();
				if (mat == "ORE")
				{
					ore += amt;
					continue;
				}

				if (excess.TryGetValue(mat, out var exAmt))
				{
					var used = Math.Min(exAmt, amt);
					amt -= used;
					excess[mat] = exAmt - used;
				}

				if (amt == 0)
					continue;

				var recipe = recipes[mat];
				var factor = (amt - 1) / recipe.output.amt + 1;
				exAmt = (factor * recipe.output.amt) - amt;
				if (exAmt != 0)
					excess[mat] = exAmt;

				foreach (var (qAmt, qMat) in recipe.inp)
					materials.Enqueue((qAmt * factor, qMat));
			}

			return ore;
		}
	}
}
