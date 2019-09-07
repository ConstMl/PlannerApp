using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PlannerApp
{
    class TaskList
    {
        
        private SortedDictionary<DateTime, TaskData> tasks;

        private static TaskList instance;
        private TaskList() => tasks = new SortedDictionary<DateTime, TaskData>();
        public static TaskList GetInstance()
        {
            if (instance == null)
            {
                instance = new TaskList();
            }
            return instance;
        }


        public void Create(TaskData newTaskData)
        {
            tasks.Add(newTaskData.date, newTaskData);
        }

        public SortedDictionary<DateTime, TaskData> ReadAll()
        {
            return tasks;
        }

        public TaskData? Search(DateTime date)
        {
            foreach (var task in tasks)
            {
                if (date == task.Value.date)
                {
                    return task.Value;
                }
            }
            return null;
        }

        public void UpdateDataTime(DateTime key, DateTime newDateTime)
        {
            var tempTask = tasks[key];
            tempTask.date = newDateTime;
            tasks.Remove(key);
            Create(tempTask);
        }

        public void UpdateTaskName(DateTime key, string newTaskName)
        {
            var tempTask = tasks[key];
            tempTask.name = newTaskName;
            tasks[key] = tempTask;
            //tasks[key].name = newTaskName; // ?..
        }

        public void Delete(DateTime key)
        {
            tasks.Remove(key);
        }

        public void ReadAndDeserialize(string path)
        {
            tasks.Clear();
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                var deserilizeTasks = (SortedDictionary<DateTime, TaskData>)formatter.Deserialize(fs);
                foreach (var task in deserilizeTasks)
                {
                    tasks.Add(task.Value.date, task.Value);
                }
            }
        }

        public void SerializeAndSave(string path)
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, tasks);
            }
        }
    }
}
