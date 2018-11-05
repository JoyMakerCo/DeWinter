using System;
using UnityEngine;

namespace Ambition
{
    public class InputHandler : MonoBehaviour
    {
        //Cursor Stuff
        public Texture2D CursorRest;
        public Texture2D CursorClick;
        public CursorMode CursorMode = CursorMode.Auto;
        public Vector2 HotSpot = Vector2.zero;

        // Use this for initialization
        void Awake()
        {
            Cursor.SetCursor(CursorRest, HotSpot, CursorMode);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("escape")) AmbitionApp.OpenDialog("EXIT_GAME");
            if (Input.GetMouseButtonDown(0)) Cursor.SetCursor(CursorClick, HotSpot, CursorMode);
            if (Input.GetMouseButtonUp(0)) Cursor.SetCursor(CursorRest, HotSpot, CursorMode);
        }
    }
}
