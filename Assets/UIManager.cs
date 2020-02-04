using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas canvas;
    public RectTransform panel;
    public GameObject buttonPrefab;
    public List<GameObject> buttons;

    public Text scoreText;

    int coroutineCounter = 0;

    public List<Mesh> buttonMeshes;
    public List<Material> buttonMaterials;

    //public bool visualiseDropping = false;
    public float buildingDropInterval = 0;
    public float scoreToDropInterval = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawField();
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
        GameManager.instance.field.scoreChanged += () => { scoreText.text = "$" + GameManager.instance.field.score; StartCoroutine(ScoreTextChanged()); };
    }

    void SetupItemButton(Item item)
    {
        GameObject button = Instantiate(buttonPrefab, panel);

        buttons.Add(button);

        button.GetComponent<IItemVisual>().Setup(item);
    }
    
    //Switching needs to be in IItemVisual
    public void SwitchItemButtons(Item from, Item to)
    {
        StartCoroutine(SwitchItemButtons(from.visualObject, to.visualObject));
    }

    public IEnumerator SwitchItemButtons(IItemVisual from, IItemVisual to)
    {
        Vector2 path = to.transform.GetComponent<RectTransform>().anchoredPosition - from.transform.GetComponent<RectTransform>().anchoredPosition;

        for (int i = 0; i < 25; i++)
        {
            from.transform.GetComponent<RectTransform>().anchoredPosition += path * 0.04f;
            to.transform.GetComponent<RectTransform>().anchoredPosition -= path * 0.04f;
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
        coroutineCounter = matches.Count;
        foreach (MatchData match in matches)
        {
            StartCoroutine(ScoreMatch(match));
        }
        while (coroutineCounter > 0) yield return null;

        foreach (Item item in GameManager.instance.field.items)
        {
            item.visualObject.UpdatePosition();
            if (buildingDropInterval != 0)
                yield return new WaitForSeconds(buildingDropInterval);
            item.visualObject.UpdateLook();
            //item.visualObject.Deselect(); does it work if i comment this? Seems to!
        }
        yield return new WaitForSeconds(scoreToDropInterval);
        //Here proceed to the next step
        GameManager.instance.DropItems();

    }

    public IEnumerator ScoreMatch(MatchData match)
    {
        foreach (Item item in match.items)
        {
            item.visualObject.Score();
        }

        yield return new WaitForSeconds(1);
        coroutineCounter--;
    }

    public void DropItemButtons()
    {
        StartCoroutine(DropItemButtonsC());
    }

    public IEnumerator DropItemButtonsC()
    {
        coroutineCounter = GameManager.instance.field.items.Count;
        foreach (Item item in GameManager.instance.field.items)
        {
            StartCoroutine(DropItemButton(item));
        }
        while (coroutineCounter > 0) yield return null;

        GameManager.instance.CheckMatches();
    }

    public IEnumerator DropItemButton(Item item)
    {
        Vector2 path = item.coordinates * 30 - item.visualObject.transform.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < 25; i++)
        {
            item.visualObject.transform.GetComponent<RectTransform>().GetComponent<RectTransform>().anchoredPosition += path * 0.04f;

            yield return new WaitForSeconds(0.02f);
        }
        coroutineCounter--;
    }

    public IEnumerator ScoreTextChanged()
    {
        for (int i = 0; i < 5; i++)
        {
            scoreText.fontSize += 1;
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < 5; i++)
        {
            scoreText.fontSize -= 1;
            yield return new WaitForSeconds(0.02f);
        }
    }
}