using System;
using System.Collections.Generic;

namespace PlannerApp
{
    class TaskVisualizer
    {
        private TaskList tasks = TaskList.GetInstance();

        public DateTime? GetDateTime()
        {
            DateTime newDate;
            Console.Write("Введите дату и время (год.месяц.число часы:минуты) : ");
            string date = Console.ReadLine();
            if (!DateTime.TryParse(date, out newDate))
            {
                Console.WriteLine("Дата введена неверно.");
                return null;
            }
            return newDate;
        }

        public DateTime? GetDateTimeAU()
        {
            var newDateTime = GetDateTime();
            if (newDateTime == null)
            {
                return null;
            }
            if (newDateTime < DateTime.Now)
            {
                Console.WriteLine("Задание не может быть назначено на прошедшую дату.");
                return null;
            }
            if (tasks.ReadAll().ContainsKey((DateTime)newDateTime))
            {
                Console.WriteLine("На эту дату уже запланировано задание, добавление невозможно.");
                return null;
            }
            return newDateTime;
        }

        public void Add()
        {
            var newDate = GetDateTimeAU();
            if (newDate == null)
            {
                return;
            }
            TaskData newTaskData = new TaskData();
            Console.Write("Введите текст задания: ");
            newTaskData.name = Console.ReadLine();
            newTaskData.date = (DateTime)newDate;

            tasks.Create(newTaskData);
        }

        public void Print()
        {
            if (tasks.ReadAll().Count == 0)
            {
                Console.WriteLine("Списко заданий пуст.");
                return;
            }
            Console.WriteLine("Задания за весь период:");
            DateTime currentDate = new DateTime(0001, 01, 01);
            bool checkPrintData = false;
            foreach (var keyValue in tasks.ReadAll())
            {
                if (currentDate != keyValue.Key.Date)
                {
                    currentDate = keyValue.Key.Date;
                    checkPrintData = false;
                    Console.WriteLine();
                }
                if ((!checkPrintData) && (currentDate == keyValue.Key.Date))
                {
                    Console.WriteLine($"----- {keyValue.Key.ToLongDateString()}");
                    checkPrintData = true;
                }
                Console.ForegroundColor = keyValue.Value.done ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"    > {keyValue.Key.ToShortTimeString()} - {keyValue.Value.name}");
                Console.ResetColor();
            }
        }

        public void SearchTaskByDate(DateTime searchData)
        {
            int count = 0;
            Console.WriteLine($"Задания на {searchData.ToLongDateString()} :");
            foreach (var keyValue in tasks.ReadAll())
            {
                if (keyValue.Key.Date == searchData)
                {
                    Console.ForegroundColor = keyValue.Value.done ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"    > {keyValue.Key.ToShortTimeString()} - {keyValue.Value.name}");
                    Console.ResetColor();
                    count++;
                }
            }
            Console.WriteLine($"Всего заданий -  {count}.");
        }

        public void SearchAndActionWhisTask(DateTime key)
        {
            if (tasks.Search(key) == null)
            {
                Console.WriteLine("Такого задания нет.");
                return;
            }
            Console.Clear();
            var searchTask = (TaskData)tasks.Search(key);
            Console.WriteLine($"----- {key.ToLongDateString()}");
            Console.ForegroundColor = searchTask.done ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"    > {key.ToShortTimeString()} - {searchTask.name}");
            Console.ResetColor();
            Console.WriteLine("Для перехода к действиям намите любую клавишу...");
            Console.ReadKey();
            Console.Clear();
            switch (Menu.PrintMenu(ConstVariable.actionsWithTask, "Для выбора действия нажмите Enter"))
            {
                case 1: // Изменение статуса на "выполнено".
                    {
                        if (searchTask.done)
                        {
                            Console.WriteLine("Задание уже выполнено. Статус не может быть изменен.");
                        }
                        else
                        {
                            tasks.SetDone(key, true);
                            Console.WriteLine("Статус изменен.");
                        }
                        break;
                    }
                case 2: // Перенос задания.
                    {
                        Console.WriteLine($"Дата и время старого задания: {searchTask.date}");
                        var newDateTime = GetDateTimeAU();
                        if (newDateTime == null)
                        {
                            Console.WriteLine("Произошла ошибка.");
                        }
                        else
                        {
                            tasks.UpdateDataTime(key, (DateTime)newDateTime);
                            Console.WriteLine("Изменения приняты.");
                        }
                        break;
                    }
                case 3: // Изменение задания.
                    {
                        Console.WriteLine($"Старое задание: {searchTask.name}");
                        Console.Write("Введите изменения: ");
                        string newName = Convert.ToString(Console.ReadLine());
                        tasks.UpdateTaskName(key, newName);
                        Console.WriteLine("Изменения приняты.");
                        break;
                    }
                case 4: // Удаление задания.
                    {
                        Console.Clear();
                        switch (Menu.PrintMenu(ConstVariable.optionYesNo, "Вы действительно хотите удалить задание?"))
                        {
                            case 1:
                                {
                                    tasks.Delete(key);
                                    Console.WriteLine("Задание удалено.");
                                    break;
                                }
                            case 2:
                                {
                                    Console.WriteLine("Удаление отменено.");
                                    break;
                                }
                            default: break;
                        }
                        break;
                    }
                default: break;
            }
            Console.ReadKey();
        }

        public void DeletePastTask()
        {
            Console.Clear();
            switch (Menu.PrintMenu(ConstVariable.optionsForDeletePastTask, "Какие задания до текущей даты удалить?"))
            {
                case 1: // выполненные
                    {
                        tasks.DeletePastTask(DeleteFlagsEnum.DONE);
                        Console.WriteLine("Все выполненные задания до текущей даты удалены.");
                        break;
                    }
                case 2: // невыполненные
                    {
                        tasks.DeletePastTask(DeleteFlagsEnum.NOT_DONE);
                        Console.WriteLine("Все невыполненные задания до текущей даты удалены.");
                        break;
                    }
                case 3: // все
                    {
                        tasks.DeletePastTask(DeleteFlagsEnum.ALL);
                        Console.WriteLine("Все задания до текущей даты удалены.");
                        break;
                    }
                default: break;
            }
        }

        public void SaveToFile()
        {
            Console.Write("Введите имя файла для сохранения: ");
            string fileName = Console.ReadLine();
            tasks.SerializeAndSave(fileName);
            Console.WriteLine("Файл сохранен.");
        }

        public void LoadFromFile()
        {
            Console.Write("Введите имя файла: ");
            string fileName = Console.ReadLine();
            tasks.ReadAndDeserialize(fileName);
            Console.WriteLine("Файл загружен.");
        }
    }
}
