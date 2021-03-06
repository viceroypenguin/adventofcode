﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode
{
	public enum CodeType
	{
		Original,
		Fastest,
	}

	public abstract class Day
	{

		public abstract int Year { get; }
		public abstract int DayNumber { get; }
		public abstract CodeType CodeType { get; }
		protected abstract void ExecuteDay(byte[] input);

		protected string PartA { get; set; }
		protected string PartB { get; set; }

		protected void Dump(char part, object output) =>
			_output.Add($"Year {Year}, Day {DayNumber}, Part {part}: {output}");

		protected void DumpScreen(char part, IEnumerable<IEnumerable<char>> output)
		{
			_output.Add($"Year {Year}, Day {DayNumber}, Part {part}:");
			_output.AddRange(output
				.Select(l => string.Join("", l)));
		}

		private readonly List<string> _output = new List<string>();

		public int TotalMicroseconds { get; protected set; }

		public void Execute()
		{
			if (Year == 0) return;

			var inputFile = $"{Year}\\day{DayNumber:00}.input.txt";
			if (!Directory.Exists(Year.ToString()))
				Directory.CreateDirectory(Year.ToString());
			if (!File.Exists(inputFile))
			{
				// bad form. I just don't want to make Execute async for performance
				var response = Program.HttpClient.GetAsync($"{Year}/day/{DayNumber}/input").Result;
				response.EnsureSuccessStatusCode();
				File.WriteAllText(inputFile, response.Content.ReadAsStringAsync().Result);
			}

			var input = File.ReadAllBytes(inputFile);

			ExecuteDay(null);

			var sw = new Stopwatch();
			sw.Start();
			ExecuteDay(input);
			sw.Stop();

			if (!_output.Any())
			{
				Dump('A', PartA);
				Dump('B', PartB);
			}

			if (Year != 0)
			{
				Console.WriteLine(string.Join(Environment.NewLine, _output));
				Console.WriteLine();
			}

			if (TotalMicroseconds == 0)
				TotalMicroseconds = (int)
					(sw.Elapsed.TotalMilliseconds * 1000);
		}
	}

	public class DummyDay : Day
	{
		public override int Year => 0;
		public override int DayNumber => 0;
		public override CodeType CodeType => CodeType.Original;
		protected override void ExecuteDay(byte[] input) { Dump('A', ""); DumpScreen('B', new[] { new[] { ' ' } }); }
	}
}
