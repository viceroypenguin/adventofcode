using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public static class Helpers
    {
        public static string GetString(this byte[] input) =>
            Encoding.ASCII.GetString(input);

        private static string[] _splitChars = new[] { "\r\n", "\n", };
        public static string[] GetLines(this byte[] input, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) =>
            GetString(input)
                .Split(_splitChars, options);
    }
}
