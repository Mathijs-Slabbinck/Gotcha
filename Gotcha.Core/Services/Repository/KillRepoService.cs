using Gotcha.Core.Data;
using Gotcha.Core.Entities;
using Gotcha.Core.Entities.Logging.LogEntities;
using Gotcha.Core.Enums;
using Gotcha.Core.Interfaces;
using Gotcha.Core.Services.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Core.Services.Repository
{
    public class KillRepoService : IRepositoryService<Kill>
    {
        private readonly GotchaDbContext _gotchaDbContext;
        private readonly LogRepoService _logRepoService;

        public KillRepoService(GotchaDbContext _context, LogRepoService logRepoService)
        {
            _gotchaDbContext = _context;
            _logRepoService = logRepoService;
        }

        [HttpGet]
        public async Task<ResultModel<List<Kill>>> GetAllAsync()
        {
            ResultModel<List<Kill>> resultModel = new ResultModel<List<Kill>>();

            try
            {
                List<Kill>? kills = await _gotchaDbContext.Kills
                                                            .ToListAsync();

                if(kills == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the kills from the server. Please try again later!",
                                            "Error in GetAllAsync() in KillRepoService");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = kills;
            }

            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "The server took too long to fetch the kills. Please try again later!",
                                        "Error in GetAllAsync() in KillRepoService");

                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                        LogSubTypes.Error_DbGet_Exception,
                        "Something went wrong while trying to fetch the kills. Please try again later!",
                        "Error in GetAllAsync() in KillRepoService");

                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpGet]
        public async Task<ResultModel<Kill>> GetByIdAsync(Guid id)
        {
            ResultModel<Kill> resultModel = new ResultModel<Kill>();

            try
            {
                Kill? kill = await _gotchaDbContext.Kills
                                                        .FirstOrDefaultAsync(a => a.Id == id);
                if(kill == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                        "Couldn't fetch the kill from the server. Please try again later!",
                        $"Error in GetByIdAsync() in KillRepoService. Kill id: {id}.");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = kill;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "Server timed out trying to fetch kill from the server. Please try again later!",
                                        $"Error in GetByIdAsync() in KillRepoService. Kill id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_Exception,
                                        "Something went wrong while trying to fetch kill. Please try again later!",
                                        $"Error in GetByIdAsync() in KillRepoService. Kill id: {id}.");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }


        [HttpPost]
        public async Task<ResultModel<Kill>> AddAsync(Kill kill)
        {
            ResultModel<Kill> resultModel = new ResultModel<Kill>();

            try
            {
                await _gotchaDbContext.Kills.AddAsync(kill);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = kill;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Concurrency_Exception,
                                        "It looks like the kill details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in AddAsync() in KillRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_TimeOut_Exception,
                                        "Server timed out trying to add kill. Please try again later!",
                                        "Error in AddAsync() in KillRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Exception,
                                        "Something went wrong while trying to create this kill. Please try again!",
                                        "Error in AddAsync() in KillRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpPost]
        public async Task<ResultModel<Kill>> UpdateAsync(Kill kill)
        {
            ResultModel<Kill> resultModel = new ResultModel<Kill>();

            try
            {
                _gotchaDbContext.Kills.Update(kill);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = kill;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Concurrency_Exception,
                                        "It looks like the kill details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in UpdateAsync() in KillRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_TimeOut_Exception,
                                        "Server timed out trying to update kill. Please try again later!",
                                        "Error in UpdateAsync() in KillRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Exception,
                                        "Something went wrong while trying to update kill. Please try again later!",
                                        "Error in UpdateAsync() in KillRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpPost]
        public async Task<ResultModel<Kill>> DeleteAsync(Guid id)
        {
            ResultModel<Kill> resultModel = new ResultModel<Kill>();

            try
            {
                Kill? kill = await _gotchaDbContext.Kills
                                                    .FirstOrDefaultAsync(l => l.Id == id);

                if (kill == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't find kill from the server (to delete it). Please try again later!",
                                            $"Error in DeleteAsync() in KillRepoService. Kill id: {id}.");
                    resultModel.Errors.Add(error);
                    return resultModel;
                }

                resultModel.Data = kill;

                _gotchaDbContext.Kills.Remove(kill);
                await _gotchaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Concurrency_Exception,
                                        "It looks like the kill's details changed recently. Please try again!",
                                        $"Error in DeleteAsync() in KillRepoService. Kill id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_TimeOut_Exception,
                                        "Server timed out trying to delete kill. Please try again later!",
                                        "Error in DeleteAsync() in KillRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Exception,
                                        "Something went wrong while trying to remove kill. Please try again later!",
                                        "Error in DeleteAsync() in KillRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }
    }
}
