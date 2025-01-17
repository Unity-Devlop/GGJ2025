using System;
using System.Text;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Unity3D;
using UnityEngine;
using ILogger = Serilog.ILogger;

namespace Game
{
    public static class GameLogger
    {
        static GameLogger()
        {
            string prefix;
#if UNITY_EDITOR
            prefix = Application.dataPath + "/Editor Default Resources/";
#else
            prefix = Application.persistentDataPath + "/";
#endif

            string logPath = prefix + $"log/{DateTime.Now:yyyy-MM-dd}-.txt";

            var config = new LoggerConfiguration().MinimumLevel.
#if UNITY_EDITOR
                    Debug()
#else
                    Error()
#endif
                    .WriteTo.Unity3D().WriteTo.File(
                        logPath,
                        restrictedToMinimumLevel: LogEventLevel.Warning, // 日志输出最低级别
                        outputTemplate:
                        @"{Timestamp:yyyy-MM-dd HH:mm-ss.fff }[{Level:u3}] {Message:lj}{NewLine}{Exception}",
                        rollingInterval: RollingInterval.Day, //日志按天保存
                        rollOnFileSizeLimit: true, // 限制单个文件的最大长度
                        fileSizeLimitBytes: 10 * 1024 * 1024, // 单个文件最大长度10M
                        encoding: Encoding.UTF8, // 文件字符编码
                        retainedFileCountLimit: 1024) // 最大保存文件数,超过最大文件数会自动覆盖原有文件
                ;
            Serilog.Log.Logger = config.CreateLogger();
        }

        public static ILogger Log => Serilog.Log.Logger;
    }
}