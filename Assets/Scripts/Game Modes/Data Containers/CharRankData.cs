using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRankData
{
    public int PlayerID { get; private set; }
    public bool IsPlayer { get; private set; }
    public int Points { get; private set; }

    public CharRankData(int _PlayerID = -1, int points = 0)
    {
        PlayerID = _PlayerID;
        Points = points;
        IsPlayer = PlayerID != -1;
    }

    public void AddPoints(int amount = 1)
    {
        if (amount <= 0)
            Debug.LogWarning("Trying to add negative points...");

        Points += amount;
    }

    public override string ToString()
    {
        return $"Player: {PlayerID} - Points: {Points}";
    }
}
