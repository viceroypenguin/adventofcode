using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day_2016_11_Original : Day
    {
        public override int Year => 2016;
        public override int DayNumber => 11;
        public override CodeType CodeType => CodeType.Original;

        [Flags]
        public enum Devices
        {
            StrontiumGenerator = 1,
            StrontiumMicrochip = 2,
            StrontiumSet = (StrontiumGenerator | StrontiumMicrochip),

            PlutoniumGenerator = 4,
            PlutoniumMicrochip = 8,
            PlutoniumSet = (PlutoniumGenerator | PlutoniumMicrochip),

            ThuliumGenerator = 16,
            ThuliumMicrochip = 32,
            ThuliumSet = (ThuliumGenerator | ThuliumMicrochip),

            RutheniumGenerator = 64,
            RutheniumMicrochip = 128,
            RutheniumSet = (RutheniumGenerator | RutheniumMicrochip),

            CuriumGenerator = 256,
            CuriumMicrochip = 512,
            CuriumSet = (CuriumGenerator | CuriumMicrochip),

            EleriumGenerator = 1024,
            EleriumMicrochip = 2048,
            EleriumSet = (EleriumGenerator | EleriumMicrochip),

            DilithiumGenerator = 4096,
            DilithiumMicrochip = 8192,
            DilithiumSet = (DilithiumGenerator | DilithiumMicrochip),

            Generators = (StrontiumGenerator | PlutoniumGenerator | ThuliumGenerator | RutheniumGenerator | CuriumGenerator | EleriumGenerator | DilithiumGenerator),
        }

        public struct State
        {
            public Devices[] Floors;
            public int ElevatorFloor;
            public int StepCount;

            public override bool Equals(object other)
            {
                return Equals((State)other);
            }

            public bool Equals(State s2)
            {
                return
                    this.Floors[0] == s2.Floors[0] &&
                    this.Floors[1] == s2.Floors[1] &&
                    this.Floors[2] == s2.Floors[2] &&
                    this.Floors[3] == s2.Floors[3] &&
                    this.ElevatorFloor == s2.ElevatorFloor;
            }

            public override int GetHashCode()
            {
                return Tuple.Create(
                    this.Floors[0],
                    this.Floors[1],
                    this.Floors[2],
                    this.Floors[3],
                    this.ElevatorFloor).GetHashCode();
            }
        }

        Dictionary<State, State> _VisitedStates = new Dictionary<State, State>();
        public bool HasVisitedState(State state)
        {
            if (_VisitedStates.ContainsKey(state))
            {
                return true;
            }
            else
            {
                _VisitedStates.Add(state, state);
                return false;
            }
        }

        public bool IsValidState(Devices d)
        {
            if ((d & Devices.StrontiumSet) == Devices.StrontiumMicrochip ||
                (d & Devices.PlutoniumSet) == Devices.PlutoniumMicrochip ||
                (d & Devices.ThuliumSet) == Devices.ThuliumMicrochip ||
                (d & Devices.RutheniumSet) == Devices.RutheniumMicrochip ||
                (d & Devices.CuriumSet) == Devices.CuriumMicrochip ||
                (d & Devices.EleriumSet) == Devices.EleriumMicrochip ||
                (d & Devices.DilithiumSet) == Devices.DilithiumMicrochip)
            {
                return (d & Devices.Generators) == 0;
            }
            else
                return true;
        }

        protected override void ExecuteDay(byte[] input)
		{
			var partAFirstFloor = Devices.StrontiumGenerator | Devices.StrontiumMicrochip | Devices.PlutoniumGenerator | Devices.PlutoniumMicrochip;
			var partBFirstFloor = Devices.StrontiumGenerator | Devices.StrontiumMicrochip | Devices.PlutoniumGenerator | Devices.PlutoniumMicrochip | Devices.EleriumGenerator | Devices.EleriumMicrochip | Devices.DilithiumGenerator | Devices.DilithiumMicrochip;

			ExecutePart(partAFirstFloor, 'A');
			ExecutePart(partBFirstFloor, 'B');
		}

		private void ExecutePart(Devices firstFloor, char part)
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

			var queue = new Queue<State>();
			queue.Enqueue(initialState);
			while (queue.Count != 0)
			{
				var curState = queue.Dequeue();

				foreach (var s in GetNextStates(curState))
				{
					if (s.Floors[0] == 0 &&
						s.Floors[1] == 0 &&
						s.Floors[2] == 0 &&
						s.Floors[3] != 0)
					{
						Dump(part, s.StepCount);
					}

					queue.Enqueue(s);
				}
			}
		}

		Devices[] IndividualDevices = new[]
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

        IEnumerable<State> GetNextStates(State s)
        {
            var floor = s.ElevatorFloor;
            var currentDevices = s.Floors[floor];

            for (int deviceAidx = 0; deviceAidx < IndividualDevices.Length; deviceAidx++)
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
                                    if (!HasVisitedState(newState))
                                        yield return newState;
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
                                    if (!HasVisitedState(newState))
                                        yield return newState;
                                }
                            }
                        }
                    }

                    for (int deviceBidx = deviceAidx + 1; deviceBidx < IndividualDevices.Length; deviceBidx++)
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
                                    if (!HasVisitedState(newState))
                                        yield return newState;
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
                                    if (!HasVisitedState(newState))
                                        yield return newState;
                                }
                            }
                        }
                    }

                }
            }
        }
    }
}
