using Core;
using System;
using UnityEngine;

namespace Ambition
{
	public class SetCommodityCmd : ICommand<CommodityVO>
	{
		public void Execute(CommodityVO cvo)
		{
			// Rather than send this through something like the RegisterReward 
			// pipeline, let's just deconstruct it here. 

			var _model = AmbitionApp.GetModel<GameModel>();
            var _factions = AmbitionApp.GetModel<FactionModel>();
			var _chars = AmbitionApp.GetModel<CharacterModel>();

			FactionType factionType;

			switch (cvo.Type)
			{
				case CommodityType.Livre:
					_model.Livre.Value = cvo.Value;
					break;

				case CommodityType.Exhaustion:
					_model.Exhaustion.Value = cvo.Value;
					break; 

				case CommodityType.Credibility:
					_model.Credibility.Value = cvo.Value;
					break; 				
					
				case CommodityType.Peril:
					_model.Peril.Value = cvo.Value;
					break; 

				case CommodityType.Reputation:
					if (cvo.ID != null && Enum.TryParse<FactionType>(cvo.ID, ignoreCase:true, out factionType))
					{
						AmbitionApp.SendMessage(FactionMessages.SET_FACTION,AdjustFactionVO.MakeReputationVO(factionType, cvo.Value));
					}
					else
					{
						_model.Reputation = cvo.Value;
					}
					break;

				case CommodityType.FactionPower:
					if (cvo.ID != null && Enum.TryParse<FactionType>(cvo.ID, ignoreCase:true, out factionType))
					{
						AmbitionApp.SendMessage(FactionMessages.SET_FACTION,AdjustFactionVO.MakePowerVO(factionType, cvo.Value));
					}
					else
					{
						Debug.LogWarning("SetCommodityCmd with FactionPower for unidentified faction ");
					}
					break;

				case CommodityType.FactionAllegiance:
					if (cvo.ID != null && Enum.TryParse<FactionType>(cvo.ID, ignoreCase:true, out factionType))
					{
						AmbitionApp.SendMessage(FactionMessages.SET_FACTION,AdjustFactionVO.MakeAllegianceVO(factionType, cvo.Value));
					}
					else
					{
						Debug.LogWarning("SetCommodityCmd with FactionAllegiance for unidentified faction ");
					}
					break;

				case CommodityType.Favor:
					var hits = _chars.GetCharacters( cvo.ID );
					if (hits.Length == 0)
					{
						Debug.LogErrorFormat("Unrecognized character '{0}'", cvo.ID);
					}
					else if (hits.Length > 1)
					{
						Debug.LogErrorFormat("Ambiguous character '{0}'", cvo.ID);
					}
					else
					{
						var character = hits[0];
						character.Favor = cvo.Value;
						if (character.Favor < 0) character.Favor = 0;
						else if (character.Favor > 100) character.Favor = 100;
					}
					break;

				default:
					Debug.LogWarningFormat("Don't know how to set commodity type '{0}'",cvo.Type);	
					break;	
			}
		}
	}
}
