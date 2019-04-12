using System.Collections.Generic;
using System;

public class TaskManager {
    private Queue<Action> queue = new Queue<Action>();
    private object _lock = 0;
    public bool taskRunning;

    public void Add(Action task) {
        lock(_lock) {
            queue.Enqueue(task);
        }
    }

    public void Do() {
        lock(_lock) {
            queue.Dequeue()();
        }
    }

    public bool HasTask() {
        return queue.Count != 0 && !taskRunning;
    }

    public int TasksCount() {
        return queue.Count;
    }

}