using Microsoft.Extensions.Options;
using SocialMedia.Framework.Core.Logging;
using SocialMedia.Framework.Data;
using SocialMedia.Framework.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SocialMedia.Framework.Services
{
    public interface ILogService
    {
        void LogError(string message, string msgTemplate, string ex);
        void LogInformation(string message, string msgTemplate);
        void LogWarning(string message, string msgTemplate, string ex);
        QueryResult<Log> GetLogs(LogQuery filter);
        Log GetLogById(int id);
    }
    public class LogService : ILogService
    {
        #region Private Fields
        private readonly SMDBContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        #endregion

        #region Constructor
        public LogService(
            SMDBContext context,
            IOptions<AppSettings> app)
        {
            _context = context;
            _appSettings = app;
        }
        #endregion

        #region Public Methods
        public void LogInformation(string message, string msgTemplate)
        {
            Log log = new Log
            {
                Message = message,
                MessageTemplate = msgTemplate,
                Level = "Information",
                TimeStamp = DateTime.Now,
            };
            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public void LogError(string message, string msgTemplate, string ex)
        {
            Log log = new Log
            {
                Message = message,
                MessageTemplate = msgTemplate,
                Level = "Error",
                TimeStamp = DateTime.Now,
                Exception = ex,
            };

            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public void LogWarning(string message, string msgTemplate, string ex)
        {
            Log log = new Log
            {
                Message = message,
                MessageTemplate = msgTemplate,
                Level = "Warning",
                TimeStamp = DateTime.Now,
                Exception = ex,
            };

            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public QueryResult<Log> GetLogs(LogQuery queryObj)
        {
            DeleteOlderLogs(_context);

            var result = new QueryResult<Log>();

            var query = _context.Logs.ToList().AsQueryable();

            query = SortLogs(queryObj, query);

            var columnsMap = new Dictionary<string, Expression<Func<Log, object>>>()
            {
                ["message"] = l => l.Message,
                ["level"] = l => l.Level,
                ["timeStamp"] = l => l.TimeStamp
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();

            query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;
        }

        public Log GetLogById(int id)
        {
            return _context.Logs.Find(id);
        }
        #endregion

        #region Private Methods
        private static IQueryable<Log> SortLogs(LogQuery queryObj, IQueryable<Log> query)
        {
            if (!string.IsNullOrWhiteSpace(queryObj.Level))
                query = query.Where(l => l.Level == queryObj.Level);

            if (queryObj.StartDate != null && queryObj.EndDate != null)
                query = query.Where(l => queryObj.StartDate <= l.TimeStamp && queryObj.EndDate >= l.TimeStamp);

            if (queryObj.StartDate != null && queryObj.EndDate == null)
                query = query.Where(l => queryObj.StartDate <= l.TimeStamp);

            if (queryObj.EndDate != null && queryObj.StartDate == null)
                query = query.Where(l => queryObj.EndDate >= l.TimeStamp);
            return query;
        }

        private void DeleteOlderLogs(SMDBContext context)
        {
            var logs = context.Logs.ToList();
            foreach (var log in logs)
            {
                if (LogLifeChecker(log))
                {
                    context.Logs.Remove(log);
                }
            }
            context.SaveChangesAsync();
        }

        private bool LogLifeChecker(Log log)
        {
            int logLifeLimit = int.Parse(_appSettings.Value.LogLife);
            int logLife = (int)(DateTime.Now - log.TimeStamp).TotalDays;

            return (logLife > logLifeLimit) ? true : false;
        }
        #endregion
    }
}
