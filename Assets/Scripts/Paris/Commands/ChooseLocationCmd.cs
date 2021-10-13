using System;
namespace Ambition
{
    public class ChooseLocationCmd : Core.ICommand<string>
    {
        public void Execute(string location)
        {
            if (!AmbitionApp.GetModel<CharacterModel>().CreateRendezvousMode)
            {
                AmbitionApp.Paris.Location = location;
            }
        }
    }
}
