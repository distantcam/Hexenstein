using System;
using Caliburn.Micro;
using NLog;

namespace Hexenstein.Framework.Caliburn
{
    internal class CaliburnMicroNLogShim : ILog
    {
        private readonly Logger innerLogger;

        public CaliburnMicroNLogShim(Type type)
        {
            innerLogger = NLog.LogManager.GetLogger(type.Name);
        }

        public void Error(Exception exception)
        {
            innerLogger.ErrorException(exception.Message, exception);
        }

        public void Info(string format, params object[] args)
        {
            innerLogger.Info(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            innerLogger.Warn(format, args);
        }
    }
}