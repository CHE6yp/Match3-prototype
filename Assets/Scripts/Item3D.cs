using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item3D : MonoBehaviour, IItemVisual
{
    public Item item { get; set; }
    public new Transform transform { get; private set; }

    public static Item3D selectedButton;

    public void Setup(Item item)
    {
        transform = GetComponent<Transform>();
        this.item = item;
        item.visualObject = this;

        transform.position = item.coordinates * 30;

        UpdateLook();

        //buttonComponent.onClick.AddListener(Select);

        //item.droppedTo += (coordinates) => { Drop(coordinates); };
        //item.scored += Score;
    }

    public void UpdateLook()
    {
        int i = 0;
        switch (item.type)
        {
            default:
            case "A": i=0; break;
            case "B": i=1; break;
            case "C": i=2; break;
            case "D": i=3; break;
            case "E": i=4; break;
        }

        transform.GetChild(0).GetComponent<MeshFilter>().mesh = UIManager.instance.buttonMeshes[i];
        transform.GetChild(0).GetComponent<MeshRenderer>().material = UIManager.instance.buttonMaterials[i];

        transform.GetChild(1).GetComponent<ParticleSystem>().startColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
        transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
    }
    
    public void UpdatePosition()
    {
        transform.position = item.coordinates * 30;
    }

    //not really used, but let's leave it
    public void MoveTo(Vector2 coordinates)
    {
        transform.position = coordinates * 30;
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
            transform.GetChild(1).GetComponent<ParticleSystem>().Play(false);
        }
        else
        {
            selectedButton = null;
        }
    }

    public void Deselect()
    {
        transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
    }

    public void Score()
    {
        transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play(true);
    }
}
