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

    private int _score = 0;
    public int score { get { return _score; } set { _score = value; scoreChanged?.Invoke(); } }

    public delegate void FieldEvent();
    public FieldEvent checkedMatches;
    public FieldEvent droppedItems;
    public FieldEvent scoreChanged;
    public delegate void SwitchItemsEvent(Item from, Item to);
    public SwitchItemsEvent itemsSwitched;
    public delegate void ScoredMatchesEvent(List<MatchData> matches);
    public ScoredMatchesEvent scoredMatches;

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

    //TODO rewrite to foreach loop somehow
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
        itemsSwitched?.Invoke(from, to);

        //CheckMatches();
    }

    public void CheckMatches()
    {
        List<MatchData> matches = new List<MatchData>();

        foreach (Item item in items)
        { 
            MatchData match = CheckMatchHorizontal(item, new MatchData(item.type));
            if (match.isValid)
                matches.Add(match);
            match = CheckMatchVertical(item, new MatchData(item.type));
            if (match.isValid)
                matches.Add(match);
        }

        foreach (Item item in items)
        {
            item.checkedForMatch = false;
            item.checkedForMatchHorizontal = false;
            item.checkedForMatchVertical = false;
        }

        CombineMatches(matches);

        if (matches.Count!=0)
            ScoreMatches(matches);

        checkedMatches?.Invoke();
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

    public MatchData CheckMatchHorizontal(Item item, MatchData matchData)
    {
        if (item.checkedForMatchHorizontal || item.type != matchData.type)
            return matchData;

        item.checkedForMatchHorizontal = true;
        matchData.items.Add(item);

        IEnumerable<Item> nextItems = items.Where(a =>
            a.coordinates == item.coordinates + new Vector2(-1, 0) ||
            a.coordinates == item.coordinates + new Vector2(1, 0)
            );
        foreach (Item nextItem in nextItems)
        {
            CheckMatchHorizontal(nextItem, matchData);
        }

        return matchData;
    }

    public MatchData CheckMatchVertical(Item item, MatchData matchData)
    {
        if (item.checkedForMatchVertical || item.type != matchData.type)
            return matchData;

        item.checkedForMatchVertical = true;
        matchData.items.Add(item);

        IEnumerable<Item> nextItems = items.Where(a =>
            a.coordinates == item.coordinates + new Vector2(0, 1) ||
            a.coordinates == item.coordinates + new Vector2(0, -1)
            );
        foreach (Item nextItem in nextItems)
        {
            CheckMatchVertical(nextItem, matchData);
        }

        return matchData;
    }

    public void CombineMatches(List<MatchData> matches)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            for (int j = i+1; j < matches.Count; j++)
            {
                if (matches[i].items.Any(it => matches[j].items.Contains(it)))
                {
                    matches[i].items.AddRange(matches[j].items);
                    matches[i].items = matches[i].items.Distinct().ToList();
                    matches.RemoveAt(j);
                }
            }
        }
    }

    public void DropItems()
    {
        foreach (Item item in items)
        {
            item.Drop();
        }
        droppedItems?.Invoke();
    }

    public void ScoreMatches(List<MatchData> matches)
    {

        List<Item> scoredItems = new List<Item>();
        foreach (MatchData match in matches)
        {
            //Debug.Log(match);
            match.ScoreMatch();
            scoredItems.AddRange(match.items);
        }
        foreach (Item item in scoredItems)
        {
            item.MoveTo(new Vector2(item.coordinates.x, Field.instance.height - 1 + item.fallingSteps));
            //item.fallingSteps = scoredItems.Where((a)=>(a.coordinates.x == item.coordinates.x)).Count();
        }
        foreach (Item item in scoredItems)
            item.fallingSteps = scoredItems.Where((a) => (a.coordinates.x == item.coordinates.x)).Count();

        scoredMatches?.Invoke(matches);
    }
}
