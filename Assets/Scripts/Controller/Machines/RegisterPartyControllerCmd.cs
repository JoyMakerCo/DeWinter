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
            AmbitionApp.RegisterCommand<InitPartyCmd, PartyVO>(PartyMessages.INITIALIZE_PARTY);
            AmbitionApp.RegisterCommand<AcceptInvitationCmd, PartyVO>(PartyMessages.ACCEPT_INVITATION);
            AmbitionApp.RegisterCommand<DeclineInvitationCmd, PartyVO>(PartyMessages.DECLINE_INVITATION);

            State("InitParty");
            AmbitionApp.RegisterState<LoadSceneState, string>(FLOW_ID, "PickOutfit", SceneConsts.LOAD_OUT_SCENE);
            Link("InitParty", "PickOutfit");
            AmbitionApp.RegisterState<SendMessageState, string>(FLOW_ID, "HideHeader", GameMessages.HIDE_HEADER);
            Link("PickOutfit", "HideHeader");
            _lastState = null;
            State("MapTransition");
            State("PickMap");
            AmbitionApp.RegisterLink<MessageLink, string>(FLOW_ID, "HideHeader", "MapTransition", GameMessages.EXIT_SCENE);
            AmbitionApp.RegisterLink<FadeOutLink>(FLOW_ID, "MapTransition", "PickMap");
            State("Intro");
            Decision<CheckTurnsLink>("Turns Left", "Map", "Outtro");
            State("Conversation");
            State("Exit Conversation");
            AmbitionApp.RegisterState<LoadSceneState, string>(FLOW_ID, "Show Map", SceneConsts.MAP_SCENE);
            Link("Map", "Show Map");
            AmbitionApp.RegisterState<SendMessageState, string>(FLOW_ID, "Broadcast Map", PartyMessages.SHOW_MAP);
            Link("Show Map", "Broadcast Map");
            State("Pick Incidents");
            AmbitionApp.RegisterLink<MessageLink, string>(FLOW_ID, "Pick Incidents", "Conversation", PartyMessages.SHOW_ROOM);
            AmbitionApp.RegisterState<LoadSceneState, string>(FLOW_ID, "After Party", SceneConsts.AFTER_PARTY_SCENE);
            Link("Exit Conversation", "Turns Left");
            _lastState = null;
            State("Exit Incident");
            Link("Outtro", "Exit Incident");
            State("After Party");
            AmbitionApp.RegisterState<SendMessageState, string>(FLOW_ID, "Hide Header", GameMessages.HIDE_HEADER);
            Link("After Party", "Hide Header");
            State("Exit Party");
            AmbitionApp.RegisterLink<MessageLink, string>(FLOW_ID, "Hide Header", "Exit Party", PartyMessages.END_PARTY);

            AmbitionApp.BindState<InitPartyState>(FLOW_ID, "InitParty");
            AmbitionApp.BindState<PickIncidentsState>(FLOW_ID, "Pick Incidents");
            AmbitionApp.BindState<PickMapState>(FLOW_ID, "PickMap");
            AmbitionApp.BindMachineState(FLOW_ID, "Intro", "IncidentController");
            AmbitionApp.BindMachineState(FLOW_ID, "Exit Incident", "IncidentController");
            AmbitionApp.BindState<ExitPartyState>(FLOW_ID, "Outtro");
            AmbitionApp.BindMachineState(FLOW_ID, "Conversation", "IncidentController");
            AmbitionApp.BindState<ExitRoomState>(FLOW_ID, "Exit Conversation");
        }

        private void State(string StateID)
        {
            AmbitionApp.RegisterState(FLOW_ID, StateID);
            if (_lastState != null) Link(_lastState, StateID);
            _lastState = StateID;
        }

        private void Decision<T>(string StateID, string Yes, string No) where T:ULink, new()
        {
            State(StateID);
            State(No);
            _lastState = null;
            State(Yes);
            AmbitionApp.RegisterLink<T>(FLOW_ID, StateID, Yes);
            _lastState = null;
        }

        private void Link(string state0, string state1)
        {
            AmbitionApp.RegisterLink(FLOW_ID, state0, state1);
            _lastState = state1;
        }
    }
}
