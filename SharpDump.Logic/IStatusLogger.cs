namespace SharpDump.Logic
{
    public interface IStatusLogger
    {
        void Log(string message, params object[] args);
    }
}
