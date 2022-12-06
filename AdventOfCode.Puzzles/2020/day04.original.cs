namespace AdventOfCode.Puzzles._2020;

[Puzzle(2020, 4, CodeType.Original)]
public class Day_04_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var required = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", };

		var passports = input.Lines
			.Segment(l => string.IsNullOrWhiteSpace(l))
			.Select(l => l.SelectMany(s => s.Split())
				.Where(s => !string.IsNullOrWhiteSpace(s))
				.Select(s => s.Split(':'))
				.ToDictionary(
					a => a[0],
					a => a[1]));

		var part1 = passports
			.Where(p => required.All(r => p.ContainsKey(r)))
			.Count()
			.ToString();

		var isValidPassports = passports
			.Select(p => (
				p,
				isValid:
					required.All(r => p.ContainsKey(r))
					&& Convert.ToInt32(p["byr"]).Between(1920, 2002)
					&& Convert.ToInt32(p["iyr"]).Between(2010, 2020)
					&& Convert.ToInt32(p["eyr"]).Between(2020, 2030)
					&& IsValidHeight(p["hgt"])
					&& Regex.IsMatch(p["hcl"], "^#[0-9a-f]{6}$")
					&& new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(p["ecl"])
					&& Regex.IsMatch(p["pid"], "^\\d{9}$")))
			.ToArray();

		var part2 = isValidPassports.Where(x => x.isValid).Count().ToString();

		static bool IsValidHeight(string s) =>
			s.Length >= 4
			&& (s[^2..] switch
			{
				"in" => Convert.ToInt32(s[..^2]).Between(59, 76),
				"cm" => Convert.ToInt32(s[..^2]).Between(150, 193),
				_ => false,
			});

		return (part1, part2);
	}
}
