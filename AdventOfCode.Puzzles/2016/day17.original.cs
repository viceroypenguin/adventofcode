using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Puzzles._2016;

[Puzzle(2016, 17, CodeType.Original)]
public class Day_17_Original : IPuzzle
{

#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
#pragma warning disable CA1850 // Prefer static 'HashData' method over 'ComputeHash'
	public (string, string) Solve(PuzzleInput input)
	{
		using var md5 = MD5.Create();
		var passcode = input.Lines[0];

		IEnumerable<(int x, int y, string path)> GetNextPositions(
			(int x, int y, string path) state)
		{
			if (state.x == 3 && state.y == 3)
				yield break;

			var hashSrc = passcode + state.path;
			var bytes = Encoding.ASCII.GetBytes(hashSrc);
			var hash = md5.ComputeHash(bytes);

			if (state.x > 0 && (hash[0] >> 4) >= 0xb) yield return (state.x - 1, state.y, state.path + 'U');
			if (state.x < 3 && (hash[0] & 0xf) >= 0xb) yield return (state.x + 1, state.y, state.path + 'D');
			if (state.y > 0 && (hash[1] >> 4) >= 0xb) yield return (state.x, state.y - 1, state.path + 'L');
			if (state.y < 3 && (hash[1] & 0xf) >= 0xb) yield return (state.x, state.y + 1, state.path + 'R');
		}

		var paths = SuperEnumerable
			.TraverseBreadthFirst(
				(x: 0, y: 0, path: string.Empty),
				GetNextPositions)
			.Where(p => p.x == 3 && p.y == 3)
			.ToList();

		return (
			paths[0].path,
			paths[^1].path.Length.ToString());
	}
#pragma warning restore CA1850 // Prefer static 'HashData' method over 'ComputeHash'
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
}
