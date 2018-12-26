/*************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    xy
** date：    2018/7/13
** desc：    打印日志封装类
** Ver.:     V1.0.0
**************************************/

using log4net;
using System.Reflection;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace permission.common
{
    /// <summary>
    /// log4net记录日志
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// WriteInfo
        /// </summary>
        /// <param name="text"></param>
        public static void WriteInfo(string text)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            //记录信息日志
            log.Info(text);
        }

        /// <summary>
        /// WriteDebug
        /// </summary>
        /// <param name="text"></param>
        public static void WriteDebug(string text)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            //记录调试日志
            log.Debug(text);
        }

        /// <summary>
        /// WriteError
        /// </summary>
        /// <param name="text"></param>
        public static void WriteError(string text)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            //记录错误日志
            log.Error(text);
        }
    }
}