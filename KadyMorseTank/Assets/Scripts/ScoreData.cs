using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ScoreData : IComparable<ScoreData>
{
    public string name;
    public float score;
    public ScoreData(string Name,float Score)
    {
        name = Name;
        score = Score;
    }
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
