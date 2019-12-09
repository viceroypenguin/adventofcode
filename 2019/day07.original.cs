using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2019_07_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 7;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var instructions = input.GetString()
				.Split(',')
				.Select(long.Parse)
				.ToList();
			
			PartA = DoPart(instructions, 0).GetAwaiter().GetResult();
			PartB = DoPart(instructions, 5).GetAwaiter().GetResult();
		}

		async Task<string> DoPart(List<long> instructions, int start) =>
			(await Task.WhenAll(
				MoreEnumerable.Permutations(Enumerable.Range(start, 5))
					.Select(async arr =>
					{
						var buffers = Enumerable.Range(0, 5).Select(i => new BufferBlock<long>()).ToList();
						buffers[0].Post(arr[0]);
						buffers[0].Post(0);
						buffers[1].Post(arr[1]);
						buffers[2].Post(arr[2]);
						buffers[3].Post(arr[3]);
						buffers[4].Post(arr[4]);

						await Task.WhenAll(
							new IntCodeComputer(instructions.ToArray(), buffers[0], buffers[1]).RunProgram(),
							new IntCodeComputer(instructions.ToArray(), buffers[1], buffers[2]).RunProgram(),
							new IntCodeComputer(instructions.ToArray(), buffers[2], buffers[3]).RunProgram(),
							new IntCodeComputer(instructions.ToArray(), buffers[3], buffers[4]).RunProgram(),
							new IntCodeComputer(instructions.ToArray(), buffers[4], buffers[0]).RunProgram());

						return buffers[0].Receive();
					})))
				.Max()
				.ToString();
	}
}
