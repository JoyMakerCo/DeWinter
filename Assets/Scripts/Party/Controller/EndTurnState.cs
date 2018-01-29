using System;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
	public class EndTurnState : UState
	{
		private ModelSvc _models = App.Service<ModelSvc>();

		public override void OnEnterState ()
		{
			MapModel _mapModel = _models.GetModel<MapModel>();
			PartyModel _model = _models.GetModel<PartyModel>();
			GuestVO [] guests = _mapModel.Room.Guests;
			foreach (GuestVO guest in guests) guest.Interest--;
			_mapModel.Room.Guests = guests;
			_mapModel.Room.Guests = guests;
			AmbitionApp.SendMessage<GuestVO[]>(_mapModel.Room.Guests);
			_model.Turn++;
			if (_model.Turn%_model.FreeRemarkCounter == 0)
				App.Service<MessageSvc>().Send(PartyMessages.ADD_REMARK);
		}
	}
}
