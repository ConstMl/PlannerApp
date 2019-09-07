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
            if (newDate < DateTime.Now)
            {
                Console.WriteLine("Задание не может быть назначено на прошедшую дату.");
                return null;
            }
            if (tasks.ReadAll().ContainsKey(newDate))
            {
                Console.WriteLine("На эту дату уже запланировано задание, добавление невозможно.");
                return null;
            }
            return newDate;
        }
        public void Add()
        {
            var newDate = GetDateTime();
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

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void SearchTaskByDate(DateTime? searchData)
        {
            if (searchData == null)
            {
                return;
            }
            int count = 0;
            Console.WriteLine($"Задания на {searchData?.ToLongDateString()} :");
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
