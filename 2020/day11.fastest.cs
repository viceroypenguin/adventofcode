using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode
{
	public class Day_2020_11_Fastest : Day
	{
		public override int Year => 2020;
		public override int DayNumber => 11;
		public override CodeType CodeType => CodeType.Fastest;

		private const byte Floor = 16;
		private const byte Occupied = 1;
		private const byte Empty = 0;

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		[SkipLocalsInit]
		protected override void ExecuteDay(byte[] input)
		{
			if (input == null) return;

			var width = FindWidth(input);
			var height = input.Length / (width + 1);

			var lanes = (width + 31) / 32 * 32;
			var simdMapWidth = lanes + 4;

			Span<byte> map1 = stackalloc byte[simdMapWidth * (height + 4)];
			Span<byte> map2 = stackalloc byte[simdMapWidth * (height + 4)];
			Span<byte> counts = stackalloc byte[simdMapWidth * (height + 4)];
			Span<(int i, int j)> gaps = stackalloc (int, int)[2048];

			CopyMap(input, map1, width, simdMapWidth);
			map2.Fill(Floor);

			PartA = DoPartA(map1, map2, counts, lanes, simdMapWidth, height).ToString();

			CopyMap(input, map1, width, simdMapWidth);
			map2.Fill(Floor);

			PartB = DoPartB(map1, map2, counts, gaps, lanes, simdMapWidth, height).ToString();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int FindWidth(byte[] input)
		{
			var arr = MemoryMarshal.Cast<byte, Vector256<byte>>(new ReadOnlySpan<byte>(input));
			var newLine = Vector256.Create((byte)'\n');
			int i = 0;
			foreach (var a in arr)
			{
				var mask = Avx2.MoveMask(Avx2.CompareEqual(a, newLine));
				if (mask != 0)
					return i + (int)Bmi1.TrailingZeroCount((uint)mask);

				i += Vector256<byte>.Count;
			}
			return 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static void CopyMap(byte[] input, Span<byte> map, int width, int simdMapWidth)
		{
			int my = 0;
			for (; my < simdMapWidth * 2; my++)
				map[my] = Floor;
			for (int iy = 0; iy < input.Length; iy += width + 1, my += simdMapWidth)
			{
				map[my] = Floor;
				map[my + 1] = Floor;
				for (int ix = 0, mx = 2; ix < width; ix++, mx++)
					map[my + mx] = input[iy + ix] == '.' ? Floor : Empty;
				for (int mx = width + 2; mx < simdMapWidth; mx++)
					map[my + mx] = Floor;
			}
			for (; my < map.Length; my++)
				map[my] = Floor;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static int DoPartA(Span<byte> map1, Span<byte> map2, Span<byte> counts, int lanes, int simdMapWidth, int height)
		{
			var changed = false;

			var ul = -simdMapWidth - 1;
			var u = -simdMapWidth;
			var ur = -simdMapWidth + 1;
			var l = -1;
			var r = +1;
			var dl = +simdMapWidth - 1;
			var d = +simdMapWidth;
			var dr = +simdMapWidth + 1;

			var threshold = Vector256.Create((byte)3);
			do
			{
				for (int i = 0, my = simdMapWidth * 2 + 2; i < height; i++, my += simdMapWidth)
				{
					for (int x = 0; x < lanes; x += Vector256<byte>.Count)
					{
						var cnt = Vector256<byte>.Zero;
						var @base = my + x;

						// above (-1, 0, +1)
						cnt = AddCount(cnt, map1, @base + ul);
						cnt = AddCount(cnt, map1, @base + u);
						cnt = AddCount(cnt, map1, @base + ur);
						// horizontal (-1, +1)
						cnt = AddCount(cnt, map1, @base + l);
						cnt = AddCount(cnt, map1, @base + r);
						// below (-1, 0, +1)
						cnt = AddCount(cnt, map1, @base + dl);
						cnt = AddCount(cnt, map1, @base + d);
						cnt = AddCount(cnt, map1, @base + dr);

						Unsafe.As<byte, Vector256<byte>>(ref counts[@base]) =
							// skip floors
							Avx2.And(cnt, Vector256.Create((byte)0x0f));
					}
				}

				changed = UpdateMap(map1, map2, counts, threshold, lanes, simdMapWidth, height);

				// swap maps
				var tmp = map2;
				map2 = map1;
				map1 = tmp;
			} while (changed);

			return CountOccupied(map1, lanes, simdMapWidth, height);
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static Vector256<byte> AddCount(Vector256<byte> cnt, Span<byte> map, int index) =>
			Avx2.Add(cnt, Unsafe.As<byte, Vector256<byte>>(ref map[index]));

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static int DoPartB(Span<byte> map1, Span<byte> map2, Span<byte> counts, Span<(int i, int j)> gaps, int lanes, int simdMapWidth, int height)
		{
			int maxGap = FindGaps(gaps, map1, lanes, simdMapWidth, height);
			gaps = gaps[..maxGap];

			var changed = false;

			var ul = -simdMapWidth - 1;
			var u = -simdMapWidth;
			var ur = -simdMapWidth + 1;
			var l = -1;
			var r = +1;
			var dl = +simdMapWidth - 1;
			var d = +simdMapWidth;
			var dr = +simdMapWidth + 1;

			var threshold = Vector256.Create((byte)4);
			do
			{
				for (int i = 0, my = simdMapWidth * 2 + 2; i < height; i++, my += simdMapWidth)
				{
					for (int x = 0; x < lanes; x += Vector256<byte>.Count)
					{
						var cnt = Vector256<byte>.Zero;
						var @base = my + x;

						// above (-1, 0, +1)
						cnt = AddCount(cnt, map1, @base + ul, @base + ul + ul);
						cnt = AddCount(cnt, map1, @base + u, @base + u + u);
						cnt = AddCount(cnt, map1, @base + ur, @base + ur + ur);
						// horizontal (-1, +1)
						cnt = AddCount(cnt, map1, @base + l, @base + l + l);
						cnt = AddCount(cnt, map1, @base + r, @base + r + r);
						// below (-1, 0, +1)
						cnt = AddCount(cnt, map1, @base + dl, @base + dl + dl);
						cnt = AddCount(cnt, map1, @base + d, @base + d + d);
						cnt = AddCount(cnt, map1, @base + dr, @base + dr + dr);

						Unsafe.As<byte, Vector256<byte>>(ref counts[@base]) =
							// skip floors
							Avx2.And(cnt, Vector256.Create((byte)0x0f));
					}
				}

				foreach (var (i, j) in gaps)
				{
					counts[i] += map1[j];
					counts[j] += map1[i];
				}

				changed = UpdateMap(map1, map2, counts, threshold, lanes, simdMapWidth, height);

				// swap maps
				var tmp = map2;
				map2 = map1;
				map1 = tmp;
			} while (changed);

			return CountOccupied(map1, lanes, simdMapWidth, height);
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static int FindGaps(Span<(int i, int j)> gaps, Span<byte> map, int lanes, int simdMapWidth, int height)
		{
			var maxGap = -1;

			for (int y = 0, my = simdMapWidth * 2 + 2; y < height; y++, my += simdMapWidth)
			{
				// horizontal gaps
				for (int x = 1, mx = my; x < lanes; x++, mx++)
				{
					if (map[mx] != Floor) continue;
					var g1 = mx - 1;
					if (map[g1] == Floor) continue;

					while (x < lanes && map[mx] == Floor)
					{
						x++; mx++;
					}
					if (x < lanes && mx - g1 > 2)
						gaps[++maxGap] = (g1, mx);
				}

				// only need to check horizontal on the last few rows
				if (y >= height - 3)
					continue;

				// vertical gaps
				for (
					int x = 0, mx = my, fx = mx + simdMapWidth, ffx = fx + simdMapWidth;
					x < lanes;
					x++, mx++, fx++, ffx++)
				{
					if (map[mx] == Floor) continue;
					if (map[fx] != Floor) continue;
					if (map[ffx] != Floor) continue;

					for (int y1 = y + 3, f1 = ffx + simdMapWidth; y1 < height; y1++, f1 += simdMapWidth)
						if (map[f1] != Floor)
						{
							gaps[++maxGap] = (mx, f1);
							break;
						}
				}

				// forward diagonal gaps
				for (
					int x = 0, mx = my, fx = mx + simdMapWidth + 1, ffx = fx + simdMapWidth + 1;
					x < lanes - 3;
					x++, mx++, fx++, ffx++)
				{
					if (map[mx] == Floor) continue;
					if (map[fx] != Floor) continue;
					if (map[ffx] != Floor) continue;

					for (
						int y1 = y + 3, x1 = x + 3, f1 = ffx + simdMapWidth + 1;
						y1 < height && x1 < lanes;
						y1++, x1++, f1 += simdMapWidth + 1)
					{
						if (map[f1] != Floor)
						{
							gaps[++maxGap] = (mx, f1);
							break;
						}
					}
				}

				// backward diagonal gaps
				for (
					int x = 3, mx = my + 3, fx = mx + simdMapWidth - 1, ffx = fx + simdMapWidth - 1;
					x < lanes;
					x++, mx++, fx++, ffx++)
				{
					if (map[mx] == Floor) continue;
					if (map[fx] != Floor) continue;
					if (map[ffx] != Floor) continue;

					for (
						int y1 = y + 3, x1 = x - 3, f1 = ffx + simdMapWidth - 1;
						y1 < height && x1 >= 0;
						y1++, x1--, f1 += simdMapWidth - 1)
					{
						if (map[f1] != Floor)
						{
							gaps[++maxGap] = (mx, f1);
							break;
						}
					}
				}
			}

			return maxGap + 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static Vector256<byte> AddCount(Vector256<byte> cnt, Span<byte> map, int index1, int index2)
		{
			var close = Unsafe.As<byte, Vector256<byte>>(ref map[index1]);
			var far = Unsafe.As<byte, Vector256<byte>>(ref map[index2]);
			return Avx2.Add(cnt, Avx2.BlendVariable(close, far, Avx2.CompareEqual(close, Vector256.Create(Floor))));
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static bool UpdateMap(Span<byte> map1, Span<byte> map2, Span<byte> counts, Vector256<byte> threshold, int lanes, int simdMapWidth, int height)
		{
			var changed = false;
			var floor = Vector256.Create(Floor);
			for (int i = 0, my = simdMapWidth * 2 + 2; i < height; i++, my += simdMapWidth)
			{
				for (int x = 0; x < lanes; x += Vector256<byte>.Count)
				{
					var cnt = Unsafe.As<byte, Vector256<byte>>(ref counts[my + x]);
					var old = Unsafe.As<byte, Vector256<byte>>(ref map1[my + x]);
					var @new = Avx2.BlendVariable(
						old,
						Vector256.Create(Occupied),
						Avx2.CompareEqual(cnt, Vector256<byte>.Zero));
					@new = Avx2.BlendVariable(
						@new,
						Vector256<byte>.Zero,
						Avx2.CompareGreaterThan(cnt.AsSByte(), threshold.AsSByte()).AsByte());
					@new = Avx2.BlendVariable(
						@new,
						floor,
						Avx2.CompareEqual(old, floor));

					changed = changed || ~Avx2.MoveMask(Avx2.CompareEqual(old, @new)) != 0;

					Unsafe.As<byte, Vector256<byte>>(ref map2[my + x]) = @new;
				}
			}

			return changed;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
		private static int CountOccupied(Span<byte> map1, int lanes, int simdMapWidth, int height)
		{
			var count = 0;
			var occupied = Vector256.Create(Occupied);
			for (int i = 0, my = simdMapWidth * 2 + 2; i < height; i++, my += simdMapWidth)
			{
				for (int x = 0; x < lanes; x += Vector256<byte>.Count)
				{
					var cnt = Avx2.MoveMask(
						Avx2.CompareEqual(
							Unsafe.As<byte, Vector256<byte>>(ref map1[my + x]),
							occupied));
					count += (int)Popcnt.PopCount((uint)cnt);
				}
			}

			return count;
		}
	}
}
