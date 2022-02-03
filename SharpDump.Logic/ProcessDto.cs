namespace SharpDump.Logic
{
    public class ProcessDto
    {
        public IntPtr ProcesHandle { get; }
        public uint ProcessId { get; }
        public string ProcessName { get; }
    }
}
