using UnityEngine;
using System;

public class PlayerInputEventsHandler : MonoBehaviour
{
    enum EMouse
    {
        left,
        right
    }

    public event Action OnLeftClick;
    public event Action OnRightClick;

    void Update()
    {
        if (Input.GetMouseButtonDown((int)EMouse.right))
        {
            OnRightClick?.Invoke();
        }
        else if (Input.GetMouseButtonDown((int)EMouse.left))
        {
            OnLeftClick?.Invoke();
        }
    }
}
