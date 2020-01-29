using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Field field;

    public Vector2 selectedItem;

    private void Awake()
    {
        instance = this;

        field = new Field();
        Debug.Log(field.ToString());
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectItem(Vector2 coord)
    {
        Debug.Log("Selected "+coord);
        if (Vector2.Distance(selectedItem, coord) > 1)
            selectedItem = coord;
        else
        {
            field.SwitchItems(selectedItem, coord);
            selectedItem = new Vector2(-1, -1);
        }
    }
}
