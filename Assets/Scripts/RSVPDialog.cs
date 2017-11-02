using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Ambition;
using Dialog;
using Core;


public class RSVPDialog : DialogView, Util.IInitializable<PartyVO>
{
	public Text TitleTxt;
	public Text BodyTxt;

	private PartyVO _party;
	private LocalizationModel _localization;


	public void Initialize(PartyVO party)
	{
		ServantModel smod = AmbitionApp.GetModel<ServantModel>();
		Dictionary <string, string> dialogsubs = new Dictionary<string, string>(){
			{"$PARTYSIZE", AmbitionApp.GetString("party_importance." + party.Importance.ToString())}};

		_party = party;
		_localization = AmbitionApp.GetModel<LocalizationModel>();
		TitleTxt.text = _localization.GetString(DialogConsts.RSVP_DIALOG + DialogConsts.TITLE);

		if (smod.Hired.ContainsKey(ServantConsts.SPYMASTER))
		{
			if (_party.Enemies != null && _party.Enemies.Length > 0)
			{
				string enemyList = "";
				Dictionary<string, string> subs = new Dictionary<string, string>();
				foreach(EnemyVO enemy in _party.Enemies)
				{
					enemyList += "\n" + enemy.Name;
				}
				subs.Add("$ENEMYLIST", enemyList);
				dialogsubs.Add("$PROMPT", _localization.GetString("party_enemies", subs));
			}
			else
			{
				dialogsubs.Add("$PROMPT", _localization.GetString("party_no_enemies"));
			}
		}
		else
		{
			dialogsubs.Add("$PROMPT", _localization.GetString("party_prompt"));
		}
		BodyTxt.text = _localization.GetString(DialogConsts.RSVP_DIALOG + DialogConsts.BODY, dialogsubs);
	}

    public void RSVPAction(int decision)
    {
        //0 means no RSVP yet, 1 means Attending and -1 means Decline
        _party.RSVP = decision;
        AmbitionApp.SendMessage<PartyVO>(PartyMessages.RSVP, _party);
    }
}