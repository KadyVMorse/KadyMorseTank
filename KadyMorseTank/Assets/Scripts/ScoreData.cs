using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ScoreData : IComparable<ScoreData>
{
    // states a string and float to tarck scores
    public string name;
    public float score;

    //Makes the name and Score mean Name and Score
    public ScoreData(string Name,float Score)
    {
        name = Name;
        score = Score;
    }
    // compares the scores obtained
    public int CompareTo(ScoreData other)
    {
        if(other == null)
        {
            return 1;
        }
        if(this.score > other.score)
        {
            return 1;
        }
        if(this.score < other.score)
        {
            return -1;
        }
        return 0;
    }
}
