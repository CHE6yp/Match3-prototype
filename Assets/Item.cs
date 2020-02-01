using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField]
    public string type;
    [SerializeField]
    public bool frozen;

    public Vector2 coordinates;
    public delegate void CoordAction(Vector2 coordinates);
    public CoordAction movedTo;
    public delegate void ItemAction();
    public ItemAction moved;
    public ItemAction scored;

    public bool checkedForMatch = false;
    public int fallingSteps = 0;
    
    public Item(string t)
    {
        type = t;
        frozen = false;
    }

    public Item(string t, Vector2 coord)
    {
        type = t;
        frozen = false;
        coordinates = coord;
    }

    public Item(string t, bool f)
    {
        type = t;
        frozen = f;
    }

    public override string ToString()
    {
        return "[" + type + fallingSteps + "]";
    }

    public Color GetColor()
    {
        
        switch (type)
        {
            default:
            case "A": return Color.red;
            case "B": return Color.green;
            case "C": return Color.blue;
            case "D": return Color.yellow;
            case "E": return Color.magenta;
        }
    }

    public void MoveTo(Vector2 coord)
    {
        coordinates = coord;
        movedTo?.Invoke(coordinates);
        moved?.Invoke(); //for some reason this doesn't work
    }

    public void Score()
    {
        IEnumerable<Item> upperItems = Field.instance.items.Where(a => a.coordinates.x == this.coordinates.x && a.coordinates.y > this.coordinates.y);
        foreach (Item item in upperItems)
        {
            item.fallingSteps++;
        }
        fallingSteps++;
        NewType();
        scored?.Invoke();
        MoveTo(coordinates +new Vector2(0, Field.instance.height-1));
    }

    public void NewType()
    {
        switch (Random.Range(0,5))
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
    }
    
}
