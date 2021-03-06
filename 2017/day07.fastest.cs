﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2017_07_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 7;
		public override CodeType CodeType => CodeType.Fastest;

		private unsafe struct Line
		{
			public long Id;
			public long Weight;
			public fixed long Children[8];
		}

		private unsafe struct HashEntry
		{
			public long Id;
			public int Idx;
			public int ParentIdx;
		}

		private const int HASH_SIZE = 4093;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected unsafe override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day07.c
			var hashTable = stackalloc HashEntry[HASH_SIZE];

			var data = stackalloc Line[input.Length / 16];
			var d = &data[0];

			var pidx = 0;
			var n = 0L;
			var cid = 0;
			foreach (var c in input)
			{
				if (c >= 'a')
					n = n << 8 | c;
				else if (c == '>')
					;
				else if (c >= '0')
					n = n * 10 + c - '0';
				else if (c == '(')
				{
					d->Id = n;
					GetHashEntry(hashTable, n)->Idx = pidx;
					n = 0;
				}
				else if (c == ')')
				{
					d->Weight = n;
					n = 0;
				}
				else if (c == ',')
				{
					d->Children[cid++] = n;
					GetHashEntry(hashTable, n)->ParentIdx = pidx;
					n = 0;
				}
				else if (c == '\n')
				{
					if (n != 0)
					{
						d->Children[cid++] = n;
						GetHashEntry(hashTable, n)->ParentIdx = pidx;
						n = 0;
					}

					d++;
					pidx++;
					cid = 0;
				}
			}

			n = data[0].Id;
			while (true)
			{
				var idx = GetHashEntry(hashTable, n)->ParentIdx;
				if (idx == 0)
					break;
				n = data[idx].Id;
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
				PartA = new string(chars, idx + 1, chars.Length - idx - 1);
			}


			PartB = (-GetRebalancedWeight(data, hashTable, root)).ToString();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe HashEntry* GetHashEntry(HashEntry* hashTable, long id)
		{
			var hash = id % HASH_SIZE;
			while (hashTable[hash].Id != 0)
			{
				if (hashTable[hash].Id == id)
					return &hashTable[hash];
				if (++hash == HASH_SIZE)
					hash = 0;
			}
			hashTable[hash].Id = id;
			return &hashTable[hash];
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private unsafe long GetRebalancedWeight(Line* data, HashEntry* hashTable, long n)
		{
			var node = &data[GetHashEntry(hashTable, n)->Idx];
			var weight = node->Weight;
			var balance = -1L;

			// leaf node case
			if (node->Children[0] == 0)
				return weight;

			for (var ctr = 0; node->Children[ctr] != 0; ctr++)
			{
				var i = GetRebalancedWeight(data, hashTable, node->Children[ctr]);
				if (i < 0)
					return i;

				weight += i;
				if (ctr == 0)
					balance = i;
				else if (balance != i)
				{
					if (ctr == 1)
					{
						// need to figure out if 0 or 1 is the balance weight
						var j = node->Children[ctr + 1];
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
					var newWeight = data[GetHashEntry(hashTable, node->Children[ctr])->Idx].Weight + variance;
					return -newWeight;
				}
			}

			return weight;
		}
	}
}
