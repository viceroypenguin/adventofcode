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
	public class Day_2019_17_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 17;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var instructions = input.GetString()
				.Split(',')
				.Select(long.Parse)
				.ToArray();

			// 64k should be enough for anyone
			Array.Resize(ref instructions, 64 * 1024);

			var inputs = new BufferBlock<long>();
			var outputs = new BufferBlock<long>();
			var pc = new IntCodeComputer(instructions.ToArray(), inputs, outputs);

			pc.RunProgram()
				.GetAwaiter()
				.GetResult();

			outputs.TryReceiveAll(out var mapData);

			var map = mapData
				.Batch(mapData.IndexOf('\n') + 1)
				.Select(r => r.Select(b => (char)b).ToArray())
				.ToArray();

			var intersections = Enumerable.Range(1, map.Length - 2)
				.SelectMany(y => Enumerable.Range(1, map[0].Length - 2)
					.Where(x => map[y][x] == '#')
					.Where(x => (map[y][x - 1] == '#') && (x < map[0].Length && map[y][x + 1] == '#'))
					.Where(x => (map[y - 1][x] == '#') && (y < map.Length && map[y + 1][x] == '#'))
					.Select(x => (x, y)))
				.ToList();
			PartA = intersections
				.Sum(p => p.x * p.y)
				.ToString();

			instructions[0] = 2;
			var data = 
@"A,C,A,B,C,B,A,C,A,B
R,6,L,10,R,8,R,8
R,12,L,10,R,6,L,10
R,12,L,8,L,10
n
"
				.Select(c => (byte)c)
				.Where(c => c != '\r')
				.ToArray();

			foreach (var c in data)
				inputs.Post(c);

			pc = new IntCodeComputer(instructions.ToArray(), inputs, outputs);

			pc.RunProgram()
				.GetAwaiter()
				.GetResult();

			outputs.TryReceiveAll(out mapData);
			PartB = mapData.Last().ToString();
		}
	}
}
