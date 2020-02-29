using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Field field;

    public GameObject buttonPrefab;

    public Item selectedItem;

    public int coroutineCounter;

    public float itemInterval = 3; //AAAAAAAAR TODO GET THIS SHIT FIGURED OUT

    public float coroutineFrames = 25;
    public float coroutineTime = 0.5f;

    //public bool visualiseDropping = false;
    public float buildingDropInterval = 0;
    public float scoreToDropInterval = 0;

    private void Awake()
    {
        selectedItem = null;
        instance = this;

        field = new Field(8,8);
        Debug.Log(field);
    }

    private void Start()
    {
        DrawField();
    }

    void DrawField()
    {
        foreach (Item item in field.Items)
        {
            SetupItemButton(item);
        }
        field.itemsSwitched += (from, to) => { StartCoroutine(SwitchItemButtons(from.visualObject, to.visualObject)); };
        field.scoredMatches += (matches) => { StartCoroutine(ScoreMatchesC(matches)); };
        field.droppedItems += () => StartCoroutine(DropItemButtonsC());
        //TODO move somewhere somehow
        field.scoreChanged += () => {
            UIManager.instance.scoreText.text = "$" + field.score;
            StartCoroutine(UIManager.instance.ScoreTextChanged());
        };
    }

    void SetupItemButton(Item item)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.GetComponent<IItemVisual>().Setup(item);
    }

    //Switching needs to be in IItemVisual (or not?)
    IEnumerator SwitchItemButtons(IItemVisual from, IItemVisual to)
    {
        Vector3 path = to.transform.position - from.transform.position;

        for (int i = 0; i < coroutineFrames / 2; i++)
        {
            from.transform.position += path * 1 / (coroutineFrames / 2);
            to.transform.position -= path * 1 / (coroutineFrames / 2);
            yield return new WaitForSeconds((1 / coroutineFrames) * coroutineTime / 2);
        }
        field.CheckMatches();
    }

    IEnumerator ScoreMatchesC(List<MatchData> matches)
    {
        Camera.main.GetComponent<AudioSource>().Play();//for now, this will do. TODO make an audioManager
        coroutineCounter = matches.Count;
        foreach (MatchData match in matches)
        {
            StartCoroutine(ScoreMatch(match));
        }
        while (coroutineCounter > 0) yield return null;

        foreach (Item item in field.Items)
        {
            item.visualObject.UpdatePosition();
            if (buildingDropInterval != 0)
                yield return new WaitForSeconds(buildingDropInterval);
            item.visualObject.UpdateLook();
            //item.visualObject.Deselect(); does it work if i comment this? Seems to!
        }
        yield return new WaitForSeconds(scoreToDropInterval);
        //Here proceed to the next step
        field.DropItems();
    }

    IEnumerator ScoreMatch(MatchData match)
    {
        foreach (Item item in match.items)
        {
            item.visualObject.Score();
        }

        yield return new WaitForSeconds(coroutineTime / 2);
        coroutineCounter--;
    }

    //TODO DELETE THIS! Its for windows debugging now
    public bool SelectItem(Item item)
    {
        if (selectedItem == null || selectedItem == item || Vector2.Distance(selectedItem.coordinates, item.coordinates) > 1)
        {
            selectedItem = item;
            return false;
        }
        else
        {
            field.SwitchItems(selectedItem, item);
            selectedItem = null;
            return true;
        }
    }

    IEnumerator DropItemButtonsC()
    {
        coroutineCounter = field.Items.Count;
        foreach (Item item in field.Items)
        {
            StartCoroutine(item.visualObject.Drop());
        }
        while (coroutineCounter > 0) yield return null;

        field.CheckMatches();
    }
}
