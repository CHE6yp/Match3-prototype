using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public Vector2 startPos;
    public Vector2 direction;
    public bool directionChosen;
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

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    startPos = touch.position;
                    directionChosen = false;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    direction = touch.position - startPos;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    directionChosen = true;

                    Debug.Log(direction);
                    if (direction.magnitude >= 100){

                        GameObject.Find("debug").GetComponent<UnityEngine.UI.Text>().text = direction.normalized.ToString();
                        if (Mathf.Abs(direction.normalized.x)>Mathf.Abs(direction.normalized.y))
                        {
                            //if x<0 left, x>0 right
                        }
                        if (Mathf.Abs(direction.normalized.x)<Mathf.Abs(direction.normalized.y))
                        {
                            //if y<0 down, y>0 up
                        }
                    }
                    break;
            }
        }
        if (directionChosen)
        {
            // Something that uses the chosen direction...
        }
    }
}
