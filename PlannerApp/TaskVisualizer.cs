using System;
using System.Collections.Generic;

namespace PlannerApp
{
    class TaskVisualizer
    {
        private TaskList tasks = TaskList.GetInstance();
        public void Create()
        {
            DateTime newDate;
            Console.Write("Введите дату и время (год.месяц.число часы:минуты) : ");
            string date = Console.ReadLine();
            if (!DateTime.TryParse(date, out newDate))
            {
                Console.WriteLine("Дата введена неверно.");
                return;
            }
            if (newDate < DateTime.Now)
            {
                Console.WriteLine("Задание не может быть назначено на прошедшую дату.");
                return;
            }
            if (tasks.ReadAll().ContainsKey(newDate))
            {
                Console.WriteLine("На эту дату уже запланировано задание, добавление невозможно.");
                return;
            }

            TaskData newTaskData = new TaskData();
            Console.Write("Введите текст задания: ");
            newTaskData.name = Console.ReadLine();
            newTaskData.date = newDate;

            tasks.Create(newTaskData);
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
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
            /*
             * ДОПИСАТЬ
             * 
             */

        }
    }
}
