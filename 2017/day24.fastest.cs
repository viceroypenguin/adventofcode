using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
	public class Day_2017_24_Fastest : Day
	{
		public override int Year => 2017;
		public override int DayNumber => 24;
		public override CodeType CodeType => CodeType.Fastest;

		private class Component
		{
			public int PortA;
			public int PortB;
			public bool Used;
		}

		private List<Component>[] lookupByA;
		private List<Component>[] lookupByB;

		private int maxStrength = -1;
		private int longestPath = -1;
		private int maxLongestPath = -1;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			// borrowed liberally from https://github.com/Voltara/advent2017-fast/blob/master/src/day24.c
			var ports = new List<Component>(input.Length / 4);
			{
				int a = 0, n = 0;
				for (int i = 0; i < input.Length; i++)
				{
					var c = input[i];
					if (c == '/')
					{
						a = n;
						n = 0;
					}
					else if (c == '\n')
					{
						ports.Add(new Component { PortA = a, PortB = n, });
						n = 0;
					}
					else if (c >= '0')
						n = n * 10 + c - '0';
				}
			}

			var maxPort = -1;
			for (int i = 0; i < ports.Count; i++)
			{
				var p = ports[i];
				if (p.PortA > maxPort)
					maxPort = p.PortA;
				if (p.PortB > maxPort)
					maxPort = p.PortB;
			}

			var byA = new List<Component>[maxPort + 1];
			var byB = new List<Component>[maxPort + 1];
			for (int i = 0; i < ports.Count; i++)
			{
				var p = ports[i];
				var arr = byA[p.PortA] = byA[p.PortA] ?? new List<Component>();
				arr.Add(p);

				if (p.PortA != p.PortB)
				{
					arr = byB[p.PortB] = byB[p.PortB] ?? new List<Component>();
					arr.Add(p);
				}
			}
			lookupByA = byA;
			lookupByB = byB;

			CalculateStrength(0, 0, 0);

			Dump('A', maxStrength);
			Dump('B', maxLongestPath);
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		private void CalculateStrength(int port, int curStrength, int curLength)
		{
			if (curStrength > maxStrength)
			{
				maxStrength = curStrength;
			}

			if (curLength > longestPath)
			{
				maxLongestPath = curStrength;
				longestPath = curLength;
			}
			else if (curLength == longestPath && curStrength > maxLongestPath)
			{
				maxLongestPath = curStrength;
			}

			curLength++;
			curStrength += port;

			var arr = lookupByA[port];
			for (int i = 0; i < arr?.Count; i++)
			{
				var p = arr[i];
				if (!p.Used)
				{
					p.Used = true;
					CalculateStrength(p.PortB, curStrength + p.PortB, curLength);
					p.Used = false;
				}
			}

			arr = lookupByB[port];
			for (int i = 0; i < arr?.Count; i++)
			{
				var p = arr[i];
				if (!p.Used)
				{
					p.Used = true;
					CalculateStrength(p.PortA, curStrength + p.PortA, curLength);
					p.Used = false;
				}
			}
		}
	}
}
