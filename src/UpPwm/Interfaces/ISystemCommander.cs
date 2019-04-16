using System.Diagnostics;

namespace UpPwm.Interfaces
{
    public interface ISystemCommander
    {
        event DataReceivedEventHandler ErrorDataReceived;
        event DataReceivedEventHandler OutputDataReceived;
        void ExecCommand(string cmd);
        bool WaitForExit(int milliseconds);
        void WaitForExit();
    }
}