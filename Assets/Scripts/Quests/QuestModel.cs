using System;
using Core;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeWinter
{
	public class QuestModel : DocumentModel
	{
		public QuestModel () : base("QuestData") {}

		public List<PierreQuest> Quests = new List<PierreQuest>();

		[JsonProperty("quests")]
		private PierreQuest [] _quests;
	}
}