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

        field = new Field();
        Debug.Log(field);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectItem(Item item)
    {
        Debug.Log("Selected "+item.coordinates);
        Debug.Log(selectedItem);
        if (selectedItem == null || selectedItem == item || Vector2.Distance(selectedItem.coordinates, item.coordinates) > 1)
            selectedItem = item;
        else
        {
            field.SwitchItems(selectedItem, item);
            selectedItem = null;
        }
    }
}
