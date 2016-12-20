<Query Kind="Statements" />

var input = 
@"Disc #1 has 5 positions; at time=0, it is at position 2.
Disc #2 has 13 positions; at time=0, it is at position 7.
Disc #3 has 17 positions; at time=0, it is at position 10.
Disc #4 has 3 positions; at time=0, it is at position 2.
Disc #5 has 19 positions; at time=0, it is at position 9.
Disc #6 has 7 positions; at time=0, it is at position 0.";

var regex = new Regex(@"Disc #(?<disc_num>\d+) has (?<num_positions>\d+) positions; at time=0, it is at position (?<initial_position>\d+).", RegexOptions.Compiled);

var discs = 
	input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
		.Select(s=>regex.Match(s))
		.Select(m=>new
		{
			disc = Convert.ToInt32(m.Groups["disc_num"].Value),
			num_positions = Convert.ToInt32(m.Groups["num_positions"].Value),
			initial_position = Convert.ToInt32(m.Groups["initial_position"].Value),
		})
		.ToList();

for (int initialTime = 0; ; initialTime++)
{
	var flag = true;
	for (int capsulePosition = 1; flag && capsulePosition <= discs.Count; capsulePosition++)
	{
		var disc = discs[capsulePosition - 1];
		var discPosition = (initialTime + capsulePosition + disc.initial_position) % disc.num_positions;
		if (discPosition != 0) flag = false;
	}
	
	if (flag)
	{
		initialTime.Dump("Part A");
		break;
	}
}

discs.Add(new { disc = 7, num_positions = 11, initial_position = 0 });
for (int initialTime = 0; ; initialTime++)
{
	var flag = true;
	for (int capsulePosition = 1; flag && capsulePosition <= discs.Count; capsulePosition++)
	{
		var disc = discs[capsulePosition - 1];
		var discPosition = (initialTime + capsulePosition + disc.initial_position) % disc.num_positions;
		if (discPosition != 0) flag = false;
	}
	
	if (flag)
	{
		initialTime.Dump("Part B");
		break;
	}
}
