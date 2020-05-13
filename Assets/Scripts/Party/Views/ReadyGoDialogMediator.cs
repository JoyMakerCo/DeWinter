using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
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
			Dictionary<string, string> conversationIntroList = AmbitionApp.GetPhrases("conversation_intro");
			DialogText.text = conversationIntroList[ Util.RNG.TakeRandom(conversationIntroList.Values)];
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
