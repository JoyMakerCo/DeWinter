using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace Ambition
{
    [Core.Saveable]
    public class LocalizationModel : Core.Model
    {
        private const string LOC_DIR = "Localization/";

        public Dictionary<string, string> Substitutions = new Dictionary<string, string>();

        public Observable<string> HeaderTitle = new Observable<string>();

        public void SetPlayerName(string name)
        {
            Substitutions[LocalizationConsts.PLAYER_NAME] = name;
        }

        public void SetMoment(MomentVO moment)
        {
            Substitutions[LocalizationConsts.CHARACTER_1] = moment?.Character1.Name;
            Substitutions[LocalizationConsts.CHARACTER_2] = moment?.Character2.Name;
            switch(moment?.Speaker)
            {
                case SpeakerType.Character1:
                    Substitutions[LocalizationConsts.SPEAKER] = moment.Character1.Name;
                    break;
                case SpeakerType.Character2:
                    Substitutions[LocalizationConsts.SPEAKER] = moment.Character1.Name;
                    break;
                case SpeakerType.Player:
                    Substitutions[LocalizationConsts.SPEAKER] = Substitutions[LocalizationConsts.PLAYER_NAME];
                    break;
                default:
                    Substitutions[LocalizationConsts.SPEAKER] = "";
                    break;
            }
        }

        public void SetCurrentQuest( PierreQuest quest )
        {
            Substitutions[LocalizationConsts.QUESTGOSSIPFACTION] = AmbitionApp.Localize(quest.Faction.ToString().ToLower());
            Substitutions[LocalizationConsts.QUESTREWARD] = AmbitionApp.Localize(quest.RewardKey);
            Substitutions[LocalizationConsts.QUESTTIME] = quest.daysTimeLimit.ToString();
        }

        public void SetPartyFaction( FactionType faction )
        {
            FactionVO fvo = AmbitionApp.GetModel<FactionModel>()[faction];

            Substitutions[LocalizationConsts.PARTYFACTION] = AmbitionApp.Localize( faction.ToString().ToLower() );

            // map -100 to +100 to 0,1,2.
            int modIndex = 0;
            if (fvo.Modesty < -33)
            {
                modIndex = 0;
            }
            else if (fvo.Modesty < 33)
            {
                modIndex = 1;
            }
            else
            {
                modIndex = 2;
            }

            int luxIndex = 0;
            if (fvo.Luxury < -33)
            {
                luxIndex = 0;
            }
            else if (fvo.Luxury < 33)
            {
                luxIndex = 1;
            }
            else
            {
                luxIndex = 2;
            }
            Substitutions[LocalizationConsts.PARTYFACTIONMODESTY] = AmbitionApp.Localize( string.Format("modesty.{0}", modIndex) );
            Substitutions[LocalizationConsts.PARTYFACTIONLUXURY] = AmbitionApp.Localize( string.Format("luxury.{0}", luxIndex) );

            Debug.LogFormat( "SetPartyFaction fac {0} name {1} mod {2} lux {3}",
                faction.ToString(),
                Substitutions[LocalizationConsts.PARTYFACTION],
                Substitutions[LocalizationConsts.PARTYFACTIONMODESTY],
                Substitutions[LocalizationConsts.PARTYFACTIONLUXURY]
                );
        }

        public void SetPartyOutfit( ItemVO outfit )
        {
            if (outfit?.Type == ItemType.Outfit)
            {
                Substitutions[LocalizationConsts.OUTFITNAME] = outfit.Name.ToLower();
                Substitutions[LocalizationConsts.OUTFITTITLE] = outfit.Name;
                Substitutions[LocalizationConsts.OUTFITMODESTY] = OutfitWrapperVO.GetModestyText(outfit);
                Substitutions[LocalizationConsts.OUTFITLUXURY] = OutfitWrapperVO.GetLuxuryText(outfit);

                Debug.LogFormat("SetPartyOutfit name {0} mod {1} lux {2}",
                    Substitutions[LocalizationConsts.OUTFITNAME],
                    Substitutions[LocalizationConsts.OUTFITMODESTY],
                    Substitutions[LocalizationConsts.OUTFITLUXURY]
                    );
            }
        }

        public string SetDate(DateTime date)
        {
            Dictionary<string, string> subs = new Dictionary<string, string>();
            Substitutions[LocalizationConsts.MONTH] = AmbitionApp.GetPhrases("month")["month." + (date.Month - 1)];
            Substitutions[LocalizationConsts.DAY] = date.Day.ToString();
            Substitutions[LocalizationConsts.YEAR] = date.Year.ToString();
            Substitutions[LocalizationConsts.WEEKDAY] = AmbitionApp.GetPhrases("weekday")["weekday." + (int)(date.DayOfWeek)];
            Substitutions[LocalizationConsts.SHORT_DATE] = AmbitionApp.GetString("short_date", Substitutions);
            return Substitutions[LocalizationConsts.DATE] = AmbitionApp.GetString("date", Substitutions);
        }

        public string Date => Substitutions[LocalizationConsts.DATE];
        public string ShortDate => Substitutions[LocalizationConsts.SHORT_DATE];

        public string SetLocation(string locationID)
        {
            return Substitutions[LocalizationConsts.LOCATION] = "location." + locationID;
        }
    }
}
