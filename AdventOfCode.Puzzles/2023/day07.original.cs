
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

	private class Hand : IComparable<Hand>
	{
		private readonly HandType _handType;
		private readonly char[] _cards;
		private readonly bool _part1;

		public Hand(string hand, bool part1 = true)
		{
			_cards = hand.ToCharArray();
			_part1 = part1;
			var cards = _cards
				.GroupBy(x => x, (x, g) => (card: x, count: g.Count(), order: _p2CardOrder.IndexOf(x)))
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

			if (cards[0].count == 5)
				_handType = HandType.FiveOfAKind;
			else if (cards[0].count == 4)
				_handType = HandType.FourOfAKind;
			else if (cards[0].count == 3 && cards[1].count == 2)
				_handType = HandType.FullHouse;
			else if (cards[0].count == 3)
				_handType = HandType.ThreeOfAKind;
			else if (cards[0].count == 2 && cards[1].count == 2)
				_handType = HandType.TwoPair;
			else if (cards[0].count == 2)
				_handType = HandType.OnePair;
			else
				_handType = HandType.HighCard;
		}

		public int CompareTo(Hand? other)
		{
			if (other == null) throw new InvalidOperationException();

			var cmp = this._handType.CompareTo(other._handType);
			if (cmp != 0) return -cmp;

			foreach (var (l, r) in _cards.Zip(other._cards))
			{
				if (_part1)
					cmp = _p1CardOrder.IndexOf(l).CompareTo(_p1CardOrder.IndexOf(r));
				else
					cmp = _p2CardOrder.IndexOf(l).CompareTo(_p2CardOrder.IndexOf(r));

				if (cmp != 0) return -cmp;
			}

			return 0;
		}

		private static readonly List<char> _p1CardOrder = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];
		private static readonly List<char> _p2CardOrder = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];
	}
}
