using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
    public delegate void MouseDragAction();
    public static event MouseDragAction OnMouseDrag;

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 5, 100, 30), "Click"))
        {
            if (OnMouseDrag != null)
                OnMouseDrag();
        }
    }
}
