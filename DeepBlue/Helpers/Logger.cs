using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using System.Configuration;

namespace DeepBlue.Helpers {
	public class Logger {

		private static ILoggingService _loggerService;
		public static ILoggingService LoggingService {
			get {
				if (_loggerService == null) {
					_loggerService = new LoggingService();
				}
				return _loggerService;
			}
			set {
				_loggerService = value;
			}
		}

		private const string DATABASE_LOGSINK = "database";
		private const string MSMQ_LOGSINK = "msmq";
		public static void Write(Log log) {
			switch (ConfigurationManager.AppSettings["Logging.Destination"].ToLower()) {
				case Logger.DATABASE_LOGSINK:
					LoggingService.SaveLog(log);
					break;
				case Logger.MSMQ_LOGSINK:
					string destinationQ = ConfigurationManager.AppSettings["Logging.MSMQ.Queue.Path"];

					// publish the request to the appropriate message queue
					System.Messaging.MessageQueue mq = new System.Messaging.MessageQueue(destinationQ);

					System.Messaging.Message msg = new System.Messaging.Message();
					msg.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(Log) });
					msg.Body = log;
					msg.Priority = System.Messaging.MessagePriority.Normal;
					msg.Recoverable = true;
					msg.Label = log.LogType.GetType().Name;
					mq.Send(msg);
					break;
				default:
					LoggingService.SaveLog(log);
					break;
			}
		}
	}
}