using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using MoreLinq;
using static AdventOfCode.Helpers;

namespace AdventOfCode
{
	public class Day_2020_19_Original : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 19;
		public override CodeType CodeType => CodeType.Original;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var segments = input.GetLines(StringSplitOptions.None)
				.Segment(string.IsNullOrWhiteSpace)
				.ToArray();

			var rulesBase = segments[0]
				.Select(x => x.Split(':', StringSplitOptions.TrimEntries))
				.ToDictionary(x => x[0], x => x[1]);
			var processed = new Dictionary<string, string>();

			string BuildRegex(string input)
			{
				if (processed.TryGetValue(input, out var s))
					return s;

				var orig = rulesBase[input];
				if (orig.StartsWith('\"'))
					return processed[input] = orig.Replace("\"", "");

				if (!orig.Contains("|"))
					return processed[input] = string.Join("", orig.Split().Select(BuildRegex));

				return processed[input] = 
					"(" +
					string.Join("", orig.Split().Select(x => x == "|" ? x : BuildRegex(x))) +
					")";
			}

			var regex = new Regex("^" + BuildRegex("0") + "$");
			PartA = segments[1].Count(regex.IsMatch).ToString();

			regex = new Regex($@"^({BuildRegex("42")})+(?<open>{BuildRegex("42")})+(?<close-open>{BuildRegex("31")})+(?(open)(?!))$");
			PartB = segments[1].Count(regex.IsMatch).ToString();
		}
	}
}
