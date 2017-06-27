using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class RepMediator : MonoBehaviour
	{
		private Text _text;

		void Start ()
		{
			_text = GetComponent<Text>();
			AmbitionApp.Subscribe<AdjustValueVO>(HandleRep);

			AdjustValueVO vo = new AdjustValueVO(GameConsts.REPUTATION, AmbitionApp.GetModel<GameModel>().Reputation, false);
			HandleRep(vo);
		}
		
		private void HandleRep (AdjustValueVO vo)
		{
			if (vo.Type == GameConsts.REPUTATION && !vo.IsRequest)
				_text.text = vo.Amount.ToString("###,###");
		}
	}
}