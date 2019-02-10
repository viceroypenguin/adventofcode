using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
	public class Day_2015_04_Original : Day
	{
		public override int Year => 2015;
		public override int DayNumber => 4;
		public override CodeType CodeType => CodeType.Original;

		bool HasLeadingZeros(int numZeros, byte[] bytes)
		{
			for (int i = 0; i < numZeros; i++)
			{
				var mask = (i % 2 == 0) ? (byte)0xf0 : (byte)0x0f;
				if ((bytes[i / 2] & mask) != 0x00)
					return false;
			}
			return true;
		}

		int GetPassword(string input, int numZeros)
		{
			using (var md5 = MD5.Create())
				for (var i = 0; ; i++)
				{
					var hashSrc = input + i.ToString();
					var hashSrcBytes = Encoding.ASCII.GetBytes(hashSrc);
					var hash = md5.ComputeHash(hashSrcBytes);
					if (HasLeadingZeros(numZeros, hash))
						return i;
				}
		}

		protected override void ExecuteDay(byte[] input)
		{
			var inp = input.GetString();
			Dump('A', GetPassword(inp, 5));
			Dump('B', GetPassword(inp, 6));
		}
	}
}
