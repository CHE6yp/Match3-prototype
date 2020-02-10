using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
                //Debug.Log(hit.collider.gameObject);
                hit.collider.GetComponent<IItemVisual>()?.Select();
            Debug.DrawRay(ray.origin, ray.direction*100, Color.yellow);
        }


    }
}
