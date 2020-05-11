﻿using System;
using UFlow;
namespace Ambition
{
    public class RegisterPartyControllerCmd : UFlowConfig
    {
        public override void Initialize()
        {
            State("InitParty");
            State("PickOutfit");
            State("HideHeader");
            State("Intro");
            State("Turns Left");
            State("Outtro");
            State("Exit Incident");
            State("After Party");
            State("Exit");
            State("Map Decision", false);
            State("Map");
            State("Broadcast Map");
            State("Pick Incidents");
            State("Conversation");
            State("Exit Conversation");

            Link("Turns Left", "Map Decision");
            Link("Exit Conversation", "Turns Left");

            BindLink<LoadSceneLink, string>("InitParty", "PickOutfit", SceneConsts.LOAD_OUT_SCENE);
            BindLink<MessageLink, string>("HideHeader", "Intro", GameMessages.COMPLETE);
            BindLink<CheckTurnsLink>("Turns Left", "Map Decision");
            BindLink<LoadSceneLink, string>("Map Decision", "Map", SceneConsts.MAP_SCENE);
            BindLink<LoadSceneLink, string>("Exit Incident", "After Party", SceneConsts.AFTER_PARTY_SCENE);
            BindLink<MessageLink, string>("After Party", "Exit", PartyMessages.END_PARTY);
            BindLink<MessageLink, string>("Pick Incidents", "Conversation", PartyMessages.SHOW_ROOM);

            Bind<InitPartyState>("InitParty");
            Bind<SendMessageState, string>("HideHeader", GameMessages.HIDE_HEADER);
            Bind<PickIncidentsState>("Pick Incidents");
            Bind<UMachine, string>("Intro", FlowConsts.INCIDENT_CONTROLLER);
            Bind<SendMessageState, string>("Broadcast Map", PartyMessages.SHOW_MAP);
            Bind<UMachine, string>("Exit Incident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<ExitPartyState>("Outtro");
            Bind<UMachine, string>("Conversation", FlowConsts.INCIDENT_CONTROLLER);
            Bind<ExitRoomState>("Exit Conversation");
            Bind<ExitAfterPartyState>("Exit");
        }
    }
}
