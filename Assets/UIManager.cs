using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform panel;
    public GameObject buttonPrefab;
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
        foreach (Item item in GameManager.instance.field.items)
        {
            SetupItemButton(item);
        }
    }

    void SetupItemButton(Item item)
    {
        GameObject buttonTemp = Instantiate(buttonPrefab, panel);
        buttonTemp.GetComponent<RectTransform>().anchoredPosition = item.coordinates * 30;

        ItemButtonLook(buttonTemp, item);

        buttonTemp.GetComponent<Button>().onClick.AddListener(() => {
            if (selectedButton != null) selectedButton.GetComponent<Outline>().enabled = false;
            if (!GameManager.instance.SelectItem(item))
            {
                selectedButton = buttonTemp;
                SelectItemButton(buttonTemp);
            }
            else
            {
                selectedButton = null;
            }
        });
        item.movedTo += (coordinates) => { MoveTo(buttonTemp.transform, coordinates); };
        //item.moved += () => { MoveTo(buttonPrefab.transform, item); };
        item.scored += () => { Score(buttonTemp, item); };
    }

    void ItemButtonLook(GameObject button, Item item)
    {
        button.transform.GetChild(0).GetComponent<Text>().text = item.type;
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

    public void SelectItemButton(GameObject button)
    {
        button.GetComponent<Outline>().enabled = true;
    }

    public void Score(GameObject button, Item item)
    {
        SelectItemButton(button);
        ItemButtonLook(button, item);
    }
}
