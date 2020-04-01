using System;
using Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Ambition
{
	public class QuestModel : DocumentModel, IConsoleEntity
	{
		public const int QuestIdle = 0;
		public const int QuestActive = 1;
		public const int QuestFailed = -1;

		public QuestModel () : base("QuestData")
		{
			AmbitionApp.Subscribe<DateTime>(HandleCalendarDay);
			UpdateQuest();
		}

		/* 
		public List<PierreQuest> Quests = new List<PierreQuest>();

		[JsonProperty("quests")]
		private PierreQuest [] _quests;
		*/
		[JsonProperty("current_quest")]
		public PierreQuest CurrentQuest;


		[JsonProperty("active_quest")]
		public int ActiveQuestState = QuestModel.QuestIdle;

		[JsonProperty("failed_quest")]
		public bool FailedQuest = true;

		void UpdateQuest()
		{
			CurrentQuest = new PierreQuest();
			AmbitionApp.GetModel<LocalizationModel>().SetCurrentQuest(CurrentQuest);
		}

		private void HandleCalendarDay(DateTime date)
		{
			Debug.Log("QuestModel.HandleCalendarDay");

			// Don't update quest daily while a quest is active
			if (ActiveQuestState == QuestIdle)
			{
				Debug.Log("no quest active, updating potential quest");

				UpdateQuest();
			}
			else if (ActiveQuestState == QuestActive)
			{
				Debug.Log("quest active");

				CurrentQuest.daysLeft--;
				if (CurrentQuest.daysLeft < 0)
				{
					Debug.Log("quest timed out");
					FailQuest();
				}
			}
			// if failed and not cleared, no change
		}

		public void FailQuest()
		{
			CurrentQuest = null;
			ActiveQuestState = QuestFailed;
			UpdateQuest();
		}

		public void CompleteQuest()
		{
			AmbitionApp.Reward(CurrentQuest.Reward);
			CurrentQuest = null;
			ActiveQuestState = QuestIdle;
			UpdateQuest();
		}

        public string[] Dump()
        {
			string prefix = "? ";
			switch (ActiveQuestState)
			{
				case 0:		prefix = "";	break;
				case 1:		prefix = "* ";	break;
				case -1:	prefix = "! ";	break;
				default: 	prefix = "? ";	break;
			}
            return new string[] 
			{
                "Current Quest: " + prefix + CurrentQuest.ToString() 
            };
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("QuestModel has no invocation.");
        }
	}
}