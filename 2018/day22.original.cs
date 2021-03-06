﻿using System;
using System.Collections.Generic;
using System.Linq;
using Medallion.Collections;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2018_22_Original : Day
	{
		public override int Year => 2018;
		public override int DayNumber => 22;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var data = input.GetLines();
			var depth = Convert.ToInt32(data[0].Split()[1]);
			var coordStr = data[1].Split()[1].Split(',');
			var destination = (x: Convert.ToInt32(coordStr[0]), y: Convert.ToInt32(coordStr[1]));
			const int margin = 100;
			const int modulo = 20183;

			var ground = Enumerable.Range(0, destination.y + margin)
				.Select(x => Enumerable.Repeat(0, destination.x + margin).ToArray())
				.ToArray();

			for (int x = 0; x < destination.x + margin; x++)
				ground[0][x] = ((x % modulo) * 16807 + depth) % modulo;

			for (int y = 0; y < destination.y + margin; y++)
				ground[y][0] = ((y % modulo) * (48271 % modulo) + depth) % modulo;

			for (int y = 1; y < destination.y + margin; y++)
			{
				var curRow = ground[y];
				var prevRow = ground[y - 1];

				for (int x = 1; x < destination.x + margin; x++)
				{
					if (x == destination.x && y == destination.y)
						curRow[x] = depth % modulo;
					else
						curRow[x] = (prevRow[x] * curRow[x - 1] + depth) % modulo;
				}
			}

			Dump('A',
				ground.Take(destination.y + 1)
					.SelectMany(y => y.Take(destination.x + 1))
					.GroupBy(y => y % 3)
					.Sum(g => g.Key * g.Count()));

			const int neither = 0;
			const int torch = 1;
			const int gear = 2;

			var queue = new PriorityQueue<(int cost, (int x, int y, int equip) pos)>();
			var visited = new HashSet<(int x, int y, int equip)>();
			queue.Enqueue((0, (0, 0, torch)));

			while (queue.Any())
			{
				var (cost, pos) = queue.Dequeue();
				if (visited.Contains(pos))
					continue;

				visited.Add(pos);
				if (pos == (destination.x, destination.y, torch))
				{
					Dump('B', cost);
					return;
				}

				void addNeighbor(int x, int y, int equip, int newCost)
				{
					if (x < 0)
						return;
					if (y < 0)
						return;
					if (x >= destination.x + margin)
						return;
					if (y >= destination.y + margin)
						return;

					var type = ground[y][x] % 3;
					switch (type)
					{
						case 0:
							if (equip == torch || equip == gear)
								queue.Enqueue((newCost, (x, y, equip)));
							return;

						case 1:
							if (equip == neither || equip == gear)
								queue.Enqueue((newCost, (x, y, equip)));
							return;

						case 2:
							if (equip == neither || equip == torch)
								queue.Enqueue((newCost, (x, y, equip)));
							return;
					}
				}

				addNeighbor(pos.x + 1, pos.y, pos.equip, cost + 1);
				addNeighbor(pos.x, pos.y + 1, pos.equip, cost + 1);
				addNeighbor(pos.x - 1, pos.y, pos.equip, cost + 1);
				addNeighbor(pos.x, pos.y - 1, pos.equip, cost + 1);
				if (pos.equip != neither) addNeighbor(pos.x, pos.y, neither, cost + 7);
				if (pos.equip != torch) addNeighbor(pos.x, pos.y, torch, cost + 7);
				if (pos.equip != gear) addNeighbor(pos.x, pos.y, gear, cost + 7);
			}
		}
	}
}
