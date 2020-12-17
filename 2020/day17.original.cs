using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using MoreLinq;
using static AdventOfCode.Helpers;

namespace AdventOfCode
{
	public class Day_2020_17_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 17;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			DoPartA(input);
			DoPartB(input);
		}

		private void DoPartA(byte[] input)
		{
			var state = new Dictionary<(int x, int y, int z), bool>(1024);
			int _x = 0, _y = 0;
			foreach (var c in input)
			{
				if (c == '\n') { _x = 0; _y++; }
				else state[(_x++, _y, 0)] = c == '#';
			}

			var count = new Dictionary<(int x, int y, int z), int>(1024);
			var dirs = Enumerable.Range(-1, 3)
				.SelectMany(x => Enumerable.Range(-1, 3)
					.SelectMany(y => Enumerable.Range(-1, 3)
						.Select(z => (x, y, z))))
				.Where(d => d != (0, 0, 0))
				.ToArray();
			for (int i = 0; i < 6; i++)
			{
				count.Clear();

				// so count has everything, and we can rely on that in final foreach
				foreach (var p in state.Keys)
					count[p] = 0;

				foreach (var ((x, y, z), alive) in state.Where(kvp => kvp.Value))
					foreach (var (dx, dy, dz) in dirs)
						count[(x + dx, y + dy, z + dz)] =
							count.GetValueOrDefault((x + dx, y + dy, z + dz)) + 1;

				foreach (var (p, c) in count)
					state[p] = (state.GetValueOrDefault(p), c) switch
					{
						(true, >= 2 and <= 3) => true,
						(false, 3) => true,
						_ => false,
					};
			}

			PartA = state.Where(kvp => kvp.Value).Count().ToString();
		}

		private void DoPartB(byte[] input)
		{
			var state = new Dictionary<(int x, int y, int z, int w), bool>(8192);
			int _x = 0, _y = 0;
			foreach (var c in input)
			{
				if (c == '\n') { _x = 0; _y++; }
				else state[(_x++, _y, 0, 0)] = c == '#';
			}

			var count = new Dictionary<(int x, int y, int z, int w), int>(8192);
			var dirs = Enumerable.Range(-1, 3)
				.SelectMany(x => Enumerable.Range(-1, 3)
					.SelectMany(y => Enumerable.Range(-1, 3)
						.SelectMany(z => Enumerable.Range(-1, 3)
							.Select(w => (x, y, z, w)))))
				.Where(d => d != (0, 0, 0, 0))
				.ToArray();
			for (int i = 0; i < 6; i++)
			{
				count.Clear();

				// so count has everything, and we can rely on that in final foreach
				foreach (var p in state.Keys)
					count[p] = 0;

				foreach (var ((x, y, z, w), alive) in state.Where(kvp => kvp.Value))
					foreach (var (dx, dy, dz, dw) in dirs)
						count[(x + dx, y + dy, z + dz, w + dw)] =
							count.GetValueOrDefault((x + dx, y + dy, z + dz, w + dw)) + 1;

				foreach (var (p, c) in count)
					state[p] = (state.GetValueOrDefault(p), c) switch
					{
						(true, >= 2 and <= 3) => true,
						(false, 3) => true,
						_ => false,
					};
			}

			PartB = state.Where(kvp => kvp.Value).Count().ToString();
		}
	}
}
