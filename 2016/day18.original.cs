using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2016_18_Original : Day
	{
		public override int Year => 2016;
		public override int DayNumber => 18;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			ExecutePart(input, 40, 'A');
			ExecutePart(input, 400000, 'B');

		}

		private void ExecutePart(byte[] input, int rows, char part)
		{
			var tiles = new List<IList<bool>>();
			tiles.Add(input.Select(c => c == '^').ToArray());

			while (tiles.Count < rows)
			{
				var row = tiles[tiles.Count - 1];
				var newRow = new bool[row.Count];

				for (int i = 0; i < row.Count; i++)
				{
					var left = i > 0 ? row[i - 1] : false;
					var right = i < row.Count - 1 ? row[i + 1] : false;
					newRow[i] = left ^ right;
				}

				tiles.Add(newRow);
			}

			Dump(part, tiles.SelectMany(x => x).Where(b => !b).Count());
		}
	}
}
