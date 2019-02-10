using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public static class Helpers
    {
        public static string GetString(this byte[] input) =>
            Encoding.ASCII.GetString(input);

        private static char[] _splitChars = new[] { '\r', '\n', };
        public static string[] GetLines(this byte[] input) =>
            GetString(input)
                .Split(_splitChars, StringSplitOptions.RemoveEmptyEntries);
    }
}
