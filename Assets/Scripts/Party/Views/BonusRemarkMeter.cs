using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class BonusRemarkMeter : MonoBehaviour
	{
		private PartyModel _model;
		private Text _label;

		void Awake()
		{
			_label = GetComponent<Text>();
            _model = AmbitionApp.GetModel<PartyModel>();
		}

		void Start ()
		{
            _label.text = _model.FreeRemarkCounter.ToString();
		}

		void OnEnable()
		{
            AmbitionApp.Subscribe<int>(PartyMessages.ROUND, HandleRound);
		}

		void OnDisable()
		{
            AmbitionApp.Unsubscribe<int>(PartyMessages.ROUND, HandleRound);
		}

        private void HandleRound(int round)
		{
            round = (round-1)%_model.FreeRemarkCounter;
            _label.text = (_model.FreeRemarkCounter - round).ToString();
		}
	}
}
