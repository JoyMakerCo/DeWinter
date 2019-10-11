namespace Ambition
{
	public static class FactionUtilities
	{
		// This should get called after any changes to faction 
		// stats; it sends an update message to the app.
		// It also updates the faction level according to the 
		// new reputation.
		public static void UpdateFactionStats( FactionVO faction )
		{
			FactionModel _model = AmbitionApp.GetModel<FactionModel>();
            FactionLevelVO[] _levels = _model.Levels;
			faction.Level = -1;
			var threshold = -9999;
			//Debug.LogFormat( "Faction rep is now {0}", faction.Reputation );
			for (int i = 0; i < _levels.Length; i++)
			{
				//Debug.LogFormat( "entry {0} requirement {1} threshold {2}", i, Levels[i].Requirement, threshold);
				if ((faction.Reputation >= _levels[i].Requirement) && (_levels[i].Requirement > threshold))
				{	
					//Debug.LogFormat( "Bumping to level {0}", i );
					threshold = _levels[i].Requirement;
					faction.Level = i;
				}
			}

			var levelData = _model.GetFactionLevel( faction.Level );
			faction.LargestAllowableParty = levelData.LargestAllowableParty;
			faction.DeckBonus = levelData.DeckBonus;
			faction.Priority = levelData.Importance;
			AmbitionApp.SendMessage<FactionVO>(faction);
		}
	}
}