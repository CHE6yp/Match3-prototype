using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float itemInterval = 3; //эту хуйню надо бл брать с другого места, TODO отсюда уебать
    public float margin = 5;

    // Use this for initialization
    void Start()
    {
        Camera.main.transform.position += new Vector3((GameManager.instance.field.width - 1) * (itemInterval / 2), 0);
        Camera.main.orthographicSize = (GameManager.instance.field.width * itemInterval + margin) * Screen.height / Screen.width * 0.5f;
    }
}
