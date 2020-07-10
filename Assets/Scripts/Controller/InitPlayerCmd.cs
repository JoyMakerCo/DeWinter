using System;
using System.Collections.Generic;
using UnityEngine;
namespace Ambition
{
    public class InitPlayerCmd : Core.ICommand<string>
    {
        public void Execute(string playerID)
        {
            PlayerConfig config = Resources.Load<PlayerConfig>(Filepath.PLAYERS + playerID);
            if (config == null) return;

            GameModel gameModel = AmbitionApp.GetModel<GameModel>();
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
            ItemVO item;
            IncidentVO incident;

            // Initialize Selected Player
            gameModel.playerID = config.name;
            gameModel.Livre.Value = config.Livre;
            gameModel.Chapters = new ChapterVO[config.Chapters.Length];
            for (int i = config.Chapters.Length - 1; i >= 0; i--)
            {
                gameModel.Chapters[i] = new ChapterVO(
                    config.name + ".chapter." + i,
                    config.Chapters[i].Date.GetDateTime(),
                    config.Chapters[i].Splash,
                    config.Chapters[i].Sting
                );
            }
            gameModel.StartDate = config.Chapters[0].Date.GetDateTime();

            foreach (IncidentConfig iconfig in config.Incidents)
            {
                incident = iconfig?.GetIncident();
                incidentModel.Schedule(incident);
            }

            foreach (ItemConfig def in config.Inventory)
            {
                item = inventory.Import(def);
                inventory.Inventory.Add(item);
            }

            AmbitionApp.GetModel<LocalizationModel>().SetPlayerName(gameModel.PlayerName);
        }
    }
}
