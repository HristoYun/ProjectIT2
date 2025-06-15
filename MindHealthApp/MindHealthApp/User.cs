using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindHealthApp
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public static string UsersFile => "users.txt";

        public static bool Register(string username, string password)
        {
            if (!File.Exists(UsersFile))
                File.Create(UsersFile).Close();

            var lines = File.ReadAllLines(UsersFile);
            if (lines.Any(l => l.Split('|')[0] == username))
                return false;

            File.AppendAllText(UsersFile, $"{username}|{password}{Environment.NewLine}");
            return true;
        }

        public static User Login(string username, string password)
        {
            if (!File.Exists(UsersFile)) return null;
            var lines = File.ReadAllLines(UsersFile);

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2 && parts[0] == username && parts[1] == password)
                    return new User { Username = username, Password = password };
            }

            return null;
        }

        public string MoodFilePath => $"data/{Username}_moods.txt";
        public string GoalsFilePath => $"data/{Username}_goals.txt";

        public void EnsureUserDataDirectory()
        {
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");

            if (!File.Exists(MoodFilePath))
                File.Create(MoodFilePath).Close();

            if (!File.Exists(GoalsFilePath))
                File.Create(GoalsFilePath).Close();
        }
    }
}
