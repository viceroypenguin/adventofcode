using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2019_13_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 13;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var instructions = input.GetString()
				.Split(',')
				.Select(long.Parse)
				.ToArray();

			var screenOffset = 639;
			var screenHeight = Math.Max(instructions[604], instructions[605]);
			var screenSize = Math.Max(instructions[620], instructions[621]);
			var screenWidth = screenSize / screenHeight;

			var scoreOffset = instructions[632];
			var magicA = Math.Max(instructions[612], instructions[613]);
			var magicB = Math.Max(instructions[616], instructions[617]);

			long numBlocks = 0, score = 0;
			for (int y = 0; y < screenHeight; y++)
				for (int x = 0; x < screenWidth; x++)
					if (instructions[screenOffset + y * screenWidth + x] == 2)
					{
						numBlocks++;
						score += instructions[scoreOffset + (((x * screenHeight + y) * magicA + magicB) % screenSize)];
					}

			PartA = numBlocks.ToString();
			PartB = score.ToString();
		}
	}
}
