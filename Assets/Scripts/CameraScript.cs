using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float margin = 5;
    public GameObject background;

    // Use this for initialization
    void Start()
    {
        Camera.main.transform.position += new Vector3((GameManager.instance.field.width - 1) * (GameManager.instance.itemInterval / 2), 0);
        background.transform.position  += new Vector3((GameManager.instance.field.width - 1) * (GameManager.instance.itemInterval / 2), 0);

        Camera.main.orthographicSize = (GameManager.instance.field.width * GameManager.instance.itemInterval + margin) * Screen.height / Screen.width * 0.5f;
    }
}
