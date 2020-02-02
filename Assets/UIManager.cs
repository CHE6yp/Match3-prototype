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

    public int scoringMatches = 0;


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
        GameManager.instance.field.itemsSwitched += SwitchItemButtons;
        GameManager.instance.field.scoredMatches += ScoreMatches;
        GameManager.instance.field.droppedItems += DropItemButtons;
    }

    void SetupItemButton(Item item)
    {
        GameObject button = Instantiate(buttonPrefab, panel);
        item.visualObject = button;
        buttons.Add(button);
        button.GetComponent<RectTransform>().anchoredPosition = item.coordinates * 30;

        ItemButtonLook(button, item);

        button.GetComponent<Button>().onClick.AddListener(() => {
            if (selectedButton != null) DeselectItemButton(selectedButton);
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
        //item.movedTo += (coordinates) => { MoveTo(button.transform, coordinates); };
        item.droppedTo += (coordinates) => { Drop(button.transform, coordinates); };
        //item.moved += () => { MoveTo(buttonPrefab.transform, item); };
        item.scored += () => { Score(button, item); };
        //Field.instance.checkedMatches += () => { ItemButtonLook(button, item); };
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
        tr.GetComponent<RectTransform>().anchoredPosition = item.coordinates * 30;
    }

    public void MoveTo(Transform tr, Vector2 coordinates)
    {
        int tempId = Random.Range(0, 256);

        tr.GetComponent<RectTransform>().anchoredPosition = coordinates * 30;
        //StartCoroutine(MoveToCouroutine(tr, coordinates));
    }

    public IEnumerator MoveToCouroutine(Transform tr, Vector2 coordinates)
    {
        Vector2 path = coordinates*30 - tr.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < 25; i++)
        {
            tr.GetComponent<RectTransform>().anchoredPosition += path * 0.04f ;
           
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void Drop(Transform tr, Vector2 coordinates)
    {
        MoveTo(tr, coordinates);
    }

    public void SelectItemButton(GameObject button)
    {
        button.GetComponent<Outline>().enabled = true;
    }

    public void DeselectItemButton(GameObject button)
    {
        button.GetComponent<Outline>().enabled = false;
    }

    public void Score(GameObject button, Item item)
    {
        SelectItemButton(button);
        //ItemButtonLook(button, item);
    }

    public void SwitchItemButtons(Item from, Item to)
    {
        StartCoroutine(SwitchItemButtons(from.visualObject, to.visualObject));
    }

    public IEnumerator SwitchItemButtons(GameObject from, GameObject to)
    {
        Vector2 path = to.GetComponent<RectTransform>().anchoredPosition - from.GetComponent<RectTransform>().anchoredPosition;
        //Debug.Log("movepath " + path);
        for (int i = 0; i < 25; i++)
        {
            from.GetComponent<RectTransform>().anchoredPosition += path * 0.04f;
            to.GetComponent<RectTransform>().anchoredPosition -= path * 0.04f;
            //Debug.Log(tr.GetComponent<RectTransform>().anchoredPosition);
            yield return new WaitForSeconds(0.02f);
        }
        GameManager.instance.CheckMatches();
    }

    public void ScoreMatches(List<MatchData> matches)
    {
        StartCoroutine(ScoreMatchesC(matches));
    }

    public IEnumerator ScoreMatchesC(List<MatchData> matches)
    {
        scoringMatches = matches.Count;
        foreach (MatchData match in matches)
        {
            StartCoroutine(ScoreMatch(match)); 
        }
        while (scoringMatches > 0) yield return null;
        //Here proceed to the next step
        GameManager.instance.DropItems();

    }

    public IEnumerator ScoreMatch(MatchData match)
    {
        foreach (Item item in match.items)
        {
            SelectItemButton(item.visualObject);
        }
        yield return new WaitForSeconds(1);
        foreach (Item item in match.items)
        {
            MoveTo(item.visualObject.transform, item);
            ItemButtonLook(item.visualObject, item);
            DeselectItemButton(item.visualObject);
        }
        yield return new WaitForSeconds(1);
        scoringMatches--;
    }

    public void DropItemButtons()
    {
        StartCoroutine(DropItemButtonsC());
    }

    public IEnumerator DropItemButtonsC()
    {
        scoringMatches = GameManager.instance.field.items.Count;
        foreach (Item item in GameManager.instance.field.items)
        {
            StartCoroutine(DropItemButton(item));
        }
        while (scoringMatches > 0) yield return null;
        //Here proceed to the next step
        GameManager.instance.DropItems();

    }

    public IEnumerator DropItemButton(Item item)
    {
        Vector2 path = item.coordinates * 30 - item.visualObject.transform.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < 25; i++)
        {
            item.visualObject.transform.GetComponent<RectTransform>().GetComponent<RectTransform>().anchoredPosition += path * 0.04f;

            yield return new WaitForSeconds(0.02f);
        }
    }

}
