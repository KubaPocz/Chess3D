using System.Diagnostics;
using UnityEngine;

namespace AI
{
    public class StockfishEngine
    {
        private Process _process;

        public void StartEngine()
        {
            _process = new Process();
            _process.StartInfo.FileName = Application.streamingAssetsPath + "/AI/stockfish.exe";
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.Start();

            SendCommand("uci");
            SendCommand("uciok");
            SendCommand("isready");
            SendCommand("reakyok");
        }
        public void StopEngine()
        {
            SendCommand("quit");
            _process.Close();
        }
        public void SetSkillLevel(int level)
        {
            level = Mathf.Clamp(level, 0, 20);
            SendCommand($"setoption name Skill Level value {level}");
        }
        public string GetBestMove(string fen, int depth = 10)
        {
            SendCommand("position fen " + fen);
            SendCommand($"go depth {depth}");
        
            string line;
            while((line = _process.StandardOutput.ReadLine()) != null)
            {
                if (line.StartsWith("bestmove"))
                {
                    return line.Split(' ')[1];
                }
            }
            return null;
        }
        private void SendCommand(string command)
        {
            UnityEngine.Debug.Log("-> " + command);
            if (_process == null || _process.HasExited)
            {
                UnityEngine.Debug.LogError("Stockfish process is not running.");
                return;
            }
            _process.StandardInput.WriteLine(command);
            _process.StandardInput.Flush();
        }
    }
}
