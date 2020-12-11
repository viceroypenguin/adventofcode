using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_11_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 11;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var map = input.GetString();
			var width = map.IndexOf('\n');

			PartA = RunPart(map, width, true);
			PartB = RunPart(map, width, false);
		}

		private static string RunPart(string map, int width, bool immediate)
		{
			while (true)
			{
				var nextMap = RunStep(map, width, immediate);

				if (map == nextMap)
					break;

				map = nextMap;
			}

			return map.Count(c => c == '#').ToString();
		}

		private static readonly (int yadj, int xadj)[] dirs = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1), };
		private static string RunStep(string map, int width, bool immediate)
		{
			Span<char> newMap = stackalloc char[map.Length];
			for (int y = 0; y < map.Length; y += width + 1)
				for (int x = 0; x < width + 1; x++)
				{
					if (map[y + x] == '.')
					{
						newMap[y + x] = '.';
						continue;
					}
					if (map[y + x] == '\n')
					{
						newMap[y + x] = '\n';
						continue;
					}

					var cnt = 0;

					foreach (var (yadj, xadj) in dirs)
					{
						int _y = y, _x = x;

						do
						{
							_y += (width + 1) * yadj;
							if (!_y.Between(0, map.Length - (width + 1)))
								break;
							_x += xadj;
							if (!_x.Between(0, width))
								break;

							if (map[_y + _x] == '#')
							{
								cnt++;
								break;
							}

							if (map[_y + _x] == 'L')
								break;
						}
						while (!immediate);
					}

					newMap[y + x] = (map[y + x], immediate, cnt) switch
					{
						('L', _, 0) => '#',
						('#', true, >= 4) => 'L',
						('#', false, >= 5) => 'L',
						(var c, _, _) => c,
					};
				}

			return new string(newMap);
		}
	}
}
