using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Logging
{
    public class Log
    {
        private readonly Guid _id;
        // in case of multiple warnings or errors in a ResultModel
        private readonly Guid _logGroupId;
        private readonly DateTime _timeStamp;
        private readonly LogTypes _logType;
        private readonly LogSubTypes _logSubType;
        private readonly string? _message;
        private readonly string? _extraInfo;
        private readonly Exception? _exception;
        private readonly Attacker? _attacker;
        private readonly Guid _attackerId;

        #region Construstors

        #region Base_Constructor
        // private because you must pass at least a message or Attacker with it
        private Log(LogTypes logTypes)
        {
            _timeStamp = DateTime.Now;
            _id = Guid.NewGuid();
            _logType = logTypes;
            _logSubType = LogSubTypes.Unknown;
            _logGroupId = Guid.NewGuid();
        }
        #endregion

        #region Constructors_For_New_logs
        public Log(LogTypes logType, LogSubTypes logSubType) : this(logType)
        {
            _logSubType = logSubType;
        }

        public Log(LogTypes logType, string message) : this(logType)
        {
            _message = message;
        }

        public Log(Exception exception, LogTypes logType, string message) : this(logType, message)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, string message) : this(logType, logSubType)
        {
            _message = message;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, string message) : this(logType, logSubType, message)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, string message, Guid logGroupId) : this(logType, logSubType, message)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, string message, Guid logGroupId) : this(logType, logSubType, message, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, string message, Guid logGroupId) : this(logType, message)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, string message, Guid logGroupId) : this(logType, message, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, string message, string? extraInfo) : this(logType, logSubType, message)
        {
            _extraInfo = extraInfo;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, string message, string? extraInfo) : this(logType, logSubType, message, extraInfo)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, string message, string? extraInfo) : this(logType, message)
        {
            _extraInfo = extraInfo;
        }

        public Log(Exception exception, LogTypes logType, string message, string? extraInfo) : this(logType, message, extraInfo)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, string message, string? extraInfo, Guid logGroupId) : this(logType, logSubType, message, extraInfo)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, string message, string? extraInfo, Guid logGroupId) : this(logType, logSubType, message, extraInfo, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, string message, string? extraInfo, Guid logGroupId) : this(logType, message, extraInfo)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, string message, string? extraInfo, Guid logGroupId) : this(logType, message, extraInfo, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, string message, string? extraInfo, Attacker attacker) : this(logType, logSubType, message, extraInfo)
        {
            _attacker = attacker;
            _attackerId = attacker.Id;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, string message, string? extraInfo, Attacker attacker) : this(logType, logSubType, message, extraInfo, attacker)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, string message, string? extraInfo, Attacker attacker) : this(logType, message, extraInfo)
        {
            _attacker = attacker;
            _attackerId = attacker.Id;
        }

        public Log(Exception exception, LogTypes logType, string message, string? extraInfo, Attacker attacker) : this(logType, message, extraInfo, attacker)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, string message, string? extraInfo, Attacker attacker, Guid logGroupId) : this(logType, logSubType, message, extraInfo, attacker)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, string message, string? extraInfo, Attacker attacker, Guid logGroupId) : this(logType, logSubType, message, extraInfo, attacker, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, string message, string? extraInfo, Attacker attacker, Guid logGroupId) : this(logType, message, extraInfo, attacker)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, string message, string? extraInfo, Attacker attacker, Guid logGroupId) : this(logType, message, extraInfo, attacker, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, Attacker attacker) : this(logType, logSubType)
        {
            _attacker = attacker;
            _attackerId = attacker.Id;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, Attacker attacker) : this(logType, logSubType, attacker)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, Attacker attacker) : this(logType)
        {
            _attacker = attacker;
        }

        public Log(Exception exception, LogTypes logType, Attacker attacker) : this(logType, attacker)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, Attacker attacker, Guid logGroupId) : this(logType, logSubType, attacker)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, Attacker attacker, Guid logGroupId) : this(logType, logSubType, attacker, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, Attacker attacker, Guid logGroupId) : this(logType, attacker)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, Attacker attacker, Guid logGroupId) : this(logType, attacker, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, string message, Attacker attacker) : this (logType, logSubType, message)
        {
            _attacker = attacker;
            _attackerId = attacker.Id;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, string message, Attacker attacker) : this(logType, logSubType, message, attacker)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, string message, Attacker attacker) : this(logType, message)
        {
            _attacker = attacker;
            _attackerId = attacker.Id;
        }

        public Log(Exception exception, LogTypes logType, string message, Attacker attacker) : this(logType, message, attacker)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, LogSubTypes logSubType, string message, Attacker attacker, Guid logGroupId) : this(logType, logSubType, message, attacker)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, LogSubTypes logSubType, string message, Attacker attacker, Guid logGroupId) : this(logType, logSubType, message, attacker, logGroupId)
        {
            _exception = exception;
        }

        public Log(LogTypes logType, string message, Attacker attacker, Guid logGroupId) : this(logType, message, attacker)
        {
            _logGroupId = logGroupId;
        }

        public Log(Exception exception, LogTypes logType, string message, Attacker attacker, Guid logGroupId) : this(logType, message, attacker, logGroupId)
        {
            _exception = exception;
        }
        #endregion

        #region Full_Constructor
        // Constructor used for db recovery

        public Log(Guid id, Exception? exception, DateTime timestamp, LogTypes logtype, LogSubTypes logSubType, string? message, string? extraInfo, Attacker attacker, Guid logGroupId)
        {
            _id = id;
            _exception = exception;
            _timeStamp = timestamp;
            _logType = logtype;
            _logSubType = logSubType;
            _message = message;
            _extraInfo = extraInfo;
            _attacker = attacker;
            _attackerId = attacker.Id;
            _logGroupId = logGroupId;
        }

        #endregion

        #endregion

        public Guid Id
        {
            get { return _id; }
        }

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
        }

        public LogTypes LogType
        {
            get { return _logType; }
        }

        public LogSubTypes LogSubType
        {
            get { return _logSubType; }
        }

        public string? Message
        {
            get { return _message; }
        }

        public Attacker? Attacker
        {
           get {
                if (_attacker == null && LogType == LogTypes.HackAttempt)
                {
                    Log log = new Log(LogTypes.Error, LogSubTypes.Error_PlayerNotFound_Exception, "Logtype is HackAttempt but attacker is null!", $"Log id: {Id}");

                }

                return _attacker;
            }
        }

        public Guid AttackerId
        {
            get { return _attackerId; }
        }

        public string? ExtraInfo
        {
            get { return _extraInfo; }
        }

        public Guid LogGroupId
        {
            get { return _logGroupId; }
        }
    }
}
