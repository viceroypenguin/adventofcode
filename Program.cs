using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Configuration;
using MoreLinq;

namespace AdventOfCode
{
	public static class Program
	{
		public static HttpClient HttpClient { get; private set; }

		public static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

			var configuration = builder.Build();
			var sessionId = configuration["sessionId"];

			if (string.IsNullOrWhiteSpace(sessionId))
				throw new ArgumentNullException(nameof(sessionId), "Please provide an AoC session id in the configuration file.");

			var baseAddress = new Uri("https://adventofcode.com");
			var cookieContainer = new CookieContainer();
			cookieContainer.Add(baseAddress, new Cookie("session", sessionId));

			HttpClient = new HttpClient(
				new HttpClientHandler
				{
					CookieContainer = cookieContainer,
					AutomaticDecompression = DecompressionMethods.All,
				})
			{
				BaseAddress = baseAddress,
			};

			// Pre-JIT Day and Stopwatch
			new DummyDay().Execute();

			var days = GetDays();
			if (args.Contains("-o"))
				days = days.Where(d => d.CodeType == CodeType.Original);
			else if (args.Contains("-f"))
				days = days.Where(d => d.CodeType == CodeType.Fastest);

			if (args.Contains("-y"))
			{
				var year = Convert.ToInt32(args[args.Index().Single(a => a.Value == "-y").Key + 1]);
				days = days.Where(d => d.Year == year);
			}

			if (args.Contains("-d"))
			{
				var day = Convert.ToInt32(args[args.Index().Single(a => a.Value == "-d").Key + 1]);
				days = days.Where(d => d.DayNumber == day);
			}

			foreach (var d in days)
				d.Execute();

			Console.WriteLine();

			foreach (var g in days.GroupBy(d => new { d.Year, d.CodeType, }))
			{
				Console.WriteLine(
					string.Join(Environment.NewLine,
						g.Select(d => $"Year {d.Year}, Day {d.DayNumber,2}, Type {d.CodeType,9}      :   {d.TotalMicroseconds,13:N0} μs")));

				Console.WriteLine("--------------------------------------------------------");
				Console.WriteLine(
					$"Year {g.Key.Year},         Type {g.Key.CodeType,9} Total:   {g.Sum(d => d.TotalMicroseconds),13:N0} μs");
				Console.WriteLine();
			}
		}

		private static IEnumerable<Day> GetDays() =>
			Assembly.GetExecutingAssembly().GetTypes()
				.Where(t => t.BaseType == typeof(Day))
				.Where(t => t.Name != nameof(DummyDay))
				.Select(t => (Day)Activator.CreateInstance(t))
				.OrderBy(d => d.Year)
				.ThenBy(d => d.DayNumber)
				.ThenBy(d => d.CodeType)
				.ToArray();
	}
}
