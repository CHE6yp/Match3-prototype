using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas canvas;
    public RectTransform panel;

    public Text scoreText;

    public Text fpsText;
    public float updateRateSeconds = 4.0F;
    int frameCount = 0;
    float dt = 0.0F;
    float fps = 0.0F;

    public List<Mesh> buttonMeshes;
    public List<Material> buttonMaterials;


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

    }


    //Todo make an animation
    public IEnumerator ScoreTextChanged()
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