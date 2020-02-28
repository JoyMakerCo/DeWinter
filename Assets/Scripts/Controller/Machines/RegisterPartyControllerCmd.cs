using System;
using UFlow;
namespace Ambition
{
    public class RegisterPartyControllerCmd : Core.ICommand
    {
        private const string FLOW_ID = "PartyController";
        private string _lastState = null;

        public void Execute()
        {
            State("InitParty");
            State("PickOutfit");
            State("Pick Outfit Input");
            State("MapTransition");
            State("PickMap");
            Link<FadeOutLink>("MapTransition", "PickMap");
            State("Intro");
            Decision<CheckTurnsLink>("Turns Left", "Map", "Outtro");
            State("Show Map");
            State("Broadcast Map");
            State("Pick Incidents");
            State("Show Room Input");
            State("Conversation");
            State("Exit Conversation");
            Link("Exit Conversation", "Turns Left");
            _lastState = null;
            State("Exit Incident");
            Link("Outtro", "Exit Incident");
            State("After Party");
            State("Hide Header");
            State("Exit Party Input");
            State("Exit Party");

            AmbitionApp.BindState<InitPartyState>(FLOW_ID, "InitParty");
            AmbitionApp.BindState<LoadSceneState>(FLOW_ID, "PickOutfit", SceneConsts.LOAD_OUT_SCENE);
            AmbitionApp.BindState<MessageInputState>(FLOW_ID, "Pick Outfit Input", GameMessages.EXIT_SCENE);
            AmbitionApp.BindState<PickIncidentsState>(FLOW_ID, "Pick Incidents");
            AmbitionApp.BindState<PickMapState>(FLOW_ID, "PickMap");
            AmbitionApp.BindState<UMachine>(FLOW_ID, "Intro", "IncidentController");
            AmbitionApp.BindState<UMachine>(FLOW_ID, "Exit Incident", "IncidentController");
            AmbitionApp.BindState<ExitPartyState>(FLOW_ID, "Outtro");
            AmbitionApp.BindState<LoadSceneState>(FLOW_ID, "Show Map", SceneConsts.MAP_SCENE);
            AmbitionApp.BindState<SendMessageState>(FLOW_ID, "Broadcast Map", PartyMessages.SHOW_MAP);
            AmbitionApp.BindState<MessageInputState>(FLOW_ID, "Show Room Input", PartyMessages.SHOW_ROOM);
            AmbitionApp.BindState<UMachine>(FLOW_ID, "Conversation", "IncidentController");
            AmbitionApp.BindState<LoadSceneState>(FLOW_ID, "After Party", SceneConsts.AFTER_PARTY_SCENE);
            AmbitionApp.BindState<ExitRoomState>(FLOW_ID, "Exit Conversation");
            AmbitionApp.BindState<SendMessageState>(FLOW_ID, "Hide Header", GameMessages.HIDE_HEADER);
            AmbitionApp.BindState<MessageInputState>(FLOW_ID, "Exit Party Input", PartyMessages.END_PARTY);

        }

        private void State(string StateID)
        {
            AmbitionApp.RegisterState(FLOW_ID, StateID);
            Link(_lastState, StateID);
        }
         
        private void Link(string state0, string state1)
        {
            if (state0 != null && state1 != null)
            {
                AmbitionApp.RegisterLink(FLOW_ID, state0, state1);
            }
            _lastState = state1;
        }

        private void Decision<L>(string StateID, string Yes, string No) where L:ULink, new()
        {
            State(StateID);
            State(No);
            _lastState = null;
            State(Yes);
            AmbitionApp.RegisterLink<L>(FLOW_ID, StateID, Yes);
            // _lastState is now the value of Yes.
        }

        private void Link<L>(string state0, string state1) where L:ULink, new()
        {
            if (state0 != null && state1 != null)
            {
                AmbitionApp.RegisterLink<L>(FLOW_ID, state0, state1);
            }
            _lastState = state1;
        }
    }
}
