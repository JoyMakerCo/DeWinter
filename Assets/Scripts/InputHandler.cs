using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Dialog;

namespace Ambition
{
    public class InputHandler : MonoBehaviour
    {
        private const string SUBMIT_BTN = "Submit";
        private const string CANCEL_BTN = "Cancel";
        private const string X_BTN = "x";
        private const string Y_BTN = "y";
        private float TEMPO = 4f; //Buttons per second

        //Cursor Stuff
        public Texture2D CursorRest;
        public Texture2D CursorClick;
        public CursorMode CursorMode = CursorMode.Auto;
        public Vector2 HotSpot = Vector2.zero;
        public DialogManager dialogManager;
        public SceneMediator sceneMediator;

        private ISubmitHandler _submitHandler = null;
        private IButtonInputHandler _buttonHandler = null;
        private IAnalogInputHandler _stickHandler = null;
        private IDirectionInputHandler _directionHandler = null;
        private Vector2[] _sticks = new Vector2[2];
        private Dictionary<int, ScrollRect> _scrollRects = new Dictionary<int, ScrollRect>();
        private Dictionary<int, int> _selectedIndices = new Dictionary<int, int>();

        void Start()
        {
            Cursor.SetCursor(CursorRest, HotSpot, CursorMode);
            AmbitionApp.Subscribe<string>(GameMessages.SCENE_LOADED, HandleSceneLoaded);
            AmbitionApp.Subscribe<string>(GameMessages.DIALOG_OPENED, HandleDialogOpened);
            AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, HandleDialogClosed);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Subscribe<float>(GameMessages.FADE_OUT, HandleFadeOut);
            HandleSceneLoaded(sceneMediator.SceneID);
        }

        void Update()
        {
            Cursor.SetCursor(Input.GetMouseButton(0) ? CursorClick : CursorRest, HotSpot, CursorMode);
#if DEBUG
            if (Input.GetKeyDown(KeyCode.F10))
            {
                AmbitionApp.SendMessage(GameMessages.TOGGLE_CONSOLE);
            }
#endif
            Vector2 direction = Vector2.zero;

            // Button Down
            if (Input.GetButtonDown(SUBMIT_BTN))
            {
                _submitHandler?.Submit();
            }
            else if (Input.GetButtonDown(CANCEL_BTN))
            {
                _submitHandler?.Cancel();
            }

            else if (Input.GetButtonDown(X_BTN))
            {
                _buttonHandler?.HandleInput(X_BTN, false);
            }
            else if (Input.GetButtonDown(Y_BTN))
            {
                _buttonHandler?.HandleInput(Y_BTN, false);
            }


            // Holding Button
            if (Input.GetButton(X_BTN))
            {
                _buttonHandler?.HandleInput(X_BTN, true);
            }
            else if (Input.GetButton(Y_BTN))
            {
                _buttonHandler?.HandleInput(Y_BTN, true);
            }

#if UNITY_STANDALONE
            if (_stickHandler != null)
            {
                direction = Vector2.zero;
                if (Input.GetKey(KeyCode.A)) direction.x = -1;
                else if (Input.GetKey(KeyCode.D)) direction.x = 1;
                if (Input.GetKey(KeyCode.W)) direction.y = 1;
                else if (Input.GetKey(KeyCode.S)) direction.y = -1;
                _sticks[0] = direction;

                if (Input.GetKey(KeyCode.Keypad1)) direction.x = direction.y = -1;
                else if (Input.GetKey(KeyCode.Keypad2)) direction.y = -1;
                else if (Input.GetKey(KeyCode.Keypad4)) direction.x = -1;
                else if (Input.GetKey(KeyCode.Keypad6)) direction.x = 1;
                else if (Input.GetKey(KeyCode.Keypad8)) direction.y = 1;
                else if (Input.GetKey(KeyCode.Keypad9)) direction.x = direction.y = 1;
                else if (Input.GetKey(KeyCode.Keypad3))
                {
                    direction.x = 1;
                    direction.y = -1;
                }
                else if (Input.GetKey(KeyCode.Keypad7))
                {
                    direction.x = -1;
                    direction.y = 1;
                }
                else direction = Vector2.zero;
                _sticks[1] = direction;
                _stickHandler.HandleInput(_sticks);
            }

            if (_directionHandler != null)
            {
                direction = Vector2.zero;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) direction.x = 1;
                else if (Input.GetKeyDown(KeyCode.RightArrow)) direction.x = 1;
                if (Input.GetKeyDown(KeyCode.UpArrow)) direction.y = 1;
                else if (Input.GetKeyDown(KeyCode.DownArrow)) direction.y = -1;
                _directionHandler?.HandleInput((int)direction.x, (int)direction.y);
            }

