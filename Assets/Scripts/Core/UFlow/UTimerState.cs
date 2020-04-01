using System;
using System.Threading;

namespace UFlow
{
    // Stubbed out class for using threading instead of coroutines for timing
    public class UTimerState : UInputState
    {
        public /*override*/ void Initialize(params object[] args)
        {
            if (args.Length == 0) Activate();
            else
            {
                float count = float.Parse(args[0] as string ?? "0");
                if (count <= 0f) Activate();
                else
                {
                    Thread thread = new Thread(() =>
                        {
                            Thread.Sleep((int)(count * 1000f));
                            Activate();
                        });
                    thread.Start();
                }
            }
        }
    }
}
