<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

var regex = new Regex(@"^((?<spin>s(?<amt>\d+))|(?<xchg>x(?<xchg_a>\d+)/(?<xchg_b>\d+))|(?<partner>p(?<part_a>\w)/(?<part_b>\w)))$", RegexOptions.Compiled);
var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day16.input.txt"))
	.Split(',')
	.Select(inst => regex.Match(inst))
	.ToList();

const int length = 16;
var programs = Enumerable.Range(0, length)
	.Select(i => (char)(i + (int)'a'))
	.ToArray();

char[] Round(char[] @in)
{
	var @out = @in.ToArray();

	foreach (var m in input)
	{
		if (m.Groups["spin"].Success)
		{
			var amt = Convert.ToInt32(m.Groups["amt"].Value);
			@out = @out.Skip(length - amt).Concat(@out.Take(length - amt)).ToArray();
		}
		else if (m.Groups["xchg"].Success)
		{
			var idx_a = Convert.ToInt32(m.Groups["xchg_a"].Value);
			var idx_b = Convert.ToInt32(m.Groups["xchg_b"].Value);
			var tmp = @out[idx_a];
			@out[idx_a] = @out[idx_b];
			@out[idx_b] = tmp;
		}
		else if (m.Groups["partner"].Success)
		{
			var a = m.Groups["part_a"].Value[0];
			var b = m.Groups["part_b"].Value[0];
			for (int i = 0; i < length; i++)
				if (@out[i] == a) @out[i] = b;
				else if (@out[i] == b) @out[i] = a;
		}
	}

	return @out;
}

programs = Round(programs);
string.Join("", programs).Dump("Part A");

var k = 1;
var programs_dbl = Round(programs);
var l = 2;

while (true)
{
	programs = Round(programs);
	programs_dbl = Round(Round(programs_dbl));
	k++;
	l += 2;
	
	if (programs.SequenceEqual(programs_dbl))
		break;
}

var final_round = 1_000_000_000;
var remainder = final_round % k;
for (int i = 0; i < remainder; i++)
	programs = Round(programs);

string.Join("", programs).Dump("Part B");
