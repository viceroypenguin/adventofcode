namespace AdventOfCode;

public class Day_2016_07_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 7;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		Func<string, bool> isABBA = (str) =>
		{
			for (int i = 0; i < str.Length - 3; i++)
				if (str[i] == str[i + 3] && str[i + 1] == str[i + 2] && str[i] != str[i + 1])
					return true;
			return false;
		};

		var splits =
			input.GetLines()
				.Select(str =>
				{
					var outside = new List<string>();
					var inside = new List<string>();

					var idx = 0;
					while (idx < str.Length)
					{
						var newIdx = str.IndexOf('[', idx);
						if (newIdx < 0)
						{
							outside.Add(str.Substring(idx));
							break;
						}

						outside.Add(str.Substring(idx, newIdx - idx));
						idx = newIdx;

						newIdx = str.IndexOf(']', idx);
						inside.Add(str.Substring(idx + 1, newIdx - idx - 1));
						idx = newIdx + 1;
					}

					return new { str, inside, outside };
				})
				.ToList();

		Dump('A',
			splits
				.Where(x => x.outside.Any(isABBA) && !x.inside.Any(isABBA))
				.Count());

		Func<IList<string>, IList<string>> getABAs = (strs) =>
		{
			var abas = new List<string>();
			foreach (var str in strs)
				for (int i = 0; i < str.Length - 2; i++)
					if (str[i] == str[i + 2] && str[i] != str[i + 1])
						abas.Add(str.Substring(i, 3));
			return abas;
		};

		Func<IList<string>, IList<string>, bool> checkBABs = (strs, abas) =>
		{
			foreach (var aba in abas)
			{
				var bab = string.Join("", new[] { aba[1], aba[0], aba[1] });
				if (strs.Any(s => s.IndexOf(bab) >= 0))
					return true;
			}
			return false;
		};

		Dump('B',
			splits
				.Select(x => new
				{
					x.str,
					x.inside,
					x.outside,
					abas = getABAs(x.outside),
				})
				.Where(x => checkBABs(x.inside, x.abas))
				.Count());
	}
}
