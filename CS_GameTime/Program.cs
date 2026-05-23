using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace CS_GameTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

            //проверка на повторный запуск и закрытие нового экземпляра
            //
            AutoStartCheck();
            if (!IsRunAsAdmin())
            {
                RestartAsAdmin();
                Environment.Exit(0);
                //return;
            }


            //ЗАПУСК 
            TgBot bot = new TgBot();
            Thread botThread = new Thread(new ThreadStart(bot.StartBot));
            botThread.Start();

            #region
            //string processName = "CellToSingularity"; // game
            ////Process process = new Process();
            //Process[] processes = Process.GetProcessesByName(processName);

            //bool isProcessRunning = IsProcessRunning(processName);
            //DateTime startProc = DateTime.Now;

            //if (isProcessRunning)
            //{
            //    startProc = processes[0].StartTime;
            //    Console.WriteLine($"Игра {processName} запущена в {startProc}");
            //}



            while (true)
            {
                //isProcessRunning = IsProcessRunning(processName);

                //if (isProcessRunning)
                //{
                //    DateTime currentDate = DateTime.Now;
                //    if (currentDate - startProc > TimeSpan.FromHours(3))
                //    {
                //        processes = Process.GetProcessesByName(processName);
                //        foreach (var proc in processes)
                //        {
                //            proc.Kill();
                //        }
                //        Console.Clear();
                //        Console.WriteLine($"Вы играли в {processName} слишком долго. Игра была закрыта.");
                //    }
                //    Console.Clear();
                //    Console.WriteLine($"Вы играете в {processName}: {currentDate - startProc}");

                //}
                //else
                //{
                //    startProc = DateTime.Now;
                //    Console.Clear();
                //    Console.WriteLine($"Отсчёт начнётся после запуска {processName}.");
                //}
                Thread.Sleep(1000);

            }
            #endregion

            bool IsProcessRunning(string processName)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                return processes.Length > 0;
            }
            void AutoStartCheck()
            {
                AutoStartChecker autoStartChecker = new AutoStartChecker();
                if (!autoStartChecker.IsInStartup())
                {
                    autoStartChecker.AddToStartup();
                }
            }

            bool IsRunAsAdmin()
            {
                using WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            void RestartAsAdmin()
            {
                var exeName = Environment.ProcessPath;

                if (string.IsNullOrEmpty(exeName))
                {
                    Console.WriteLine("Не удалось получить путь к exe.");
                    return;
                }

                var startInfo = new ProcessStartInfo
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
                    Console.WriteLine("Пользователь отклонил запуск от имени администратора.");
                }
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
            if (key == null) return false;

            return key.GetValue(AppName) != null;
        }

        public void AddToStartup()
        {
            string exePath = Process.GetCurrentProcess().MainModule!.FileName!;

            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKey, true);
            if (key == null) return;

            key.SetValue(AppName, $"\"{exePath}\"");
        }
    }
}
