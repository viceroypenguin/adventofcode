using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode
{
    public enum CodeType
    {
        Original,
        Fastest,
    }

    public abstract class Day
    {
        public abstract int Year { get; }
        public abstract int DayNumber { get; }
        public abstract CodeType CodeType { get; }
        protected abstract void ExecuteDay(byte[] input);

        protected void Dump(char part, object output) =>
            _output.Add($"Year {Year}, Day {DayNumber}, Part {part}: {output}");

        private List<string> _output = new List<string>();

        public int TotalMicroseconds { get; private set; }

        public void Execute()
        {
            var input = File.ReadAllBytes($@"..\..\..\{Year}\day{DayNumber:00}.input.txt");

            var sw = new Stopwatch();
            sw.Start();
            ExecuteDay(input);
            sw.Stop();

            Console.WriteLine(string.Join(Environment.NewLine, _output));
            Console.WriteLine();

            TotalMicroseconds = (int)
                (sw.Elapsed.TotalMilliseconds * 1000);
        }
    }

    public class DummyDay : Day
    {
        public override int Year => 2015;
        public override int DayNumber => 1;
        public override CodeType CodeType => CodeType.Original;
        protected override void ExecuteDay(byte[] input) { }
    }
}
