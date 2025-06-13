using System;
using UnityEngine;

namespace EIDOS.Debugging
{
    public static class Debugger
    {
        /// <summary>
        /// Logs a message with the specified header and log type.
        /// </summary>
        /// <param name="header">The header or category of the log message.</param>
        /// <param name="message">The content or body of the log message.</param>
        /// <param name="type">The log type, which determines the level of the log (Info, Warning, Error).</param>
        public static void Log(string header, string message, LogType type)
        {
            // Log the message based on the log type
            switch (type)
            {
                case LogType.Info:
                    Debug.Log($"{header} {message}");
                    break;
                
                case LogType.Warning:
                    Debug.LogWarning($"{header} {message}");
                    break;
                
                case LogType.Error:
                    Debug.LogError($"{header} {message}");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
