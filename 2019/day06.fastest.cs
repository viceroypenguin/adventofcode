namespace AdventOfCode;

public unsafe class Day_2019_06_Fastest : Day
{
	public override int Year => 2019;
	public override int DayNumber => 6;
	public override CodeType CodeType => CodeType.Fastest;

	struct Orbit
	{
		public int Orbited;
		public int Orbiter;
		public int NumOrbits;
		public int OrbitedIndex;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var orbits = stackalloc Orbit[input.Length / 8];
		Orbit* curOrbit = orbits, comOrbit = null;
		var n = 0;
		foreach (var c in input)
		{
			if (c == ')')
			{
				curOrbit->Orbited = n;
				if (n == 0x00434F4D)
					comOrbit = curOrbit;
				n = 0;
			}
			else if (c == '\n')
			{
				curOrbit->Orbiter = n;
				n = 0;
				curOrbit++;
			}
			else if (c >= '0')
			{
				n = n << 8 | c;
			}
		}

		var endOrbit = curOrbit;

		Swap(comOrbit, &orbits[0]);
		comOrbit = orbits;
		comOrbit->NumOrbits = 1;

		Orbit* sanOrbit = null, youOrbit = null;
		var sortedOrbits = comOrbit;
		var totalOrbits = 1;
		for (curOrbit = comOrbit; curOrbit < endOrbit; curOrbit++)
		{
			for (var parentOrbit = sortedOrbits + 1; parentOrbit < endOrbit; parentOrbit++)
			{
				if (parentOrbit->Orbited == curOrbit->Orbiter)
				{
					Swap(++sortedOrbits, parentOrbit);
					totalOrbits += (
						sortedOrbits->NumOrbits = curOrbit->NumOrbits + 1);
					sortedOrbits->OrbitedIndex = (int)(curOrbit - orbits);

					if (sortedOrbits->Orbiter == 0x0053414E)
						sanOrbit = sortedOrbits;
					else if (sortedOrbits->Orbiter == 0x00594F55)
						youOrbit = sortedOrbits;
				}
			}
		}

		PartA = totalOrbits.ToString();

		var sanPath = stackalloc int[sanOrbit->NumOrbits - 1];
		for (int idx = sanOrbit->OrbitedIndex, sanCnt = 0; idx != 0; idx = orbits[idx].OrbitedIndex)
			sanPath[sanCnt++] = orbits[idx].Orbiter;

		var youPath = stackalloc int[youOrbit->NumOrbits - 1];
		for (int idx = youOrbit->OrbitedIndex, youCnt = 0; idx != 0; idx = orbits[idx].OrbitedIndex)
			youPath[youCnt++] = orbits[idx].Orbiter;

		int sanIdx = sanOrbit->NumOrbits - 2, youIdx = youOrbit->NumOrbits - 2;
		while (sanPath[sanIdx] == youPath[youIdx])
		{ sanIdx--; youIdx--; }

		PartB = (sanIdx + youIdx + 2).ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static void Swap(Orbit* a, Orbit* b)
	{
		// only need to switch Orbiter and Orbited
		ulong tmp = *(ulong*)a;
		*(ulong*)a = *(ulong*)b;
		*(ulong*)b = tmp;
	}
}
