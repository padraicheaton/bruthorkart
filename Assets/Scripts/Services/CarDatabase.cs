using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDatabase : Singleton<CarDatabase>
{
    [SerializeField] private List<CarSettings> cars;

    public CarSettings First() => cars[0];
    public CarSettings Get(int index) => cars[index];
    public int Next(int index)
    {
        int nextIndex = index + 1;

        if (nextIndex >= cars.Count)
            nextIndex = 0;

        return nextIndex;
    }
    public int Previous(int index)
    {
        int nextIndex = index - 1;

        if (nextIndex <= 0)
            nextIndex = cars.Count - 1;

        return nextIndex;
    }
}
