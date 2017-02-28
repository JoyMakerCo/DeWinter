﻿using System;
using Core;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeWinter
{
	public class QuestModel : DocumentModel
	{
		public QuestModel () : base("QuestData")
		{
			DeWinterApp.Subscribe<CalendarDayVO>(HandleCalendarDay);
		}

		public List<PierreQuest> Quests = new List<PierreQuest>();

		[JsonProperty("quests")]
		private PierreQuest [] _quests;

		public int NextQuestDay=0;

		private void HandleCalendarDay(CalendarDayVO msg)
		{
			if (msg.Day >= NextQuestDay)
			{
//TODO: Send a command that updates quests
				NextQuestDay = msg.Day + (new Random()).Next(3, 6);
			}
		}
	}
}