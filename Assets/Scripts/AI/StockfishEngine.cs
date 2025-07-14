using System.Diagnostics;
using UnityEngine;

public class StockfishEngine
{
    private Process process;

    public void StartEngine()
    {
        process = new Process();
        process.StartInfo.FileName = Application.streamingAssetsPath + "/AI/stockfish.exe";
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        SendCommand("uci");
        SendCommand("uciok");
        SendCommand("isready");
        SendCommand("reakyok");
    }
    public void StopEngine()
    {
        SendCommand("quit");
        process.Close();
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
        while((line = process.StandardOutput.ReadLine()) != null)
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
        if (process == null || process.HasExited)
        {
            UnityEngine.Debug.LogError("Stockfish process is not running.");
            return;
        }
        process.StandardInput.WriteLine(command);
        process.StandardInput.Flush();
    }

    private void WaitFor(string keyword)
    {
        string line;
        while((line=process.StandardOutput.ReadLine()) != null)
        {
            if (line.Contains(keyword))
                return;
        }
    }
}
