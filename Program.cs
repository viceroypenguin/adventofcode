using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace AdventOfCode
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Pre-JIT Day and Stopwatch
            new DummyDay().Execute();

            var days = GetDays();
            if (args.Contains("-o"))
                days = days.Where(d => d.CodeType == CodeType.Original);
            else if (args.Contains("-f"))
                days = days.Where(d => d.CodeType == CodeType.Fastest);

            foreach (var d in days)
                d.Execute();

            Console.WriteLine();

            foreach (var g in days.GroupBy(d => new { d.Year, d.CodeType, }))
            {
                Console.WriteLine(
                    string.Join(Environment.NewLine,
                        g.Select(d => $"Year {d.Year}, Day {d.DayNumber,2}, Type {d.CodeType,9}      :   {d.TotalMicroseconds,13:N0} μs")));

                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine(
                    $"Year {g.Key.Year},         Type {g.Key.CodeType,9} Total:   {g.Sum(d => d.TotalMicroseconds),13:N0} μs");
                Console.WriteLine();
            }
        }

        private static IEnumerable<Day> GetDays() =>
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.BaseType == typeof(Day))
                .Where(t => t.Name != nameof(DummyDay))
                .Select(t => (Day)Activator.CreateInstance(t))
                .OrderBy(d => d.Year)
                .ThenBy(d => d.DayNumber)
                .ThenBy(d => d.CodeType)
                .ToArray();
    }
}
