using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Field field;

    public Item selectedItem;

    private void Awake()
    {
        selectedItem = null;
        instance = this;

        field = new Field(8,8);
        Debug.Log(field);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool SelectItem(Item item)
    {
        Debug.Log("Selected " + item.coordinates);
        Debug.Log(selectedItem);
        if (selectedItem == null || selectedItem == item || Vector2.Distance(selectedItem.coordinates, item.coordinates) > 1)
        {
            selectedItem = item;
            return false;
        }
        else
        {
            field.SwitchItems(selectedItem, item);
            selectedItem = null;
            return true;
        }
    }

    public void CheckMatches()
    {
        field.CheckMatches();
    }

    public void DropItems()
    {
        field.DropItems();
    }
}
