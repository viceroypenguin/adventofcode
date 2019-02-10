using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2015_10_Original : Day
	{
		public override int Year => 2015;
		public override int DayNumber => 10;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			foreach (var _ in Enumerable.Range(1, 50))
			{
				var curLength = 1;
				var curChar = input[0];
				var newInput = new List<byte>();

				var idx = 1;
				while (idx < input.Length)
				{
					var c = input[idx];
					idx++;

					if (c == curChar)
						curLength++;
					else
					{
						newInput.AddRange(curLength.ToString().ToCharArray().Select(x => (byte)x));
						newInput.Add(curChar);

						curLength = 1;
						curChar = c;
					}
				}

				newInput.AddRange(curLength.ToString().ToCharArray().Select(x => (byte)x));
				newInput.Add(curChar);

				input = newInput.ToArray();

				if (_ == 40 || _ == 50)
					Dump(_ == 40 ? 'A' : 'B', input.Length);
			}
		}
	}
}
