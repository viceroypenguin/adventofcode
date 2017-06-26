<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

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

void Main()
{
	var input = "yzbqklnj";
	GetPassword(input, 5).Dump("Part A");
	GetPassword(input, 6).Dump("Part B");
}
