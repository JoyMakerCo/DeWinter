using System;
using System.Collections.Generic;
using UnityEngine;
namespace Ambition
{
    public class RestorePlayerCmd : Core.ICommand<PlayerConfig>
    {
        public void Execute(PlayerConfig config)
        {
            if (config == null) return;

            GameModel gameModel = AmbitionApp.GetModel<GameModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>() ?? AmbitionApp.RegisterModel<IncidentModel>();
            ServantModel servantModel = AmbitionApp.GetModel<ServantModel>();
            IncidentVO incident;

            // Initialize Selected Player
            gameModel.playerID = config.name;
            gameModel.Chapters = new ChapterVO[config.Chapters.Length];
            gameModel.StartInvitationsReqirements = config.InvitationRequirements;
            for (int i = config.Chapters.Length - 1; i >= 0; i--)
            {
                gameModel.Chapters[i] = new ChapterVO()
                {
                    ID = config.name + ".chapter." + i,
                    Date = config.Chapters[i].Date.GetDateTime(),
                    Splash = config.Chapters[i].Splash,
                    Sting = config.Chapters[i].Sting,
                    EstateMusic = config.Chapters[i].EstateMusic,
                    TrivialPartyChance = config.Chapters[i].TrivialPartyChance,
                    DecentPartyChance = config.Chapters[i].DecentPartyChance,
                    GrandPartyChance = config.Chapters[i].GrandPartyChance
                };
            }
            Array.Sort(gameModel.Chapters, (x, y) => x.Date < y.Date ? -1 : x.Date > y.Date ? 1 : 0);
            calendar.StartDate = config.Chapters[0].Date.GetDateTime();

            foreach (IncidentConfig iconfig in config.Incidents)
            {
                incident = iconfig.GetIncident();
                if (incident != null) incidentModel.Incidents[iconfig.name] = incident;
            }

            // TODO: Figure out the difference between new and loaded games
            // Then update servants, characters, and inventory to account for any changes in status
            // Right now, this only works because Camille is a known entity and Yvette is the only player
            foreach(ServantConfig sconfig in config.Servants)
            {
                servantModel.Servants.Add(sconfig.GetServant());
            }

            Array.ForEach(config.Inventory, i=>inventory.Import(i));

            AmbitionApp.SendMessage(FactionMessages.UPDATE_STANDINGS);
            AmbitionApp.GetModel<LocalizationModel>().SetPlayerName(gameModel.PlayerName);
        }
    }
}
