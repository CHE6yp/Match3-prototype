using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform panel;
    public GameObject buttonPrefab;

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
        buttonTemp.GetComponent<RectTransform>().anchoredPosition = item.coordinates*30;

        buttonTemp.transform.GetChild(0).GetComponent<Text>().text = item.type;
        buttonTemp.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        buttonTemp.GetComponent<Image>().color = item.GetColor();
        buttonTemp.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.SelectItem(item); });
        item.movedTo += (coordinates) => { MoveTo(buttonTemp.transform, coordinates); };
    }

    public void MoveTo(Transform tr, Vector2 coordinates)
    {
        Debug.Log("moved to " + coordinates);
        //tr.position = new Vector3(x * 30 + canvas.GetComponent<RectTransform>().rect.width / 2, y * 30 + canvas.GetComponent<RectTransform>().rect.height / 2);
        tr.GetComponent<RectTransform>().anchoredPosition = coordinates*30;
    }
}
