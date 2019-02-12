using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2017_03_Original : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 3;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			(int x, int y) GetPosition(int i)
			{
				var maxRoot = (int)Math.Sqrt(i - 1);
				var rank = (maxRoot + 1) / 2;
				return GetPositionWithRank(i, rank);
			}

			(int x, int y) GetPositionWithRank(int i, int rank)
			{
				if (i <= 1) return (0, 0);

				var rankRoot = (rank * 2) + 1;
				var botRight = rankRoot * rankRoot;
				var sideLength = rank * 2;

				var sideNum = (botRight - i) / sideLength;
				var lastInSide = botRight - (sideLength * sideNum);
				switch (sideNum)
				{
					case 0:
						// bottom; count backwards from (rank, -rank)
						return (x: rank - (lastInSide - i), y: -rank);
					case 1:
						// left; count backwards from (-rank, -rank)
						return (x: -rank, y: (lastInSide - i) - rank);
					case 2:
						// top; count backwards from (-rank, rank)
						return (x: (lastInSide - i) - rank, y: +rank);
					case 3:
						// right; count backwards from (rank, rank)
						return (x: +rank, y: rank - (lastInSide - i));
					default:
						throw new InvalidOperationException("??");
				}
			}

			var number = Convert.ToInt32(input.GetString());

			{
				var position = GetPosition(number);
				var distance = Math.Abs(position.x) + Math.Abs(position.y);
				Dump('A', distance);
			}

			var gridSize = (int)Math.Ceiling(Math.Log10(number));
			var gridSideLength = Math.Max(gridSize * 2, 6);
			gridSize = gridSideLength / 2;
			var grid = new int?[gridSideLength + 1, gridSideLength + 1];
			Func<int, int, int> getPositionSum = (int x, int y) =>
			{
				return
					(grid[x - 1, y] ?? 0) +
					(grid[x + 1, y] ?? 0) +
					(grid[x, y - 1] ?? 0) +
					(grid[x, y + 1] ?? 0) +
					(grid[x - 1, y - 1] ?? 0) +
					(grid[x - 1, y + 1] ?? 0) +
					(grid[x + 1, y - 1] ?? 0) +
					(grid[x + 1, y + 1] ?? 0);
			};

			var idx = 1;
			var gridRank = 0;
			var gridBotRight = 1;

			var pos = (x: gridSize, y: gridSize);
			grid[pos.x, pos.y] = 1;

			while (grid[pos.x, pos.y] < number)
			{
				if (gridBotRight == idx)
				{
					gridRank++;
					var tmp = (gridRank * 2) + 1;
					gridBotRight = tmp * tmp;
				}

				idx++;
				{
					var (x, y) = GetPositionWithRank(idx, gridRank);
					// translate to printed output as expected
					pos = (x: -y + gridSize, y: x + gridSize);
				}

				grid[pos.x, pos.y] = getPositionSum(pos.x, pos.y);
			}

			Dump('B', grid[pos.x, pos.y]);
		}
	}
}
