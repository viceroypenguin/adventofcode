using System.Diagnostics;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 23, CodeType.Original)]
public class Day_23_Original : IPuzzle
{
	public (string part1, string part2) Solve(PuzzleInput input)
	{
		var map = input.Bytes.GetMap();

		var start = new Board() { _padding = 0, };
		for (var i = 0; i < 4; i++)
		{
			var room = start._rooms[i];
			room[0] = map[2][(i * 2) + 3];
			room[1] = map[3][(i * 2) + 3];
		}

		var goal = new Board();
		for (var i = 0; i < 4; i++)
		{
			var room = goal._rooms[i];
			room[0] = (byte)('A' + i);
			room[1] = (byte)('A' + i);
		}

		var part1 = SuperEnumerable.GetShortestPathCost<Board, int>(
			start,
			GetPossibleMovesA,
			goal).ToString();

		var insert = new[]
		{
			"DD"u8.ToArray(),
			"CB"u8.ToArray(),
			"BA"u8.ToArray(),
			"AC"u8.ToArray(),
		};

		for (var i = 0; i < 4; i++)
		{
			var room = start._rooms[i];
			room[3] = room[1];
			room[1] = insert[i][0];
			room[2] = insert[i][1];

			room = goal._rooms[i];
			room[2] = (byte)('A' + i);
			room[3] = (byte)('A' + i);
		}

		var part2 = SuperEnumerable.GetShortestPathCost<Board, int>(
			start,
			GetPossibleMovesB,
			goal).ToString();

		return (part1, part2);
	}

	private static int GetCost(byte b) =>
		b switch
		{
			A => 1,
			B => 10,
			C => 100,
			D => 1000,
			_ => throw new UnreachableException(),
		};

	private static (Board, int) MoveTokenToHallway(Board board, int cost, int room, int i, int hallway)
	{
		var roomSpan = board._rooms[room];
		var hallwaySpan = board._hallway;

		var moveCost = GetCost(roomSpan[i]);
		var steps = GetSteps(room, hallway) + i + 1;

		(roomSpan[i], hallwaySpan[hallway]) =
			(Empty, roomSpan[i]);
		return (board, cost + (moveCost * steps));
	}

	private static (Board, int) MoveTokenToRoom(Board board, int cost, int room, int i, int hallway)
	{
		var roomSpan = board._rooms[room];
		var hallwaySpan = board._hallway;

		var moveCost = GetCost(hallwaySpan[hallway]);
		var steps = GetSteps(room, hallway) + i + 1;

		(hallwaySpan[hallway], roomSpan[i]) =
			(Empty, hallwaySpan[hallway]);
		return (board, cost + (moveCost * steps));
	}

	private static (Board, int) MoveTokenToRoom(Board board, int cost, int from, int i, int to, int j)
	{
		var fromSpan = board._rooms[from];
		var toSpan = board._rooms[to];

		var moveCost = GetCost(fromSpan[i]);
		var steps = (Math.Abs(from - to) * 2) + i + 1 + j + 1;

		(fromSpan[i], toSpan[j]) =
			(Empty, fromSpan[i]);
		return (board, cost + (moveCost * steps));
	}

	private static int GetSteps(int room, int hallway) =>
		(room, hallway) switch
		{
			(0, 0) => 2,
			(0, 1) => 1,
			(0, 2) => 1,
			(0, 3) => 3,
			(0, 4) => 5,
			(0, 5) => 7,
			(0, 6) => 8,
			(1, 0) => 4,
			(1, 1) => 3,
			(1, 2) => 1,
			(1, 3) => 1,
			(1, 4) => 3,
			(1, 5) => 5,
			(1, 6) => 6,
			(2, 0) => 6,
			(2, 1) => 5,
			(2, 2) => 3,
			(2, 3) => 1,
			(2, 4) => 1,
			(2, 5) => 3,
			(2, 6) => 4,
			(3, 0) => 8,
			(3, 1) => 7,
			(3, 2) => 5,
			(3, 3) => 3,
			(3, 4) => 1,
			(3, 5) => 1,
			(3, 6) => 2,
			_ => throw new UnreachableException(),
		};

	private static IEnumerable<(Board, int)> GetPossibleMovesA(Board board, int cost) =>
		GetPossibleMoves(board, cost, 2);

