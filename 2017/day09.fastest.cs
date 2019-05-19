﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
	public class Day_2017_09_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 9;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			int score = 0, garbage = 0, depth = 0, g = 0;
			for (int i = 0; i < input.Length; i++)
			{
				var c = input[i];
				switch (g | c)
				{
					case '}': score += depth; goto case '{';
					case '{': depth += '|' - c; break;
					case '<':
					case '>' | 0x80: g ^= 0x80; break;
					case '!' | 0x80: i++; break;
					default: garbage += g != 0 ? 1 : 0; break;
				}
			}

			Dump('A', score);
			Dump('B', garbage);
		}
	}
}