﻿//FROM MSDN

using System;
using System.Runtime.InteropServices;

namespace H3Hacker.Memory
{
    public class ProcessMemoryReaderApi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        // constants information can be found in <winnt.h>
        [Flags]
        public enum ProcessAccessType
        {
            PROCESS_TERMINATE = (0x0001),
            PROCESS_CREATE_THREAD = (0x0002),
            PROCESS_SET_SESSIONID = (0x0004),
            PROCESS_VM_OPERATION = (0x0008),
            PROCESS_VM_READ = (0x0010),
            PROCESS_VM_WRITE = (0x0020),
            PROCESS_DUP_HANDLE = (0x0040),
            PROCESS_CREATE_PROCESS = (0x0080),
            PROCESS_SET_QUOTA = (0x0100),
            PROCESS_SET_INFORMATION = (0x0200),
            PROCESS_QUERY_INFORMATION = (0x0400),
            PROCESS_QUERY_LIMITED_INFORMATION = (0x1000)
        }

        // function declarations are found in the MSDN and in <winbase.h> 

        //  HANDLE OpenProcess(
        //   DWORD dwDesiredAccess,  // access flag
        //   BOOL bInheritHandle,    // handle inheritance option
        //   DWORD dwProcessId       // process identifier
        //   );
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, uint bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        //  BOOL CloseHandle(
        //   HANDLE hObject   // handle to object
        //   );
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);

        //  BOOL ReadProcessMemory(
        //   HANDLE hProcess,              // handle to the process
        //   LPCVOID lpBaseAddress,        // base of memory area
        //   LPVOID lpBuffer,              // data buffer
        //   SIZE_T nSize,                 // number of bytes to read
        //   SIZE_T * lpNumberOfBytesRead  // number of bytes read
        //   );
        [DllImport("kernel32.dll")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesRead);

        //  BOOL WriteProcessMemory(
        //   HANDLE hProcess,                // handle to process
        //   LPVOID lpBaseAddress,           // base of memory area
        //   LPCVOID lpBuffer,               // data buffer
        //   SIZE_T nSize,                   // count of bytes to write
        //   SIZE_T * lpNumberOfBytesWritten // count of bytes written
        //   );
        [DllImport("kernel32.dll")]
        public static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesWritten);
    }
}
