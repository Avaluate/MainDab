using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MainDabRedo.Execution.KRNL
{
    class KRNLInjection
    {
		public enum krnlInjectionResult
		{
			DllNotFound,
			ProcessNotFound,
			Failed,
			Success,
			threaderr
		}

		public sealed class DllInjector
		{
			private static DllInjector _instance;

			public static DllInjector GetInstance
			{
				get
				{
					if (_instance == null)
					{
						_instance = new DllInjector();
					}
					return _instance;
				}
			}

			[DllImport("kernel32.dll")]
			private static extern int ResumeThread(IntPtr hThread);

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern int CloseHandle(IntPtr hObject);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern bool WaitNamedPipe(string name, int timeout);

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern IntPtr GetModuleHandle(string lpModuleName);

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

			private DllInjector()
			{
			}

			private bool dllinject(uint pToBeInjected, string sDllPath)
			{
				IntPtr intPtr = OpenProcess(1082u, 1, pToBeInjected);
				if (intPtr != INTPTR_ZERO)
				{
					IntPtr procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
					if (procAddress != INTPTR_ZERO)
					{
						IntPtr intPtr2 = VirtualAllocEx(intPtr, (IntPtr)0, (IntPtr)sDllPath.Length, 12288u, 64u);
						IntPtr intPtr3 = CreateRemoteThread(intPtr, (IntPtr)0, INTPTR_ZERO, procAddress, intPtr2, 0u, (IntPtr)0);
						if (intPtr2 != INTPTR_ZERO)
						{
							if (WriteProcessMemory(intPtr, intPtr2, Encoding.ASCII.GetBytes(sDllPath), (uint)Encoding.ASCII.GetBytes(sDllPath).Length, 0) == 0)
							{
								return false;
							}
							if (intPtr3 != INTPTR_ZERO)
							{
								CloseHandle(intPtr);
								return true;
							}
							return false;
						}
						return false;
					}
					return false;
				}
				return false;
			}

			public krnlInjectionResult Inject(string sDllPath, int PID, bool threadject = false)
			{
				if (File.Exists(sDllPath))
				{
					if (!threadject)
					{
						Process[] processes = Process.GetProcesses();
						int i = 0;
						uint num = 0u;
						for (; i < processes.Length; i++)
						{
							if (processes[i].Id == PID)
							{
								num = (uint)PID;
								break;
							}
						}
						if (num != 0)
						{
							if (!dllinject(num, sDllPath))
							{
								return krnlInjectionResult.Failed;
							}
							return krnlInjectionResult.Success;
						}
						return krnlInjectionResult.ProcessNotFound;
					}
					return krnlInjectionResult.threaderr;
				}
				return krnlInjectionResult.DllNotFound;
			}
		}

		public static IntPtr INTPTR_ZERO = IntPtr.Zero;
	}
}
