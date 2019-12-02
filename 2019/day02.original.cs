using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2019_02_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 2;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var instructions = input.GetString()
				.Split(',')
				.Select(s => Convert.ToInt32(s))
				.ToList();

			var partA = new List<int>(instructions);
			partA[1] = 12;
			partA[2] = 2;

			RunProgram(partA);
			PartA = partA[0].ToString();

			for (int noun = 0; noun < 100; noun++)
				for (int verb = 0; verb < 100; verb++)
				{
					var partB = new List<int>(instructions);
					partB[1] = noun;
					partB[2] = verb;

					RunProgram(partB);
					if (partB[0] == 19690720)
					{
						PartB = (noun * 100 + verb).ToString();
						return;
					}
				}
		}

		static void RunProgram(List<int> instructions)
		{
			var ip = 0;
			while (ip < instructions.Count && instructions[ip] != 99)
			{
				var num1 = instructions[instructions[ip + 1]];
				var num2 = instructions[instructions[ip + 2]];
				var res = instructions[ip] switch
				{
					1 => num1 + num2,
					2 => num1 * num2,
					_ => 0,
				};
				instructions[instructions[ip + 3]] = res;

				ip += 4;
			}
		}
	}
}
