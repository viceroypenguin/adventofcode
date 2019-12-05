using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2019_05_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 5;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var instructions = input.GetString()
				.Split(',')
				.Select(s => Convert.ToInt32(s))
				.ToList();

			var partA = new List<int>(instructions);
			progInput = 1;
			RunProgram(partA);
			PartA = progOutput.ToString();

			var partB = new List<int>(instructions);
			progInput = 5;
			RunProgram(partB);
			PartB = progOutput.ToString();
		}

		private int progInput = 0;
		private int progOutput = 0;

		void RunProgram(List<int> instructions)
		{
			var ip = 0;
			while (ip < instructions.Count && instructions[ip] != 99)
			{
				static int GetFirstParameterMode(int instruction) =>
					(instruction / 100) % 10;
				static int GetSecondParameterMode(int instruction) =>
					instruction / 1000;
				int GetValue(int mode, int value) =>
					mode != 0 ? value : instructions[value];

				switch (instructions[ip] % 100)
				{
					case 1:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							instructions[instructions[ip + 3]] = num1 + num2;
							ip += 4;
							break;
						}
					case 2:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							instructions[instructions[ip + 3]] = num1 * num2;
							ip += 4;
							break;
						}

					case 3:
						{
							instructions[instructions[ip + 1]] = progInput;
							ip += 2;
							break;
						}

					case 4:
						{
							progOutput = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							ip += 2;
							break;
						}

					case 5:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							ip = num1 == 0 ? ip + 3 : num2;
							break;
						}

					case 6:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							ip = num1 != 0 ? ip + 3 : num2;
							break;
						}

					case 7:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							instructions[instructions[ip + 3]] = num1 < num2 ? 1 : 0;
							ip += 4;
							break;
						}

					case 8:
						{
							var num1 = GetValue(GetFirstParameterMode(instructions[ip]), instructions[ip + 1]);
							var num2 = GetValue(GetSecondParameterMode(instructions[ip]), instructions[ip + 2]);
							instructions[instructions[ip + 3]] = num1 == num2 ? 1 : 0;
							ip += 4;
							break;
						}
				}
			}
		}
	}
}
