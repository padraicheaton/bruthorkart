using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectDatabase<T> : Singleton<ObjectDatabase<T>>
{
    [SerializeField] private List<T> objects;

    public T First() => objects[0];
    public T Get(int index) => objects[index];
    public T Random() => Extensions.GetRandom(objects);
    public int Next(int index)
    {
        int nextIndex = index + 1;

        if (nextIndex >= objects.Count)
            nextIndex = 0;

        return nextIndex;
    }
    public int Previous(int index)
    {
        int nextIndex = index - 1;

        if (nextIndex < 0)
            nextIndex = objects.Count - 1;

        return nextIndex;
    }
}
