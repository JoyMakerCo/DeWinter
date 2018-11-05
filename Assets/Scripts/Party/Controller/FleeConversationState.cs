using UFlow;

namespace Ambition
{
    public class FleeConversationState : UState
	{
		public override void OnEnterState()
		{
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO party = model.Party;

            // Lose a turn
            model.Turn++;

            //The Player is relocated to the Entrance
            MapModel mmod = AmbitionApp.GetModel<MapModel>();
            mmod.Room = mmod.Map.Entrance;

            AmbitionApp.OpenDialog("DEFEAT");
            AmbitionApp.SendMessage(PartyMessages.FLEE_CONVERSATION);
            AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
     	}
	}
}
