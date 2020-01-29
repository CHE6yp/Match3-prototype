using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Field
{
    [SerializeField]
    public Item[][] items;

    public Field(int width, int height)
    {
        items = new Item[height][];
        for (int i = 0; i < height; i++)
        {
            items[i] = new Item[width];
            for (int k = 0; k < width; k++)
            {
                items[i][k] = CreateItem(Random.Range(0, 5));
            }
        }
    }

    public Field(int size) : this(size,size) {}
    public Field() : this(8) { }

    override public string ToString()
    {
        string result = "";
        for (int i = 0; i < items.Length; i++)
        {
            for (int k = 0; k < items[i].Length; k++)
            {
                result += items[i][k].ToString();
            }
            result += "\n";
        }
        return result;
    }

    public Item CreateItem(int id)
    {
        switch (id)
        {
            default:
            case 0:
                return new Item("A");
            case 1:
                return new Item("B");
            case 2:
                return new Item("C");
            case 3:
                return new Item("D");
            case 4:
                return new Item("E");
        }
    }

    public void SwitchItems(Vector2 from, Vector2 to)
    {
        //todo checks
        Item item = items[(int)to.y][(int)to.x];
        items[(int)to.y][(int)to.x] = items[(int)from.y][(int)from.x];
        items[(int)to.y][(int)to.x] = item;
    }
}
