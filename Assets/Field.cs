using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Field
{
    [SerializeField]
    public List<Item> items;
    public int width;
    public int height;

    public Item this[int x, int y]
    {
        get { return items.Where(a=>a.coordinates == new Vector2(x,y)).First(); }
        //set { items.Where(a=>a.coordinates == new Vector2(x, y)) = value; }
    }
    //public int Length { get { return items.Length; } }

    public Field(int width, int height)
    {
        this.width = width;
        this.height = height;
        items = new List<Item>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                items.Add(CreateItem(Random.Range(0, 5),new Vector2(x,y)));
            }
        }
    }
    public Field(int size) : this(size,size) {}
    public Field() : this(8) { }


    override public string ToString()
    {
        string result = "";
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result += items.Where(a => a.coordinates == new Vector2(x,y)).First().ToString();
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
        //todo checks
        /*
        Item item = items[(int)to.y][(int)to.x];
        items[(int)to.y][(int)to.x] = items[(int)from.y][(int)from.x];
        items[(int)to.y][(int)to.x] = item;
        */
    }
}
