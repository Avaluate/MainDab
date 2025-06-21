using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MainDabWRDWrapper
{
    public class WRDCert
    {

        private const string Domain = "mboost.me";
        private const string Api = "api.mboost.me";
        private const string Localhost = "127.0.0.1";

        private const string CertSubject = "CN=mboost.me, OU=MainDab Development, O=mboost.me bypass, L=Central, S=VC, C=RV";

        private const string CertFileName = "mboost.me.crt";
        private const string KeyFileName = "mboost.me.key";
        private const string OpenSslSubDir = "OpenSSL";
        private static string OpenSslPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, OpenSslSubDir, "openssl.exe");

        private const string HostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
        private const string HostsFileEntryTemplate = "{0} {1}";

        private const string NodeServerExeName = "WRDFakeServer.exe";

        // openssl cert config
        private static readonly string Cert = $@"
        [req]
        distinguished_name = req_distinguished_name
        x509_extensions = v3_req
        prompt = no
        [req_distinguished_name]
        C = RV
        ST = VC
        L = Central
        O = mboost.me bypass
        OU = MainDab Development
        CN = {Domain}
        [v3_req]
        keyUsage = nonRepudiation, digitalSignature, keyEncipherment
        extendedKeyUsage = serverAuth
        subjectAltName = @alt_names
        [alt_names]
        DNS.1 = {Domain}
        DNS.2 = {Api}
        IP.1 = {Localhost}
    ";

        public static bool EnsureBypassConfigurationEstablished()
        {
            Console.WriteLine("starting bypass configuration...");

            if (!IsAdministrator()) { return false; }
            if (!EnsureOpenSslAvailable()) { return false; }
            if (!CreateAndInstallCertificate()) { return false; }

            Console.WriteLine("Bypass configuration completed successfully");
            return true;
        }

        private static bool IsAdministrator()
        {
            using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        private static bool EnsureOpenSslAvailable()
        {
            if (File.Exists(OpenSslPath))
            {
                Console.WriteLine($"found openssl.exe at: {OpenSslPath}");
                return true;
            }

            // some ppl like me have it at PATH. so we'll check
            var psi = new ProcessStartInfo("openssl", "--version")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                using (var process = Process.Start(psi))
                {
                    process.WaitForExit(5000); // just in case 
                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine("openssl.exe found in system PATH.");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"openssl.exe not found in PATH check: {ex.Message}");
            }
            return false;
        }

        private static bool CreateAndInstallCertificate()
        {
            Console.WriteLine("Checking SSL certificate...");

            X509Certificate2 existingCert = GetCertificateBySubject();

            // is already installed???
            if (existingCert != null)
            {
                // see if file is there or not. will not work is cert is not there
                if (!File.Exists("cert.conf") || !File.Exists(CertFileName) || !File.Exists(KeyFileName))
                {
                    Console.WriteLine("Certificate files are missing / not made before");
                    try { File.Delete("cert.conf"); } catch { }
                    try { File.Delete(CertFileName); } catch { }
                    try { File.Delete(KeyFileName); } catch { }
                }
                else
                {
                    Console.WriteLine("Certificate already installed in Trusted Root Certification Authorities");
                    return true;
                }
            }

            Console.WriteLine("Certificate not found so generating and installing...");

            string CertConfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cert.conf");
            string CertOutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CertFileName);
            string KeyOutputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, KeyFileName);

            try
            {
                File.WriteAllText(CertConfPath, Cert);

                Console.WriteLine("Generating private key...");
                if (!ExecuteOpenSsl($"genrsa -out \"{KeyOutputPath}\" 2048"))
                {
                    return false;
                }

                Console.WriteLine("Generating self-signed certificate for 10 years (more than enough)...");
                if (!ExecuteOpenSsl($"req -x509 -new -nodes -key \"{KeyOutputPath}\" -sha256 -days 3650 -out \"{CertOutputPath}\" -config \"{CertConfPath}\""))
                {
                    return false;
                }

                Console.WriteLine("Installing certificate to Trusted Root Certification Authority...");
                var cert = new X509Certificate2(CertOutputPath);
                using (var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(cert);
                    store.Close();
                }
                Console.WriteLine("Certificate installed successfully.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during certificate make: {ex.Message}");
                return false;
            }
        }

        private static bool ExecuteOpenSsl(string arguments)
        {
            var Proc = new ProcessStartInfo(OpenSslPath, arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
            };

            try
            {
                using (var process = Process.Start(Proc))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine($"OpenSSL command failed: {arguments}\nOutput: {output}\nError: {error}");
                        return false;
                    }
                    Console.WriteLine($"OpenSSL output: {output}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing OpenSSL: {ex.Message}\nCommand: {OpenSslPath} {arguments}");
                return false;
            }
        }

        private static X509Certificate2 GetCertificateBySubject()
        {
            Console.WriteLine(CertSubject);
            using (var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                foreach (var cert in store.Certificates)
                {
                    Console.WriteLine(cert.Subject);
                    if (cert.Subject == CertSubject)
                    {
                        // subject is sufficient
                        Console.WriteLine($"found certificate: {cert.Subject}");
                        return cert;
                    }
                }
                return null;
            }
        }

        private static bool ModifyHostsFile()
        {
            Console.WriteLine("Checking hosts file...");

            try
            {
                var lines = File.ReadAllLines(HostsFilePath).ToList();
                bool hostsFileChanged = false;

                // Check and add mboost.me entry
                string mboostEntry = string.Format(HostsFileEntryTemplate, Localhost, Domain);
                if (!IsHostsEntryPresent(lines, Domain))
                {
                    lines.Add(mboostEntry + " # Local Bypass");
                    hostsFileChanged = true;
                    Console.WriteLine($"Added '{mboostEntry}' to hosts file.");
                }
                else
                {
                    Console.WriteLine($"Entry for '{Domain}' already present in hosts file.");
                    return true;
                }

                // Check and add api.mboost.me entry
                string apiMboostEntry = string.Format(HostsFileEntryTemplate, Localhost, Api);
                if (!IsHostsEntryPresent(lines, Api))
                {
                    lines.Add(apiMboostEntry + " # Local Bypass");
                    hostsFileChanged = true;
                    Console.WriteLine($"Added '{apiMboostEntry}' to hosts file.");
                }
                else
                {
                    Console.WriteLine($"Entry for '{Api}' already present in hosts file.");
                }

                if (hostsFileChanged)
                {
                    File.WriteAllLines(HostsFilePath, lines);
                    Console.WriteLine("Hosts file updated. Flushing DNS cache...");
                    FlushDnsCache();
                    Console.WriteLine("DNS cache flushed.");
                }
                else
                {
                    Console.WriteLine("No changes needed for hosts file.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying hosts file: {ex.Message}");
                return false;
            }
        }

        private static bool IsHostsEntryPresent(List<string> lines, string domain)
        {
            var pattern = new Regex($@"^(?:\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\s+)?(?i:{domain})(\s+#.*)?$", RegexOptions.Multiline);
            return lines.Any(line => pattern.IsMatch(line.Trim()));
        }

        private static void FlushDnsCache()
        {
            // ipconfig /flushdns command
            var psi = new ProcessStartInfo("ipconfig", "/flushdns")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(psi);
            process.WaitForExit();
        }

        public static Process LaunchNodeBypassServer()
        {

            if (!File.Exists(NodeServerExeName))
            {
                Console.WriteLine($"Error: Node.js bypass server executable not found at {NodeServerExeName}.");
                return null;
            }

            try
            {
                ModifyHostsFile();
            }
            catch
            {
                return null;
            }


            // just in case its running
            try
            {
                foreach (Process proc in Process.GetProcessesByName("WRDFakeServer"))
                {
                    proc.Kill();
                }
            }
            catch { }

            Task.Delay(1000); // if any other fake sevrers are running

            Console.WriteLine($"Launching Node.js bypass server: {NodeServerExeName}");
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            var processinfo = new ProcessStartInfo();
            processinfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            processinfo.FileName         = NodeServerExeName;
            processinfo.UseShellExecute  = true;

            Process process = Process.Start(processinfo);

            if (process == null)
            {
                Console.WriteLine("Failed to start Node.js bypass server process.");
                return null;
            }
            else
            {
                return process;
            }

        }

    }
}
