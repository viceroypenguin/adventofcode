<Query Kind="Program" />

enum State
{
	OutsideString,
	NormalCharacter,
	SpecialCharacter,
	HexCharacter1,
	HexCharacter2,
}

void Main()
{
	var filename = @"C:\users\stuart.turner\desktop\input.txt";

	var totalStringCharacters = 0;
	var totalLiteralCharacters = 0;
	var state = State.OutsideString;
	int chr;
	using (var file = File.OpenRead(filename))
		while ((chr = file.ReadByte()) != -1)
		{
			switch (state)
			{
				case State.OutsideString:
					if (chr == (int)'"')
					{
						state = State.NormalCharacter;
						totalLiteralCharacters += 3;
						totalStringCharacters++;
					}
					continue;

				case State.NormalCharacter:
					if (chr == (int)'"')
					{
						state = State.OutsideString;
						totalLiteralCharacters += 3;
						totalStringCharacters++;
					}
					else if (chr == (int)'\\')
					{
						state = State.SpecialCharacter;
						totalLiteralCharacters += 2;
						totalStringCharacters++;
					}
					else
					{
						totalLiteralCharacters++;
						totalStringCharacters++;
					}
					continue;

				case State.SpecialCharacter:
					if (chr == (int)'x')
					{
						totalLiteralCharacters++;
						totalStringCharacters++;
						state = State.HexCharacter1;
					}
					else
					{
						totalLiteralCharacters+=2;
						totalStringCharacters++;
						state = State.NormalCharacter;
					}
					continue;

				case State.HexCharacter1:
					totalLiteralCharacters++;
					totalStringCharacters++;
					state = State.HexCharacter2;
					continue;

				case State.HexCharacter2:
					totalLiteralCharacters++;
					totalStringCharacters++;
					state = State.NormalCharacter;
					continue;
			}
		}

	$"totalLiteralCharacters: {totalLiteralCharacters}".Dump();
	$"totalStringCharacters: {totalStringCharacters}".Dump();
	(totalLiteralCharacters - totalStringCharacters).Dump();
}

// Define other methods and classes here
