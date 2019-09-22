using System;

namespace Ambition
{
    public class InitPlayerCmd : Core.ICommand<PlayerConfig>
    {
        public void Execute(PlayerConfig config)
        {
            if (config == null) return;

            GameModel gameModel = AmbitionApp.GetModel<GameModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();

            // Initialize Selected Player
            gameModel.PlayerPhrase = config.name;
            gameModel.Livre.Value = config.Livre;
            IncidentVO incident;
            foreach (IncidentConfig iconfig in config.Incidents)
            {
                incident = iconfig.GetIncident();
                if (incident.IsScheduled) calendar.Schedule(incident);
                else calendar.Unschedule(incident);
            }

            ChapterConfig chapter;
            for (int i=config.Chapters.Length-1; i>=0; i--)
            {
                chapter = config.Chapters[i];
                ChapterVO vo = new ChapterVO()
                {
                    Name = config.name + ".chapter." + (i+1).ToString(),
                    Date = chapter.Date.GetDateTime(),
                    IsComplete = false,
                    Splash = chapter.Splash,
                    Sting = chapter.Sting
                };
                calendar.Schedule(vo);
            }
            calendar.StartDate = config.Chapters[0].Date.GetDateTime();

            ItemVO item;
            foreach (InventoryItem def in config.Inventory)
            {
                item = def.GetItem();
                inventory.Add(item);
                if (def.Equipped) inventory.Equip(item);
            }
        }
    }
}
