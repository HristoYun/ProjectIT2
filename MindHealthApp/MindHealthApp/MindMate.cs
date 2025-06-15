using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MindHealthApp
{
    internal class MindMate
    {
        private List<MoodEntry> entries = new List<MoodEntry>();
        private RedBlackTree statsTree = new RedBlackTree();
        private string moodFilePath;

        public MindMate(User user)
        {
            user.EnsureUserDataDirectory();
            moodFilePath = user.MoodFilePath;
            LoadFromFile();
        }

        public void AddMoodEntry(MoodEntry entry)
        {
            entries.Add(entry);
            statsTree.Insert(entry.Date.Date);
            SaveToFile();
        }

        public void ShowAllEntries()
        {
            foreach (var entry in entries)
            {
                Console.WriteLine(entry);
            }
        }

        public void ShowStats()
        {
            statsTree.PrintInOrder();
        }

        public void SearchByMood(string mood)
        {
            foreach (var entry in entries.Where(e => e.Mood.ToLower() == mood.ToLower()))
            {
                Console.WriteLine(entry);
            }
        }

        public void SaveToFile()
        {
            File.WriteAllLines(moodFilePath, entries.Select(e => e.ToString()));
        }

        public void LoadFromFile()
        {
            if (File.Exists(moodFilePath))
            {
                var lines = File.ReadAllLines(moodFilePath, Encoding.UTF8);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        try
                        {
                            var entry = MoodEntry.FromString(line);
                            entries.Add(entry);
                            statsTree.Insert(entry.Date.Date);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"⚠ Пропуснат ред: {line} ({ex.Message})");
                        }
                    }
                }
            }
        }

        public List<MoodEntry> GetAllEntries()
        {
            return entries;
        }
    }
}
