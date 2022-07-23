using System;
using System.Reflection;

namespace Assets.Arch
{
    /// <summary>
    /// Logging utils.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Whether or not to try to auto hook common loggers such as UnityEngine.Debug
        /// </summary>
        public static bool TryAutoHookCommonLoggers = true;

        public static event Action<string> InfoLog = (value) => Console.WriteLine("[INFO] " + value);

        public static event Action<string> WarningLog = (value) =>
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[WARNING] " + value);
            Console.BackgroundColor = ConsoleColor.Black;
        };

        public static event Action<string> ErrorLog = (value) =>
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + value);
            Console.BackgroundColor = ConsoleColor.Black;
        };

        public static event Action<string> DataLog = (value) =>
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[DATA] " + value);
            Console.BackgroundColor = ConsoleColor.Black;
        };

        public static LogLevel CurrentLogLevel = LogLevel.Info;

        internal static void LogInfo(string value) =>
            InfoLog?.Invoke(value);

        internal static void LogWarning(string value) =>
            WarningLog?.Invoke(value);

        internal static void LogError(string value) =>
            ErrorLog?.Invoke(value);

        internal static void LogData(string value) =>
            DataLog?.Invoke(value);


        static Logging()
        {
            if (!TryAutoHookCommonLoggers) return;

            try
            {
                var unityDebugType = Type.GetType("UnityEngine.Debug, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");

                if (unityDebugType == null) return;

                var infoLogMethod = unityDebugType.GetMethod("Log", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
                var warningLogMethod = unityDebugType.GetMethod("LogWarning", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
                var errorLogMethod = unityDebugType.GetMethod("LogError", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
                var dataLogMethod = unityDebugType.GetMethod("LogData", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(object) }, null);

                if (infoLogMethod != null)
                {
                    InfoLog += (value) =>
                    {
                        infoLogMethod.Invoke(null, new object[] { value });
                    };

                    if (CurrentLogLevel <= LogLevel.Debug) LogInfo("UnityEngine.Debug.Log(object) was hooked");
                }

                if (warningLogMethod != null)
                {
                    WarningLog += (value) =>
                    {
                        warningLogMethod.Invoke(null, new object[] { value });
                    };

                    if (CurrentLogLevel <= LogLevel.Debug) LogInfo("UnityEngine.Debug.LogWarning(object) was hooked");
                }

                if (errorLogMethod != null)
                {
                    ErrorLog += (value) =>
                    {
                        errorLogMethod.Invoke(null, new object[] { value });
                    };

                    if (CurrentLogLevel <= LogLevel.Debug) LogInfo("UnityEngine.Debug.LogError(object) was hooked");
                }

                if (dataLogMethod != null)
                {
                    DataLog += (value) =>
                    {
                        dataLogMethod.Invoke(null, new object[] { value });
                    };

                    if (CurrentLogLevel <= LogLevel.Debug) LogInfo("UnityEngine.Debug.LogData(object) was hooked");
                }
            }
            catch (TypeLoadException)
            {
                if (CurrentLogLevel <= LogLevel.Debug) LogInfo("Could not load custom logging hook from UnityEngine.Debug");
            }
        }
    }

    /// <summary>
    /// Log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Detailed steps of every event.
        /// </summary>
        Debug,
        /// <summary>
        /// General events such as when a client connects.
        /// </summary>
        Info,
        /// <summary>
        /// A potential problem has occured. It doesnt prevent us from continuing. This occurs for things that might be others fault, such as invalid configurations.
        /// </summary>
        Warning,
        /// <summary>
        /// An error that affects us occured. Usually means the fault of us.
        /// </summary>
        Error,
        /// <summary>
        /// Logs nothing.
        /// </summary>
        Nothing
    }
}
