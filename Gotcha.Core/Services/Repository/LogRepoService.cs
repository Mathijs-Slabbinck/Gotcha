using Gotcha.Core.Data;
using Gotcha.Core.Services.ResultModel;
using Microsoft.EntityFrameworkCore;
using Gotcha.Core.Enums;
using Gotcha.Core.Interfaces;
using Gotcha.Core.Entities.Logging.LogEntities;
using System.Data;
using Gotcha.Core.Entities.Logging.Models;

namespace Gotcha.Core.Services.Repository
{
    public class LogRepoService : IRepositoryService<Log>
    {
        private readonly GotchaDbContext _gotchaDbContext;

        public LogRepoService(GotchaDbContext context)
        {
            _gotchaDbContext = context;
        }

        public async Task<ResultModel<T>> HandleAddingResultModelToLoggerAsync<T>(ResultModel<T> resultModel, Guid groupId)
        {
            if (!resultModel.FullSuccess)
            {
                await HandleErrorsAsync(resultModel, groupId, 0);
                await HandleWarningsAsync(resultModel, groupId, 0);
                return resultModel;
            }
            
            return resultModel;
        }

        private async Task HandleErrorsAsync<T>(ResultModel<T> resultModel, Guid groupId, int retryCount)
        {
            if (!resultModel.HasErrors || retryCount >= 10)
            {
                return;
            }

            foreach (Error error in resultModel.Errors)
            {
                Log log = new Log
                {
                    LogType = LogTypes.Error,
                    LogSubType = error.LogSubType,
                    Message = error.Message,
                    ExtraInfo = error.ExtraInfo,
                    LogGroupId = groupId
                };
                ResultModel<Log> result = await AddAsync(log);

                // If logging failed, try to log the logging failure itself (with retry limit)
                if (!result.FullSuccess)
                {
                    if (result.Data != null)
                    {
                        await HandleErrorsAsync(result, result.Data.LogGroupId, retryCount + 1);
                    }
                    else
                    {
                        await HandleErrorsAsync(result, Guid.Empty, retryCount + 1);
                    }
                }
            }
        }

        private async Task HandleWarningsAsync<T>(ResultModel<T> resultModel, Guid groupId, int retryCount)
        {
            if (!resultModel.HasWarnings || retryCount >= 10)
            {
                return;
            }

            foreach (Warning warning in resultModel.Warnings)
            {
                Log log = new Log
                {
                    LogType = LogTypes.Warning,
                    LogSubType = warning.LogSubType,
                    Message = warning.Message,
                    ExtraInfo = warning.ExtraInfo,
                    LogGroupId = groupId
                };
                ResultModel<Log> result = await AddAsync(log);

                if (!result.FullSuccess)
                {
                    // If logging failed, try to log the logging failure itself (with retry limit)
                    if (result.Data != null)
                    {
                        await HandleWarningsAsync(result, result.Data.LogGroupId, retryCount + 1);
                    }
                    else
                    {
                        await HandleWarningsAsync(result, Guid.Empty, retryCount + 1);
                    }
                }
            }
        }

        public async Task<ResultModel<Log>> AddAsync(Log log)
        {
            ResultModel<Log> resultModel = new ResultModel<Log>();

            if (log.LogType != LogTypes.Clean)
            {
                try
                {
                    await _gotchaDbContext.Logs.AddAsync(log);
                    await _gotchaDbContext.SaveChangesAsync();
                    resultModel.Data = log;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Error error = new Error(ex,
                                            LogSubTypes.Error_DbAdd_Concurrency_Exception,
                                            "It looks like the log details changed recently. To avoid overwriting other changes, please try again!",
                                            "Error in AddAsync() in LogRepoService");
                    resultModel.Errors.Add(error);
                }
                catch (TimeoutException ex)
                {
                    Error error = new Error(ex,
                                            LogSubTypes.Error_DbAdd_TimeOut_Exception,
                                            "Server timed out trying to add log. Please try again later!",
                                            "Error in AddAsync() in LogRepoService");
                    resultModel.Errors.Add(error);
                }
                catch (Exception ex)
                {
                    Error error = new Error(ex,
                                            LogSubTypes.Error_DbAdd_Exception,
                                            "Something went wrong while trying to create this log. Please try again!",
                                            "Error in AddAsync() in LogRepoService");
                    resultModel.Errors.Add(error);
                }
            }
            else
            {
                Warning warning = new Warning(LogSubTypes.Warning_Other,
                                              "A clean log was passed as param to AddAsync(), but logs here should never be clean!",
                                              "Warning in AddAsync() in LogRepoService");
                resultModel.Warnings.Add(warning);
            }

            return resultModel;
        }

