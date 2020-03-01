using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Field field;

    public GameObject itemVisualPrefab;

    public Item selectedItem;

    //todo rename
    private int _dropCounter = 0;
    public int dropCounter { get { return _dropCounter; }
        set {
            _dropCounter = value;
            if (_dropCounter == 0)
                field.CheckMatches();
        }
    }

    private int _scoringCounter = 0;
    public int scoringCounter
    {
        get { return _scoringCounter; }
        set
        {
            _scoringCounter = value;
            if (_scoringCounter == 0)
            {
                //get this out of here
                foreach (Item item in field.Items)
                {
                    //todo make method
                    item.visualObject.UpdatePosition();
                    item.visualObject.UpdateLook();
                }
                field.DropItems();
            }
        }
    }

    public float itemInterval = 3; //AAAAAAAAR TODO GET THIS SHIT FIGURED OUT

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
            SetupItemVisual(item);
        }
        field.itemsSwitched += SwitchItems;
        field.scoredMatches += ScoreMatches;
        field.droppedItems += DropItems;
        //TODO move somewhere somehow
        field.scoreChanged += () => {
            UIManager.instance.scoreText.text = "$" + field.score;
            UIManager.instance.ScoreTextChanged();
        };
    }

    void SetupItemVisual(Item item)
    {
        GameObject button = Instantiate(itemVisualPrefab);
        button.GetComponent<IItemVisual>().Setup(item);
    }

    //Switching needs to be in IItemVisual (or not?)
    void SwitchItems(Item from, Item to)
    {
        dropCounter = 2;
        StartCoroutine(from.visualObject.MoveTo(Vector2.zero));
        StartCoroutine(to.visualObject.MoveTo(Vector2.zero));
    }

    void ScoreMatches(List<MatchData> matches)
    {
        Camera.main.GetComponent<AudioSource>().Play();//for now, this will do. TODO make an audioManager
        scoringCounter = matches.Count;
        foreach (MatchData match in matches)
        {
            StartCoroutine(ScoreMatch(match));
        }
    }

    //TODO think about this
    IEnumerator ScoreMatch(MatchData match)
    {
        foreach (Item item in match.items)
        {
            item.visualObject.Score();
        }

        yield return new WaitForSeconds(.5f);
        scoringCounter--;
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

    void DropItems()
    {
        dropCounter = field.Items.Count;
        foreach (Item item in field.Items)
        {
            StartCoroutine(item.visualObject.Drop());
        }
    }
}
