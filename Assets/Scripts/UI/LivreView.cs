using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class LivreView : MonoBehaviour
	{
		private Text _text;
        private GameModel _model = AmbitionApp.GetModel<GameModel>();

        void Awake ()
		{
			_text = GetComponent<Text>();
			AmbitionApp.Subscribe<int>(GameConsts.LIVRE, HandleLivre);
            GetLivre(); // Gotta set those initial values
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(GameConsts.LIVRE, HandleLivre);
		}

		private void HandleLivre (int livre)
		{
			_text.text = "£" + livre.ToString("### ###");
		}

        private void GetLivre()
        {
            _text.text = "£" + _model.Livre.ToString("### ###");
        }
	}
}
