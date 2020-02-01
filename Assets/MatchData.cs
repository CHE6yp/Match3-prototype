using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchData 
{
    public string type;
    public List<Item> items;

    public MatchData(string t)
    {
        type = t;
        items = new List<Item>();
    }

    public override string ToString()
    {
        //return base.ToString();
        string result = "type "+type+"\n";
        foreach (Item item in items)
            result += item.coordinates.ToString()+"\n";
        return result;
    }

    public bool IsValid()
    {
        return (items.Count>=3);
    }
}
