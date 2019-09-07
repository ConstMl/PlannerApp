using System;

namespace PlannerApp
{
    public class Menu
    {
        static private void PrintMenuFields(int itemNum, string[] items, string head)
        {
            Console.WriteLine(head);
            for (int i = 0; i < items.Length; i++)
            {
                if ((i + 1) != itemNum)
                {
                    Console.WriteLine($" {items[i]} ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($">{items[i]}<");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("Для выхода нажмите клавишу Esc...");
        }

        static public int PrintMenu(string[] items, string head)
        {
            PrintMenuFields(1, items, head);
            int itemNum = 1;
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow: // вверх
                        {
                            Console.Clear();
                            itemNum--;
                            if (itemNum == 0) itemNum = items.Length;
                            PrintMenuFields(itemNum, items, head);
                            break;
                        }
                    case ConsoleKey.DownArrow: // вниз
                        {
                            Console.Clear();
                            itemNum++;
                            if (itemNum == (items.Length + 1)) itemNum = 1;
                            PrintMenuFields(itemNum, items, head);
                            break;
                        }
                    case ConsoleKey.Enter: // выбор пнута
                        {
                            return itemNum;
                        }
                    case ConsoleKey.Escape: // выход
                        {
                            return 0;
                        }
                }
            }
        }

        static public void WaitKey()
        {
            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
