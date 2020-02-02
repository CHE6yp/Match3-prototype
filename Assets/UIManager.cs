using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform panel;
    public GameObject buttonPrefab;
    public List<GameObject> buttons;
    public GameObject selectedButton;


    // Start is called before the first frame update
    void Start()
    {
        DrawField();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawField()
    {
        buttons = new List<GameObject>();
        foreach (Item item in GameManager.instance.field.items)
        {
            SetupItemButton(item);
        }
    }

    void SetupItemButton(Item item)
    {
        GameObject button = Instantiate(buttonPrefab, panel);
        buttons.Add(button);
        button.GetComponent<RectTransform>().anchoredPosition = item.coordinates * 30;

        ItemButtonLook(button, item);

        button.GetComponent<Button>().onClick.AddListener(() => {
            if (selectedButton != null) selectedButton.GetComponent<Outline>().enabled = false;
            if (!GameManager.instance.SelectItem(item))
            {
                selectedButton = button;
                SelectItemButton(button);
            }
            else
            {
                selectedButton = null;
            }
        });
        item.movedTo += (coordinates) => { MoveTo(button.transform, coordinates); };
        item.droppedTo += (coordinates) => { Drop(button.transform, coordinates); };
        //item.moved += () => { MoveTo(buttonPrefab.transform, item); };
        item.scored += () => { Score(button, item); };
        Field.instance.checkedMatches += () => { ItemButtonLook(button, item); };
    }

    void ItemButtonLook(GameObject button, Item item)
    {
        button.transform.GetChild(0).GetComponent<Text>().text = item.type +item.fallingSteps;
        button.GetComponent<Image>().color = item.GetColor();
    }

    //Why doesn't it work? Seems like a Unity bug
    public void MoveTo(Transform tr, Item item)
    {
        int tempId = Random.Range(0, 256);
        Debug.Log(tempId+"moved to " + item.coordinates);
        //tr.position = new Vector3(x * 30 + canvas.GetComponent<RectTransform>().rect.width / 2, y * 30 + canvas.GetComponent<RectTransform>().rect.height / 2);
        Debug.Log(tempId + " position " + tr.GetComponent<RectTransform>().anchoredPosition);
        tr.GetComponent<RectTransform>().anchoredPosition = item.coordinates * 30;
        Debug.Log(tempId + " new position " + tr.GetComponent<RectTransform>().anchoredPosition);
    }

    public void MoveTo(Transform tr, Vector2 coordinates)
    {
        int tempId = Random.Range(0, 256);
        Debug.Log(tempId + "moved to " + coordinates);
        //tr.position = new Vector3(x * 30 + canvas.GetComponent<RectTransform>().rect.width / 2, y * 30 + canvas.GetComponent<RectTransform>().rect.height / 2);
        Debug.Log(tempId + " position " + tr.GetComponent<RectTransform>().anchoredPosition);
        tr.GetComponent<RectTransform>().anchoredPosition = coordinates * 30;
        Debug.Log(tempId + " new position " + tr.GetComponent<RectTransform>().anchoredPosition);
    }

    public void Drop(Transform tr, Vector2 coordinates)
    {
        MoveTo(tr, coordinates);
    }

    public void SelectItemButton(GameObject button)
    {
        button.GetComponent<Outline>().enabled = true;
    }

    public void Score(GameObject button, Item item)
    {
        SelectItemButton(button);
        //ItemButtonLook(button, item);
    }
}
