using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2017_08_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 8;
		public override CodeType CodeType => CodeType.Fastest;

		private const int REGISTER_COUNT = 32 * 32 * 32;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected unsafe override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var registers = stackalloc int[REGISTER_COUNT];
			var maxValue = 0;

			var idx = 0;
			var c = input[idx];
			while (true)
			{
				while ((c = input[idx]) < 'a')
					if (++idx >= input.Length)
						goto done;

				// destination register
				var dst = c - 'a';
				while ((c = input[++idx]) >= 'a')
					dst = (dst << 5) + c - 'a';

				// inc/dec
				var neg = input[idx + 1] == 'd';

				// amt
				c = input[idx += 5];
				if (c == '-')
				{
					neg = !neg;
					c = input[++idx];
				}
				var num = c - '0';
				while ((c = input[++idx]) >= '0')
					num = num * 10 + c - '0';
				if (neg) num = -num;

				// src
				c = input[idx += 4];
				var src = c - 'a';
				while ((c = input[++idx]) >= 'a')
					src = (src << 5) + c - 'a';

				// op
				var op = input[idx + 1] << 8 | input[idx + 2];

				if ((c = input[idx += 3]) == ' ')
					c = input[++idx];

				// chk
				neg = false;
				if (c == '-')
				{
					neg = !neg;
					c = input[++idx];
				}
				var chk = c - '0';
				while ((c = input[++idx]) >= '0')
					chk = chk * 10 + c - '0';
				if (neg) chk = -chk;

				src = registers[src];
				var flag = op switch
				{
					0x3c20 => src < chk,
					0x3e20 => src > chk,
					0x3c3d => src <= chk,
					0x3e3d => src >= chk,
					0x3d3d => src == chk,
					0x213d => src != chk,
					_ => false,
				};

				if (flag)
				{
					chk = (registers[dst] += num);
					if (chk > maxValue)
						maxValue = chk;
				}
			}

done:
			var maxEnd = 0;
			for (int i = 0; i < REGISTER_COUNT; i++)
				if (registers[i] > maxEnd)
					maxEnd = registers[i];

			PartA = maxEnd.ToString();
			PartB = maxValue.ToString();
		}
	}
}
