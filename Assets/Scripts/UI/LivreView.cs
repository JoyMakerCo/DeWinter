using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class LivreView : MonoBehaviour
	{
		private Text _text;

		void Awake ()
		{
			_text = GetComponent<Text>();
		}

		void OnEnable()
		{
			AmbitionApp.Subscribe<int>(GameConsts.LIVRE, HandleLivre);
			HandleLivre(AmbitionApp.GetModel<GameModel>().Livre);
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<int>(GameConsts.LIVRE, HandleLivre);
		}

		private void HandleLivre (int livre)
		{
			_text.text = "£" + livre.ToString("### ###");
		}
	}
}
