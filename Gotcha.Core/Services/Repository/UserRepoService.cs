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
    public class UserRepoService : IRepositoryService<User>
    {
        private readonly GotchaDbContext _gotchaDbContext;
        private readonly LogRepoService _logRepoService;

        public UserRepoService(GotchaDbContext _context, LogRepoService logRepoService)
        {
            _gotchaDbContext = _context;
            _logRepoService = logRepoService;
        }

        [HttpGet]
        public async Task<ResultModel<List<User>>> GetAllAsync()
        {
            ResultModel<List<User>> resultModel = new ResultModel<List<User>>();

            try
            {
                List<User>? users = await _gotchaDbContext.Users
                                                            .ToListAsync();

                if(users == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the users from the server. Please try again later!",
                                            "Error in GetAllAsync() in UserRepoService");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = users;
            }

            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "The server took too long to fetch the users. Please try again later!",
                                        "Error in GetAllAsync() in UserRepoService");

                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                        LogSubTypes.Error_DbGet_Exception,
                        "Something went wrong while trying to fetch the users. Please try again later!",
                        "Error in GetAllAsync() in UserRepoService");

                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpGet]
        public async Task<ResultModel<User>> GetByIdAsync(Guid id)
        {
            ResultModel<User> resultModel = new ResultModel<User>();

            try
            {
                User? user = await _gotchaDbContext.Users
                                                        .FirstOrDefaultAsync(a => a.Id == id);
                if(user == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                        $"Couldn't fetch the user with id {id} from the server. Please try again later!",
                        "Error in GetByIdAsync() in UserRepoService");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = user;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        $"Server timed out trying to fetch user with id {id} from the server. Please try again later!",
                                        "Error in GetByIdAsync() in UserRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_Exception,
                                        $"Something went wrong while trying to fetch user with id {id}. Please try again later!",
                                        "Error in GetByIdAsync() in UserRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }


        [HttpPost]
        public async Task<ResultModel<User>> AddAsync(User user)
        {
            ResultModel<User> resultModel = new ResultModel<User>();

            try
            {
                await _gotchaDbContext.Users.AddAsync(user);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = user;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Concurrency_Exception,
                                        "It looks like the user details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in AddAsync() in UserRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_TimeOut_Exception,
                                        "Server timed out trying to add user. Please try again later!",
                                        "Error in AddAsync() in UserRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Exception,
                                        "Something went wrong while trying to create this user. Please try again!",
                                        "Error in AddAsync() in UserRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpPost]
        public async Task<ResultModel<User>> UpdateAsync(User user)
        {
            ResultModel<User> resultModel = new ResultModel<User>();

            try
            {
                _gotchaDbContext.Users.Update(user);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = user;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Concurrency_Exception,
                                        "It looks like the user details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in UpdateAsync() in UserRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_TimeOut_Exception,
                                        "Server timed out trying to update user. Please try again later!",
                                        "Error in UpdateAsync() in UserRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Exception,
                                        "Something went wrong while trying to update user. Please try again later!",
                                        "Error in UpdateAsync() in UserRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpPost]
        public async Task<ResultModel<User>> DeleteAsync(Guid id)
        {
            ResultModel<User> resultModel = new ResultModel<User>();

            try
            {
                User? user = await _gotchaDbContext.Users
                                                    .FirstOrDefaultAsync(l => l.Id == id);

                if (user == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            $"Couldn't find user with id {id} from the server (to delete it). Please try again later!",
                                            "Error in DeleteAsync() in LogRepoService");
                    resultModel.Errors.Add(error);
                    return resultModel;
                }

                resultModel.Data = user;

                _gotchaDbContext.Users.Remove(user);
                await _gotchaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Concurrency_Exception,
                                        $"It looks like the user with id {id}'s details changed recently. Please try again!",
                                        "Error in DeleteAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_TimeOut_Exception,
                                        "Server timed out trying to delete user. Please try again later!",
                                        "Error in DeleteAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Exception,
                                        "Something went wrong while trying to remove user. Please try again later!",
                                        "Error in DeleteAsync() in LogRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }
    }
}
