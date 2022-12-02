namespace AdventOfCode;

public unsafe class Day_2019_02_Fastest : Day
{
	public override int Year => 2019;
	public override int DayNumber => 2;
	public override CodeType CodeType => CodeType.Fastest;

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var nums = stackalloc int[input.Length / 2];
		int numCount = 0, n = 0;
		foreach (var c in input)
		{
			if (c == ',')
			{
				nums[numCount++] = n;
				n = 0;
			}
			else if (c >= '0')
				n = n * 10 + c - '0';
		}

		var copy = stackalloc int[numCount];
		Unsafe.CopyBlock((void*)copy, (void*)nums, (uint)numCount * sizeof(int));
		copy[1] = 12;
		copy[2] = 2;

		RunProgram(copy, numCount);
		PartA = copy[0].ToString();

		Unsafe.CopyBlock((void*)copy, (void*)nums, (uint)numCount * sizeof(int));
		copy[1] = 0;
		copy[2] = 0;

		RunProgram(copy, numCount);
		var zero = copy[0];

		Unsafe.CopyBlock((void*)copy, (void*)nums, (uint)numCount * sizeof(int));
		copy[1] = 1;
		copy[2] = 0;

		RunProgram(copy, numCount);
		var one = copy[0];

		var perNoun = one - zero;
		var noun = (19690720 - zero) / perNoun;
		var verb = 19690720 - (zero + perNoun * noun);

		PartB = (noun * 100 + verb).ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	static void RunProgram(int* instructions, int instructionCount)
	{
		var ip = 0;
		while (ip < instructionCount && instructions[ip] != 99)
		{
			var num1 = instructions[instructions[ip + 1]];
			var num2 = instructions[instructions[ip + 2]];
			var res = instructions[ip] switch
			{
				1 => num1 + num2,
				2 => num1 * num2,
				_ => 0,
			};
			instructions[instructions[ip + 3]] = res;

			ip += 4;
		}
	}
}
