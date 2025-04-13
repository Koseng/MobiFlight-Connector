using System.Diagnostics;

namespace MobiFlight.Scripts
{
    internal interface IChildProcessMonitor
    {
        void AddChildProcess(IProcess process);
    }
}
