using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2017_11_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 11;
		public override CodeType CodeType => CodeType.Fastest;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			int dir = 0, nw = 0, n = 0, ne = 0, max = 0;
			foreach (var c in input)
			{
				if (c >= 'a')
					dir = (dir << 8) + c;
				else
				{
					switch (dir)
					{
						case 0x6e: // n
							if (n < 0) n++;
							else if (nw < 0 || ne < 0) { ne++; nw++; }
							else n++;
							break;

						case 0x73: // s
							if (n > 0) n--;
							else if (nw > 0 || ne > 0) { nw--; ne--; }
							else n--;
							break;

						case 0x6e65: //ne
							if (ne < 0) ne++;
							else if (n < 0 || nw > 0) { nw--; n++; }
							else ne++;
							break;

						case 0x6e77: //nw
							if (nw < 0) nw++;
							else if (n < 0 || ne > 0) { ne--; n++; }
							else nw++;
							break;

						case 0x7377: //sw
							if (ne > 0) ne--;
							else if (n > 0 || nw < 0) { nw++; n--; }
							else ne--;
							break;

						case 0x7365: //se
							if (nw > 0) nw--;
							else if (n > 0 || ne < 0) { ne++; n--; }
							else nw--;
							break;
					}
					max = Math.Max(max, Math.Abs(nw) + Math.Abs(n) + Math.Abs(ne));
					dir = 0;
				}
			}

			Dump('A', Math.Abs(nw) + Math.Abs(n) + Math.Abs(ne));
			Dump('B', max);
		}
	}
}
