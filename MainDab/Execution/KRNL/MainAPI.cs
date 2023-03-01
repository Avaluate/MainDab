using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MainDabRedo.Execution.KRNL
{
    class MainAPI
    {
		private static int injectedPID;

		public bool update = false;

		private bool Finished = false;

		private static int filesizeweb;

		private static bool loaded;

		public static bool RemoveInstaller;

		private static DispatcherTimer AutAttach;

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public static async void Load()
		{
			Timer();
			try
			{
				if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "krnl.dll"))
				{
					File.Delete(AppDomain.CurrentDomain.BaseDirectory + "krnl.dll");
				}
				await Task.Delay(100);
				if (!RemoveInstaller)
				{
					Reinstall();
				}
			}
			catch
			{
				// No messagebox is needed here
				// MessageBox.Show("[krnl issue] It seems like krnl has changed their web host, and therefore krnl's files cannont be downloaded. Please wait for it to come back.", "MainDab");
			}
		}

		private static void Reinstall()
		{
			WebClient webClient = new WebClient();
			string text = webClient.DownloadString("https://raw.githubusercontent.com/JalapenoGuy/KrnlAPI/main/PowerShell");
			string text2 = text.Replace("FileName.Extension", AppDomain.CurrentDomain.BaseDirectory + "krnl.dll");
			string arguments = "/C " + text2;
			Process process = new Process();
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.FileName = "cmd.exe";
			processStartInfo.Arguments = arguments;
			process.StartInfo = processStartInfo;
			process.Start();
			loaded = true;
		}

		public static bool Loaded()
		{
			if (loaded)
			{
				return true;
			}
			return false;
		}

		public void CrossLoadSet(bool bol)
		{
			loaded = bol;
		}

		public static void Latest()
		{
		}

		private static void Timer()
		{
			AutAttach = new DispatcherTimer();
			AutAttach.Interval = new TimeSpan(0, 0, 0, 3);
			AutAttach.Tick += AutAttach_Tick;
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async void AutAttach_Tick(object sender, EventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
			if (Process.GetProcessesByName("RobloxPlayerLauncher").Length == 0 && Process.GetProcessesByName("RobloxPlayerBeta").Length != 0 && loaded && !(((dynamic)findpipe("krnlpipe")) ? true : false))
			{
				Inject();
			}
		}

		public static void AutoAttach(bool autoattach)
		{
			if (autoattach)
			{
				AutAttach.Start();
			}
			else
			{
				AutAttach.Stop();
			}
		}

		private static bool FileCheck()
		{
			long length = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "krnl.dll").Length;
			if (filesizeweb == 0)
			{
				WebClient webClient = new WebClient();
				webClient.OpenRead("https://k-storage.com/bootstrapper/files/krnl.dll");
				long num = Convert.ToInt64(webClient.ResponseHeaders["Content-Length"]);
				long num2 = num;
				string value = num2.ToString();
				int num3 = (filesizeweb = Convert.ToInt32(value));
			}
			int num4 = Convert.ToInt32(length);
			if (num4 == filesizeweb)
			{
				return true;
			}
			MessageBox.Show("[krnl message] krnl.dll hasn't finished installing, please wait: " + num4 + "\\" + filesizeweb + " bytes", "MainDab");
			return false;
		}

		public static void Inject()
		{
			if (File.Exists( "krnl.dll"))
			{
				if (!FileCheck())
				{
					return;
				}
				new Thread((ParameterizedThreadStart)delegate
				{
					try
					{
						Process[] processesByName = Process.GetProcessesByName("RobloxPlayerBeta");
						IntPtr intPtr = FindWindowA("WINDOWSCLIENT", "Roblox");
						int lpdwProcessId = 0;
						GetWindowThreadProcessId(intPtr, out lpdwProcessId);
						if ((dynamic)(nint)intPtr != IntPtr.Zero)
						{
							List<string> list = processesByName[0].MainModule.FileName.Split('\\').ToList();
							list.Remove("RobloxPlayerBeta.exe");
							string directory = string.Join("\\", list.ToArray());
							Program.writeToDir(directory);
							try
							{
								bool flag = injectdll("krnl.dll", lpdwProcessId);
								return;
							}
							catch
							{
								return;
							}
						}
						MessageBox.Show("Please open Roblox first before injecting!", "MainDab");
					}
					catch(Exception ex)
					{
						MessageBox.Show("[krnl error] krnl has caught an unknown error while injecting! Here is the error:\n\n" + ex + "\n\nYou can try to report this in MainDab's Discord Server at discord.io/maindab, but there is likely nothing we can do about it. You can alternatively join krnl's Discord at krnl.place/invite.html to report the issue.", "MainDab");
					}
				}).Start();
			}
			else
			{
				MessageBox.Show("[krnl error] krnl.dll isn't found. Either krnl.dll is still downloading, or that your antivirus has somehow deleted it (in which case you should disable your antivirus). You can join MainDab's Discord at discord.io/maindab if you need more help.", "MainDab");
			}
		}

		private static bool injectdll(dynamic filename, int PID)
		{
			Program.injecting = true;
			KRNLInjection.krnlInjectionResult krnlInjectionResult = KRNLInjection.DllInjector.GetInstance.Inject(AppDomain.CurrentDomain.BaseDirectory + $"\\\\{(object)filename}", PID);
			string text = "";
			string text2 = "";
			if ((dynamic)krnlInjectionResult == KRNLInjection.krnlInjectionResult.DllNotFound)
			{
				MessageBox.Show($"[krnl issue] {(object)filename} is missing! Your antivirus (or yourself) might have deleted it. krnl will now redownload krnl.dll.", "MainDab");
				Reinstall();
			}
			if ((dynamic)krnlInjectionResult == KRNLInjection.krnlInjectionResult.Failed)
			{
				MessageBox.Show("[krnl error] krnl has caught an unknown error while injecting! You can try to report this 'issue' in MainDab's Discord Server at discord.io/maindab, but there is likely nothing we can do about it. You can alternatively join krnl's Discord at krnl.place/invite.html to report the issue.", "MainDab");
			}
			if ((dynamic)krnlInjectionResult == KRNLInjection.krnlInjectionResult.Success)
			{
				injectedPID = PID;
			}
			if ((dynamic)krnlInjectionResult == KRNLInjection.krnlInjectionResult.threaderr)
			{
				MessageBox.Show("[krnl error] Some sort of thread error has occured.", "MainDab");
			}
			if ((dynamic)(!string.IsNullOrEmpty(text)))
			{
				Program.injecting = false;
				Program.failed_inject = true;
				MessageBox.Show(text, (text2 != "") ? text2 : "Krnl Error");
				return false;
			}
			return true;
		}

		private static void pipeshit(string script)
		{
			try
			{
				if ((dynamic)findpipe("krnlpipe"))
				{
					using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", "krnlpipe", PipeDirection.Out))
					{
						namedPipeClientStream.Connect();
						if ((dynamic)(!namedPipeClientStream.IsConnected))
						{
							throw new IOException("Failed To Connect To Pipe....");
						}
						StreamWriter streamWriter = new StreamWriter(namedPipeClientStream, Encoding.Default, 999999);
						streamWriter.Write(script);
						streamWriter.Dispose();
						return;
					}
				}
				MessageBox.Show("You must inject before executing a script! Click the inject button to continue.", "MainDab");
			}
			catch (Exception)
			{
			}
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool WaitNamedPipe(string name, int timeout);

		private static bool findpipe(string pipeName)
		{
			if ((dynamic)(!WaitNamedPipe(Path.GetFullPath("\\\\.\\pipe\\" + pipeName), 0)) && (Marshal.GetLastWin32Error() == 0 || Marshal.GetLastWin32Error() == 2))
			{
				return false;
			}
			return true;
		}

		public static void Execute(string script)
		{
			if ((dynamic)findpipe("krnlpipe"))
			{
				pipeshit(script);
			}
			else
			{
				MessageBox.Show("You must inject before executing a script! Click the inject button to continue.", "MainDab");
			}
		}

		public static bool IsAttached()
		{
			if ((dynamic)findpipe("krnlpipe"))
			{
				return true;
			}
			return false;
		}
	}
}
