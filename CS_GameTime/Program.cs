using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Principal;

namespace CS_GameTime
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AutoStartCheck();

            if (!IsRunAsAdmin())
            {
                RestartAsAdmin();

                Environment.Exit(0);
                return;
            }

            TgBot bot = new TgBot();

            bot.StartBot();

            Console.WriteLine("Приложение запущено.");

            await Task.Delay(-1);
        }

        static void AutoStartCheck()
        {
            AutoStartChecker autoStartChecker = new();

            if (!autoStartChecker.IsInStartup())
            {
                autoStartChecker.AddToStartup();
            }
        }

        static bool IsRunAsAdmin()
        {
            using WindowsIdentity identity = WindowsIdentity.GetCurrent();

            WindowsPrincipal principal = new(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void RestartAsAdmin()
        {
            string? exeName = Environment.ProcessPath;

            if (string.IsNullOrEmpty(exeName))
            {
                Console.WriteLine("Не удалось получить путь к exe.");

                return;
            }

            ProcessStartInfo startInfo = new()
            {
                FileName = exeName,
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                Process.Start(startInfo);
            }
            catch
            {
                Console.WriteLine("Пользователь отклонил UAC.");
            }
        }
    }

    class AutoStartChecker
    {
        private const string AppName = "CS_GameTime";

        private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public bool IsInStartup()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKey, false);

            if (key == null)
                return false;

            return key.GetValue(AppName) != null;
        }

        public void AddToStartup()
        {
            string? exePath =
                Environment.ProcessPath;

            if (string.IsNullOrEmpty(exePath))
                return;

            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKey, true);

            if (key == null)
                return;

            key.SetValue(AppName, $"\"{exePath}\"");
        }
    }
}