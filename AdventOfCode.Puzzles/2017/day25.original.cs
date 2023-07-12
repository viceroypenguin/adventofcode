namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 25, CodeType.Original)]
public class Day_25_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var instructions = input.Lines;
		var initialState = instructions[0][15];
		var steps = Convert.ToInt32(instructions[1].Split()[5]);

		var states =
			instructions.Skip(3)
				.Batch(10)
				.Cast<string[]>()
				.Select(x => new
				{
					State = x[0][9],
					Transitions =
						x.Skip(1)
							.Batch(4)
							.Cast<string[]>()
							.Select(y => new
							{
								WriteValue = (int)y[1][22] - (int)'0',
								MoveDirection = y[2].Split()[10] == "left." ? -1 : 1,
								NextState = y[3][26],
							})
							.Take(2)
							.ToList(),
				})
				.ToDictionary(x => x.State);

		var posList = new List<int>();
		var negList = new List<int>();

		int GetListValue(List<int> l, int index) =>
			index < l.Count ? l[index] : 0;
		int GetValue(int position) =>
			position >= 0 ? GetListValue(posList, position) : GetListValue(negList, -1 - position);

		void SetListValue(List<int> l, int index, int value)
		{
			if (index >= l.Count)
				l.AddRange(Enumerable.Repeat(0, index - l.Count + 1));
			l[index] = value;
		}
		void SetValue(int position, int value)
		{
			if (position >= 0)
				SetListValue(posList, position, value);
			else
				SetListValue(negList, -1 - position, value);
		}

		var currentPos = 0;
		var state = initialState;

		for (var i = 0; i < steps; i++)
		{
			var currentValue = GetValue(currentPos);
			var transition = states[state].Transitions[currentValue];
			SetValue(currentPos, transition.WriteValue);
			state = transition.NextState;
			currentPos += transition.MoveDirection;
		}

		return (
			negList.Concat(posList).Count(x => x == 1).ToString(),
			string.Empty);
	}
}
