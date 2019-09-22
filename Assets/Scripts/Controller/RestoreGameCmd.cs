using System;
namespace Application
{
    public class RestoreGameCmd : Core.ICommand<string>
    {
        public void Execute(string saved)
        {
            // TODO: Restore game from packed data
        }
    }
}
