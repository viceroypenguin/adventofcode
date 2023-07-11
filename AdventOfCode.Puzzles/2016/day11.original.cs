namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{
	[Flags]
	public enum Devices
	{
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
		public Devices[] Floors;
		public int ElevatorFloor;
		public int StepCount;

		public override bool Equals(object other)
		{
			return Equals((State)other);
		}

		public bool Equals(State s2) =>
			Floors[0] == s2.Floors[0]
			&& Floors[1] == s2.Floors[1]
			&& Floors[2] == s2.Floors[2]
			&& Floors[3] == s2.Floors[3]
			&& ElevatorFloor == s2.ElevatorFloor;

		public override int GetHashCode() =>
			HashCode.Combine(
				Floors[0],
				Floors[1],
				Floors[2],
				Floors[3],
				ElevatorFloor);
	}

	public static bool IsValidState(Devices d)
	{
		return (d & Devices.Generators) == 0
			|| ((d & Devices.StrontiumSet) != Devices.StrontiumMicrochip
				&& (d & Devices.PlutoniumSet) != Devices.PlutoniumMicrochip
				&& (d & Devices.ThuliumSet) != Devices.ThuliumMicrochip
				&& (d & Devices.RutheniumSet) != Devices.RutheniumMicrochip
				&& (d & Devices.CuriumSet) != Devices.CuriumMicrochip
				&& (d & Devices.EleriumSet) != Devices.EleriumMicrochip
				&& (d & Devices.DilithiumSet) != Devices.DilithiumMicrochip);
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
			Floors = new Devices[]
			{
				firstFloor,
				Devices.ThuliumGenerator | Devices.RutheniumGenerator | Devices.RutheniumMicrochip | Devices.CuriumGenerator | Devices.CuriumMicrochip,
				Devices.ThuliumMicrochip,
				0,
			},
			ElevatorFloor = 0,
			StepCount = 0,
		};

		return SuperEnumerable.GetShortestPathCost<State, int>(
			initialState,
			GetNextStates,
			s => s.Floors[0] == 0
				&& s.Floors[1] == 0
				&& s.Floors[2] == 0
				&& s.Floors[3] != 0);
	}

	private static readonly Devices[] IndividualDevices = new[]
	{
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
	};

	private static IEnumerable<(State s, int cost)> GetNextStates(State s, int cost)
	{
		var floor = s.ElevatorFloor;
		var currentDevices = s.Floors[floor];

		for (var deviceAidx = 0; deviceAidx < IndividualDevices.Length; deviceAidx++)
		{
			var deviceA = IndividualDevices[deviceAidx];
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

				for (var deviceBidx = deviceAidx + 1; deviceBidx < IndividualDevices.Length; deviceBidx++)
				{
					var deviceB = IndividualDevices[deviceBidx];
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
