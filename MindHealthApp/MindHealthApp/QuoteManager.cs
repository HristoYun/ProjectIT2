using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindHealthApp
{
    internal class QuoteManager
    {
        private Queue<string> quotes = new Queue<string>();

        public QuoteManager()
        {
            quotes.Enqueue("You are stronger than you think.");
            quotes.Enqueue("This too shall pass.");
            quotes.Enqueue("Keep breathing, keep believing.");
            quotes.Enqueue("Your mental health is a priority. Your happiness is an essential. Your self - care is a necessity.");
            quotes.Enqueue("It’s okay to not be okay. Just don’t give up.");
            quotes.Enqueue("Healing takes time, and that’s okay");
        }

        public string GetNextQuote()
        {
            string quote = quotes.Dequeue();
            quotes.Enqueue(quote);
            return quote;
        }
    }
    public enum Color { Red, Black }

    public class RBTNode
    {
        public DateTime Key;
        public int Count;
        public Color Color;
        public RBTNode Left, Right, Parent;

        public RBTNode(DateTime key)
        {
            Key = key;
            Count = 1;
            Color = Color.Red;
        }
    }
    public class RedBlackTree
    {
        private RBTNode root;

        public void Insert(DateTime key)
        {
            RBTNode newNode = new RBTNode(key);
            RBTNode y = null;
            RBTNode x = root;

            while (x != null)
            {
                y = x;
                if (key == x.Key)
                {
                    x.Count++;
                    return;
                }
                else if (key < x.Key)
                    x = x.Left;
                else
                    x = x.Right;
            }

            newNode.Parent = y;
            if (y == null)
                root = newNode;
            else if (newNode.Key < y.Key)
                y.Left = newNode;
            else
                y.Right = newNode;

            FixInsert(newNode);
        }

        private void FixInsert(RBTNode z)
        {
            while (z.Parent != null && z.Parent.Color == Color.Red)
            {
                RBTNode gp = z.Parent.Parent;
                if (z.Parent == gp.Left)
                {
                    RBTNode y = gp.Right;
                    if (y != null && y.Color == Color.Red)
                    {
                        z.Parent.Color = Color.Black;
                        y.Color = Color.Black;
                        gp.Color = Color.Red;
                        z = gp;
                    }
                    else
                    {
                        if (z == z.Parent.Right)
                        {
                            z = z.Parent;
                            RotateLeft(z);
                        }
                        z.Parent.Color = Color.Black;
                        gp.Color = Color.Red;
                        RotateRight(gp);
                    }
                }
                else
                {
                    RBTNode y = gp.Left;
                    if (y != null && y.Color == Color.Red)
                    {
                        z.Parent.Color = Color.Black;
                        y.Color = Color.Black;
                        gp.Color = Color.Red;
                        z = gp;
                    }
                    else
                    {
                        if (z == z.Parent.Left)
                        {
                            z = z.Parent;
                            RotateRight(z);
                        }
                        z.Parent.Color = Color.Black;
                        gp.Color = Color.Red;
                        RotateLeft(gp);
                    }
                }
            }
            root.Color = Color.Black;
        }

        private void RotateLeft(RBTNode x)
        {
            RBTNode y = x.Right;
            x.Right = y.Left;
            if (y.Left != null) y.Left.Parent = x;
            y.Parent = x.Parent;
            if (x.Parent == null) root = y;
            else if (x == x.Parent.Left) x.Parent.Left = y;
            else x.Parent.Right = y;
            y.Left = x;
            x.Parent = y;
        }

        private void RotateRight(RBTNode x)
        {
            RBTNode y = x.Left;
            x.Left = y.Right;
            if (y.Right != null) y.Right.Parent = x;
            y.Parent = x.Parent;
            if (x.Parent == null) root = y;
            else if (x == x.Parent.Right) x.Parent.Right = y;
            else x.Parent.Left = y;
            y.Right = x;
            x.Parent = y;
        }

        public void PrintInOrder()
        {
            Console.WriteLine("📅 Статистика по дати:");
            PrintInOrder(root);
        }

        private void PrintInOrder(RBTNode node)
        {
            if (node != null)
            {
                PrintInOrder(node.Left);
                Console.WriteLine($"{node.Key.ToShortDateString()}: {node.Count} запис(а)");
                PrintInOrder(node.Right);
            }
        }
    }
}
