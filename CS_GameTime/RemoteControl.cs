using System.Diagnostics;

namespace CS_GameTime
{
    public class RemoteControl
    {
        public string GetStatGame(string processName)
        {
            try
            {
                Process? process = Process
                    .GetProcessesByName(processName)
                    .FirstOrDefault();

                if (process == null)
                    return $"{processName}: не запущен";

                DateTime startProc = process.StartTime;
                TimeSpan time = DateTime.Now - startProc;

                try
                {
                    return $"{process.ProcessName}: " +
                           $"{DateTime.Now - process.StartTime:hh\\:mm\\:ss}";
                }
                catch
                {
                    return $"{processName}: не удалось получить время";
                }
            }
            catch (Exception ex)
            {
                return $"Ошибка: {ex.Message}";
            }
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
