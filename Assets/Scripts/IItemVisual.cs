using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemVisual
{
    Item item { get; set; }
    Transform transform { get; }

    void Setup(Item item);
    void Select();
    void Deselect();
    void UpdateLook();
    void UpdatePosition();
    IEnumerator MoveTo(Vector2 coordinates);
    IEnumerator Drop();
    void Score();
}
