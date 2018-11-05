using Core;

namespace Ambition
{
    public class RegisterPartyControllerCmd : ICommand
    {
        public void Execute()
        {
            // Initial party states
            AmbitionApp.RegisterState<InitPartyGearState>("PartyController", "InitPartyGear");
            AmbitionApp.RegisterState<InitPartyState>("PartyController", "InitParty");
            AmbitionApp.RegisterState<GenerateMapState>("PartyController", "InitMap");
            AmbitionApp.RegisterState<LoadSceneState, string>("PartyController", "PickOutfit", SceneConsts.LOAD_OUT_SCENE);
            AmbitionApp.RegisterState<LoadSceneState, string>("PartyController", "EnterParty", SceneConsts.PARTY_SCENE);

            AmbitionApp.RegisterLink<CheckOutfitLink>("PartyController", "InitPartyGear", "InitParty");
            AmbitionApp.RegisterLink<NoOutfitLink>("PartyController", "InitPartyGear", "PickOutfit");
            AmbitionApp.RegisterLink<WaitForOutfitLink>("PartyController", "PickOutfit", "InitParty");
            AmbitionApp.RegisterLink("PartyController", "InitParty", "InitMap");
            AmbitionApp.RegisterLink("PartyController", "InitMap", "EnterParty");


            // Turn states
            AmbitionApp.RegisterState("PartyController", "CheckIncidents");
            AmbitionApp.RegisterState<InvokeMachineState, string>("PartyController", "Incident", "IncidentController");
            AmbitionApp.RegisterState<SendMessageState, string>("PartyController", "PartyMap", PartyMessages.SHOW_MAP);
            AmbitionApp.RegisterState<SendMessageState, string>("PartyController", "ShowRoom", PartyMessages.SHOW_ROOM);
            AmbitionApp.RegisterState<InvokeMachineState, string>("PartyController", "Conversation", "ConversationController");
            AmbitionApp.RegisterState("PartyController", "ValidateRoom");
            AmbitionApp.RegisterState<SendMessageState, string>("PartyController", "EndTurn", PartyMessages.END_TURN);

            AmbitionApp.RegisterLink("PartyController", "EnterParty", "CheckIncidents");
            AmbitionApp.RegisterLink<CheckIncidentLink>("PartyController", "CheckIncidents", "Incident");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "Incident", "PartyMap", IncidentMessages.END_INCIDENTS);
            AmbitionApp.RegisterLink("PartyController", "CheckIncidents", "PartyMap");
            AmbitionApp.RegisterLink<WaitForRoomLink>("PartyController", "PartyMap", "ValidateRoom");
            AmbitionApp.RegisterLink<ValidateRoomLink>("PartyController", "ValidateRoom", "ShowRoom");
            AmbitionApp.RegisterLink("PartyController", "ValidateRoom", "EndTurn");
            AmbitionApp.RegisterLink("PartyController", "ShowRoom", "Conversation");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "Conversation", "EndTurn", PartyMessages.END_CONVERSATION);
            AmbitionApp.RegisterLink("PartyController", "EndTurn", "PartyMap");

            // End Party
            AmbitionApp.RegisterState<LoadSceneState, string>("PartyController", "AfterPartyResults", SceneConsts.AFTER_PARTY_SCENE);
            AmbitionApp.RegisterState<SendMessageState, string>("PartyController", "NextDay", CalendarMessages.NEXT_DAY);
            AmbitionApp.RegisterState<InvokeMachineState, string>("PartyController", "ReturnToEstate", "EstateController"); // TODO: Make PartyController sub to GameController

            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "PartyMap", "AfterPartyResults", PartyMessages.LEAVE_PARTY);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "AfterPartyResults", "ReturnToEstate", PartyMessages.END_PARTY);
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("PartyController", "ReturnToEstate", "NextDay", GameMessages.FADE_OUT_COMPLETE);
        }
    }
}
