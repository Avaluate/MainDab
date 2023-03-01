using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


namespace MainDabRedo.Execution.CustomInjection
{
    class NamedPipes
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WaitNamedPipe(string name, int timeout);

        // Token: 0x0600015B RID: 347 RVA: 0x000187D4 File Offset: 0x000169D4
        public static bool NamedPipeExist(string pipeName)
        {
            bool result;
            try
            {
                bool flag = !NamedPipes.WaitNamedPipe("\\\\.\\pipe\\" + pipeName, 0);
                if (flag)
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    bool flag2 = lastWin32Error == 0;
                    if (flag2)
                    {
                        return false;
                    }
                    bool flag3 = lastWin32Error == 2;
                    if (flag3)
                    {
                        return false;
                    }
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        // Token: 0x0600015C RID: 348 RVA: 0x0001883C File Offset: 0x00016A3C
        public static void LuaPipe(string script)
        {
            bool flag = NamedPipes.NamedPipeExist(NamedPipes.luapipename);
            if (flag)
            {
                new Thread(delegate ()
                {
                    try
                    {
                        using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", NamedPipes.luapipename, PipeDirection.Out))
                        {
                            namedPipeClientStream.Connect();
                            using (StreamWriter streamWriter = new StreamWriter(namedPipeClientStream, Encoding.Default, 999999))
                            {
                                streamWriter.Write(script);
                                streamWriter.Dispose();
                            }
                            namedPipeClientStream.Dispose();
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Error occured connecting to the pipe.", "Connection Failed!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }).Start();
            }
            else
            {
                MessageBox.Show("Please inject first! Click the inject button first, then you can execute.", "Please start ROBLOX first!");
            }
        }

        // Token: 0x04000174 RID: 372
        public static string luapipename = "MainDab";
    }
}
