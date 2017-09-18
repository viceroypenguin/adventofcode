<Query Kind="Statements" />

var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day14.input.txt"));
var time = 2503;

var reindeer = input
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
	.OrderByDescending(x=>x.totalDistance)
	.ToList();
	
reindeer.First().totalDistance.Dump("Part A");

Enumerable.Range(1, time)
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
	.count
	.Dump("Part B");