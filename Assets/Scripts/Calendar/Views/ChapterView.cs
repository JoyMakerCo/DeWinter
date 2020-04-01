using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class ChapterView : MonoBehaviour
    {
        public Image Splash;
        public Text Title;
        private void Start()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            GameModel model = AmbitionApp.GetModel<GameModel>();
            ChapterVO chapter = calendar.GetEvent<ChapterVO>();
            AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
            if (!string.IsNullOrWhiteSpace(chapter?.Sting.Name))
                AmbitionApp.SendMessage(AudioMessages.PLAY, chapter.Sting.Name);
            Splash.sprite = chapter?.Splash;
            AmbitionApp.SendMessage(GameMessages.INHIBIT_MENU);
            Title.text = AmbitionApp.Localize(chapter.Name);
        }
    }
}
