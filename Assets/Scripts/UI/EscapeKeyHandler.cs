using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

namespace Ambition
{
    public class EscapeKeyHandler : MonoBehaviour
    {
        public DialogManager DialogManager;
        private bool _inhibited;

        private void Awake()
        {
            _inhibited = true;
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, OnInhibitMenu);
            AmbitionApp.Subscribe(GameMessages.FADE_IN, OnInhibitMenu);
            AmbitionApp.Subscribe(GameMessages.FADE_IN_COMPLETE, OnUninhibitMenu);
            AmbitionApp.Subscribe(GameMessages.INHIBIT_MENU, OnInhibitMenu);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject dialog = DialogManager.GetTopDialog();
                if (dialog?.GetComponent<DialogView>() is IEscapeCloseDialog)
                {
                    DialogManager.Close(dialog);
                }
                else if (!_inhibited)
                {
                    DialogManager.Open(DialogConsts.GAME_MENU);
                }
            }
        }

        private void OnInhibitMenu() => _inhibited = true;
        private void OnUninhibitMenu() => _inhibited = false;

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN, OnInhibitMenu);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN_COMPLETE, OnUninhibitMenu);
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT, OnInhibitMenu);
            AmbitionApp.Unsubscribe(GameMessages.INHIBIT_MENU, OnInhibitMenu);
        }
    }
}