	private static IEnumerable<(Board, int)> GetPossibleMovesB(Board board, int cost) =>
		GetPossibleMoves(board, cost, 4);

	private static IEnumerable<(Board, int)> GetPossibleMoves(Board board, int cost, int depth) =>
		GetHallToRoomMoves(board, cost, depth)
			.FallbackIfEmpty(GetRoomToRoomMoves(board, cost, depth))
			.FallbackIfEmpty(GetRoomToHallMoves(board, cost));

	private static IEnumerable<(Board, int)> GetHallToRoomMoves(Board board, int cost, int depth)
	{
		for (var i = 0; i < 7; i++)
		{
			// do we have a token to play with?
			if (board._hallway[i] == Empty) continue;

			// where are we trying to go?
			var dest = board._hallway[i] - 'A';

			// look for any blocking tokens
			var flag = false;
			for (var x = i + 1; !flag && x <= dest + 1; x++)
			{
				if (board._hallway[x] != Empty)
					flag = true;
			}

			for (var x = i - 1; !flag && x > dest + 1; x--)
			{
				if (board._hallway[x] != Empty)
					flag = true;
			}

			if (flag) continue;

			// are there any blocking tokens in the room?
			for (var x = 0; !flag && x < depth; x++)
			{
				if (board._rooms[dest][x] != Empty
					&& board._rooms[dest][x] != board._hallway[i])
				{
					flag = true;
				}
			}

			if (flag) continue;

			// ok, we can move to room
			for (var x = depth - 1; !flag && x >= 0; x--)
			{
				if (board._rooms[dest][x] == Empty)
				{
					yield return MoveTokenToRoom(board, cost, dest, x, i);
					flag = true;
				}
			}
		}
	}

	private static IEnumerable<(Board, int)> GetRoomToRoomMoves(Board board, int cost, int depth)
	{
		for (var from = 0; from < 4; from++)
		{
			for (var i = 0; i < depth; i++)
			{
				if (board._rooms[from][i] == Empty)
					continue;

				// where are we trying to go?
				var dest = board._rooms[from][i] - 'A';
				if (dest == from) break;

				// look for any blocking tokens in hallway
				var flag = false;
				for (var x = from + 2; !flag && x <= dest + 1; x++)
				{
					if (board._hallway[x] != Empty)
						flag = true;
				}

				for (var x = from + 1; !flag && x > dest + 1; x--)
				{
					if (board._hallway[x] != Empty)
						flag = true;
				}

				if (flag) break;

				// are there any blocking tokens in the room?
				for (var x = 0; !flag && x < depth; x++)
				{
					if (board._rooms[dest][x] != Empty
						&& board._rooms[dest][x] != board._rooms[from][i])
					{
						flag = true;
					}
				}

				if (flag) break;

				// ok, we can move to room
				for (var x = depth - 1; !flag && x >= 0; x--)
				{
					if (board._rooms[dest][x] == Empty)
					{
						yield return MoveTokenToRoom(board, cost, from, i, dest, x);
						flag = true;
					}
				}

				break;
			}
		}
	}

	private static IEnumerable<(Board, int)> GetRoomToHallMoves(Board board, int cost)
	{
		for (var from = 0; from < 4; from++)
		{
			for (var i = 0; i < 4; i++)
			{
				if (board._rooms[from][i] == Empty)
					continue;

				for (var hallway = 0; hallway < 7; hallway++)
				{
					// look for any blocking tokens in hallway
					var flag = false;
					for (var x = from + 2; !flag && x <= hallway; x++)
					{
						if (board._hallway[x] != Empty)
							flag = true;
					}

					for (var x = from + 1; !flag && x >= hallway; x--)
					{
						if (board._hallway[x] != Empty)
							flag = true;
					}

					// ok, we can move to hallway
					if (!flag)
						yield return MoveTokenToHallway(board, cost, from, i, hallway);
				}

				break;
			}
		}
	}

	private const byte A = (byte)'A';
	private const byte B = (byte)'B';
	private const byte C = (byte)'C';
	private const byte D = (byte)'D';
	private const byte Empty = 0;

	private record struct Board
	{
		public ValueArray4<ValueArray4<byte>> _rooms;
		public ValueArray8<byte> _hallway;
		public ulong _padding;
	}
}
