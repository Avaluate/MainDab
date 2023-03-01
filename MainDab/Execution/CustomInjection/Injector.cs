using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MainDabRedo.Execution.CustomInjection
{
    class Injector
    {
        public enum DllInjectionResult
        {
            // Token: 0x040001AC RID: 428
            DllNotFound,

            // Token: 0x040001AD RID: 429
            GameProcessNotFound,

            // Token: 0x040001AE RID: 430
            InjectionFailed,

            // Token: 0x040001AF RID: 431
            Success
        }

        // Token: 0x0200001F RID: 31
        public sealed class DllInjector
        {
            // Token: 0x060001C4 RID: 452
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

            // Token: 0x060001C5 RID: 453
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern int CloseHandle(IntPtr hObject);

            // Token: 0x060001C6 RID: 454
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

            // Token: 0x060001C7 RID: 455
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetModuleHandle(string lpModuleName);

            // Token: 0x060001C8 RID: 456
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

            // Token: 0x060001C9 RID: 457
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

            // Token: 0x060001CA RID: 458
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

            // Token: 0x1700001E RID: 30
            // (get) Token: 0x060001CB RID: 459 RVA: 0x0001B848 File Offset: 0x00019A48
            public static Injector.DllInjector GetInstance
            {
                get
                {
                    bool flag = Injector.DllInjector._instance == null;
                    if (flag)
                    {
                        Injector.DllInjector._instance = new Injector.DllInjector();
                    }
                    return Injector.DllInjector._instance;
                }
            }

            // Token: 0x060001CC RID: 460 RVA: 0x0001B260 File Offset: 0x00019460
            private DllInjector()
            {
            }

            // Token: 0x060001CD RID: 461 RVA: 0x0001B878 File Offset: 0x00019A78
            public Injector.DllInjectionResult Inject(string sProcName, string sDllPath)
            {
                bool flag = !File.Exists(sDllPath);
                Injector.DllInjectionResult result;
                if (flag)
                {
                    result = Injector.DllInjectionResult.DllNotFound;
                }
                else
                {
                    uint num = 0U;
                    Process[] processes = Process.GetProcesses();
                    for (int i = 0; i < processes.Length; i++)
                    {
                        bool flag2 = processes[i].ProcessName == sProcName;
                        if (flag2)
                        {
                            num = (uint)processes[i].Id;
                            break;
                        }
                    }
                    bool flag3 = num == 0U;
                    if (flag3)
                    {
                        result = Injector.DllInjectionResult.GameProcessNotFound;
                    }
                    else
                    {
                        bool flag4 = !this.bInject(num, sDllPath);
                        if (flag4)
                        {
                            result = Injector.DllInjectionResult.InjectionFailed;
                        }
                        else
                        {
                            result = Injector.DllInjectionResult.Success;
                        }
                    }
                }
                return result;
            }

            // Token: 0x060001CE RID: 462 RVA: 0x0001B908 File Offset: 0x00019B08
            private bool bInject(uint pToBeInjected, string sDllPath)
            {
                IntPtr intPtr = Injector.DllInjector.OpenProcess(1082U, 1, pToBeInjected);
                bool flag = intPtr == Injector.DllInjector.INTPTR_ZERO;
                bool result;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    IntPtr procAddress = Injector.DllInjector.GetProcAddress(Injector.DllInjector.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                    bool flag2 = procAddress == Injector.DllInjector.INTPTR_ZERO;
                    if (flag2)
                    {
                        result = false;
                    }
                    else
                    {
                        IntPtr intPtr2 = Injector.DllInjector.VirtualAllocEx(intPtr, (IntPtr)null, (IntPtr)sDllPath.Length, 12288U, 64U);
                        bool flag3 = intPtr2 == Injector.DllInjector.INTPTR_ZERO;
                        if (flag3)
                        {
                            result = false;
                        }
                        else
                        {
                            byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);
                            bool flag4 = Injector.DllInjector.WriteProcessMemory(intPtr, intPtr2, bytes, (uint)bytes.Length, 0) == 0;
                            if (flag4)
                            {
                                result = false;
                            }
                            else
                            {
                                bool flag5 = Injector.DllInjector.CreateRemoteThread(intPtr, (IntPtr)null, Injector.DllInjector.INTPTR_ZERO, procAddress, intPtr2, 0U, (IntPtr)null) == Injector.DllInjector.INTPTR_ZERO;
                                if (flag5)
                                {
                                    result = false;
                                }
                                else
                                {
                                    Injector.DllInjector.CloseHandle(intPtr);
                                    result = true;
                                }
                            }
                        }
                    }
                }
                return result;
            }

            // Token: 0x040001B0 RID: 432
            private static readonly IntPtr INTPTR_ZERO = (IntPtr)0;

            // Token: 0x040001B1 RID: 433
            private static Injector.DllInjector _instance;
        }
    }
}
