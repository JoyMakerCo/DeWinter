using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class BonusRemarkMeter : MonoBehaviour
	{
		private const float INTERP_TIME = 0.5f;

		private ModelSvc _models = App.Service<ModelSvc>();
		private MessageSvc _messages = App.Service<MessageSvc>();
		private PartyModel _model;
		private Text _label;

		void Awake()
		{
			_label = GetComponent<Text>();
			_model = _models.GetModel<PartyModel>();
		}

		void Start ()
		{
			_label.text = _model.FreeRemarkCounter.ToString();
		}

		void OnEnable()
		{
			_messages.Subscribe<int>(PartyConstants.TURN, HandleTurn);
		}

		void OnDisable()
		{
			_messages.Unsubscribe<int>(PartyConstants.TURN, HandleTurn);
		}

		private void HandleTurn(int turn)
		{
			int total = _model.FreeRemarkCounter;
			turn = (turn-1)%total;
			_label.text = (total - turn).ToString();
		}
	}
}
