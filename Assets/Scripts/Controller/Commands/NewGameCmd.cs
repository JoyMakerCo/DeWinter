using System;
using System.Collections.Generic;
using Core;
namespace Ambition
{
    public class NewGameCmd : Core.ICommand<string>
    {
        public void Execute(string playerID)
        {
            GameModel game = AmbitionApp.Game;
            if (!game.Initialized) AmbitionApp.Execute<InitGameCmd>();
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();

            AmbitionApp.Execute<InitPlayerCmd, string>(playerID);
            List<IncidentVO> incidents = new List<IncidentVO>(incidentModel.Incidents.Values);

            foreach (IncidentVO incident in incidents)
            {
                if (incident.IsScheduled) incidentModel.Schedule(incident, incident.Date);
            }
            IncidentVO incdent = incidentModel.UpdateIncident();

            game.Tutorials = new List<string>()
            {
                TutorialConsts.TUTORIAL_INCIDENT,
                TutorialConsts.TUTORIAL_CREDIBILITY,
                TutorialConsts.TUTORIAL_EXHAUSTION,
                TutorialConsts.TUTORIAL_PERIL,
                TutorialConsts.TUTORIAL_ALLEGIANCE,
                TutorialConsts.TUTORIAL_POWER,
                TutorialConsts.TUTORIAL_LIVRE
            };
            AmbitionApp.UFlow.Register<IncidentTutorialController>(TutorialConsts.TUTORIAL_INCIDENT);
            AmbitionApp.UFlow.Invoke(FlowConsts.GAME_CONTROLLER);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
        }
    }
}
