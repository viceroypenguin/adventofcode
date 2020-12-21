using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;
using static AdventOfCode.Helpers;

namespace AdventOfCode
{
	public class Day_2020_21_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 21;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var regex = new Regex(@"^((?<ingredient>\w+) )+\(contains ((?<allergen>\w+)(, )?)+\)$", RegexOptions.ExplicitCapture);
			var recipes = input.GetLines()
				.Select(l => regex.Match(l))
				.Select(m => (
					ingredients: m.Groups["ingredient"].Captures.Select(c => c.Value).ToList(),
					allergens: m.Groups["allergen"].Captures.Select(c => c.Value).ToList()))
				.ToList();

			var allergenMap = new Dictionary<string, string>();
			var allergens = recipes.SelectMany(r => r.allergens).Distinct().ToList();
			do
			{
				foreach (var a in allergens.ToList())
				{
					var candidates = recipes.First(r => r.allergens.Contains(a)).ingredients.ToHashSet();
					foreach (var r in recipes.Where(r => r.allergens.Contains(a)).Skip(1))
						candidates.IntersectWith(r.ingredients);
					if (candidates.Count == 1)
					{
						var i = allergenMap[a] = candidates.Single();
						foreach (var r in recipes)
							r.ingredients.Remove(i);
						allergens.Remove(a);
					}
				}
			} while (allergens.Count != 0);

			PartA = recipes.SelectMany(r => r.ingredients).Count().ToString();
			PartB = string.Join(",", allergenMap.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));
		}
	}
}
