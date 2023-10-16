using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode.Puzzles._2019;

[Puzzle(2019, 22, CodeType.Original)]
public partial class Day_22_Original : IPuzzle
{
	private enum Instruction
	{
		NewStack,
		Cut,
		Increment,
	}

	[GeneratedRegex(@"
			(?<new_stack>deal\sinto\snew\sstack) |
			(?<cut>cut\s(?<n>-?\d+)) |
			(?<increment>deal\swith\sincrement\s(?<n>\d+))",
		RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace)]
	private static partial Regex InstructionRegex();

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = InstructionRegex();

		var instructions = input.Lines
			.Select(s => regex.Match(s))
			.Select(m =>
				m.Groups["new_stack"].Success ? (Instruction.NewStack, 0) :
				m.Groups["cut"].Success ? (Instruction.Cut, Convert.ToInt32(m.Groups["n"].Value)) :
				m.Groups["increment"].Success ? (Instruction.Increment, Convert.ToInt32(m.Groups["n"].Value)) :
				throw new InvalidOperationException("unknown instruction"))
			.ToList();

		return (
			DoPartA(instructions),
			DoPartB(instructions));
	}

	private static string DoPartA(List<(Instruction, int)> instructions)
	{
		const long DeckSize = 10007;
		const long InitialCard = 2019;
		return instructions
			.Aggregate(InitialCard, (c, i) =>
				i switch
				{
					(Instruction.NewStack, _) => DeckSize - c - 1,
					(Instruction.Cut, var n) => (c - n + DeckSize + DeckSize) % DeckSize,
					(Instruction.Increment, var n) => c * n % DeckSize,
					_ => throw new UnreachableException(),
				})
			.ToString();
	}

	private static string DoPartB(List<(Instruction, int)> instructions)
	{
		const long Shuffles = 101741582076661L;
		const long DeckSize = 119315717514047L;
		const int InitialCard = 2020;

		static BigInteger DeckInverse(BigInteger n) =>
			BigInteger.ModPow(n, DeckSize - 2, DeckSize);

		static BigInteger Normalize(BigInteger value) =>
			value < 0
				? value + (DeckSize * ((-value / DeckSize) + 1))
				: value % DeckSize;

		// build per-loop offset/increment
		// net change is v = increment * v + offset
		var (_increment, _offset) = instructions
			.Aggregate((increment: BigInteger.One, offset: BigInteger.Zero), (x, i) =>
				i switch
				{
					(Instruction.NewStack, _) => (-x.increment, DeckSize - x.offset - 1),
					(Instruction.Cut, var n) => (x.increment, DeckSize + x.offset - n),
					(Instruction.Increment, var n) => (x.increment * n, x.offset * n),
					_ => throw new UnreachableException(),
				});

		// execute Shuffles loops
		var increment = BigInteger.ModPow(_increment, Shuffles, DeckSize);
		var offset = _offset
			* (increment - 1)
			* DeckInverse(_increment - 1)
			% DeckSize;

		// make final adjustments
		var originalPosition = Normalize(
			(InitialCard - offset) % DeckSize * DeckInverse(increment));

		return originalPosition.ToString();
	}
}
