﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class LivreMediator : MonoBehaviour
	{
		private Text _text;

		void Start ()
		{
			_text = GetComponent<Text>();
			DeWinterApp.Subscribe<AdjustValueVO>(HandleLivre);

			AdjustValueVO vo = new AdjustValueVO(GameConsts.LIVRE, DeWinterApp.GetModel<GameModel>().Livre, false);
			HandleLivre(vo);
		}
		
		private void HandleLivre (AdjustValueVO vo)
		{
			if (vo.Type == GameConsts.LIVRE && !vo.IsRequest)
				_text.text = "£" + vo.Amount.ToString("### ###");
		}
	}
}