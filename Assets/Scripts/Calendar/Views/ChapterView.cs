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
            AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
            ChapterVO chapter = Array.Find(model.Chapters, d => d.Date == calendar.Today);
            Splash.sprite = chapter.Splash;
            AmbitionApp.SendMessage(AudioMessages.PLAY, chapter.Sting);
            AmbitionApp.SendMessage(GameMessages.INHIBIT_MENU);
            Title.text = AmbitionApp.Localize(chapter.ID);
        }
    }
}
