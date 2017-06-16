<Query Kind="Statements" />

var input =
@"Vixen can fly 8 km/s for 8 seconds, but then must rest for 53 seconds.
Blitzen can fly 13 km/s for 4 seconds, but then must rest for 49 seconds.
Rudolph can fly 20 km/s for 7 seconds, but then must rest for 132 seconds.
Cupid can fly 12 km/s for 4 seconds, but then must rest for 43 seconds.
Donner can fly 9 km/s for 5 seconds, but then must rest for 38 seconds.
Dasher can fly 10 km/s for 4 seconds, but then must rest for 37 seconds.
Comet can fly 3 km/s for 37 seconds, but then must rest for 76 seconds.
Prancer can fly 9 km/s for 12 seconds, but then must rest for 97 seconds.
Dancer can fly 37 km/s for 1 seconds, but then must rest for 36 seconds.

";
var time = 2503;

var raindeer = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
	.Select(x =>
	{
		var splits = x.Split();
		var name = splits[0];
		var speed = Convert.ToInt32(splits[3]);
		var flyingTime = Convert.ToInt32(splits[6]);
		var restTime = Convert.ToInt32(splits[13]);
		var totalTime = flyingTime + restTime;
		return new
		{
			name,
			speed,
			flyingTime,
			restTime,
			totalTime,
		};
	})
	.ToList();

Enumerable.Range(1, time)
	.Select(t => raindeer
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
		.OrderByDescending(d=>d.totalDistance)
		.Select(d=>d.name)
		.First())
	.ToList()
	.GroupBy(x => x,
		(x, _) => new { x, count = _.Count() })
	.Dump();
