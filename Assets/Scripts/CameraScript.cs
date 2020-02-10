using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Camera.main.transform.position += new Vector3(GameManager.instance.field.width*30 / 2 - 15, 0, 0);
        Camera.main.orthographicSize = (GameManager.instance.field.width*30 +50) * Screen.height / Screen.width * 0.5f;
        //Camera.main.orthographicSize = GameManager.instance.field.width *30 / 2;
    }
}
