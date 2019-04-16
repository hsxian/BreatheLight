using System;
using System.Diagnostics;
using System.IO;
using UpPwm.Interfaces;

namespace UpPwm.Implements
{
    public class SystemCommander : ISystemCommander
    {
        private readonly Process _commander;
        private readonly StreamWriter _streamWriter;
        public event DataReceivedEventHandler ErrorDataReceived;
        public event DataReceivedEventHandler OutputDataReceived;
        public SystemCommander()
        {
            var psi = new ProcessStartInfo("bash")
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            _commander = Process.Start(psi);
            if (_commander != null)
            {
                _streamWriter = _commander.StandardInput;
                _commander.OutputDataReceived += (sen, e) =>
                {
                    OutputDataReceived?.Invoke(sen, e);
                    //Console.WriteLine(e.Data);
                };
                _commander.ErrorDataReceived += (sen, e) =>
               {
                   ErrorDataReceived?.Invoke(sen, e);
                   Console.WriteLine("ERROR: " + e.Data);
                   _commander.CancelOutputRead();
                   _commander.CancelErrorRead();
                   //_commander.Kill();
               };
                _commander.BeginOutputReadLine();
                _commander.BeginErrorReadLine();
            }
        }
        public void ExecCommand(string cmd)
        {
            if (_streamWriter == null)
            {
                Console.WriteLine("error in ExecCommand");
                return;
            }
            _streamWriter.WriteLine(cmd);
        }

        public bool WaitForExit(int milliseconds)
        {
            return _commander.WaitForExit(milliseconds);
        }

        public void WaitForExit()
        {
            _commander.WaitForExit();
        }
    }
}