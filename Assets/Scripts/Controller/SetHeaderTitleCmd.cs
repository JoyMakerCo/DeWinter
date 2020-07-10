using System;
using Core;
namespace Ambition
{
    public class SetHeaderTitleCmd : ICommand<string>
    {
        public void Execute(string title)
        {
            AmbitionApp.GetModel<LocalizationModel>().HeaderTitle.Value = title;
        }
    }

    public class ShowHeaderCmd : ICommand<string>
    {
        public void Execute(string title)
        {
            AmbitionApp.SendMessage(GameMessages.SHOW_HEADER);
            AmbitionApp.GetModel<LocalizationModel>().HeaderTitle.Value = title;
        }
    }

}
