using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting;

namespace Entropia.Root.Lifetime;

[Preserve]
public class CrashHandler : MonoBehaviour, ICrashHandlerUnitySingleton
{
    private static bool _isEditor;
    private static string _crashLogPath;

    private static int _crashed = 0;

    private void Awake()
    {
        try
        {
            _isEditor = Application.isEditor;
            _crashLogPath = Path.Combine(Application.persistentDataPath, "crash.log");

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }
        catch
        {
            Process.GetCurrentProcess().Kill();
        }
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        UniversalExceptionHandle($"Exception:\n{e?.ExceptionObject}");
    }

    private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            UniversalExceptionHandle($"Unity exception:\n{condition}\n{stackTrace}");
        }
    }

    private static void UniversalExceptionHandle(string message)
    {
        if (_isEditor) return;

        if (Interlocked.CompareExchange(ref _crashed, 1, 0) != 0) return;

        try
        {
            File.AppendAllText(_crashLogPath, $"[{DateTime.Now}]\n{message}\n");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo("notepad.exe", $"\"{_crashLogPath}\"") { UseShellExecute = false });

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start(new ProcessStartInfo("xdg-open", $"\"{_crashLogPath}\"") { UseShellExecute = true });

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Process.Start(new ProcessStartInfo("open", $"\"{_crashLogPath}\"") { UseShellExecute = true });

            else throw new PlatformNotSupportedException();
        }
        catch { }

        Process.GetCurrentProcess().Kill();
    }
}
