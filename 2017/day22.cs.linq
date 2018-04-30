<Query Kind="Statements">
  <Namespace>System.Collections.Specialized</Namespace>
</Query>

var map = new Dictionary<(int x, int y), char>();
var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day22.input.txt"));
var n = input.Length / 2;
foreach (var l in input.Select((l, i) => (l, i)))
	foreach (var c in l.l.Select((c, j) => (c, j)))
		map[(n - l.i, c.j - n)] = c.c == '#' ? 'I' : 'C';

var position = (x: 0, y: 0, dir: 'n');
var enable = 0;

void StepPartA()
{
	var flag = map.ContainsKey((position.x, position.y))
		? map[(position.x, position.y)] == 'I'
		: false;
	position.dir =
		position.dir == 'n' ?
			flag ? 'e' : 'w' :
		position.dir == 's' ?
			flag ? 'w' : 'e' :
		position.dir == 'e' ?
			flag ? 's' : 'n' :
			/* position.dir == 'w' ? */
			flag ? 'n' : 's';

	if (!flag)
		enable++;
	map[(position.x, position.y)] =
		flag ? 'C' : 'I';

	switch (position.dir)
	{
		case 'n':
			position.x++;
			break;

		case 's':
			position.x--;
			break;

		case 'e':
			position.y++;
			break;

		case 'w':
			position.y--;
			break;
	}
}

for (int i = 0; i < 10000; i++)
	StepPartA();
enable.Dump("Part A");

map = new Dictionary<(int x, int y), char>();
foreach (var l in input.Select((l, i) => (l, i)))
	foreach (var c in l.l.Select((c, j) => (c, j)))
		map[(n - l.i, c.j - n)] = c.c == '#' ? 'I' : 'C';

position = (x: 0, y: 0, dir: 'n');
enable = 0;
void StepPartB()
{
	var state = map.ContainsKey((position.x, position.y))
		? map[(position.x, position.y)]
		: 'C';
	position.dir =
		position.dir == 'n' ?
			state == 'C' ? 'w' :
			state == 'W' ? 'n' :
			state == 'I' ? 'e' :
			/*state == 'F' ? */ 's' :
		position.dir == 's' ?
			state == 'C' ? 'e' :
			state == 'W' ? 's' :
			state == 'I' ? 'w' :
			/*state == 'F' ? */ 'n' :
		position.dir == 'e' ?
			state == 'C' ? 'n' :
			state == 'W' ? 'e' :
			state == 'I' ? 's' :
			/*state == 'F' ? */ 'w' :
		/* position.dir == 'w' ? */
			state == 'C' ? 's' :
			state == 'W' ? 'w' :
			state == 'I' ? 'n' :
			/*state == 'F' ? */ 'e';

	var newState =
		state == 'C' ? 'W' :
		state == 'W' ? 'I' :
		state == 'I' ? 'F' :
		/* state == 'F' ? */ 'C';
	if (newState == 'I')
		enable++;
	map[(position.x, position.y)] = newState;

	switch (position.dir)
	{
		case 'n':
			position.x++;
			break;

		case 's':
			position.x--;
			break;

		case 'e':
			position.y++;
			break;

		case 'w':
			position.y--;
			break;
	}
}

for (int i = 0; i < 10000000; i++)
	StepPartB();
enable.Dump("Part B");
