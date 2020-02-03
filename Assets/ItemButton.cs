using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{

    Item item;
    public static ItemButton selectedButton;
    public RectTransform rectTransform;
    public Button buttonComponent;

    public void Setup(Item item)
    {
        this.item = item;
        item.visualObject = this;

        rectTransform.anchoredPosition = item.coordinates * 30;

        ItemButtonLook();

        buttonComponent.onClick.AddListener(() =>
        {
            if (selectedButton != null) selectedButton.DeselectItemButton();
            if (!GameManager.instance.SelectItem(item))
            {
                SelectItemButton();
            }
            else
            {
                selectedButton = null;
            }
        });
        item.droppedTo += (coordinates) => { Drop(coordinates); };
        item.scored += Score;
    }

    public void ItemButtonLook()
    {
        transform.GetChild(0).GetComponent<Text>().text = item.type + item.fallingSteps;
        GetComponent<Image>().color = item.GetColor();

        switch (item.type)
        {
            default:
            case "A":
                transform.GetChild(1).GetComponent<MeshFilter>().mesh = UIManager.instance.buttonMeshes[0];
                transform.GetChild(1).GetComponent<MeshRenderer>().material = UIManager.instance.buttonMaterials[0];
                break;
            case "B":
                transform.GetChild(1).GetComponent<MeshFilter>().mesh = UIManager.instance.buttonMeshes[1];
                transform.GetChild(1).GetComponent<MeshRenderer>().material = UIManager.instance.buttonMaterials[1];
                break;
            case "C":
                transform.GetChild(1).GetComponent<MeshFilter>().mesh = UIManager.instance.buttonMeshes[2];
                transform.GetChild(1).GetComponent<MeshRenderer>().material = UIManager.instance.buttonMaterials[2];
                break;
            case "D":
                transform.GetChild(1).GetComponent<MeshFilter>().mesh = UIManager.instance.buttonMeshes[3];
                transform.GetChild(1).GetComponent<MeshRenderer>().material = UIManager.instance.buttonMaterials[3];
                break;
            case "E":
                transform.GetChild(1).GetComponent<MeshFilter>().mesh = UIManager.instance.buttonMeshes[4];
                transform.GetChild(1).GetComponent<MeshRenderer>().material = UIManager.instance.buttonMaterials[4];
                break;
        }
        transform.GetChild(2).GetComponent<ParticleSystem>().startColor = transform.GetChild(1).GetComponent<MeshRenderer>().material.color;
        transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>().startColor = transform.GetChild(1).GetComponent<MeshRenderer>().material.color;
    }

    //Why doesn't it work? Seems like a Unity bug
    public void MoveTo()
    {
        int tempId = Random.Range(0, 256);
        transform.GetComponent<RectTransform>().anchoredPosition = item.coordinates * 30;
    }

    public void MoveTo(Vector2 coordinates)
    {
        transform.GetComponent<RectTransform>().anchoredPosition = coordinates * 30;
    }

    public IEnumerator MoveToCouroutine(Transform tr, Vector2 coordinates)
    {
        Vector2 path = coordinates * 30 - tr.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < 25; i++)
        {
            tr.GetComponent<RectTransform>().anchoredPosition += path * 0.04f;

            yield return new WaitForSeconds(0.02f);
        }
    }

    public void Drop(Vector2 coordinates)
    {
        MoveTo(coordinates);
    }

    public void SelectItemButton()
    {
        selectedButton = this;
        GetComponent<Outline>().enabled = true;
        transform.GetChild(2).GetComponent<ParticleSystem>().Play(false);
    }

    public void DeselectItemButton()
    {
        GetComponent<Outline>().enabled = false;
        transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
    }

    public void Score()
    {
        transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>().Play(true);
    }
}
