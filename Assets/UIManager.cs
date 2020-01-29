using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
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
        for (int i = 0; i < GameManager.instance.field.Length; i++)
        {
            for (int k = 0; k < GameManager.instance.field[i].Length; k++)
            {
                SetupItemButton(k, i);
            }
        }
    }

    void SetupItemButton(int x, int y)
    {
        GameObject buttonTemp = Instantiate(buttonPrefab, new Vector3(
                    x * 30 + canvas.GetComponent<RectTransform>().rect.width / 2,
                    y * 30 + canvas.GetComponent<RectTransform>().rect.height / 2,
                    0),
                    Quaternion.identity, canvas.transform);
        buttonTemp.transform.GetChild(0).GetComponent<Text>().text = GameManager.instance.field[y][x].type;
        buttonTemp.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        buttonTemp.GetComponent<Image>().color = GameManager.instance.field[y][x].GetColor();
        buttonTemp.GetComponent<Button>().onClick.AddListener(() => { GameManager.instance.SelectItem(new Vector2(x, y)); });
        GameManager.instance.field[y][x].movedTo += (xc,yc) => { MoveTo(buttonTemp.transform, xc, yc); };

    }

    public void MoveTo(Transform tr, int x, int y)
    {
        Debug.Log("moved to " + x + "/" + y);
        tr.position = new Vector3(x * 30 + canvas.GetComponent<RectTransform>().rect.width / 2, y * 30 + canvas.GetComponent<RectTransform>().rect.height / 2);
    }

}
