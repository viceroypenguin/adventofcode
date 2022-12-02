namespace AdventOfCode;

public class Day_2021_20_Original : Day
{
	public override int Year => 2021;
	public override int DayNumber => 20;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		var tmp = input.GetLines();
		// algorithm is in first line
		var algorithm = tmp[0].Select(b => b == '#').ToList();
		// image is in remainder of text
		var image = tmp.Skip(1)
			.SelectMany((l, y) =>
				l.Select((b, x) => (x, y, b)))
			.ToDictionary(v => (v.x, v.y), v => v.b == '#');

		// do it 2 times
		PartA = DoPart(algorithm, image, 2).ToString();
		// do it 50 times
		PartB = DoPart(algorithm, image, 50).ToString();
	}

	private static Dictionary<(int x, int y), bool> ImageProcessingStep(
			List<bool> algorithm,
			Dictionary<(int x, int y), bool> image,
			int step)
	{
		// what are the dimensions?
		var minX = image.Min(kvp => kvp.Key.x) - 1;
		var maxX = image.Max(kvp => kvp.Key.x) + 1;
		var minY = image.Min(kvp => kvp.Key.y) - 1;
		var maxY = image.Max(kvp => kvp.Key.y) + 1;
		// keep track of the infinite expanse
		var def = algorithm[step % 2 == 0 ? 0 : 511];

		return Enumerable.Range(minY, maxY - minY + 1)
			.SelectMany(y => Enumerable.Range(minX, maxX - minX + 1)
				// for each pixel
				.Select(x =>
				{
					// collect neighboring values and coalesce into index
					var idx =
						(image.GetValueOrDefault((x - 1, y - 1), def) ? 1 : 0) << 8
						| (image.GetValueOrDefault((x, y - 1), def) ? 1 : 0) << 7
						| (image.GetValueOrDefault((x + 1, y - 1), def) ? 1 : 0) << 6
						| (image.GetValueOrDefault((x - 1, y), def) ? 1 : 0) << 5
						| (image.GetValueOrDefault((x, y), def) ? 1 : 0) << 4
						| (image.GetValueOrDefault((x + 1, y), def) ? 1 : 0) << 3
						| (image.GetValueOrDefault((x - 1, y + 1), def) ? 1 : 0) << 2
						| (image.GetValueOrDefault((x, y + 1), def) ? 1 : 0) << 1
						| (image.GetValueOrDefault((x + 1, y + 1), def) ? 1 : 0) << 0;
					// check the algorithm at index
					var b = algorithm[idx];
					// return value at point
					return (x, y, b);
				}))
			// convert back into the dictionary
			.ToDictionary(x => (x.x, x.y), x => x.b);
	}

	private void DumpImage(Dictionary<(int x, int y), bool> image, int step)
	{
		var minX = image.Min(kvp => kvp.Key.x) - 1;
		var maxX = image.Max(kvp => kvp.Key.x) + 1;
		var minY = image.Min(kvp => kvp.Key.y) - 1;
		var maxY = image.Max(kvp => kvp.Key.y) + 1;
		var def = step % 2 == 1;
		Console.WriteLine(
			string.Join(Environment.NewLine, Enumerable.Range(minY, maxY - minY + 1)
				.Select(y => string.Join("", Enumerable.Range(minX, maxX - minX + 1)
					.Select(x => image.GetValueOrDefault((x, y), def) ? '#' : '.')))));
		Console.WriteLine();
	}

	private static int DoPart(List<bool> algorithm, Dictionary<(int x, int y), bool> image, int steps)
	{
		// process the image n times
		for (int i = 1; i <= steps; i++)
			image = ImageProcessingStep(algorithm, image, i);
		// how many lit values are there?
		return image.Where(kvp => kvp.Value).Count();
	}
}
