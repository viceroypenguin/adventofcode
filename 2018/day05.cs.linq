<Query Kind="Statements">
  <NuGetReference>morelinq</NuGetReference>
  <Namespace>MoreLinq</Namespace>
</Query>

//var input = "bBdabAcCaCBAcCcaDA";
var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day05.input.txt")).TrimEnd();

int GetReducedPolymerLength(string polymer)
{
	var characters = polymer
		.Select(c => (c, isActive: true))
		.ToArray();
	
	for (int i = 1; i < characters.Length; i++)
	{
		int j = i - 1;
		while (j >= 0 && !characters[j].isActive)
			j--;
		if (j >= 0 &&
			char.IsUpper(characters[i].c) != char.IsUpper(characters[j].c) &&
			char.ToUpper(characters[i].c) == char.ToUpper(characters[j].c))
		{
			characters[i].isActive = false;
			characters[j].isActive = false;
		}
	}
	
	return characters.Where(x => x.isActive).Count();
}

GetReducedPolymerLength(input)
	.Dump("Part A");

Enumerable.Range(0, 26)
	.Select(i => (char)(i + (int)'a'))
	.Select(c => Regex.Replace(input, c.ToString(), "", RegexOptions.IgnoreCase))
	.Min(s => GetReducedPolymerLength(s))
	.Dump("Part B");
	