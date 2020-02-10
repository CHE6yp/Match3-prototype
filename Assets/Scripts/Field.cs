using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Field : IField
{
    public static Field instance;
    [SerializeField]
    public List<Item> items;
    public List<Item> Items { get { return items; } set { items = value; } }
    public int width;
    public int height;

    private int _score = 0;
    public int score { get { return _score; } set { _score = value; scoreChanged?.Invoke(); } }

    public delegate void FieldEvent();
    public event FieldEvent checkedMatches;
    public FieldEvent droppedItems;
    public FieldEvent scoreChanged;
    public delegate void SwitchItemsEvent(Item from, Item to);
    public SwitchItemsEvent itemsSwitched;
    public delegate void ScoredMatchesEvent(List<MatchData> matches);
    public ScoredMatchesEvent scoredMatches;

    public Item this[int x, int y]
    {
        get { return items.Where(a => a.coordinates == new Vector2(x, y)).First(); }
    }

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

    //TODO rewrite this to be more useful
    override public string ToString()
    {
        string result = "";
        foreach (Item item in items.OrderBy((i) => i.coordinates.x).OrderBy((i) => i.coordinates.y))
            result += item.ToString()+(item.coordinates.x==(width-1)?"\n":"");
        return result;
    }

    Item CreateItem(int id, Vector2 coordinates)
    {
        string type;
        switch (id)
        {
            default:
            case 0: 
                type = "A";
                break;
            case 1: 
                type = "B";
                break;
            case 2: 
                type = "C";
                break;
            case 3: 
                type = "D";
                break;
            case 4:
                type = "E";
                break;
        }

        //TODO Think about this monstrosity
        if ((items.Any((i) => i.coordinates == coordinates - new Vector2(0, 1)) && 
            items.Where((i) => i.coordinates == coordinates - new Vector2(0, 1)).First().type == type &&
            items.Any((i) => i.coordinates == coordinates - new Vector2(0, 2)) && 
            items.Where((i) => i.coordinates == coordinates - new Vector2(0, 2)).First().type == type)
            ||
            (items.Any((i) => i.coordinates == coordinates - new Vector2(1, 0)) &&
            items.Where((i) => i.coordinates == coordinates - new Vector2(1, 0)).First().type == type &&
            items.Any((i) => i.coordinates == coordinates - new Vector2(2, 0)) &&
            items.Where((i) => i.coordinates == coordinates - new Vector2(2, 0)).First().type == type))
        {
            Debug.Log("fofo");
            return CreateItem(Random.Range(0, 5), coordinates);
        }
            
        return new Item(type, coordinates);
    }

    public void SwitchItems(Item from, Item to)
    {
        Debug.Log("Switched items " + from + " " + to);
        Vector2 temp = from.coordinates;
        from.MoveTo(to.coordinates);
        to.MoveTo(temp);
        itemsSwitched?.Invoke(from, to);
    }

    public void SlideItem(Item item, Vector2 destination)
    {
        Item to = items.Where((i) => i.coordinates == item.coordinates + destination).First();
        if (to != null)
        {
            SwitchItems(item, to);
        }
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

    MatchData CheckMatchRecursive(Item item, MatchData matchData)
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

    MatchData CheckMatchHorizontal(Item item, MatchData matchData)
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

    MatchData CheckMatchVertical(Item item, MatchData matchData)
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

    void CombineMatches(List<MatchData> matches)
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
                    //TODO Check if I should do j-- here. Wont it skip a match?
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

    void ScoreMatches(List<MatchData> matches)
    {
        List<Item> scoredItems = new List<Item>();
        //3 foreach loops, cuz this needed to be in an order;
        foreach (MatchData match in matches)
        {
            match.ScoreMatch();
            scoredItems.AddRange(match.items);
        }
        foreach (Item item in scoredItems)
            item.MoveTo(new Vector2(item.coordinates.x, Field.instance.height - 1 + item.fallingSteps));
        foreach (Item item in scoredItems)
            item.fallingSteps = scoredItems.Where((a) => (a.coordinates.x == item.coordinates.x)).Count();

        scoredMatches?.Invoke(matches);
    }
}
