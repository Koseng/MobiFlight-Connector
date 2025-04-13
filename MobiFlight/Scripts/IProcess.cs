using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace MobiFlight.Scripts
{
    public interface IProcess : IDisposable
    {
        ProcessStartInfo StartInfo { get; set; }

        StreamReader StandardOutput { get; }

        int ExitCode { get;}

        bool HasExited { get; }

        IntPtr Handle { get; }

        bool Start();

        void Kill();

        Process GetProcess();

        void BeginOutputReadLine();

        void BeginErrorReadLine();

        event DataReceivedEventHandler OutputDataReceived;

        event DataReceivedEventHandler ErrorDataReceived;
    }
}
