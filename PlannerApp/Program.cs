using System;

namespace PlannerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var plannerTasks = new TaskVisualizer();
            while (true)
            {
                Console.Clear();
                switch (Menu.PrintMenu(ConstVariable.mainMenuItem, "Для выбора пункта меню нажмите Enter"))
                {
                    case 1: // Просмотр всех заданий.
                        {
                            Console.Clear();
                            plannerTasks.Print();
                            Menu.WaitKey();
                            break;
                        }
                    case 2: // Поиск заданий по дате (только дата).
                        {
                            Console.Clear();
                            var key = plannerTasks.GetDateTime();
                            if (key != null)
                            {
                                plannerTasks.SearchTaskByDate((DateTime)key);
                            }
                            Menu.WaitKey();
                            break;
                        }
                    case 3: // Задания на сегодня.
                        {
                            Console.Clear();
                            plannerTasks.SearchTaskByDate(DateTime.Now.Date);
                            Menu.WaitKey();
                            break;
                        }
                    case 4: // Добавление задания.
                        {
                            Console.Clear();
                            plannerTasks.Add();
                            Menu.WaitKey();
                            break;
                        }
                    case 5: // Поиск конкретного задания (дата и время).
                        {
                            Console.Clear();
                            var key = plannerTasks.GetDateTime();
                            if (key != null)
                            {
                                plannerTasks.SearchAndActionWhisTask((DateTime)key);
                            }
                            Menu.WaitKey();
                            break;
                        }
                    case 6: // Удаление всех заданий до текущей даты.
                        {
                            Console.Clear();
                            plannerTasks.DeletePastTask();
                            Menu.WaitKey();
                            break;
                        }
                    case 7: // Загрузка.
                        {
                            Console.Clear();
                            switch (Menu.PrintMenu(ConstVariable.optionYesNo, "Внимание, загрузка файла очистит текущий список дел. Продолжить?"))
                            {
                                case 1:
                                    {
                                        plannerTasks.LoadFromFile();
                                        break;
                                    }
                                case 2:
                                    {
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            Menu.WaitKey();
                            break;
                        }
                    case 8: // Сохранение.
                        {
                            Console.Clear();
                            plannerTasks.SaveToFile();
                            Menu.WaitKey();
                            break;
                        }
                    case 0: // Выход.
                        {
                            Console.Clear();
                            switch (Menu.PrintMenu(ConstVariable.optionYesNo, "Вы действительно хотите выйти?"))
                            {
                                case 1:
                                    {
                                        Menu.WaitKey();
                                        return;
                                    }
                                case 2:
                                    {
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                
            }
        }
    }
}
