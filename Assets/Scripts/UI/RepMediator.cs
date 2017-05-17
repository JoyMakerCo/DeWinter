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
			AmbitionApp.Subscribe<PlayerReputationVO>(HandleRep);
			_text.text = AmbitionApp.GetModel<GameModel>().Reputation.ToString("###,###");
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<PlayerReputationVO>(HandleRep);
		}

		private void HandleRep (PlayerReputationVO vo)
		{
			_text.text = vo.Reputation.ToString("###,###");
		}
	}
}