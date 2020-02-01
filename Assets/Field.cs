using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Field
{
    public static Field instance;
    [SerializeField]
    public List<Item> items;
    public int width;
    public int height;

    public Item this[int x, int y]
    {
        get { return items.Where(a => a.coordinates == new Vector2(x, y)).First(); }
        //set { items.Where(a=>a.coordinates == new Vector2(x, y)) = value; }
    }
    //public int Length { get { return items.Length; } }

    public Field(int width, int height)
    {
        instance = this;
        this.width = width;
        this.height = height;
        items = new List<Item>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                items.Add(CreateItem(Random.Range(0, 5), new Vector2(x, y)));
            }
        }
    }
    public Field(int size) : this(size, size) { }
    public Field() : this(8) { }


    override public string ToString()
    {
        string result = "";
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result += items.Where(a => a.coordinates == new Vector2(x, y)).First().ToString();
            }
            result += "\n";
        }
        return result;
    }

    public Item CreateItem(int id, Vector2 coordinates)
    {
        switch (id)
        {
            default:
            case 0:
                return new Item("A", coordinates);
            case 1:
                return new Item("B", coordinates);
            case 2:
                return new Item("C", coordinates);
            case 3:
                return new Item("D", coordinates);
            case 4:
                return new Item("E", coordinates);
        }
    }

    public void SwitchItems(Item from, Item to)
    {
        Debug.Log("Switched items " + from + " " + to);
        Vector2 temp = from.coordinates;
        from.MoveTo(to.coordinates);
        to.MoveTo(temp);

        CheckMatches();
    }

    public void CheckMatches()
    {
        List<MatchData> matches = new List<MatchData>();

        foreach (Item item in items)
        { 
            MatchData match = CheckMatchRecursive(item, new MatchData(item.type));
            if (match.isValid)
                matches.Add(match);
        }

        foreach (MatchData match in matches)
        {
            //Debug.Log(match);
            match.ScoreMatch();
        }

        foreach (Item item in items)
        {
            item.checkedForMatch = false;
        }
    }

    public MatchData CheckMatchRecursive(Item item, MatchData matchData)
    {
        if (item.checkedForMatch || item.type != matchData.type)
            return matchData;

        item.checkedForMatch = true;
        matchData.items.Add(item);

        IEnumerable<Item> nextItems = items.Where(a => 
            a.coordinates == item.coordinates + new Vector2(0,  1) ||
            a.coordinates == item.coordinates + new Vector2(0, -1) ||
            a.coordinates == item.coordinates + new Vector2(-1, 0) ||
            a.coordinates == item.coordinates + new Vector2(1,  0)
            );
        foreach (Item nextItem in nextItems)
        {
            CheckMatchRecursive(nextItem, matchData);
        }

        return matchData;
    }
}
