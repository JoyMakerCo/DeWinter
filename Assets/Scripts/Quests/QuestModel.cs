using System;
using Core;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ambition
{
	public class QuestModel : DocumentModel
	{
		public QuestModel () : base("QuestData")
		{
			AmbitionApp.Subscribe<DateTime>(HandleCalendarDay);
		}

		public List<PierreQuest> Quests = new List<PierreQuest>();

		[JsonProperty("quests")]
		private PierreQuest [] _quests;

		public DateTime NextQuestDay;

		private void HandleCalendarDay(DateTime date)
		{
			if (date >= NextQuestDay)
			{
//TODO: Send a command that updates quests
				NextQuestDay = date.AddDays(UnityEngine.Random.Range(3, 6));
			}
		}
	}
}