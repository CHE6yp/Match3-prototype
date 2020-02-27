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


    public Text fpsText;
    public float updateRateSeconds = 4.0F;
    int frameCount = 0;
    float dt = 0.0F;
    float fps = 0.0F;

    int coroutineCounter = 0;

    public List<Mesh> buttonMeshes;
    public List<Material> buttonMaterials;

    //public bool visualiseDropping = false;
    public float buildingDropInterval = 0;
    public float scoreToDropInterval = 0;

    public float itemInterval = 3; //AAAAAAAAR TODO GET THIS SHIT FIGURED OUT

    public float coroutineFrames = 25;
    public float coroutineTime = 0.5f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 1.0 / updateRateSeconds)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0F / updateRateSeconds;
        }
        fpsText.text = System.Math.Round(fps, 1).ToString("0.0");
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawField();
    }

    void DrawField()
    {
        buttons = new List<GameObject>();
        foreach (Item item in GameManager.instance.field.Items)
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
        GameObject button = Instantiate(buttonPrefab);

        buttons.Add(button);

        button.GetComponent<IItemVisual>().Setup(item);
    }
    
    //Switching needs to be in IItemVisual
    void SwitchItemButtons(Item from, Item to)
    {
        StartCoroutine(SwitchItemButtons(from.visualObject, to.visualObject));
    }

    IEnumerator SwitchItemButtons(IItemVisual from, IItemVisual to)
    {
        Vector3 path = to.transform.position - from.transform.position;

        for (int i = 0; i < coroutineFrames; i++)
        {
            from.transform.position += path * 1/ coroutineFrames;
            to.transform.position -= path * 1 / coroutineFrames;
            yield return new WaitForSeconds((1 / coroutineFrames) * coroutineTime/2);
        }
        GameManager.instance.CheckMatches();
    }

    void ScoreMatches(List<MatchData> matches)
    {
        StartCoroutine(ScoreMatchesC(matches));
        Camera.main.GetComponent<AudioSource>().Play();//for now, this will do. TODO make an audioManager
    }

    IEnumerator ScoreMatchesC(List<MatchData> matches)
    {
        coroutineCounter = matches.Count;
        foreach (MatchData match in matches)
        {
            StartCoroutine(ScoreMatch(match));
        }
        while (coroutineCounter > 0) yield return null;

        foreach (Item item in GameManager.instance.field.Items)
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

    IEnumerator ScoreMatch(MatchData match)
    {
        foreach (Item item in match.items)
        {
            item.visualObject.Score();
        }

        yield return new WaitForSeconds(coroutineTime);
        coroutineCounter--;
    }

    void DropItemButtons()
    {
        StartCoroutine(DropItemButtonsC());
    }

    IEnumerator DropItemButtonsC()
    {
        coroutineCounter = GameManager.instance.field.Items.Count;
        foreach (Item item in GameManager.instance.field.Items)
        {
            StartCoroutine(DropItemButton(item));
        }
        while (coroutineCounter > 0) yield return null;

        GameManager.instance.CheckMatches();
    }

    //why is this here?! Wtf
    IEnumerator DropItemButton(Item item)
    {
        Vector3 path = (Vector3)item.coordinates * itemInterval - item.visualObject.transform.position;

        for (int i = 0; i < coroutineFrames; i++)
        {
            item.visualObject.transform.position += path * 1/ coroutineFrames;

            yield return new WaitForSeconds((1 / coroutineFrames)*coroutineTime/2);//falling frame time 
        }
        GameObject.Find("debug2").GetComponent<Text>().text = coroutineFrames+"; "+coroutineTime+"; "+ (1 / coroutineFrames) * coroutineTime / 2;
        coroutineCounter--;
    }

    IEnumerator ScoreTextChanged()
    {
        for (int i = 0; i < 5; i++)
        {
            scoreText.fontSize += 10;
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < 5; i++)
        {
            scoreText.fontSize -= 10;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}