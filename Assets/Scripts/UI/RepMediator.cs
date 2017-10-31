using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class RepMediator : MonoBehaviour
	{
		private Text _text;

		void Awake ()
		{
			_text = GetComponent<Text>();
			AmbitionApp.Subscribe<ReputationVO>(HandleRep);
			_text.text = AmbitionApp.GetModel<GameModel>().Reputation.ToString("###,###");
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<ReputationVO>(HandleRep);
		}

		private void HandleRep (ReputationVO vo)
		{
			_text.text = vo.Reputation.ToString("###,###");
		}
	}
}