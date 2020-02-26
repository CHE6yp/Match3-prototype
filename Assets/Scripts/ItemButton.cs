﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour, IItemVisual
{
    public Item item { get; set; }
    public new Transform transform { get; private set; }

    public static ItemButton selectedButton;
    public RectTransform rectTransform;
    public Button buttonComponent;

    public void Setup(Item item)
    {
        transform = GetComponent<Transform>();
        this.item = item;
        item.visualObject = this;

        rectTransform.anchoredPosition = item.coordinates * 30;

        UpdateLook();

        buttonComponent.onClick.AddListener(Select);

        //item.droppedTo += (coordinates) => { Drop(coordinates); };
        //item.scored += Score;
    }

    public void UpdateLook()
    {
        transform.GetChild(0).GetComponent<Text>().text = item.type + item.fallingSteps;
        GetComponent<Image>().color = item.GetColor();
    }
    
    public void UpdatePosition()
    {
        transform.GetComponent<RectTransform>().anchoredPosition = item.coordinates * 30;
    }

    //not really used, but let's leave it
    public void MoveTo(Vector2 coordinates)
    {
        transform.GetComponent<RectTransform>().anchoredPosition = coordinates * 30;
    }

    //todo make drops a couroutine and use this one
    public void Drop() { }
    public void Drop(Vector2 coordinates)
    {
        MoveTo(coordinates);
    }

    public void Select()
    {
        if (selectedButton != null) selectedButton.Deselect();

        //todo change gamemanage.inst.SelectItem(item) to item.Select();
        if (!GameManager.instance.SelectItem(item))
        {
            selectedButton = this;
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            selectedButton = null;
        }
    }

    void Deselect()
    {
        GetComponent<Outline>().enabled = false;
    }

    public void Score()
    {
        StartCoroutine(Score(1));
    }

    IEnumerator Score(float time)
    {
        GetComponent<Outline>().enabled = true;
        yield return new WaitForSeconds(time);
        Deselect();
    }
}