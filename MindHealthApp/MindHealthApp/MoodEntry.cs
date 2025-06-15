using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindHealthApp
{
    internal class MoodEntry
    {
        public DateTime Date { get; set; }
        public string Mood { get; set; }
        public string Note { get; set; }

        public MoodEntry(DateTime date, string mood, string note)
        {
            Date = date;
            Mood = mood;
            Note = note;
        }

        public override string ToString()
        {
            return $"{Date:dd.MM.yyyy}| {Mood} | {Note}";
        }
        public static MoodEntry FromString(string line)
        {
            var parts = line.Split('|');
            string[] formats = { "d.M.yyyy", "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy" };

            if (!DateTime.TryParseExact(parts[0], formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                throw new FormatException($"Invalid date format: {parts[0]}");
            }

            return new MoodEntry(date, parts[1], parts[2]);
        }
    }
}
