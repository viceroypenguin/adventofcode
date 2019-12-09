using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow;
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
				.Select(long.Parse)
				.ToList();

			var inputs = new BufferBlock<long>();
			inputs.Post(1);
			var outputs = new BufferBlock<long>();
			var pc = new IntCodeComputer(instructions.ToArray(), inputs, outputs);
			pc.RunProgram()
				.GetAwaiter()
				.GetResult();
			while (outputs.Count > 0)
			{
				var value = outputs.Receive();
				if (value > 0)
				{
					PartA = value.ToString();
					break;
				}
			}

			inputs = new BufferBlock<long>();
			inputs.Post(5);
			outputs = new BufferBlock<long>();
			pc = new IntCodeComputer(instructions.ToArray(), inputs, outputs);
			pc.RunProgram()
				.GetAwaiter()
				.GetResult();
			PartB = outputs.Receive().ToString();
		}
	}
}
