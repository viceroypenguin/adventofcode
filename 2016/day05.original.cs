using System.Security.Cryptography;

namespace AdventOfCode;

public class Day_2016_05_Original : Day
{
	public override int Year => 2016;
	public override int DayNumber => 5;
	public override CodeType CodeType => CodeType.Original;

	protected override void ExecuteDay(byte[] input)
	{
		if (input == null) return;

		DoPartA(input);
		DoPartB(input);
	}

	private void DoPartA(byte[] input)
	{
		using (var md5 = MD5.Create())
		{
			var key = input.GetString();
			var password = "";

			var cnt = 0L;
			while (password.Length < 8)
			{
				cnt++;
				var hashSrc = key + cnt.ToString();
				var bytes = Encoding.ASCII.GetBytes(hashSrc);
				var hash = md5.ComputeHash(bytes);
				if (hash[0] == 0x00 &&
					hash[1] == 0x00 &&
					(hash[2] & 0xf0) == 0x00)
				{
					password = password + (hash[2] & 0x0f).ToString("x");
				}
			}

			Dump('A', password);
		}
	}

	private void DoPartB(byte[] input)
	{
		using (var md5 = MD5.Create())
		{
			var key = input.GetString();
			var password = new char?[8];

			var cnt = 0L;
			while (password.Any(c => !c.HasValue))
			{
				cnt++;
				var hashSrc = input + cnt.ToString();
				var bytes = Encoding.ASCII.GetBytes(hashSrc);
				var hash = md5.ComputeHash(bytes);
				if (hash[0] == 0x00 &&
					hash[1] == 0x00 &&
					(hash[2] & 0xf0) == 0x00)
				{
					var idx = hash[2] & 0x0f;
					if (idx >= 8) continue;
					if (password[idx].HasValue) continue;

					password[idx] = ((hash[3] & 0xf0) >> 4).ToString("x")[0];
					Dump('B',
						string.Join("", password.Select(c => c ?? '_')));
				}
			}
		}
	}
}
