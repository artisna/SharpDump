using SharpDump.Logic;

namespace SharpDump.Infrastructure
{
    public class ProcessProvider : IProcessProvider
    {
        public ProcessDto GetProcess(int processId)
        {
            throw new NotImplementedException();

            //IntPtr targetProcessHandle = IntPtr.Zero;
            //uint targetProcessId = 0;

            //Process targetProcess = null;
            //if (pid == -1)
            //{
            //    Process[] processes = Process.GetProcessesByName("lsass");
            //    targetProcess = processes[0];
            //}
            //else
            //{
            //    try
            //    {
            //        targetProcess = Process.GetProcessById(pid);
            //    }
            //    catch (Exception ex)
            //    {
            //        // often errors if we can't get a handle to LSASS
            //        logger.Log(String.Format("\n[X]Exception: {0}\n", ex.Message));
            //        return;
            //    }
            //}

            //if (targetProcess.ProcessName == "lsass" && !IsHighIntegrity())
            //{
            //    logger.Log("\n[X] Not in high integrity, unable to MiniDump!\n");
            //    return;
            //}

            //try
            //{
            //    targetProcessId = (uint)targetProcess.Id;
            //    targetProcessHandle = targetProcess.Handle;
            //}
            //catch (Exception ex)
            //{
            //    logger.Log(String.Format("\n[X] Error getting handle to {0} ({1}): {2}\n", targetProcess.ProcessName, targetProcess.Id, ex.Message));
            //    return;
            //}

        }
    }
}