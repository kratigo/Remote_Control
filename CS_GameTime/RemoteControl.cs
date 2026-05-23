using System.Diagnostics;

namespace CS_GameTime
{
    public class RemoteControl
    {
        public string GetStatGame(string processName)
        {
            DateTime currentDate = DateTime.Now;
            Process[] processes = Process.GetProcessesByName(processName);
            string stat = "";
            try
            {
                DateTime startProc = processes[0].StartTime;
                stat += $"Вы играете в {processes[0].ProcessName}: {currentDate - startProc}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении статуса игр: {ex.Message}");
            }
            
            return stat;
        }
        public void StopGame(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (var proc in processes)
            {
                try
                {
                    proc.Kill();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при попытке закрыть игру {proc.ProcessName}: {ex.Message}");
                }
            }
        }
        public void StartGame(string processName)
        {
            try
            {
                Process.Start(processName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при попытке запустить игру: {ex.Message}");
            }
        }
        public void StartSteamGame(int steamGameId)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = $"steam://rungameid/{steamGameId}",
                    UseShellExecute = true
                };
                Process.Start(psi);
                Console.WriteLine("Игра запущена через Steam!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запуске Steam-игры: {ex.Message}");
            }
        }
        public bool IsGameRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }
    }
}
