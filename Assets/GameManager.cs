using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Field field;

    // Start is called before the first frame update
    void Start()
    {
        field = new Field();
        Debug.Log(field.ToString());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
