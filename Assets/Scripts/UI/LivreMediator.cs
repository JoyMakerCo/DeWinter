using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class LivreMediator : MonoBehaviour
	{
		private Text _text;

		void Awake ()
		{
			_text = GetComponent<Text>();
			DeWinterApp.Subscribe<int>(GameConsts.LIVRE, HandleLivre);
			HandleLivre(DeWinterApp.GetModel<GameModel>().Livre);
		}

		void OnDestroy()
		{
			DeWinterApp.Unsubscribe<int>(GameConsts.LIVRE, HandleLivre);
		}

		private void HandleLivre (int livre)
		{
			_text.text = "£" + livre.ToString("### ###");
		}
	}
}