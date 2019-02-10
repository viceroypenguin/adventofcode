using System;
using System.Linq;

namespace AdventOfCode
{
	public class Day_2015_14_Original : Day
	{
		public override int Year => 2015;
		public override int DayNumber => 14;
		public override CodeType CodeType => CodeType.Original;

		protected override void ExecuteDay(byte[] input)
		{
			var time = 2503;

			var reindeer = input
				.GetLines()
				.Select(x =>
				{
					var splits = x.Split();
					var name = splits[0];
					var speed = Convert.ToInt32(splits[3]);
					var flyingTime = Convert.ToInt32(splits[6]);
					var restTime = Convert.ToInt32(splits[13]);
					var totalTime = flyingTime + restTime;

					var loops = time / totalTime;
					var finalLoopTime = time % totalTime;

					var loopsDistance = loops * speed * flyingTime;
					var finalLoopDistance = Math.Min(finalLoopTime, flyingTime) * speed;

					var totalDistance = loopsDistance + finalLoopDistance;

					return new
					{
						name,
						speed,
						flyingTime,
						restTime,
						totalTime,
						loops,
						finalLoopTime,
						totalDistance,
					};
				})
				.OrderByDescending(x => x.totalDistance)
				.ToList();

			Dump('A', reindeer.First().totalDistance);

			var partB = Enumerable.Range(1, time)
				.SelectMany(t =>
				{
					var distancesAtTime = reindeer
						.Select(r =>
						{
							var loops = t / r.totalTime;
							var finalLoopTime = t % r.totalTime;

							var loopsDistance = loops * r.speed * r.flyingTime;
							var finalLoopDistance = Math.Min(finalLoopTime, r.flyingTime) * r.speed;

							var totalDistance = loopsDistance + finalLoopDistance;

							return new
							{
								r.name,
								totalDistance,
							};
						})
						.ToList();
					var maxDistance = distancesAtTime.Max(x => x.totalDistance);
					return distancesAtTime
						.Where(x => x.totalDistance == maxDistance)
						.Select(x => x.name);
				})
				.ToList()
				.GroupBy(x => x,
					(x, _) => new { x, count = _.Count() })
				.OrderByDescending(x => x.count)
				.First()
				.count;

			Dump('B', partB);
		}
	}
}
