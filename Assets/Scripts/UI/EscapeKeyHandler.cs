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
            AmbitionApp.Subscribe(GameMessages.FADE_IN_COMPLETE, OnFadeInComplete);
            AmbitionApp.Subscribe(GameMessages.END_GAME, OnInhibitMenu);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, OnInhibitMenu);
            AmbitionApp.Subscribe(GameMessages.INHIBIT_MENU, OnInhibitMenu);
        }
        void Update()
        {
            if (!_inhibited && Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject dialog = DialogManager.GetTopDialog();
                if (dialog?.GetComponent<DialogView>() as IEscapeCloseDialog != null)
                    DialogManager.Close(dialog);
                else DialogManager.Open(DialogConsts.GAME_MENU);
            }
        }

        private void OnInhibitMenu() => _inhibited = true;
        private void OnFadeInComplete() => _inhibited = false;
    }
}
