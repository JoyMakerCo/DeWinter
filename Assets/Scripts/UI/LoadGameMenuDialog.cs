using System;
using UnityEngine;
using UnityEngine.UI;
using Core;
namespace Ambition
{
    public class LoadGameMenuDialog : MonoBehaviour
    {
        public LoadGameView LoadView;
        public Text TitleText;

        private string _data;

        private void OnEnable()
        {
            TitleText.text = App.Service<LocalizationSvc>().GetString("restore.title");
            LoadView.PopulateDialog();
        }

        public void LoadSavedGameData(SavedGameListItem item)
        {
            if (!string.IsNullOrEmpty(item.Data))
            {
                _data = item.Data;
                App.Service<MessageSvc>().Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFade);
                App.Service<MessageSvc>().Send(GameMessages.FADE_OUT);
            }
        }

        private void HandleFade()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFade);
            AmbitionApp.Execute<LoadGameCmd, string>(_data);
        }
    }
}
