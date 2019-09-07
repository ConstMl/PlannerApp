using System;

namespace PlannerApp
{
    [Serializable]
    public struct TaskData
    {
        public DateTime date;
        public string name;
        public bool done;
    }
}
