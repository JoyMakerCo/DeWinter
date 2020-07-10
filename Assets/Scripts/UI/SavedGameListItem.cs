using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;
namespace Ambition
{
    public class SavedGameListItem : MonoBehaviour
    {
        public Text SaveText;
        public LoadGameView View;
        public string Data;

        public void Load()
        {
            AmbitionApp.OpenDialog("restore", OnConfirm, new Dictionary<string, string>() { { "%g", SaveText.text } });
        }

        private void OnConfirm() => AmbitionApp.SendMessage(GameMessages.LOAD_GAME, Data);
    }
}
