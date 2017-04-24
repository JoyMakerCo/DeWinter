using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DeWinter;
using Dialog;
using Core;


public class RSVPDialogMediator : DialogView, IDialog<Party>
{
	public const string DIALOG_ID = "RSVPPopUpModal";
	
	public Text TitleTxt;
	public Text BodyTxt;

	private Party _party;
	private LocalizationModel _localization;


	public void OnOpen(Party party)
	{
		ServantModel smod = DeWinterApp.GetModel<ServantModel>();
		Dictionary <string, string> dialogsubs = new Dictionary<string, string>(){
			{"$PARTYSIZE", party.SizeString()}};

		_party = party;
		_localization = DeWinterApp.GetModel<LocalizationModel>();
		TitleTxt.text = _localization.GetString(DialogConsts.RSVP_DIALOG + DialogConsts.TITLE);

		if (smod.Servants.ContainsKey(ServantConsts.SPYMASTER))
		{
			if (_party.enemyList != null && _party.enemyList.Count > 0)
			{
				string enemyList = "";
				Dictionary<string, string> subs = new Dictionary<string, string>();
				foreach(Enemy enemy in _party.enemyList)
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
        DeWinterApp.SendMessage<Party>(CalendarMessages.RSVP, _party);
    }
}