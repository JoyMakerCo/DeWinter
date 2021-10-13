using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class ChapterView : SceneView, ISubmitHandler
    {
        public Image Splash;
        public Text Title;
        public Text Date;
        public FMODEvent AudioSting;

        private bool _interactable=false;

        public void Cancel() => Submit();
        public void Submit()
        {
            AmbitionApp.SendMessage(_interactable
                ? GameMessages.CONTINUE
                : GameMessages.INTERRUPT_FADE);
        }

        private void Awake() => AmbitionApp.Subscribe(GameMessages.FADE_IN_COMPLETE, HandleFade);
        private void OnDestroy() => AmbitionApp.Unsubscribe(GameMessages.FADE_IN_COMPLETE, HandleFade);
        private void HandleFade() => _interactable = true;
        private void Start()
        {
            CalendarModel calendar = AmbitionApp.Calendar;
            GameModel model = AmbitionApp.GetModel<GameModel>();
            AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
            ChapterVO chapter = Array.Find(model.Chapters, d => d.Date == calendar.Today);
            if (chapter.Splash != null) Splash.sprite = chapter.Splash;
            Title.text = AmbitionApp.Localize(chapter.ID);
            Date.text = AmbitionApp.GetModel<LocalizationModel>().Date;
            if (!string.IsNullOrWhiteSpace(chapter.Sting.Name))
               AmbitionApp.SendMessage(AudioMessages.PLAY, chapter.Sting);
            else if (!string.IsNullOrWhiteSpace(AudioSting.Name))
                AmbitionApp.SendMessage(AudioMessages.PLAY, AudioSting);
        }
    }
}
