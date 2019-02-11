using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2016_02_Original : Day
	{
		public override int Year => 2016;
		public override int DayNumber => 2;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			DoPartA(input);
			DoPartB(input);
		}

		private void DoPartA(byte[] input)
		{
			var buttons = new[]
			{
				new [] { 1, 2, 3 },
				new [] { 4, 5, 6 },
				new [] { 7, 8, 9 },
			};

			var x = 1;
			var y = 1;

			var password = new List<int>();

			foreach (var line in input.GetLines())
			{
				foreach (var c in line.Trim())
				{
					switch (c)
					{
						case 'U':
							y = Math.Max(y - 1, 0);
							break;

						case 'D':
							y = Math.Min(y + 1, buttons.Length - 1);
							break;

						case 'L':
							x = Math.Max(x - 1, 0);
							break;

						case 'R':
							x = Math.Min(x + 1, buttons[0].Length - 1);
							break;
					}
				}

				password.Add(buttons[y][x]);
			}

			Dump('A', string.Join("", password));
		}

		private void DoPartB(byte[] input)
		{
			var buttons = new[]
			{
				new char? [] { null, null,  '1', null, null, },
				new char? [] { null,  '2',  '3',  '4', null, },
				new char? [] {  '5',  '6',  '7',  '8',  '9', },
				new char? [] { null,  'A',  'B',  'C', null, },
				new char? [] { null, null,  'D', null, null, },
			};

			var x = 0;
			var y = 2;

			var password = new List<char>();

			foreach (var line in input.GetLines())
			{
				foreach (var c in line.Trim())
				{
					switch (c)
					{
						case 'U':
							var newY = Math.Max(y - 1, 0);
							if (buttons[newY][x].HasValue)
								y = newY;
							break;

						case 'D':
							newY = Math.Min(y + 1, buttons.Length - 1);
							if (buttons[newY][x].HasValue)
								y = newY;
							break;

						case 'L':
							var newX = Math.Max(x - 1, 0);
							if (buttons[y][newX].HasValue)
								x = newX;
							break;

						case 'R':
							newX = Math.Min(x + 1, buttons[0].Length - 1);
							if (buttons[y][newX].HasValue)
								x = newX;
							break;
					}
				}

				password.Add(buttons[y][x].Value);
			}

			Dump('B', string.Join("", password));
		}
	}
}
