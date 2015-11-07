using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public delegate void MouseDragAction();
    public static event MouseDragAction OnMouseDrag;

    void OnGUI()
    {
        if (OnMouseDrag != null)
            OnMouseDrag();
    }
}
