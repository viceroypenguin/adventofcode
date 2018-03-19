<Query Kind="Program" />

char[][] input;

(int x, int y) coords;
char direction;
int count;

Queue<char> queue;

void Main()
{
	input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day19.input.txt"))
		.Select(s => s.ToArray())
		.ToArray();

	coords = (x: 0, y: input[0].Select((c, i) => new { c, i }).First(x => x.c == '|').i);
	direction = 's';
	count = 0;

	queue = new Queue<char>();

	while (MoveNext())
		count++;

	string.Join("", queue).Dump("Part A");
	count.Dump("Part B");
}

bool MoveNext()
{
	// $"Coords: {coords}; Value: {input[coords.x][coords.y]}".Dump();
	switch (input[coords.x][coords.y])
	{
		case '|':
		case '-':
			MoveStraight();
			return true;

		case '+':
			ChangeDirection();
			return true;

		case ' ':
			return false;

		default:
			queue.Enqueue(input[coords.x][coords.y]);
			goto case '|';
	}
}

void MoveStraight()
{
	switch (direction)
	{
		case 'n':
			coords.x--;
			return;

		case 's':
			coords.x++;
			return;

		case 'w':
			coords.y--;
			break;

		case 'e':
			coords.y++;
			break;

		default:
			throw new InvalidOperationException("wtf?");
	}
}

void ChangeDirection()
{
	if (direction != 's' &&
		coords.x > 0 &&
		input[coords.x - 1][coords.y] != ' ')
	{
		direction = 'n';
		MoveStraight();
		return;
	}

	if (direction != 'n' &&
		coords.x < (input.Length - 1) &&
		input[coords.x + 1][coords.y] != ' ')
	{
		direction = 's';
		MoveStraight();
		return;
	}

	if (direction != 'e' &&
		coords.y > 0 &&
		input[coords.x][coords.y - 1] != ' ')
	{
		direction = 'w';
		MoveStraight();
		return;
	}

	if (direction != 'w' &&
		coords.y < (input[coords.x].Length - 1) &&
		input[coords.x][coords.y + 1] != ' ')
	{
		direction = 'e';
		MoveStraight();
		return;
	}
}
