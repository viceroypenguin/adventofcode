using System.Threading.Tasks.Dataflow;

namespace AdventOfCode;

public class Day_2019_11_Original : Day
{
	public override int Year => 2019;
	public override int DayNumber => 11;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var instructions = input.GetString()
			.Split(',')
			.Select(long.Parse)
			.ToArray();

		// 640k should be enough for anyone
		Array.Resize(ref instructions, 640 * 1024);

		var map = RunPart(instructions, 0);
		Dump('A', map.Count.ToString());

		map = RunPart(instructions, 1);
		var minX = map.Keys.Select(p => (int)(p >> 32)).Min();
		var maxX = map.Keys.Select(p => (int)(p >> 32)).Max();
		var minY = map.Keys.Select(p => (int)(p & 0xFFFFFFFF)).Min();
		var maxY = map.Keys.Select(p => (int)(p & 0xFFFFFFFF)).Max();

		DumpScreen('B', Enumerable.Range(minY, maxY - minY + 1)
			.Select(y => Enumerable.Range(minX, maxX - minX + 1)
				.Select(x => map.GetValueOrDefault(GetCoordinate(x, y)) == 0 ? ' ' : '█'))
			.Reverse());
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static long GetCoordinate(int x, int y) =>
		((long)x << 32) | (uint)y;

	private static Dictionary<long, long> RunPart(long[] instructions, int initialPointValue)
	{
		var inputs = new BufferBlock<long>();
		var outputs = new BufferBlock<long>();
		var pc = new IntCodeComputer(instructions.ToArray(), inputs, outputs);

		var map = new Dictionary<long, long>();

		var tcs = new CancellationTokenSource();
		Task.Run(async () =>
		{
			var token = tcs.Token;
			var coord = (x: 0, y: 0);
			var dir = 0;

			map[GetCoordinate(coord.x, coord.y)] = initialPointValue;

			while (true)
			{
				var coordValue = GetCoordinate(coord.x, coord.y);
				inputs.Post(map.GetValueOrDefault(coordValue));

				map[coordValue] = await outputs.ReceiveAsync(token);
				var turn = await outputs.ReceiveAsync();
				dir = turn switch { 0 => (dir + 3) % 4, 1 => (dir + 1) % 4, };
				coord = dir switch
				{
					0 => (coord.x, coord.y + 1),
					1 => (coord.x + 1, coord.y),
					2 => (coord.x, coord.y - 1),
					3 => (coord.x - 1, coord.y),
				};
			}
		});

		pc.RunProgram()
			.GetAwaiter()
			.GetResult();

		tcs.Cancel();
		return map;
	}
}
