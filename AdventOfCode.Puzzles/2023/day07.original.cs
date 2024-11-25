
namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 07, CodeType.Original)]
public partial class Day_07_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var hands = input.Lines
			.Select(l => l.Split())
			.Select(l => (hand: new Hand(l[0]), bid: int.Parse(l[1])))
			.OrderBy(x => x.hand)
			.ToList();

		var part1 = hands
			.Select((x, i) => (long)(i + 1) * x.bid)
			.Sum();

		hands = input.Lines
			.Select(l => l.Split())
			.Select(l => (hand: new Hand(l[0], part1: false), bid: int.Parse(l[1])))
			.OrderBy(x => x.hand)
			.ToList();

		var part2 = hands
			.Select((x, i) => (long)(i + 1) * x.bid)
			.Sum();

		return (part1.ToString(), part2.ToString());
	}

	public enum HandType
	{
		None = 0,

		FiveOfAKind,
		FourOfAKind,
		FullHouse,
		ThreeOfAKind,
		TwoPair,
		OnePair,
		HighCard,
	}

	private sealed class Hand : IComparable<Hand>, IEquatable<Hand>
	{
		private readonly HandType _handType;
		private readonly char[] _cards;
		private readonly bool _part1;

		public Hand(string hand, bool part1 = true)
		{
			_cards = hand.ToCharArray();
			_part1 = part1;
			var cards = _cards
				.GroupBy(x => x, (x, g) => (card: x, count: g.Count(), order: s_p2CardOrder.IndexOf(x)))
				.OrderByDescending(x => x.count)
				.ThenBy(x => x.order)
				.ToList();

			if (!part1)
			{
				var jCount = cards.FirstOrDefault(x => x.card == 'J').count;
				if (jCount is not 0 and not 5)
				{
					_ = cards.RemoveAll(x => x.card == 'J');
					cards[0] = (cards[0].card, cards[0].count + jCount, cards[0].order);
				}
			}

			_handType = (cards[0].count, cards[1].count) switch
			{
				(5, _) => HandType.FiveOfAKind,
				(4, _) => HandType.FourOfAKind,
				(3, 2) => HandType.FullHouse,
				(3, _) => HandType.ThreeOfAKind,
				(2, 2) => HandType.TwoPair,
				(2, _) => HandType.OnePair,
				_ => HandType.HighCard,
			};
		}

		public int CompareTo(Hand? other)
		{
			if (other == null) throw new InvalidOperationException();

			var cmp = _handType.CompareTo(other._handType);
			if (cmp != 0) return -cmp;

			foreach (var (l, r) in _cards.Zip(other._cards))
			{
				cmp = _part1
					? s_p1CardOrder.IndexOf(l).CompareTo(s_p1CardOrder.IndexOf(r))
					: s_p2CardOrder.IndexOf(l).CompareTo(s_p2CardOrder.IndexOf(r));

				if (cmp != 0) return -cmp;
			}

			return 0;
		}

		public bool Equals(Hand? other) =>
			other is not null
			&& CompareTo(other) == 0;

		public override bool Equals(object? obj) => Equals(obj as Hand);
		public override int GetHashCode() => throw new InvalidOperationException();

		private static readonly List<char> s_p1CardOrder = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];
		private static readonly List<char> s_p2CardOrder = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];
	}
}
