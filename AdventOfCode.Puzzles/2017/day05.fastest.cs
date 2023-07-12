namespace AdventOfCode.Puzzles._2017;

[Puzzle(2017, 05, CodeType.Fastest)]
public class Day_05_Fastest : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var span = input.Span;

		Span<int> nums1 = stackalloc int[span.Length / 4];
		Span<int> nums2 = stackalloc int[span.Length / 4];
		var count = 0;
		for (var i = 0; i < span.Length;)
		{
			var (x, y) = span[i..].AtoI();
			nums1[count] = nums2[count] = x;
			count++;
			i += y + 1;
		}

		return (
			DoPartA(nums1[..count]).ToString(),
			DoPartB(nums2[..count]).ToString());
	}

	private static int DoPartA(Span<int> nums)
	{
		var steps = 0;
		var cnt = (uint)nums.Length;
		for (var i = 0; i >= 0 && i < cnt; steps++)
			i += nums[i]++;

		return steps;
	}

	private static int DoPartB(Span<int> nums)
	{
		var steps = 0;
		var cnt = (uint)nums.Length;
		for (var i = 0; i >= 0 && i < cnt; steps++)
		{
			var j = nums[i];
			nums[i] += -(((j - 3) >> 31) << 1) - 1;
			i += j;
		}

		return steps;
	}
}
