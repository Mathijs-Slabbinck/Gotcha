using Gotcha.Core.Data;
using Gotcha.Core.Services.ResultModel;
using Microsoft.EntityFrameworkCore;

using Gotcha.Core.Entities.Logging;
using Gotcha.Core.Entities.Logging.LogEntities;
using Gotcha.Core.Enums;
using Gotcha.Core.Interfaces;

namespace Gotcha.Core.Services.Repository
{
    public class AttackerRepoService : IRepositoryService<Attacker>
    {
        private readonly GotchaDbContext _gotchaDbContext;
        private readonly LogRepoService _logRepoService;

        public AttackerRepoService(GotchaDbContext _context, LogRepoService logRepoService)
        {
            _gotchaDbContext = _context;
            _logRepoService = logRepoService;
        }

        public async Task<ResultModel<List<Attacker>>> GetAllAsync()
        {
            ResultModel<List<Attacker>> resultModel = new ResultModel<List<Attacker>>();

            try
            {
                List<Attacker>? attackers = await _gotchaDbContext.Attackers
                                                                .OrderByDescending(a => a.TimeStamp)
                                                                .ToListAsync();

                if(attackers == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the attackers from the server. Please try again later!",
                                            "Error in GetAllAsync() in AttackerRepoService.");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = attackers;
            }

            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "The server took too long to fetch the attackers. Please try again later!",
                                        "Error in GetAllAsync() in AttackerRepoService.");

                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                        LogSubTypes.Error_DbGet_Exception,
                        "Something went wrong while trying to fetch the attackers. Please try again later!",
                        "Error in GetAllAsync() in AttackerRepoService.");

                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Attacker>> GetByIdAsync(Guid id)
        {
            ResultModel<Attacker> resultModel = new ResultModel<Attacker>();

            try
            {
                Attacker? attacker = await _gotchaDbContext.Attackers
                                                                .OrderByDescending(a => a.TimeStamp)
                                                                .FirstOrDefaultAsync(a => a.Id == id);
                if(attacker == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                        "Couldn't fetch the attacker from the server. Please try again later!",
                        $"Error in GetByIdAsync() in AttackerRepoService. Attacker id: {id}.");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = attacker;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "Server timed out trying to fetch attacker from the server. Please try again later!",
                                        $"Error in GetByIdAsync() in AttackerRepoService. Attacker id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_Exception,
                                        "Something went wrong while trying to fetch attacker. Please try again later!",
                                        $"Error in GetByIdAsync() in AttackerRepoService. Attacker id: {id}.");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }


        public async Task<ResultModel<Attacker>> AddAsync(Attacker attacker)
        {
            ResultModel<Attacker> resultModel = new ResultModel<Attacker>();

            try
            {
                await _gotchaDbContext.Attackers.AddAsync(attacker);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = attacker;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Concurrency_Exception,
                                        "It looks like the attacker details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in AddAsync() in AttackerRepoService.");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_TimeOut_Exception,
                                        "Server timed out trying to add attacker. Please try again later!",
                                        "Error in AddAsync() in AttackerRepoService.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Exception,
                                        "Something went wrong while trying to create this attacker. Please try again!",
                                        "Error in AddAsync() in AttackerRepoService.");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Attacker>> UpdateAsync(Attacker attacker)
        {
            ResultModel<Attacker> resultModel = new ResultModel<Attacker>();

            try
            {
                _gotchaDbContext.Attackers.Update(attacker);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = attacker;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Concurrency_Exception,
                                        "It looks like the attacker details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in UpdateAsync() in AttackerRepoService.");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_TimeOut_Exception,
                                        "Server timed out trying to update attacker. Please try again later!",
                                        "Error in UpdateAsync() in AttackerRepoService.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Exception,
                                        "Something went wrong while trying to update attacker. Please try again later!",
                                        "Error in UpdateAsync() in AttackerRepoService.");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Attacker>> DeleteAsync(Guid id)
        {
            ResultModel<Attacker> resultModel = new ResultModel<Attacker>();

            try
            {
                Attacker? attacker = await _gotchaDbContext.Attackers
                                                    .FirstOrDefaultAsync(l => l.Id == id);

                if (attacker == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't find attacker from the server (to delete it). Please try again later!",
                                            $"Error in DeleteAsync() in AttackerRepoService. Attacker id: {id}.");
                    resultModel.Errors.Add(error);
                    return resultModel;
                }

                resultModel.Data = attacker;

                _gotchaDbContext.Attackers.Remove(attacker);
                await _gotchaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Concurrency_Exception,
                                        "It looks like the attacker's details changed recently. Please try again!",
                                        $"Error in DeleteAsync() in AttackerRepoService. Attacker id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_TimeOut_Exception,
                                        "Server timed out trying to delete attacker. Please try again later!",
                                        $"Error in DeleteAsync() in AttackerRepoService. Attacker id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Exception,
                                        "Something went wrong while trying to remove attacker. Please try again later!",
                                        $"Error in DeleteAsync() in AttackerRepoService. Attacker id: {id}.");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }
    }
}
