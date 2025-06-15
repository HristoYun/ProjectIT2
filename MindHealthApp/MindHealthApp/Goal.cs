using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MindHealthApp
{
    internal class Goal
    {
        public string Description { get; set; }
        public int TargetPerWeek { get; set; }
        public Func<MoodEntry, bool> MatchCondition { get; set; }
        public string MatchConditionKeyword { get; set; }
        public List<DateTime> CompletionDates { get; set; } = new List<DateTime>();

        public Goal(string description, int target, Func<MoodEntry, bool> condition, string keyword)
        {
            Description = description;
            TargetPerWeek = target;
            MatchCondition = condition;
            MatchConditionKeyword = keyword;
        }

        public int CountCompletionsThisWeek()
        {
            var now = DateTime.Today;
            return CompletionDates.Count(d => d >= now.AddDays(-7));
        }

        public void PrintProgress()
        {
            int count = CountCompletionsThisWeek();
            int total = TargetPerWeek;
            int bars = 4;
            int filled = Math.Min(bars, count * bars / total);
            string bar = "[" + new string('█', filled) + new string('░', bars - filled) + "]";
            Console.WriteLine($"🎯 {Description} {bar} {count}/{total}");
        }

        public static void SaveGoalsToFile(List<Goal> goals, string filePath)
        {
            var lines = goals.Select(g =>
                $"{g.Description}|{g.TargetPerWeek}|{g.MatchConditionKeyword}|{string.Join(",", g.CompletionDates.Select(d => d.ToString("yyyy-MM-dd")))}");
            File.WriteAllLines(filePath, lines);
        }

        public static List<Goal> LoadGoalsFromFile(string filePath)
        {
            var list = new List<Goal>();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 3 && int.TryParse(parts[1], out int target))
                    {
                        string description = parts[0];
                        string keyword = parts[2].Trim().ToLower();
                        var goal = new Goal(description, target, e => e.Mood.ToLower().Contains(keyword), keyword);

                        if (parts.Length >= 4)
                        {
                            var dateParts = parts[3].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var dateStr in dateParts)
                            {
                                if (DateTime.TryParse(dateStr, out DateTime date))
                                    goal.CompletionDates.Add(date);
                            }
                        }
                        list.Add(goal);
                    }
                }
            }
            return list;
        }
    }
}
