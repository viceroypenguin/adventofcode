namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 19, CodeType.Original)]
public class Day_19_Original : IPuzzle
{
	private byte[][] _map;

	private (int x, int y) _coords;
	private char _direction;
	private int _count;

	private Queue<byte> _queue;

	public (string, string) Solve(PuzzleInput input)
	{
		_map = input.Bytes.GetMap();

		_coords = (x: 0, y: _map[0].Select((c, i) => new { c, i }).First(x => x.c == '|').i);
		_direction = 's';
		_count = 0;

		_queue = new();

		while (MoveNext())
			_count++;

		var partA = string.Join("", _queue.Select(c => (char)c));
		var partB = _count;

		return (partA, partB.ToString());
	}

	private bool MoveNext()
	{
		// $"Coords: {coords}; Value: {map[coords.x][coords.y]}".Dump();
		switch (_map[_coords.x][_coords.y])
		{
			case (byte)'|':
			case (byte)'-':
				MoveStraight();
				return true;

			case (byte)'+':
				ChangeDirection();
				return true;

			case (byte)' ':
				return false;

			default:
				_queue.Enqueue(_map[_coords.x][_coords.y]);
				goto case (byte)'|';
		}
	}

	private void MoveStraight()
	{
		switch (_direction)
		{
			case 'n':
				_coords.x--;
				return;

			case 's':
				_coords.x++;
				return;

			case 'w':
				_coords.y--;
				break;

			case 'e':
				_coords.y++;
				break;

			default:
				throw new InvalidOperationException("wtf?");
		}
	}

	private void ChangeDirection()
	{
		if (_direction != 's' &&
			_coords.x > 0 &&
			_map[_coords.x - 1][_coords.y] != ' ')
		{
			_direction = 'n';
			MoveStraight();
			return;
		}

		if (_direction != 'n' &&
			_coords.x < (_map.Length - 1) &&
			_map[_coords.x + 1][_coords.y] != ' ')
		{
			_direction = 's';
			MoveStraight();
			return;
		}

		if (_direction != 'e' &&
			_coords.y > 0 &&
			_map[_coords.x][_coords.y - 1] != ' ')
		{
			_direction = 'w';
			MoveStraight();
			return;
		}

		if (_direction != 'w' &&
			_coords.y < (_map[_coords.x].Length - 1) &&
			_map[_coords.x][_coords.y + 1] != ' ')
		{
			_direction = 'e';
			MoveStraight();
			return;
		}
	}
}
