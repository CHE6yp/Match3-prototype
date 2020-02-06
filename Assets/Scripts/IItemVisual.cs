using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemVisual
{
    Item item { get; set; }
    Transform transform { get; }

    void Setup(Item item);
    void Select();
    void UpdateLook();
    void UpdatePosition();
    void MoveTo(Vector2 coordinates);
    void Drop();
    void Score();
}
