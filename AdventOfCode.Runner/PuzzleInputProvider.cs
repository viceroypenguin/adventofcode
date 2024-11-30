using System.Net;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode.Runner;

public sealed class PuzzleInputProvider
{
	public static PuzzleInputProvider Instance { get; } = new();

	private readonly HttpClient _httpClient;

	private PuzzleInputProvider()
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			.AddEnvironmentVariables()
			.Build();
		var sessionId = configuration["sessionId"];

		var baseAddress = new Uri("https://adventofcode.com");
		var cookieContainer = new CookieContainer();
		cookieContainer.Add(baseAddress, new Cookie("session", sessionId));

		_httpClient = new HttpClient(
			new HttpClientHandler
			{
				CookieContainer = cookieContainer,
				AutomaticDecompression = DecompressionMethods.All,
			})
		{
			BaseAddress = baseAddress,
			DefaultRequestHeaders =
			{
				{ "User-Agent", ".NET/9.0 (https://github.com/viceroypenguin/adventofcode by stuart@turner-isageek.com)" },
			},
		};
	}

	public PuzzleInput GetRawInput(int year, int day)
	{
		var inputFile = $"Inputs/{year}/day{day:00}.input.txt";
		_ = Directory.CreateDirectory(Path.GetDirectoryName(inputFile)!);
		if (!File.Exists(inputFile))
		{
			var response = _httpClient.GetAsync($"{year}/day/{day}/input")
				.GetAwaiter()
				.GetResult();

			var text = response
				.EnsureSuccessStatusCode()
				.Content.ReadAsStringAsync()
				.GetAwaiter()
				.GetResult();
			File.WriteAllText(inputFile, text);
		}

		return new(
			File.ReadAllBytes(inputFile),
			File.ReadAllText(inputFile),
			File.ReadAllLines(inputFile));
	}
}
