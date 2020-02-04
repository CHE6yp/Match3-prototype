using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for future
public interface IField
{
    List<Item> Items { get; set; }

    void SwitchItems(Item from, Item to);
    void CheckMatches();
    void DropItems();
}
