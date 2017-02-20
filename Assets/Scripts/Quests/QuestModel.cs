using System;
using Core;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeWinter
{
	public class QuestModel : DocumentModel
	{
		public QuestModel () : base("QuestData")
		{
			DeWinterApp.Subscribe<CalendarDayVO>(CalendarConsts.DAY_START, HandleCalendarDay);
		}

		public List<PierreQuest> Quests = new List<PierreQuest>();

		[JsonProperty("quests")]
		private PierreQuest [] _quests;

		private int _nextQuestDay=0;

		private void HandleCalendarDay(CalendarDayVO msg)
		{
			if (msg.Day >= _nextQuestDay)
			{
//TODO: Send a command that updates quests
				_nextQuestDay = msg.Day + (new Random()).Next(3, 6);
			}
		}
	}
}