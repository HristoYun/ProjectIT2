using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindHealthApp
{
    internal class MoodGraph
    {
        private Dictionary<string, Dictionary<string, int>> graph = new Dictionary<string, Dictionary<string, int>>();

        public MoodGraph(List<MoodEntry> entries)
        {
            var sorted = entries.OrderBy(e => e.Date).ToList();

            for (int i = 0; i < sorted.Count - 1; i++)
            {
                string from = sorted[i].Mood.Trim().ToLower();
                string to = sorted[i + 1].Mood.Trim().ToLower();

                if (!graph.ContainsKey(from))
                    graph[from] = new Dictionary<string, int>();

                if (graph[from].ContainsKey(to))
                    graph[from][to]++;
                else
                    graph[from][to] = 1;
            }
        }

        public void FindShortestPath(string start, string target)
        {
            var dist = new Dictionary<string, int>();
            var prev = new Dictionary<string, string>();
            var queue = new Queue<string>();
            var visited = new HashSet<string>();

            foreach (var node in graph.Keys)
                dist[node] = int.MaxValue;

            dist[start] = 0;
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                visited.Add(current);

                if (!graph.ContainsKey(current)) continue;

                foreach (var neighbor in graph[current])
                {
                    int cost = 1;
                    if (!dist.ContainsKey(neighbor.Key) || dist[current] + cost < dist[neighbor.Key])
                    {
                        dist[neighbor.Key] = dist[current] + cost;
                        prev[neighbor.Key] = current;
                        if (!visited.Contains(neighbor.Key))
                            queue.Enqueue(neighbor.Key);
                    }
                }
            }

            if (!prev.ContainsKey(target))
            {
                Console.WriteLine("❌ Няма път от '" + start + "' до '" + target + "'.");
                return;
            }

            List<string> path = new List<string>();
            string step = target;
            while (step != start)
            {
                path.Add(step);
                step = prev[step];
            }
            path.Add(start);
            path.Reverse();

            Console.WriteLine("🧠 Най-кратък път от '" + start + "' до '" + target + "':");
            Console.WriteLine(string.Join(" → ", path));
        }
    }
}

