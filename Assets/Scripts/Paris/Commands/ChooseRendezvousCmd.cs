using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class ChooseRendezvousCmd : ICommand<string>
    {
        public void Execute(string location)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            if (model.CreateRendezvousMode)
            {
                CharacterVO character = model.GetCharacter(model.CreateRendezvous.Character);
                character.LiaisonDay = model.CreateRendezvous.Day;
                model.CreateRendezvous.Location = location;
                AmbitionApp.Calendar.Schedule(model.CreateRendezvous, model.CreateRendezvous.Day);
                model.CreateRendezvous = null;
            }
        }
    }
}
