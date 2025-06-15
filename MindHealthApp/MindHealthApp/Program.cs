using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MindHealthApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("=== MindMate ===");
            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Вход");
            Console.Write("Избор: ");
            string mode = Console.ReadLine();

            User currentUser = null;
            while (currentUser == null)
            {
                if (mode == "1")
                {
                    Console.Write("Избери потребителско име: ");
                    string username = Console.ReadLine();
                    Console.Write("Избери парола: ");
                    string password = ReadPassword();

                    if (password.Length < 8)
                    {
                        Console.WriteLine("❗ Паролата трябва да е поне 8 символа.");
                        return;
                    }

                    if (User.Register(username, password))
                    {
                        Console.WriteLine("✅ Регистрация успешна. Влез с акаунта си.");
                        mode = "2";
                    }
                    else
                    {
                        Console.WriteLine("❌ Потребителското име вече съществува.");
                        return;
                    }
                }
                else if (mode == "2")
                {
                    Console.Write("Потребителско име: ");
                    string username = Console.ReadLine();
                    Console.Write("Парола: ");
                    string password = ReadPassword();
                    currentUser = User.Login(username, password);

                    if (currentUser == null)
                    {
                        Console.WriteLine("❌ Грешно потребителско име или парола.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Невалиден избор. Изход.");
                    return;
                }
            }

            Console.Clear();
            Console.WriteLine($"👤 Добре дошъл, {currentUser.Username}!");
            MindMate app = new MindMate(currentUser);
            List<Goal> goals = Goal.LoadGoalsFromFile(currentUser.GoalsFilePath);
            QuoteManager quotes = new QuoteManager();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n=== MindMate CLI ===");
                Console.WriteLine("1. Добави настроение");
                Console.WriteLine("2. Покажи всички записи");
                Console.WriteLine("3. Търси по настроение");
                Console.WriteLine("4. Мотивационна мисъл");
                Console.WriteLine("5. Статистика по дати (RBT)");
                Console.WriteLine("6. Добави нова цел");
                Console.WriteLine("7. Провери прогрес по цели");
                Console.WriteLine("8. Отчети изпълнение по цел");
                Console.WriteLine("9. Най-кратък път между настроения");
                Console.WriteLine("10. Изход");
                Console.WriteLine("11. Подкрепа при нужда");
                Console.Write("Избор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Настроение: ");
                        string mood = Console.ReadLine();
                        Console.Write("Бележка: ");
                        string note = Console.ReadLine();
                        app.AddMoodEntry(new MoodEntry(DateTime.Now, mood, note));
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.Clear();
                        app.ShowAllEntries();
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.Clear();
                        Console.Write("Търси настроение: ");
                        string search = Console.ReadLine();
                        app.SearchByMood(search);
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.Clear();
                        PrintColored(quotes.GetNextQuote());
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "5":
                        Console.Clear();
                        app.ShowStats();
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "6":
                        Console.Clear();
                        Console.Write("Описание на целта: ");
                        string desc = Console.ReadLine();
                        Console.Write("Колко пъти седмично: ");
                        int.TryParse(Console.ReadLine(), out int target);
                        Console.Write("Ключова дума: ");
                        string keyword = Console.ReadLine().ToLower();
                        goals.Add(new Goal(desc, target, e => e.Mood.ToLower().Contains(keyword), keyword));
                        Goal.SaveGoalsToFile(goals, currentUser.GoalsFilePath);
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "7":
                        Console.Clear();
                        foreach (var g in goals)
                            g.PrintProgress();
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "8":
                        Console.Clear();
                        Console.WriteLine("Избери цел за ръчно отчитане:");
                        for (int i = 0; i < goals.Count; i++)
                            Console.WriteLine($"{i + 1}. {goals[i].Description}");
                        Console.Write("Номер: ");
                        if (int.TryParse(Console.ReadLine(), out int gIndex) && gIndex >= 1 && gIndex <= goals.Count)
                        {
                            goals[gIndex - 1].CompletionDates.Add(DateTime.Today);
                            Goal.SaveGoalsToFile(goals, currentUser.GoalsFilePath);
                            Console.WriteLine("✅ Отчетено успешно!");
                        }
                        else Console.WriteLine("❌ Невалиден избор.");
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "9":
                        Console.Clear();
                        Console.Write("От кое настроение искаш да тръгнеш: ");
                        string fromMood = Console.ReadLine().Trim().ToLower();

                        Console.Write("До кое настроение искаш да стигнеш: ");
                        string toMood = Console.ReadLine().Trim().ToLower();

                        MoodGraph moodGraph = new MoodGraph(app.GetAllEntries());
                        moodGraph.FindShortestPath(fromMood, toMood);

                        Console.WriteLine("\nНатисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    case "10":
                        return;
                    case "11":
                        Console.Clear();
                        Console.WriteLine("🤝 НЕ СИ САМ");
                        Console.WriteLine("Ако се чувстваш изгубен, изтощен или безнадежден – има помощ.");
                        Console.WriteLine("📞 Национална телефонна линия за подкрепа: 0800 11 977 (анонимна и безплатна)");
                        Console.WriteLine("🌐 Повече информация: https://www.psihichnozdrave.bg");
                        Console.WriteLine("Моля, не се отказвай – има хора, които се грижат за теб и искат да ти помогнат.");
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Невалиден избор.");
                        Console.WriteLine("Натисни клавиш за продължение...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static string ReadPassword()
        {
            StringBuilder input = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    input.Append(key.KeyChar);
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return input.ToString();
        }

        static void PrintColored(string text)
        {
            ConsoleColor[] colors = {
                ConsoleColor.Red, ConsoleColor.Yellow,
                ConsoleColor.Green, ConsoleColor.Cyan,
                ConsoleColor.Magenta, ConsoleColor.Blue
            };
            var rand = new Random();
            Console.ForegroundColor = colors[rand.Next(colors.Length)];
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}

