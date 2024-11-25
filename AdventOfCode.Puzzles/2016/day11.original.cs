namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	[Flags]
	public enum Devices
	{
		None = 0,

		StrontiumGenerator = 1,
		StrontiumMicrochip = 2,
		StrontiumSet = StrontiumGenerator | StrontiumMicrochip,

		PlutoniumGenerator = 4,
		PlutoniumMicrochip = 8,
		PlutoniumSet = PlutoniumGenerator | PlutoniumMicrochip,

		ThuliumGenerator = 16,
		ThuliumMicrochip = 32,
		ThuliumSet = ThuliumGenerator | ThuliumMicrochip,

		RutheniumGenerator = 64,
		RutheniumMicrochip = 128,
		RutheniumSet = RutheniumGenerator | RutheniumMicrochip,

		CuriumGenerator = 256,
		CuriumMicrochip = 512,
		CuriumSet = CuriumGenerator | CuriumMicrochip,

		EleriumGenerator = 1024,
		EleriumMicrochip = 2048,
		EleriumSet = EleriumGenerator | EleriumMicrochip,

		DilithiumGenerator = 4096,
		DilithiumMicrochip = 8192,
		DilithiumSet = DilithiumGenerator | DilithiumMicrochip,

		Generators = StrontiumGenerator | PlutoniumGenerator | ThuliumGenerator | RutheniumGenerator | CuriumGenerator | EleriumGenerator | DilithiumGenerator,
	}

	private struct State : IEquatable<State>
	{
		public Devices[] Floors { get; set; }
		public int ElevatorFloor { get; set; }
		public int StepCount { get; set; }

		public override readonly bool Equals(object other)
		{
			return Equals((State)other);
		}

		public readonly bool Equals(State s2) =>
			Floors[0] == s2.Floors[0]
			&& Floors[1] == s2.Floors[1]
			&& Floors[2] == s2.Floors[2]
			&& Floors[3] == s2.Floors[3]
			&& ElevatorFloor == s2.ElevatorFloor;

		public override readonly int GetHashCode() =>
			HashCode.Combine(
				Floors[0],
				Floors[1],
				Floors[2],
				Floors[3],
				ElevatorFloor
			);
	}

	public static bool IsValidState(Devices d)
	{
		return (d & Devices.Generators) is Devices.None
			|| ((d & Devices.StrontiumSet) is not Devices.StrontiumMicrochip
				&& (d & Devices.PlutoniumSet) is not Devices.PlutoniumMicrochip
				&& (d & Devices.ThuliumSet) is not Devices.ThuliumMicrochip
				&& (d & Devices.RutheniumSet) is not Devices.RutheniumMicrochip
				&& (d & Devices.CuriumSet) is not Devices.CuriumMicrochip
				&& (d & Devices.EleriumSet) is not Devices.EleriumMicrochip
				&& (d & Devices.DilithiumSet) is not Devices.DilithiumMicrochip);
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var partAFirstFloor = Devices.StrontiumGenerator | Devices.StrontiumMicrochip | Devices.PlutoniumGenerator | Devices.PlutoniumMicrochip;
		var partBFirstFloor = partAFirstFloor | Devices.EleriumGenerator | Devices.EleriumMicrochip | Devices.DilithiumGenerator | Devices.DilithiumMicrochip;

		return (
			ExecutePart(partAFirstFloor).ToString(),
			ExecutePart(partBFirstFloor).ToString());
	}

	private static int ExecutePart(Devices firstFloor)
	{
		var initialState = new State
		{
			Floors =
			[
				firstFloor,
				Devices.ThuliumGenerator | Devices.RutheniumGenerator | Devices.RutheniumMicrochip | Devices.CuriumGenerator | Devices.CuriumMicrochip,
				Devices.ThuliumMicrochip,
				Devices.None,
			],
			ElevatorFloor = 0,
			StepCount = 0,
		};

		return SuperEnumerable.GetShortestPathCost<State, int>(
			initialState,
			GetNextStates,
			s => s.Floors[0] is Devices.None
				&& s.Floors[1] is Devices.None
				&& s.Floors[2] is Devices.None
				&& s.Floors[3] is not Devices.None);
	}

	private static readonly Devices[] s_individualDevices =
	[
		Devices.StrontiumGenerator,
		Devices.StrontiumMicrochip,
		Devices.PlutoniumGenerator,
		Devices.PlutoniumMicrochip,
		Devices.ThuliumGenerator,
		Devices.ThuliumMicrochip,
		Devices.RutheniumGenerator,
		Devices.RutheniumMicrochip,
		Devices.CuriumGenerator,
		Devices.CuriumMicrochip,
		Devices.EleriumGenerator,
		Devices.EleriumMicrochip,
		Devices.DilithiumGenerator,
		Devices.DilithiumMicrochip,
	];

	private static IEnumerable<(State s, int cost)> GetNextStates(State s, int cost)
	{
		var floor = s.ElevatorFloor;
		var currentDevices = s.Floors[floor];

		for (var deviceAidx = 0; deviceAidx < s_individualDevices.Length; deviceAidx++)
		{
			var deviceA = s_individualDevices[deviceAidx];
			if ((currentDevices & deviceA) == deviceA)
			{
				{
					var newCurrentFloor = currentDevices & ~deviceA;
					if (IsValidState(newCurrentFloor))
					{
						if (floor < 3)
						{
							var newHigherFloor = s.Floors[floor + 1] | deviceA;
							if (IsValidState(newHigherFloor))
							{
								var newState = new State()
								{
									Floors = s.Floors.ToArray(),
									ElevatorFloor = floor + 1,
									StepCount = s.StepCount + 1,
								};
								newState.Floors[floor] = newCurrentFloor;
								newState.Floors[floor + 1] = newHigherFloor;
								yield return (newState, cost + 1);
							}
						}

						if (floor > 0)
						{
							var newLowerFloor = s.Floors[floor - 1] | deviceA;
							if (IsValidState(newLowerFloor))
							{
								var newState = new State()
								{
									Floors = s.Floors.ToArray(),
									ElevatorFloor = floor - 1,
									StepCount = s.StepCount + 1,
								};
								newState.Floors[floor] = newCurrentFloor;
								newState.Floors[floor - 1] = newLowerFloor;
								yield return (newState, cost + 1);
							}
						}
					}
				}

				for (var deviceBidx = deviceAidx + 1; deviceBidx < s_individualDevices.Length; deviceBidx++)
				{
					var deviceB = s_individualDevices[deviceBidx];
					if ((currentDevices & deviceB) == deviceB)
					{
						var newCurrentFloor = currentDevices & ~deviceA & ~deviceB;
						if (!IsValidState(newCurrentFloor))
							continue;

						if (floor < 3)
						{
							var newHigherFloor = s.Floors[floor + 1] | deviceA | deviceB;
							if (IsValidState(newHigherFloor))
							{
								var newState = new State()
								{
									Floors = s.Floors.ToArray(),
									ElevatorFloor = floor + 1,
									StepCount = s.StepCount + 1,
								};
								newState.Floors[floor] = newCurrentFloor;
								newState.Floors[floor + 1] = newHigherFloor;
								yield return (newState, cost + 1);
							}
						}

						if (floor > 0)
						{
							var newLowerFloor = s.Floors[floor - 1] | deviceA | deviceB;
							if (IsValidState(newLowerFloor))
							{
								var newState = new State()
								{
									Floors = s.Floors.ToArray(),
									ElevatorFloor = floor - 1,
									StepCount = s.StepCount + 1,
								};
								newState.Floors[floor] = newCurrentFloor;
								newState.Floors[floor - 1] = newLowerFloor;
								yield return (newState, cost + 1);
							}
						}
					}
				}
			}
		}
	}
}
