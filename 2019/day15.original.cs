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
	public class Day_2019_15_Original : Day
	{
		public override int Year => 2019;
		public override int DayNumber => 15;
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

			var pc = new IntCodeComputer(instructions.ToArray(), inputs, outputs);

			Task.Run(async () =>
			{
				for (int i = 1; i <= 4; i++)
					await HandleDirection((0, 0), i, 1);

				// anything outside 1-4 kills the program
				inputs.Post(5);
			});

			pc.RunProgram()
				.GetAwaiter()
				.GetResult();

			PartA = map[oxygenLocation].distance.ToString();

			var queue = new Queue<((int x, int y) position, int distance)>();
			queue.Enqueue((oxygenLocation, 0));

			map[oxygenLocation] = (1, 0); // simplify type check below
			while (queue.Any())
			{
				var pos = queue.Dequeue();
				if (map[pos.position].type != 1)
					continue;

				map[pos.position] = (3, pos.distance);

				queue.Enqueue(((pos.position.x, pos.position.y - 1), pos.distance + 1));
				queue.Enqueue(((pos.position.x, pos.position.y + 1), pos.distance + 1));
				queue.Enqueue(((pos.position.x - 1, pos.position.y), pos.distance + 1));
				queue.Enqueue(((pos.position.x + 1, pos.position.y), pos.distance + 1));
			}

			PartB = map.Values.Where(x => x.type == 3).Max(x => x.distance).ToString();
		}

		private readonly BufferBlock<long> inputs = new BufferBlock<long>();
		private readonly BufferBlock<long> outputs = new BufferBlock<long>();
		private readonly Dictionary<(int x, int y), (int type, int distance)> map =
			new Dictionary<(int x, int y), (int type, int distance)>()
			{
				[(0, 0)] = (0, 0),
			};
		(int x, int y) oxygenLocation = default;

		private async Task HandleDirection((int x, int y) position, int direction, int distance)
		{
			var newPosition = direction switch
			{
				1 => (position.x, position.y - 1),
				2 => (position.x, position.y + 1),
				3 => (position.x - 1, position.y),
				4 => (position.x + 1, position.y),
			};

			if (map.ContainsKey(newPosition))
				return;

			inputs.Post(direction);
			var response = await outputs.ReceiveAsync();
			map[newPosition] = ((int)response, distance);
			if (response == 0)
				return;

			if (response == 2)
				oxygenLocation = newPosition;

			for (int i = 1; i <= 4; i++)
				await HandleDirection(newPosition, i, distance + 1);

			inputs.Post(direction switch { 1 => 2, 2 => 1, 3 => 4, 4 => 3, });
			await outputs.ReceiveAsync();
		}
	}
}
