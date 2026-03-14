using System.Diagnostics;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CS_GameTime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ЗАПУСК БОТА
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
                //Thread.Sleep(1000);

            }
            #endregion

            bool IsProcessRunning(string processName)
            {
                Process[] processes = Process.GetProcessesByName(processName);
                return processes.Length > 0;
            }

        }
        
    }
}
