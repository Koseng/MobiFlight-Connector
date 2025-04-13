using System.Diagnostics;

namespace MobiFlight.Scripts
{
    internal class ProcessFactory : IProcessFactory
    {
        public IProcess CreateProcess()
        {
            return new MobiProcess();
        }

        public IProcess CreateProcess(ProcessStartInfo startInfo)
        {
            return new MobiProcess(startInfo);
        }

        public IProcess CreateProcess(string fileName)
        {
            return new MobiProcess(new ProcessStartInfo(fileName));        
        }
    }
}
