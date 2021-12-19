﻿using System.Text;

namespace AdventOfCode.Y2021.Day14
{
    public class Solution : IPuzzleSolution
    {
        public long Part1(StreamReader reader)
            => Polymerization(reader, 10);

        public long Part2(StreamReader reader)
            => Polymerization(reader, 40);

        private static long Polymerization(StreamReader reader, int steps)
        {
            var template = reader.ReadLine();
            var tokens = new Dictionary<string, long>();
            for (var i = 0; i < template.Length - 1; i++)
            {
                var token = template.Substring(i, 2);
                if (!tokens.TryAdd(token, 1))
                {
                    tokens[token]++;
                }
            }

            reader.ReadLine();
            var rules = new Dictionary<string, (string, string)>();
            while (reader.TryReadLine(out string line))
            {
                var pair = line.Split(" -> ");
                var sub = pair[0];
                var add1 = $"{pair[0][0]}{pair[1]}";
                var add2 = $"{pair[1]}{pair[0][1]}";
                rules.Add(sub, (add1, add2));
                tokens.TryAdd(sub, 0);
                tokens.TryAdd(add1, 0);
                tokens.TryAdd(add2, 0);
            }

            for (var step = 1; step <= steps; step++)
            {
                // Record the amount we need to change before making changes
                var alterations = new Dictionary<string, long>();
                foreach (var rule in rules)
                {
                    alterations.Add(rule.Key, tokens[rule.Key]);
                }

                // Apply only the amounts we observed
                foreach (var alteration in alterations)
                {
                    tokens[alteration.Key] -= alteration.Value;
                    tokens[rules[alteration.Key].Item1] += alteration.Value;
                    tokens[rules[alteration.Key].Item2] += alteration.Value;
                }
            }

            var characters = string.Join("", tokens.Keys)
                .Distinct()
                .ToDictionary(x => x, x => 0L);

            foreach (var token in tokens)
            {
                var key = token.Key;
                // Only need to account for the first of each pair...
                characters[key[0]] += token.Value;
            }

            // ...and add on the last character of the original string
            characters[template[^1]]++;

            var ordered = characters
                .Select(x => x.Value)
                .OrderBy(x => x)
                .ToList();

            return ordered[^1] - ordered[0];
        }
    }
}
