namespace AdventOfCode;

public class Day_2015_06_Original : Day
{
	public override int Year => 2015;
	public override int DayNumber => 6;
	public override CodeType CodeType => CodeType.Original;

	enum Command
	{
		TurnOn,
		TurnOff,
		Toggle,
	}

	struct Action
	{
		public Command Command;
		public int StartX;
		public int StartY;
		public int EndX;
		public int EndY;
	}

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var regex = new Regex(
			@"((?<on>turn on)|(?<off>turn off)|(?<toggle>toggle)) (?<startX>\d+),(?<startY>\d+) through (?<endX>\d+),(?<endY>\d+)",
			RegexOptions.Compiled | RegexOptions.ExplicitCapture);

		var actions = input.GetLines()
			.Select(l => regex.Match(l))
			.Select(m => new Action
			{
				Command =
					m.Groups["on"].Success ? Command.TurnOn :
					m.Groups["off"].Success ? Command.TurnOff :
					m.Groups["toggle"].Success ? Command.Toggle :
					throw new InvalidOperationException(),
				StartX = int.Parse(m.Groups["startX"].Value),
				StartY = int.Parse(m.Groups["startY"].Value),
				EndX = int.Parse(m.Groups["endX"].Value),
				EndY = int.Parse(m.Groups["endY"].Value),
			})
			.ToList();

		Func<Func<Command, Func<int, int>>, int> ProcessActions =
			(getLightProcessor) =>
			{
				var lights = new int[1000, 1000];

				foreach (var a in actions)
				{
					var lightProcessor = getLightProcessor(a.Command);

					for (var x = a.StartX; x <= a.EndX; x++)
						for (var y = a.StartY; y <= a.EndY; y++)
							lights[x, y] = lightProcessor(lights[x, y]);
				}

				var cnt = 0;
				for (var x = 0; x < 1000; x++)
					for (var y = 0; y < 1000; y++)
						cnt += lights[x, y];

				return cnt;
			};

		Dump('A',
			ProcessActions(c =>
				c == Command.TurnOn ? (Func<int, int>)(i => 1) :
				c == Command.TurnOff ? (Func<int, int>)(i => 0) :
				c == Command.Toggle ? (Func<int, int>)(i => 1 - i) :
				throw new InvalidOperationException()));

		Dump('B',
			ProcessActions(c =>
				c == Command.TurnOn ? (Func<int, int>)(i => i + 1) :
				c == Command.TurnOff ? (Func<int, int>)(i => Math.Max(i - 1, 0)) :
				c == Command.Toggle ? (Func<int, int>)(i => i + 2) :
				throw new InvalidOperationException()));
	}
}
