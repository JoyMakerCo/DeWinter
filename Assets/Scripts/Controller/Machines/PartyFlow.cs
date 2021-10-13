using System;
using UFlow;
namespace Ambition
{
    public class PartyFlow : UFlowConfig
    {
        public override void Configure()
        {
            State("InitParty");
            State("DefaultIntro?");
            State("DefaultIntro");
            State("Intro");
            State("Next Turn");
            State("Next Turn?");
            State("Map");
            State("Pick Incidents");
            State("Pick Room");
            State("Conversation");
            State("Exit Conversation");

            State("Outtro", false);
            State("Exit Incident");
            State("After Party");
            State("AfterPartyInput");
            State("Exit");


            Link("Next Turn?", "Outtro");
            Link("Exit Conversation", "Next Turn");
            Link("DefaultIntro?", "Intro");

            Decision("Next Turn?", ()=> AmbitionApp.GetModel<PartyModel>().TurnsLeft > 0);
            Decision("DefaultIntro?", () => AmbitionApp.GetModel<IncidentModel>().Incident == null);

            Bind<LoadSceneInput, string>("Map", SceneConsts.MAP_SCENE);
            Bind<LoadSceneInput, string>("After Party", SceneConsts.AFTER_PARTY_SCENE);
            Bind<MessageInput, string>("Pick Room", PartyMessages.SHOW_ROOM);
            Bind<MessageInput, string>("AfterPartyInput", GameMessages.COMPLETE);
            Bind<MessageInput, string>("Pick Incidents", PartyMessages.SHOW_ROOM);

            Bind<InitPartyState>("InitParty");
            Bind<InitDefaultIntroState>("DefaultIntro");
            Bind<PickIncidentsState>("Pick Incidents");
            Bind<UMachine>("Intro", FlowConsts.INCIDENT_CONTROLLER);
            Bind<NextTurnState>("Next Turn");
            Bind<UMachine>("Exit Incident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<ExitPartyState>("Outtro");
            Bind<UMachine>("Conversation", FlowConsts.INCIDENT_CONTROLLER);
            Bind<ExitRoomState>("Exit Conversation");
            Bind<ExitAfterPartyState>("Exit");
        }
    }
}
