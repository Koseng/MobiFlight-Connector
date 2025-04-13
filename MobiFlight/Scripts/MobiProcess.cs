using System;
using System.Diagnostics;
using System.IO;

namespace MobiFlight.Scripts
{
    internal class MobiProcess : IProcess
    {
        private Process _process = new Process();

        public event DataReceivedEventHandler OutputDataReceived;
        public event DataReceivedEventHandler ErrorDataReceived;

        public ProcessStartInfo StartInfo { get => _process.StartInfo; set => _process.StartInfo = value; }

        public int ExitCode => _process.ExitCode;

        public bool HasExited => _process.HasExited;

        public StreamReader StandardOutput => _process.StandardOutput;

        public IntPtr Handle => _process.Handle;

        public MobiProcess()
        {
            _process.OutputDataReceived += _process_OutputDataReceived;
            _process.ErrorDataReceived += _process_ErrorDataReceived;
        }

        public MobiProcess(ProcessStartInfo startInfo)
        {
            StartInfo = startInfo;
            _process.OutputDataReceived += _process_OutputDataReceived;
            _process.ErrorDataReceived += _process_ErrorDataReceived;
        }


        private void _process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived?.Invoke(this, e);
        }

        private void _process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ErrorDataReceived?.Invoke(this, e);
        }

        public bool Start()
        {
            return _process.Start();
        }

        public Process GetProcess()
        {
            return _process;
        }

        public void BeginOutputReadLine()
        {
            _process.BeginOutputReadLine();
        }

        public void BeginErrorReadLine()
        {
            _process.BeginErrorReadLine();
        }

        public void Dispose()
        {
            _process.OutputDataReceived -= _process_OutputDataReceived;
            _process.ErrorDataReceived -= _process_ErrorDataReceived;
            _process.Dispose();
        }

        public void Kill()
        {
            _process.Kill();
        }
    }
}
