namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 16, CodeType.Original)]
public class Day_16_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var bits = input.Bytes
			.Select(b => b >= 'A' ? b - 'A' + 10 : b - '0')
			.Batch(16)
			.Select(g => g.Pad(16)
				.Aggregate(0UL, (a, b) => (a << 4) | (uint)b))
			.ToArray();

		uint GetBits(int p, int l)
		{
			var mask = (1UL << l) - 1;

			var mod = (p & 63) + l;
			var i = bits[p >> 6];
			if (mod > 64)
			{
				i = (i << 32) | (bits[(p >> 6) + 1] >> 32);
				mod -= 32;
			}

			var shift = 64 - mod;
			return (uint)((i & (mask << shift)) >> shift);
		}

		(Packet packet, int numBits) ParsePackets(int idx)
		{
			var version = GetBits(idx, 3);
			var type = GetBits(idx + 3, 3);
			var numBits = 6;

			if (type == 4)
			{
				var value = 0ul;
				while (true)
				{
					var bits = GetBits(idx + numBits, 5);
					numBits += 5;
					value = (value << 4) | (bits & 0xF);

					if ((bits & 0x10) == 0)
						return (
							new Packet
							{
								Version = version,
								Type = (PacketType)type,
								Value = value,
							},
							numBits);
				}
			}
			else
			{
				var lengthType = GetBits(idx + numBits, 1);
				numBits++;
				uint? childBitCount =
					lengthType == 0
						? GetBits(idx + numBits, 15)
						: default(uint?);
				uint? childPackets =
					lengthType == 1
						? GetBits(idx + numBits, 11)
						: default(uint?);
				numBits += lengthType == 0 ? 15 : 11;

				var packets = new List<Packet>();
				while (childBitCount > 0 || childPackets > 0)
				{
					var (p, i) = ParsePackets(idx + numBits);
					numBits += i;
					packets.Add(p);
					childBitCount -= (uint)i;
					childPackets -= 1;
				}

				return (
					new Packet
					{
						Version = version,
						Type = (PacketType)type,
						Children = packets,
					},
					numBits);
			}
		}

		var (packet, _) = ParsePackets(0);
		var part1 = packet.GetVersionSum().ToString();
		var part2 = packet.GetValue().ToString();
		return (part1, part2);
	}

	public enum PacketType : uint
	{
		Sum = 0,
		Product = 1,
		Minimum = 2,
		Maximum = 3,
		Literal = 4,
		GreaterThan = 5,
		LessThan = 6,
		EqualTo = 7,
	}

	private class Packet
	{
		public uint Version { get; set; }
		public PacketType Type { get; set; }
		public ulong Value { get; set; }
		public IReadOnlyList<Packet> Children { get; set; } = Array.Empty<Packet>();

		public int GetVersionSum() =>
			(int)Version + Children.Sum(c => c.GetVersionSum());

		public long GetValue() =>
			Type switch
			{
				PacketType.Sum => Children.Sum(c => c.GetValue()),
				PacketType.Product => Children.Aggregate(1L, (a, b) => a * b.GetValue()),
				PacketType.Minimum => Children.Aggregate(long.MaxValue, (a, b) => Math.Min(a, b.GetValue())),
				PacketType.Maximum => Children.Aggregate(long.MinValue, (a, b) => Math.Max(a, b.GetValue())),
				PacketType.Literal => (long)Value,
				PacketType.GreaterThan => Children[0].GetValue() > Children[1].GetValue() ? 1 : 0,
				PacketType.LessThan => Children[0].GetValue() < Children[1].GetValue() ? 1 : 0,
				PacketType.EqualTo => Children[0].GetValue() == Children[1].GetValue() ? 1 : 0,
			};

		public override string ToString() =>
			Type switch
			{
				PacketType.Sum => $"Sum({string.Join(", ", Children)})",
				PacketType.Product => $"Product({string.Join(", ", Children)})",
				PacketType.Minimum => $"Min({string.Join(", ", Children)})",
				PacketType.Maximum => $"Max({string.Join(", ", Children)})",
				PacketType.Literal => Value.ToString(),
				PacketType.GreaterThan => $"{Children[0]} > {Children[1]}",
				PacketType.LessThan => $"{Children[0]} < {Children[1]}",
				PacketType.EqualTo => $"{Children[0]} == {Children[1]}",
			};
	}
}
