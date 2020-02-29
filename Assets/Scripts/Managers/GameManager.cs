using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Field field;
    public UnityEventQueueSystem queueSystem;

    public Item selectedItem;

    private void Awake()
    {
        selectedItem = null;
        instance = this;

        field = new Field(8,8);
        Debug.Log(field);
    }

    //public bool SelectItem(Item item)
    //{
    //    if (selectedItem == null || selectedItem == item || Vector2.Distance(selectedItem.coordinates, item.coordinates) > 1)
    //    {
    //        selectedItem = item;
    //        return false;
    //    }
    //    else
    //    {
    //        field.SwitchItems(selectedItem, item);
    //        selectedItem = null;
    //        return true;
    //    }
    //}

    public void CheckMatches()
    {
        field.CheckMatches();
    }

    public void DropItems()
    {
        field.DropItems();
    }
}
