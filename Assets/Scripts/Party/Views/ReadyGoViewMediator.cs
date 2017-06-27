using UnityEngine.UI;
using System;
using Dialog;

namespace Ambition
{
	public class ReadyGoViewMediator : DialogView
	{
		public Text dialogText;
		void Start()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			string[] conversationIntroList = model.ConversationIntros;
			dialogText.text = conversationIntroList[new System.Random().Next(conversationIntroList.Length)];
		}
	}
}
