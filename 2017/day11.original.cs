using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2017_11_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 11;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var dirs = input.GetString()
				.Trim()
				.Split(',')
				.ToList();

			(int nw, int n, int ne) Move((int nw, int n, int ne) old, string dir)
			{
				switch (dir)
				{
					case "n":
						if (old.n < 0)
							return (old.nw, old.n + 1, old.ne);
						if (old.ne < 0)
							return (old.nw + 1, old.n, old.ne + 1);
						if (old.nw < 0)
							return (old.nw + 1, old.n, old.ne + 1);
						return (old.nw, old.n + 1, old.ne);

					case "s":
						if (old.n > 0)
							return (old.nw, old.n - 1, old.ne);
						if (old.ne > 0)
							return (old.nw - 1, old.n, old.ne - 1);
						if (old.nw > 0)
							return (old.nw - 1, old.n, old.ne - 1);
						return (old.nw, old.n - 1, old.ne);

					case "ne":
						if (old.ne < 0)
							return (old.nw, old.n, old.ne + 1);
						if (old.nw > 0)
							return (old.nw - 1, old.n + 1, old.ne);
						if (old.n < 0)
							return (old.nw - 1, old.n + 1, old.ne);
						return (old.nw, old.n, old.ne + 1);

					case "nw":
						if (old.nw < 0)
							return (old.nw + 1, old.n, old.ne);
						if (old.ne > 0)
							return (old.nw, old.n + 1, old.ne - 1);
						if (old.n < 0)
							return (old.nw, old.n + 1, old.ne - 1);
						return (old.nw + 1, old.n, old.ne);

					case "sw":
						if (old.ne > 0)
							return (old.nw, old.n, old.ne - 1);
						if (old.nw < 0)
							return (old.nw + 1, old.n - 1, old.ne);
						if (old.n > 0)
							return (old.nw + 1, old.n - 1, old.ne);
						return (old.nw, old.n, old.ne - 1);

					case "se":
						if (old.nw > 0)
							return (old.nw - 1, old.n, old.ne);
						if (old.ne < 0)
							return (old.nw, old.n - 1, old.ne + 1);
						if (old.n > 0)
							return (old.nw, old.n - 1, old.ne + 1);
						return (old.nw - 1, old.n, old.ne);

					default:
						throw new InvalidOperationException($"Unknown direction: {dir}.");
				}
			}

			var positions = dirs
				.Scan((nw: 0, n: 0, ne: 0), Move)
				.Select(x => Math.Abs(x.nw) + Math.Abs(x.n) + Math.Abs(x.ne))
				.ToList();

			Dump('A', positions[positions.Count - 1]);
			Dump('B', positions.Max());
		}
	}
}
