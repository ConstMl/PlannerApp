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

        public TaskData? Search(DateTime key)
        {
            if (!tasks.ContainsKey(key))
            {
                return null;
            }
            return tasks[key];
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

        public void SetDone(DateTime key, bool done)
        {
            var tempTask = tasks[key];
            tempTask.done = done;
            tasks[key] = tempTask;
            //tasks[key].done = done; // ?..
        }

        public void Delete(DateTime key)
        {
            tasks.Remove(key);
        }

        public void DeleteAll()
        {
            tasks.Clear();
        }

        public void DeletePastTask(DeleteFlagsEnum flag)
        {
            switch (flag)
            {
                case DeleteFlagsEnum.ALL:
                    {

                        foreach (var keyValue in tasks)
                            if (keyValue.Key < DateTime.Now.Date)
                                tasks.Remove(keyValue.Key);
                        break;
                    }
                case DeleteFlagsEnum.DONE:
                    {
                        foreach (var keyValue in tasks)
                            if ((keyValue.Key < DateTime.Now.Date) && keyValue.Value.done)
                                tasks.Remove(keyValue.Key);
                        break;
                    }
                case DeleteFlagsEnum.NOT_DONE:
                    {
                        foreach (var keyValue in tasks)
                            if ((keyValue.Key < DateTime.Now.Date) && !keyValue.Value.done)
                                tasks.Remove(keyValue.Key);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
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
