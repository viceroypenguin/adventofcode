using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 07, CodeType.Fastest)]
public unsafe class Day_07_Fastest : IPuzzle
{
	[StructLayout(LayoutKind.Auto)]
	private struct Line
	{
		public long _id;
		public long _weight;
		public ValueArray8<long> _children;
	}

	[StructLayout(LayoutKind.Auto)]
	private struct HashEntry
	{
		public long _id;
		public int _idx;
		public int _parentIdx;
	}

	private const int HASH_SIZE = 4093;

	public (string, string) Solve(PuzzleInput input)
	{
		// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day07.c
		var hashTable = stackalloc HashEntry[HASH_SIZE];

		var span = input.Span;
		var data = stackalloc Line[span.Length / 16];
		var d = &data[0];

		var pidx = 0;
		var n = 0L;
		var cid = 0;
		foreach (var c in span)
		{
			if (c >= 'a')
			{
				n = (n << 8) | c;
			}
			else if (c == '>')
			{
				continue;
			}
			else if (c >= '0')
			{
				n = (n * 10) + c - '0';
			}
			else if (c == '(')
			{
				d->_id = n;
				GetHashEntry(hashTable, n)->_idx = pidx;
				n = 0;
			}
			else if (c == ')')
			{
				d->_weight = n;
				n = 0;
			}
			else if (c == ',')
			{
				d->_children[cid++] = n;
				GetHashEntry(hashTable, n)->_parentIdx = pidx;
				n = 0;
			}
			else if (c == '\n')
			{
				if (n != 0)
				{
					d->_children[cid++] = n;
					GetHashEntry(hashTable, n)->_parentIdx = pidx;
					n = 0;
				}

				d++;
				pidx++;
				cid = 0;
			}
		}

		n = data[0]._id;
		while (true)
		{
			var idx = GetHashEntry(hashTable, n)->_parentIdx;
			if (idx == 0)
				break;
			n = data[idx]._id;
		}

		var root = n;
		{
			var chars = new char[sizeof(long)];
			var idx = chars.Length - 1;
			while (n != 0)
			{
				chars[idx--] = (char)(n & 0xff);
				n >>= 8;
			}

			return (
				new string(chars, idx + 1, chars.Length - idx - 1),
				(-GetRebalancedWeight(data, hashTable, root)).ToString());
		}
	}

	private HashEntry* GetHashEntry(HashEntry* hashTable, long id)
	{
		var hash = id % HASH_SIZE;
		while (hashTable[hash]._id != 0)
		{
			if (hashTable[hash]._id == id)
				return &hashTable[hash];
			if (++hash == HASH_SIZE)
				hash = 0;
		}

		hashTable[hash]._id = id;
		return &hashTable[hash];
	}

	private long GetRebalancedWeight(Line* data, HashEntry* hashTable, long n)
	{
		var node = &data[GetHashEntry(hashTable, n)->_idx];
		var weight = node->_weight;
		var balance = -1L;

		// leaf node case
		if (node->_children[0] == 0)
			return weight;

		for (var ctr = 0; node->_children[ctr] != 0; ctr++)
		{
			var i = GetRebalancedWeight(data, hashTable, node->_children[ctr]);
			if (i < 0)
				return i;

			weight += i;
			if (ctr == 0)
			{
				balance = i;
			}
			else if (balance != i)
			{
				if (ctr == 1)
				{
					// need to figure out if 0 or 1 is the balance weight
					var j = node->_children[ctr + 1];
					// assume if only two children, then can't be unbalanced
					if (j == 0) continue;

					j = GetRebalancedWeight(data, hashTable, j);
					if (j == i)
					{
						ctr = 0;
						i = balance;
						balance = j;
					}
				}

				var variance = balance - i;
				var newWeight = data[GetHashEntry(hashTable, node->_children[ctr])->_idx]._weight + variance;
				return -newWeight;
			}
		}

		return weight;
	}
}
