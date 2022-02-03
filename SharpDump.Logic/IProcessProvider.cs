namespace SharpDump.Logic
{
    public interface IProcessProvider
    {
        ProcessDto GetProcess(int processId);
    }
}