            float val = Input.GetAxis("direction_1_x");
            if (val != 0) Debug.Log(val);
#else
                        else if (_submitHandler != null)
                        {
                            if (Input.GetButtonDown("Submit"))
                            {
                                _submitHandler.Submit();
                                _submitHandler = null;
                            }
                            else if (Input.GetButtonDown("Cancel"))
                            {
                                _submitHandler.Cancel();
                                _submitHandler = null;
                            }
                        }

                        if (_buttonHandler != null)
                        {
                            for (int i = 1; i <= 4; ++i)
                            {
                                if (Input.GetButtonDown("button_" + i))
                                    _buttonHandler.HandleInput(i);
                            }
                        }

                        if (_stickHandler != null)
                        {
                            // 0 = left analog
                            // 1 = right analog
                            // 2 = directional pad
                            // 3 = bumpers/triggers
                            Vector2 direction;
                            for (int i = 0; i <= 2; ++i)
                            {
                                direction.x = Input.GetAxis("direction_" + i + "_x");
                                direction.y = Input.GetAxis("direction_" + i + "_x");
                                if (direction.sqrMagnitude > 0)
                                    _stickHandler.HandleInput(i, direction);
                            }
                            direction = Vector2.zero;
                            if (Input.GetButtonDown("right")) direction.x = 1f;
                            if (Input.GetButtonDown("left")) direction.x -= 1f;
                            if (Input.GetButtonDown("up")) direction.y = 1f;
                            if (Input.GetButtonDown("down")) direction.y -= 1f;
                            if (direction.sqrMagnitude > 0)
                                _stickHandler.HandleInput(3, direction);

                            direction = Vector2.zero;
                            if (Input.GetButtonDown("bumper_r")) direction.x = 1f;
                            if (Input.GetButtonDown("bumper_l")) direction.x -= 1f;
                            if (direction.x != 0) _stickHandler.HandleInput(4, direction);
                        }
#endif
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<string>(GameMessages.SCENE_LOADED, HandleSceneLoaded);
            AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_OPENED, HandleDialogOpened);
            AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_CLOSED, HandleDialogClosed);
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Unsubscribe<float>(GameMessages.FADE_OUT, HandleFadeOut);
        }

        private void HandleSceneLoaded(string sceneID) => HandleDialog();
        private void HandleDialogOpened(string dialogID) => HandleDialog();
        private void HandleDialogClosed(string dialogID) => HandleDialog();
        private void HandleDialog()
        {
            ScrollRect[] scrollRects;
            _scrollRects = new Dictionary<int, ScrollRect>();
            _selectedIndices = new Dictionary<int, int>();
            if (dialogManager.NumDialogs > 0)
            {
                DialogView dialog = dialogManager.GetTopDialog().GetComponent<DialogView>();
                _submitHandler = dialog as ISubmitHandler;
                _buttonHandler = dialog as IButtonInputHandler;
                _stickHandler = dialog as IAnalogInputHandler;
                _directionHandler = dialog as IDirectionInputHandler;
                scrollRects = (dialog as IScrollInputHandler)?.GetScrollRects();
            }
            else
            {
                SceneView scene = sceneMediator.SceneObject.GetComponent<SceneView>();
                _submitHandler = scene as ISubmitHandler;
                _buttonHandler = scene as IButtonInputHandler;
                _stickHandler = scene as IAnalogInputHandler;
                _directionHandler = scene as IDirectionInputHandler;
                scrollRects = (scene as IScrollInputHandler)?.GetScrollRects();
            }
            if (scrollRects != null)
            {
                for (int i= scrollRects.Length-1; i>=0; --i)
                {
                    if (scrollRects[i] != null)
                    {
                        _scrollRects[i] = scrollRects[i];
                        _selectedIndices[i] = 0;
                    }
                }
            }
        }

        private void HandleFadeOut(float time) => HandleFadeOut();
        private void HandleFadeOut()
        {
            _submitHandler = null;
            _buttonHandler = null;
            _stickHandler = null;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
