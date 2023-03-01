using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace MainDabRedo.Execution.CustomInjection
{
    class DLLInjection
    {
        public enum DllInjectionResult
        {
            // Token: 0x040001C1 RID: 449
            DllNotFound,

            // Token: 0x040001C2 RID: 450
            GameProcessNotFound,

            // Token: 0x040001C3 RID: 451
            InjectionFailed,

            // Token: 0x040001C4 RID: 452
            Success
        }

        // Token: 0x02000024 RID: 36
        public sealed class DllInjector
        {
            // Token: 0x1700001F RID: 31
            // (get) Token: 0x060001D8 RID: 472 RVA: 0x0001BDC8 File Offset: 0x00019FC8
            public static DllInjector GetInstance
            {
                get
                {
                    bool flag = DllInjector._instance == null;
                    if (flag)
                    {
                        DllInjector._instance = new DllInjector();
                    }
                    return DllInjector._instance;
                }
            }

            // Token: 0x060001D9 RID: 473 RVA: 0x0001B260 File Offset: 0x00019460
            private DllInjector()
            {
            }

            // Token: 0x060001DA RID: 474 RVA: 0x0001BDF8 File Offset: 0x00019FF8
            private bool bInject(uint pToBeInjected, string sDllPath)
            {
                IntPtr intPtr = DllInjector.OpenProcess(1082U, 1, pToBeInjected);
                bool flag = intPtr == DllInjector.INTPTR_ZERO;
                bool result;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    IntPtr procAddress = DllInjector.GetProcAddress(DllInjector.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    bool flag2 = procAddress == DllInjector.INTPTR_ZERO;
                    if (flag2)
                    {
                        result = false;
                    }
                    else
                    {
                        IntPtr intPtr2 = DllInjector.VirtualAllocEx(intPtr, (IntPtr)0, (IntPtr)sDllPath.Length, 12288U, 64U);
                        bool flag3 = intPtr2 == DllInjector.INTPTR_ZERO;
                        if (flag3)
                        {
                            result = false;
                        }
                        else
                        {
                            byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);
                            bool flag4 = DllInjector.WriteProcessMemory(intPtr, intPtr2, bytes, (uint)bytes.Length, 0) == 0;
                            if (flag4)
                            {
                                result = false;
                            }
                            else
                            {
                                bool flag5 = DllInjector.CreateRemoteThread(intPtr, (IntPtr)0, DllInjector.INTPTR_ZERO, procAddress, intPtr2, 0U, (IntPtr)0) == DllInjector.INTPTR_ZERO;
                                if (flag5)
                                {
                                    result = false;
                                }
                                else
                                {
                                    DllInjector.CloseHandle(intPtr);
                                    result = true;
                                }
                            }
                        }
                    }
                }
                return result;
            }

            // Token: 0x060001DB RID: 475
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern int CloseHandle(IntPtr hObject);

            // Token: 0x060001DC RID: 476
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

            // Token: 0x060001DD RID: 477
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);

            // Token: 0x060001DE RID: 478
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

            // Token: 0x060001DF RID: 479 RVA: 0x0001BEF8 File Offset: 0x0001A0F8
            public DllInjectionResult Inject(string sProcName, string sDllPath)
            {
                bool flag = !File.Exists(sDllPath);
                DllInjectionResult result;
                if (flag)
                {
                    result = DllInjectionResult.DllNotFound;
                }
                else
                {
                    uint num = 0U;
                    Process[] processes = Process.GetProcesses();
                    for (int i = 0; i < processes.Length; i++)
                    {
                        bool flag2 = !(processes[i].ProcessName != sProcName);
                        if (flag2)
                        {
                            num = (uint)processes[i].Id;
                            break;
                        }
                    }
                    bool flag3 = num == 0U;
                    if (flag3)
                    {
                        result = DllInjectionResult.GameProcessNotFound;
                    }
                    else
                    {
                        bool flag4 = !this.bInject(num, sDllPath);
                        if (flag4)
                        {
                            result = DllInjectionResult.InjectionFailed;
                        }
                        else
                        {
                            result = DllInjectionResult.Success;
                        }
                    }
                }
                return result;
            }

            // Token: 0x060001E0 RID: 480
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

            // Token: 0x060001E1 RID: 481
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

            // Token: 0x060001E2 RID: 482
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

            // Token: 0x040001C5 RID: 453
            private static readonly IntPtr INTPTR_ZERO = (IntPtr)0;

            // Token: 0x040001C6 RID: 454
            private static DllInjector _instance;
        }
    }
}
