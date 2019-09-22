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

        void Awake() => Cursor.SetCursor(CursorRest, HotSpot, CursorMode);
        void Update() => Cursor.SetCursor(Input.GetMouseButton(0) ? CursorClick : CursorRest, HotSpot, CursorMode);
    }
}
