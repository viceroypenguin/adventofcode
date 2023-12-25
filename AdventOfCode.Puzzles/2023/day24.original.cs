namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 24, CodeType.Original)]
public partial class Day_24_Original : IPuzzle
{
	//private const double Low = 7;
	//private const double High = 27;
	private const double Low = 200_000_000_000_000;
	private const double High = 400_000_000_000_000;

	private record struct Hailstone(
		double Px, double Py, double Pz,
		int Vx, int Vy, int Vz)
	{
		private readonly double _c2d = (Py * Vx) - (Px * Vy);

		public readonly bool IsValid2dIntersection(Hailstone other)
		{
			var det = (Vx * other.Vy) - (Vy * other.Vx);
			if (det == 0)
				return false;

			var x = ((_c2d * other.Vx) - (Vx * other._c2d)) / det;
			if (!x.Between(Low, High))
				return false;

			var y = ((_c2d * other.Vy) - (Vy * other._c2d)) / det;
			if (!y.Between(Low, High))
				return false;

			return (x - Px) / Vx > 0
				&& (x - other.Px) / other.Vx > 0;
		}
	}

	private static long FindIntersection(Hailstone a, Hailstone b, Hailstone c)
	{
		// for any given stone, A = A0 + Av*t, our ray (P + Qt) should have a matching t value.
		// With three stones, we have nine equations and nine unknowns (t, u, v, Px, Py, Pz, Qx, Qy, Qz), assuming that
		//  any solution for three will work for all:
		// A0x + Avx*t = Px + Qx*t
		// A0y + Avy*t = Py + Qy*t
		// A0z + Avz*t = Pz + Qz*t
		//
		// B0x + Bvx*u = Px + Qx*u
		// B0y + Bvy*u = Py + Qy*u
		// B0z + Bvz*u = Pz + Qz*u
		//
		// C0x + Cvx*v = Px + Qx*v
		// C0y + Cvy*v = Py + Qy*v
		// C0z + Cvz*v = Pz + Qz*v
		//
		// We can eliminate t, u, and v and end up with 6 equations with 6 unknowns (Px, Py, Pz, Qx, Qy, Qz):
		// (Px - A0x) / (Avx - Qx) = (Py - A0y) / (Avy - Qy) = (Pz - A0z) / (Avz - Qz)
		// (Px - B0x) / (Bvx - Qx) = (Py - B0y) / (Bvy - Qy) = (Pz - B0z) / (Bvz - Qz)
		// (Px - C0x) / (Cvx - Qx) = (Py - C0y) / (Cvy - Qy) = (Pz - C0z) / (Cvz - Qz)

		// Rearranging the Px/Py pairing:

		// Px * Avy - Px * Qy - A0x * Avy + A0x * Qy = Py * Avx - Py * Qx - A0y * Avx + A0y * Qx
		// (Px * Qy - Py * Qx) = (Px * Avy - Py * Avx) + (A0y * Avx - A0x * Avy) + (A0x * Qy - A0y * Qx)
		// (Px * Qy - Py * Qx) = (Px * Bvy - Py * Bvx) + (B0y * Bvx - B0x * Bvy) + (B0x * Qy - B0y * Qx)
		// (Px * Qy - Py * Qx) = (Px * Cvy - Py * Cvx) + (C0y * Cvx - C0x * Cvy) + (C0x * Qy - C0y * Qx)
		//
		// Note that this gets a common (Px * Qy - Py * Qx) on the left side of everything, and the right side of each is
		//  now just a linear equation.
		// Do the same for the Pz/Px and Py/Pz pairints:
		//
		// (Pz * Qx - Px * Qz) = (Pz * Avx - Px * Avz) + (A0x * Avz - A0z * Avx) + (A0z * Qx - A0x * Qz)
		// (Pz * Qx - Px * Qz) = (Pz * Bvx - Px * Bvz) + (B0x * Bvz - B0z * Bvx) + (B0z * Qx - B0x * Qz)
		// (Pz * Qx - Px * Qz) = (Pz * Cvx - Px * Cvz) + (C0x * Cvz - C0z * Cvx) + (C0z * Qx - C0x * Qz)
		//
		// (Py * Qz - Pz * Qy) = (Py * Avz - Pz * Avy) + (A0z * Avy - A0y * Avz) + (A0y * Qz - A0z * Qy)
		// (Py * Qz - Pz * Qy) = (Py * Bvz - Pz * Bvy) + (B0z * Bvy - B0y * Bvz) + (B0y * Qz - B0z * Qy)
		// (Py * Qz - Pz * Qy) = (Py * Cvz - Pz * Cvy) + (C0z * Cvy - C0y * Cvz) + (C0y * Qz - C0z * Qy)
		//
		// This now turns into a series of 6 straight-up linear equations, which we can solve in, you know, the normal way.
		// [Avy - Bvy]Px - [Avx - Bvx]Py - [A0y - B0y]Qx + [A0x - B0x]Qy = (B0y * Bvx - B0x * Bvy) - (A0y * Avx - A0x * Avy)
		// [Avy - Cvy]Px - [Avx - Cvx]Py - [A0y - C0y]Qx + [A0x - C0x]Qy = (C0y * Cvx - C0x * Cvy) - (A0y * Avx - A0x * Avy)
		// [Avx - Bvx]Pz - [Avz - Bvz]Px - [A0x - B0x]Qz + [A0z - B0z]Qx = (B0x * Bvz - B0z * Bvx) - (A0x * Avz - A0z * Avx)
		// [Avx - Cvx]Pz - [Avz - Cvz]Px - [A0x - C0x]Qz + [A0z - C0z]Qx = (C0x * Cvz - C0z * Cvx) - (A0x * Avz - A0z * Avx)
		// [Avz - Bvz]Py - [Avy - Bvy]Pz - [A0z - B0z]Qy + [A0y - B0y]Qz = (B0z * Bvy - B0y * Bvz) - (A0z * Avy - A0y * Avz)
		// [Avz - Cvz]Py - [Avy - Cvy]Pz - [A0z - C0z]Qy + [A0y - C0y]Qz = (C0z * Cvy - C0y * Cvz) - (A0z * Avy - A0y * Avz)
		//
		// Combine some terms to get:
		double abvx = a.Vx - b.Vx;
		double abvy = a.Vy - b.Vy;
		double abvz = a.Vz - b.Vz;

		double acvx = a.Vx - c.Vx;
		double acvy = a.Vy - c.Vy;
		double acvz = a.Vz - c.Vz;

		var ab0x = a.Px - b.Px;
		var ab0y = a.Py - b.Py;
		var ab0z = a.Pz - b.Pz;

		var ac0x = a.Px - c.Px;
		var ac0y = a.Py - c.Py;
		var ac0z = a.Pz - c.Pz;

		var h0 = (b.Py * b.Vx) - (b.Px * b.Vy) - ((a.Py * a.Vx) - (a.Px * a.Vy));
		var h1 = (c.Py * c.Vx) - (c.Px * c.Vy) - ((a.Py * a.Vx) - (a.Px * a.Vy));
		var h2 = (b.Px * b.Vz) - (b.Pz * b.Vx) - ((a.Px * a.Vz) - (a.Pz * a.Vx));
		var h3 = (c.Px * c.Vz) - (c.Pz * c.Vx) - ((a.Px * a.Vz) - (a.Pz * a.Vx));
		var h4 = (b.Pz * b.Vy) - (b.Py * b.Vz) - ((a.Pz * a.Vy) - (a.Py * a.Vz));
		var h5 = (c.Pz * c.Vy) - (c.Py * c.Vz) - ((a.Pz * a.Vy) - (a.Py * a.Vz));

		// abvy*Px - abvx*Py - ab0y*Qx + ab0x*Qy = h0
		// acvy*Px - acvx*Py - ac0y*Qx + ac0x*Qy = h1
		// abvx*Pz - abvz*Px - ab0x*Qz + ab0z*Qx = h2
		// acvx*Pz - acvz*Px - ac0x*Qz + ac0z*Qx = h3
		// abvz*Py - abvy*Pz - ab0z*Qy + ab0y*Qz = h4
		// acvz*Py - acvy*Pz - ac0z*Qy + ac0y*Qz = h5

		// Now we're going to take each pair of those eliminate its initial P variable (leaving just the other)
		// Okay now that's 6 linear equations and 6 variable, right?
		//
		// Px = [h0 + abvx*Py + ab0y*Qx - ab0x*Qy]/abvy
		// Px = [h1 + acvx*Py + ac0y*Qx - ac0x*Qy]/acvy
		//
		// [h0 + abvx*Py + ab0y*Qx - ab0x*Qy]/abvy = [h1 + acvx*Py + ac0y*Qx - ac0x*Qy]/acvy
		// (acvy*abvx - abvy*acvx)*Py = [abvy*ac0y - acvy*ab0y]*Qx + [acvy*ab0x - abvy*ac0x]*Qy + [abvy*h1 - acvy*h0]
		//
		// -----------
		// Py = ([abvy*ac0y - acvy*ab0y]*Qx + [acvy*ab0x - abvy*ac0x]*Qy + [abvy*h1 - acvy*h0])/(acvy*abvx - abvy*acvx)
		// -----------
		//
		// [h4 + abvy*Pz + ab0z*Qy - ab0y*Qz]/abvz = [h5 + acvy*Pz + ac0z*Qy - ac0y*Qz]/acvz
		// (acvz*abvy - abvz*acvy)*Pz = [abvz*ac0z - acvz*ab0z]*Qy + [acvz*ab0y - abvz*ac0y)*Qz + [abvz*h5 - acvz*h4]
		//
		//
		// -----------
		// Pz = ([abvz*ac0z - acvz*ab0z]*Qy + [acvz*ab0y - abvz*ac0y)*Qz + [abvz*h5 - acvz*h4])/(acvz*abvy - abvz*acvy)
		// -----------
		//
		// [h2 + abvz*Px + ab0x*Qz - ab0z*Qx]/abvx = [h3 + acvz*Px + ac0x*Qz - ac0z*Qx]/acvx
		// (acvx*abvz - abvx*acvz)*Px = [abvx*ac0x - acvx*ab0x]*Qz + [acvx*ab0z - abvx*ac0z]*Qx + [abvx*h3 - acvx*h2]
		//
		// -----------
		// Px = ([abvx*ac0x - acvx*ab0x]*Qz + [acvx*ab0z - abvx*ac0z]*Qx + [abvx*h3 - acvx*h2])/(acvx*abvz - abvx*acvz)
		// Py = ([abvy*ac0y - acvy*ab0y]*Qx + [acvy*ab0x - abvy*ac0x]*Qy + [abvy*h1 - acvy*h0])/(acvy*abvx - abvy*acvx)
		// Pz = ([abvz*ac0z - acvz*ab0z]*Qy + [acvz*ab0y - abvz*ac0y)*Qz + [abvz*h5 - acvz*h4])/(acvz*abvy - abvz*acvy)
		// -----------
		//
		// Alright, now we can sub these into (half of) our original linear equations and rearrange in terms of Qx, Qy,
		//  and Qz, leaving us with three equations and three variables.
		// abvy*Px - abvx*Py - ab0y*Qx + ab0x*Qy = h0
		// abvx*Pz - abvz*Px - ab0x*Qz + ab0z*Qx = h2
		// abvz*Py - abvy*Pz - ab0z*Qy + ab0y*Qz = h4
		//
		// Make some more variables to make this easier
		// Px = (Pxz*Qz + Pxx*Qx + Pxc)/Pxd
		// Py = (Pyx*Qx + Pyy*Qy + Pyc)/Pyd
		// Pz = (Pzy*Qy + Pzz*Qz + Pzc)/Pzd
		var Pxx = (acvx * ab0z) - (abvx * ac0z);
		var Pyy = (acvy * ab0x) - (abvy * ac0x);
		var Pzz = (acvz * ab0y) - (abvz * ac0y);

		var Pxz = (abvx * ac0x) - (acvx * ab0x);
		var Pzy = (abvz * ac0z) - (acvz * ab0z);
		var Pyx = (abvy * ac0y) - (acvy * ab0y);

		var Pxc = (abvx * h3) - (acvx * h2);
		var Pyc = (abvy * h1) - (acvy * h0);
		var Pzc = (abvz * h5) - (acvz * h4);

		var Pxd = (acvx * abvz) - (abvx * acvz);
		var Pyd = (acvy * abvx) - (abvy * acvx);
		var Pzd = (acvz * abvy) - (abvz * acvy);

		// abvy*[(Pxz*Qz + Pxx*Qx + Pxc)/Pxd] - abvx*[(Pyx*Qx + Pyy*Qy + Pyc)/Pyd] - ab0y*Qx + ab0x*Qy = h0
		// abvx*[(Pzy*Qy + Pzz*Qz + Pzc)/Pzd] - abvz*[(Pxz*Qz + Pxx*Qx + Pxc)/Pxd] - ab0x*Qz + ab0z*Qx = h2
		// abvz*[(Pyx*Qx + Pyy*Qy + Pyc)/Pyd] - abvy*[(Pzy*Qy + Pzz*Qz + Pzc)/Pzd] - ab0z*Qy + ab0y*Qz = h4
		//
		// okay this is unintelligible garbage now but we're almost there:
		//
		// ([abvy/Pxd]*Pxz)*Qz + ([abvy/Pxd]*Pxx - [abvx/Pyd]*Pyx - ab0y)*Qx + (ab0x - [abvx/Pyd]*Pyy)*Qy
		//   = h0 - [abvy/Pxd]*Pxc + [abvx/Pyd]*Pyc
		// ([abvx/Pzd]*Pzy)*Qy + ([abvx/Pzd]*Pzz - [abvz/Pxd]*Pxz - ab0x)*Qz + (ab0z - [abvz/Pxd]*Pxx)*Qx
		//   = h2 - [abvx/Pzd]*Pzc + [abvz/Pxd]*Pxc
		// ([abvz/Pyd]*Pyx)*Qx + ([abvz/Pyd]*Pyy - [abvy/Pzd]*Pzy - ab0z)*Qy + (ab0y - [abvy/Pzd]*Pzz)*Qz
		//   = h4 - [abvz/Pyd]*Pyc + [abvy/Pzd]*Pzc
		//
		// MOAR VARIABLES
		var Qz0 = abvy / Pxd * Pxz;
		var Qx0 = (abvy / Pxd * Pxx) - (abvx / Pyd * Pyx) - ab0y;
		var Qy0 = ab0x - (abvx / Pyd * Pyy);
		var r0 = h0 - (abvy / Pxd * Pxc) + (abvx / Pyd * Pyc);
		var Qy1 = abvx / Pzd * Pzy;
		var Qz1 = (abvx / Pzd * Pzz) - (abvz / Pxd * Pxz) - ab0x;
		var Qx1 = ab0z - (abvz / Pxd * Pxx);
		var r1 = h2 - (abvx / Pzd * Pzc) + (abvz / Pxd * Pxc);
		var Qx2 = abvz / Pyd * Pyx;
		var Qy2 = (abvz / Pyd * Pyy) - (abvy / Pzd * Pzy) - ab0z;
		var Qz2 = ab0y - (abvy / Pzd * Pzz);
		var r2 = h4 - (abvz / Pyd * Pyc) + (abvy / Pzd * Pzc);

		// Qz0*Qz + Qx0*Qx + Qy0*Qy = r0
		// Qy1*Qy + Qz1*Qz + Qx1*Qx = r1
		// Qx2*Qx + Qy2*Qy + Qz2*Qz = r2
		//
		// Qx = [r0 - Qy0*Qy - Qz0*Qz]/Qx0
		//    = [r1 - Qy1*Qy - Qz1*Qz]/Qx1
		//    = [r2 - Qy2*Qy - Qz2*Qz]/Qx2
		//
		// Qx1*r0 - Qx1*Qy0*Qy - Qx1*Qz0*Qz = Qx0*r1 - Qx0*Qy1*Qy - Qx0*Qz1*Qz
		// Qy = ([Qx0*Qz1 - Qx1*Qz0]Qz + [Qx1*r0 - Qx0*r1])/[Qx1*Qy0 - Qx0*Qy1]
		//    = ([Qx0*Qz2 - Qx2*Qz0]Qz + [Qx2*r0 - Qx0*r2])/[Qx2*Qy0 - Qx0*Qy2]
		//
		// ([Qx2*Qy0 - Qx0*Qy2][Qx0*Qz1 - Qx1*Qz0] - [Qx1*Qy0 - Qx0*Qy1][Qx0*Qz2 - Qx2*Qz0])Qz
		//   = [Qx1*Qy0 - Qx0*Qy1][Qx2*r0 - Qx0*r2] - [Qx2*Qy0 - Qx0*Qy2][Qx1*r0 - Qx0*r1]

		// Alright after alllll that we can now solve for Qz, and then backsolve for everything else up the chain.
		var Qz = ((((Qx1 * Qy0) - (Qx0 * Qy1)) * ((Qx2 * r0) - (Qx0 * r2))) - (((Qx2 * Qy0) - (Qx0 * Qy2)) * ((Qx1 * r0) - (Qx0 * r1))))
		  / ((((Qx2 * Qy0) - (Qx0 * Qy2)) * ((Qx0 * Qz1) - (Qx1 * Qz0))) - (((Qx1 * Qy0) - (Qx0 * Qy1)) * ((Qx0 * Qz2) - (Qx2 * Qz0))));

		var Qy = ((((Qx0 * Qz1) - (Qx1 * Qz0)) * Qz) + ((Qx1 * r0) - (Qx0 * r1))) / ((Qx1 * Qy0) - (Qx0 * Qy1));

		var Qx = (r0 - (Qy0 * Qy) - (Qz0 * Qz)) / Qx0;

		var Px = ((Pxz * Qz) + (Pxx * Qx) + Pxc) / Pxd;
		var Py = ((Pyx * Qx) + (Pyy * Qy) + Pyc) / Pyd;
		var Pz = ((Pzy * Qy) + (Pzz * Qz) + Pzc) / Pzd;

		// Now, sum the (rounded, to deal with float precision issues) coordinates of the starting position together.
		//  The end. FINALLY.
		return (long)Math.Round(Px) + (long)Math.Round(Py) + (long)Math.Round(Pz);
	}

	public (string, string) Solve(PuzzleInput input)
	{
		var regex = HailstoneRegex();
		var hailstones = input.Lines
			.Select(l => regex.Match(l))
			.Select(m => new Hailstone(
				Px: double.Parse(m.Groups[1].Value),
				Py: double.Parse(m.Groups[2].Value),
				Pz: double.Parse(m.Groups[3].Value),
				Vx: int.Parse(m.Groups[4].Value),
				Vy: int.Parse(m.Groups[5].Value),
				Vz: int.Parse(m.Groups[6].Value)))
			.ToList();

		var part1 = 0;
		for (var i = 0; i < hailstones.Count; i++)
		{
			for (var j = i + 1; j < hailstones.Count; j++)
			{
				if (hailstones[i].IsValid2dIntersection(hailstones[j]))
					part1++;
			}
		}

		var part2 = FindIntersection(hailstones[0], hailstones[1], hailstones[2]);

		return (part1.ToString(), part2.ToString());
	}

	[GeneratedRegex(@"^(-?\d+),\s+(-?\d+),\s+(-?\d+)\s+@\s+(-?\d+),\s+(-?\d+),\s+(-?\d+)$")]
	private static partial Regex HailstoneRegex();
}
