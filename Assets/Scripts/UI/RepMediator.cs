using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class RepMediator : MonoBehaviour
	{
		private Text _text;

		void Awake ()
		{
			_text = GetComponent<Text>();
			DeWinterApp.Subscribe<PlayerReputationVO>(GameConsts.REPUTATION, HandleRep);

			GameModel model = DeWinterApp.GetModel<GameModel>();
			PlayerReputationVO vo = new PlayerReputationVO(model.Reputation, model.ReputationLevel);
			HandleRep(vo);
		}

		void OnDestroy()
		{
			DeWinterApp.Unsubscribe<PlayerReputationVO>(GameConsts.REPUTATION, HandleRep);
		}

		private void HandleRep (PlayerReputationVO vo)
		{
			_text.text = vo.Reputation.ToString("###,###");
		}
	}
}