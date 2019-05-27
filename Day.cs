using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

		private List<string> _output = new List<string>();

		public int TotalMicroseconds { get; protected set; }

		public void Execute()
		{
			var input = Year != 0
				? File.ReadAllBytes($@"..\..\..\{Year}\day{DayNumber:00}.input.txt")
				: null;

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
