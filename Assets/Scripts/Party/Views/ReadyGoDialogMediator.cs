using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using Dialog;
using Core;

namespace Ambition
{
	public class ReadyGoDialogMediator : DialogView, IPointerClickHandler
	{
		public const string DIALOG_ID = "READY_GO";
        public Text DialogText;
		void Start()
		{
			LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
			string[] conversationIntroList = model.GetList("conversation_intro");
			DialogText.text = conversationIntroList[Util.RNG.Generate(conversationIntroList.Length)];
		}

		public void OnPointerClick(PointerEventData eventData)
	    {
            FadeView view = GetComponent<FadeView>();
            if (view == null) Close();
            else
            {
                view.FadeOut();
                StartCoroutine(WaitForClose(view.FadeOutSeconds));
            }
        }

        public override void OnClose ()
        {
            AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
        }

        IEnumerator WaitForClose(float time)
        {
            yield return new WaitForSeconds(time);
            Close();
        }
	}
}
