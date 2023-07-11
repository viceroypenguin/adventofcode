namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 14, CodeType.Original)]
public class Day_14_Original : IPuzzle
{
	public (string, string) Solve(PuzzleInput input)
	{
		var time = 2503;

		var reindeer = input.Lines
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

		var partA = reindeer.First().totalDistance;

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

		return (partA.ToString(), partB.ToString());
	}
}
