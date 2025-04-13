using System.Diagnostics;

namespace MobiFlight.Scripts
{
    internal interface IProcessFactory
    {
        IProcess CreateProcess();

        IProcess CreateProcess(string fileName);

        IProcess CreateProcess(ProcessStartInfo startInfo);
    }
}
