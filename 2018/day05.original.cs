using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2018_05_Original : Day
	{
		public override int Year => 2018;
		public override int DayNumber => 5;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var poly = input.GetString();

			int GetReducedPolymerLength(string polymer)
			{
				var characters = polymer
					.Select(c => (c, isActive: true))
					.ToArray();

				for (int i = 1; i < characters.Length; i++)
				{
					int j = i - 1;
					while (j >= 0 && !characters[j].isActive)
						j--;
					if (j >= 0 &&
						char.IsUpper(characters[i].c) != char.IsUpper(characters[j].c) &&
						char.ToUpper(characters[i].c) == char.ToUpper(characters[j].c))
					{
						characters[i].isActive = false;
						characters[j].isActive = false;
					}
				}

				return characters.Where(x => x.isActive).Count();
			}

			Dump('A', GetReducedPolymerLength(poly));

			Dump('B',
				Enumerable.Range(0, 26)
					.Select(i => (char)(i + (int)'a'))
					.Select(c => Regex.Replace(poly, c.ToString(), "", RegexOptions.IgnoreCase))
					.Min(s => GetReducedPolymerLength(s)));
		}
	}
}
