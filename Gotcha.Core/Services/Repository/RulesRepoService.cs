using Gotcha.Core.Data;
using Gotcha.Core.Entities;
using Gotcha.Core.Entities.Logging.LogEntities;
using Gotcha.Core.Enums;
using Gotcha.Core.Interfaces;
using Gotcha.Core.Services.ResultModel;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Core.Services.Repository
{
    public class RulesRepoService : IRepositoryService<Rules>
    {
        private readonly GotchaDbContext _gotchaDbContext;
        private readonly LogRepoService _logRepoService;

        public RulesRepoService(GotchaDbContext _context, LogRepoService logRepoService)
        {
            _gotchaDbContext = _context;
            _logRepoService = logRepoService;
        }

        public async Task<ResultModel<List<Rules>>> GetAllAsync()
        {
            ResultModel<List<Rules>> resultModel = new ResultModel<List<Rules>>();

            try
            {
                List<Rules>? rulesList = await _gotchaDbContext.Rules
                                                                .ToListAsync();

                if(rulesList == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the rules from the server. Please try again later!",
                                            "Error in GetAllAsync() in RulesRepoService");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = rulesList;
            }

            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "The server took too long to fetch the rules. Please try again later!",
                                        "Error in GetAllAsync() in RulesRepoService");

                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                        LogSubTypes.Error_DbGet_Exception,
                        "Something went wrong while trying to fetch the rules. Please try again later!",
                        "Error in GetAllAsync() in RulesRepoService");

                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Rules>> GetByIdAsync(Guid id)
        {
            ResultModel<Rules> resultModel = new ResultModel<Rules>();

            try
            {
                Rules? rules = await _gotchaDbContext.Rules
                                                        .FirstOrDefaultAsync(a => a.Id == id);
                if(rules == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                        "Couldn't fetch the rules from the server. Please try again later!",
                        $"Error in GetByIdAsync() in RulesRepoService. Rules id: {id}.");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = rules;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "Server timed out trying to fetch rules from the server. Please try again later!",
                                        $"Error in GetByIdAsync() in RulesRepoService. Rules id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_Exception,
                                        "Something went wrong while trying to fetch rules. Please try again later!",
                                        $"Error in GetByIdAsync() in RulesRepoService. Rules id: {id}.");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }


        public async Task<ResultModel<Rules>> AddAsync(Rules rules)
        {
            ResultModel<Rules> resultModel = new ResultModel<Rules>();

            try
            {
                await _gotchaDbContext.Rules.AddAsync(rules);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = rules;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Concurrency_Exception,
                                        "It looks like the rules details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in AddAsync() in RulesRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_TimeOut_Exception,
                                        "Server timed out trying to add rules. Please try again later!",
                                        "Error in AddAsync() in RulesRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Exception,
                                        "Something went wrong while trying to create this rules. Please try again!",
                                        "Error in AddAsync() in RulesRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Rules>> UpdateAsync(Rules rules)
        {
            ResultModel<Rules> resultModel = new ResultModel<Rules>();

            try
            {
                _gotchaDbContext.Rules.Update(rules);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = rules;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Concurrency_Exception,
                                        "It looks like the rules details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in UpdateAsync() in RulesRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_TimeOut_Exception,
                                        "Server timed out trying to update rules. Please try again later!",
                                        "Error in UpdateAsync() in RulesRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Exception,
                                        "Something went wrong while trying to update rules. Please try again later!",
                                        "Error in UpdateAsync() in RulesRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Rules>> DeleteAsync(Guid id)
        {
            ResultModel<Rules> resultModel = new ResultModel<Rules>();

            try
            {
                Rules? rules = await _gotchaDbContext.Rules
                                                        .FirstOrDefaultAsync(r => r.Id == id);

                if (rules == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't find rules from the server (to delete it). Please try again later!",
                                            $"Error in DeleteAsync() in RulesRepoService. Rules id: {id}.");
                    resultModel.Errors.Add(error);
                    return resultModel;
                }

                resultModel.Data = rules;

                _gotchaDbContext.Rules.Remove(rules);
                await _gotchaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Concurrency_Exception,
                                        "It looks like the rules' details changed recently. Please try again!",
                                        $"Error in DeleteAsync() in RulesRepoService. Rules id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_TimeOut_Exception,
                                        "Server timed out trying to delete rules. Please try again later!",
                                        "Error in DeleteAsync() in RulesRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Exception,
                                        "Something went wrong while trying to remove rules. Please try again later!",
                                        "Error in DeleteAsync() in RulesRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }
    }
}
