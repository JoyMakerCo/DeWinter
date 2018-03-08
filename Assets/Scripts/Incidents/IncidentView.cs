using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Dialog;

namespace Ambition
{
	public class IncidentView : DialogView
	{
		public const string DIALOG_ID = "INCIDENT";
	    public Text titleText;
	    public Text descriptionText;

	    public AvatarView Character1;
		public AvatarView Character2;

		private Sprite _background;

	    public override void OnOpen ()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			AmbitionApp.Subscribe<MomentVO>(HandleMoment);
			model.Moment = model.Incident.Moments[0];
			titleText.text = model.Incident.Name;
			_background = gameObject.GetComponent<Image>().sprite;
		}

		public override void OnClose ()
		{
			AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
	    }

 		private void HandleMoment(MomentVO moment)
		{
			if (moment != null)
			{
				descriptionText.text = moment.Text;
				if (moment.Background != null) _background=moment.Background;
			}
		}
	}
}