        public async Task<ResultModel<List<Log>>> GetAllAsync()
        {
            ResultModel<List<Log>> resultModel = new ResultModel<List<Log>>();

            try
            {
                List<Log>? logs = await _gotchaDbContext.Logs
                                                           .OrderByDescending(l => l.TimeStamp)
                                                           .ToListAsync();

                if (logs == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the logs from the server. Please try again later!",
                                            "Error in GetAllAsync() in LogRepoService");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = logs;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "The server took too long to fetch the logs. Please try again later!",
                                        "Error in GetAllAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                        LogSubTypes.Error_DbGet_Exception,
                        "Something went wrong while trying to fetch the logs. Please try again later!",
                        "Error in GetAllAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }

            return resultModel;
        }

        public async Task<ResultModel<Log>> GetByIdAsync(Guid id)
        {
            ResultModel<Log> resultModel = new ResultModel<Log>();

            try
            {
                Log? log = await _gotchaDbContext.Logs
                                                    .FirstOrDefaultAsync(l => l.Id == id);

                if (log == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the log from the server. Please try again later!",
                                            $"Error in GetByIdAsync() in LogRepoService. Log id: {id}.");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = log;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "Server timed out trying to fetch log from the server. Please try again later!",
                                        $"Error in GetByIdAsync() in LogRepoService. Log id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_Exception,
                                        "Something went wrong while trying to fetch log. Please try again later!",
                                        $"Error in GetByIdAsync() in LogRepoService. Log id: {id}.");
                resultModel.Errors.Add(error);
            }

            return resultModel;
        }

        public async Task<ResultModel<List<Log>>> GetByTypeAsync(LogTypes logType)
        {
            ResultModel<List<Log>> resultModel = new ResultModel<List<Log>>();

            try
            {
                List<Log>? logs = await _gotchaDbContext.Logs
                                                            .Where(l => l.LogType == logType)
                                                            .OrderByDescending(l => l.TimeStamp)
                                                            .ToListAsync();

                if (logs == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the logs from the server. Please try again later!",
                                            $"Error in GetByTypeAsync() in LogRepoService. LogType: {logType}.");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = logs;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "Server timed out trying to fetch logs from the server. Please try again later!",
                                        $"Error in GetByTypeAsync() in LogRepoService. LogType: {logType}.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                        LogSubTypes.Error_DbGet_Exception,
                        "Something went wrong while trying to fetch logs. Please try again later!",
                        $"Error in GetByTypeAsync() in LogRepoService. LogType: {logType}.");
                resultModel.Errors.Add(error);
            }

            return resultModel;
        }

        public async Task<ResultModel<Log>> UpdateAsync(Log log)
        {
            ResultModel<Log> resultModel = new ResultModel<Log>();

            try
            {
                _gotchaDbContext.Logs.Update(log);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = log;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Concurrency_Exception,
                                        "It looks like the log details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in UpdateAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_TimeOut_Exception,
                                        "Server timed out trying to update log. Please try again later!",
                                        "Error in UpdateAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Exception,
                                        "Something went wrong while trying to update log. Please try again later!",
                                        "Error in UpdateAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }

            return resultModel;
        }

        public async Task<ResultModel<Log>> DeleteAsync(Guid id)
        {
            ResultModel<Log> resultModel = new ResultModel<Log>();

            try
            {
                Log? log = await _gotchaDbContext.Logs
                                                    .FirstOrDefaultAsync(l => l.Id == id);

                if (log == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't find log from the server (to delete it). Please try again later!",
                                            $"Error in DeleteAsync() in LogRepoService. Log id: {id}.");
                    resultModel.Errors.Add(error);
                    return resultModel;
                }

                resultModel.Data = log;

                _gotchaDbContext.Logs.Remove(log);
                await _gotchaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Concurrency_Exception,
                                        "It looks like the log's details changed recently. Please try again!",
                                        $"Error in DeleteAsync() in LogRepoService. Log id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_TimeOut_Exception,
                                        "Server timed out trying to delete log. Please try again later!",
                                        "Error in DeleteAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Exception,
                                        "Something went wrong while trying to remove log. Please try again later!",
                                        "Error in DeleteAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }

            return resultModel;
        }
    }
}