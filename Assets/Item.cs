using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Item
{
    [SerializeField]
    public string type;
    [SerializeField]
    public bool frozen;

    public Item(string t)
    {
        type = t;
        frozen = false;
    }

    public Item(string t, bool f)
    {
        type = t;
        frozen = f;
    }

    public override string ToString()
    {
        return "[" + type + ((frozen) ? "F" : " ") + "]";
    }

}
