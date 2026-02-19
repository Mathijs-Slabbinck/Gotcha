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
    public class PlayerRepoService : IRepositoryService<Player>
    {
        private readonly GotchaDbContext _gotchaDbContext;
        private readonly LogRepoService _logRepoService;

        public PlayerRepoService(GotchaDbContext _context, LogRepoService logRepoService)
        {
            _gotchaDbContext = _context;
            _logRepoService = logRepoService;
        }

        [HttpGet]
        public async Task<ResultModel<List<Player>>> GetAllAsync()
        {
            ResultModel<List<Player>> resultModel = new ResultModel<List<Player>>();

            try
            {
                List<Player>? players = await _gotchaDbContext.Players
                                                            .ToListAsync();

                if(players == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the players from the server. Please try again later!",
                                            "Error in GetAllAsync() in PlayerRepoService");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = players;
            }

            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "The server took too long to fetch the players. Please try again later!",
                                        "Error in GetAllAsync() in PlayerRepoService");

                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                        LogSubTypes.Error_DbGet_Exception,
                        "Something went wrong while trying to fetch the players. Please try again later!",
                        "Error in GetAllAsync() in PlayerRepoService");

                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpGet]
        public async Task<ResultModel<Player>> GetByIdAsync(Guid id)
        {
            ResultModel<Player> resultModel = new ResultModel<Player>();

            try
            {
                Player? player = await _gotchaDbContext.Players
                                                        .FirstOrDefaultAsync(a => a.Id == id);
                if(player == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                        "Couldn't fetch the player from the server. Please try again later!",
                        $"Error in GetByIdAsync() in PlayerRepoService. Player id: {id}.");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = player;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "Server timed out trying to fetch player from the server. Please try again later!",
                                        $"Error in GetByIdAsync() in PlayerRepoService. Player id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_Exception,
                                        "Something went wrong while trying to fetch player. Please try again later!",
                                        $"Error in GetByIdAsync() in PlayerRepoService. Player id: {id}.");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }


        [HttpPost]
        public async Task<ResultModel<Player>> AddAsync(Player player)
        {
            ResultModel<Player> resultModel = new ResultModel<Player>();

            try
            {
                await _gotchaDbContext.Players.AddAsync(player);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = player;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Concurrency_Exception,
                                        "It looks like the player details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in AddAsync() in PlayerRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_TimeOut_Exception,
                                        "Server timed out trying to add player. Please try again later!",
                                        "Error in AddAsync() in PlayerRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Exception,
                                        "Something went wrong while trying to create this player. Please try again!",
                                        "Error in AddAsync() in PlayerRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpPost]
        public async Task<ResultModel<Player>> UpdateAsync(Player player)
        {
            ResultModel<Player> resultModel = new ResultModel<Player>();

            try
            {
                _gotchaDbContext.Players.Update(player);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = player;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Concurrency_Exception,
                                        "It looks like the player details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in UpdateAsync() in PlayerRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_TimeOut_Exception,
                                        "Server timed out trying to update player. Please try again later!",
                                        "Error in UpdateAsync() in PlayerRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Exception,
                                        "Something went wrong while trying to update player. Please try again later!",
                                        "Error in UpdateAsync() in PlayerRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        [HttpPost]
        public async Task<ResultModel<Player>> DeleteAsync(Guid id)
        {
            ResultModel<Player> resultModel = new ResultModel<Player>();

            try
            {
                Player? player = await _gotchaDbContext.Players
                                                    .FirstOrDefaultAsync(l => l.Id == id);

                if (player == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't find player from the server (to delete it). Please try again later!",
                                            $"Error in DeleteAsync() in PlayerRepoService. Player id: {id}.");
                    resultModel.Errors.Add(error);
                    return resultModel;
                }

                resultModel.Data = player;

                _gotchaDbContext.Players.Remove(player);
                await _gotchaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Concurrency_Exception,
                                        "It looks like the player's details changed recently. Please try again!",
                                        $"Error in DeleteAsync() in PlayerRepoService. Player id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_TimeOut_Exception,
                                        "Server timed out trying to delete player. Please try again later!",
                                        "Error in DeleteAsync() in PlayerRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Exception,
                                        "Something went wrong while trying to remove player. Please try again later!",
                                        "Error in DeleteAsync() in PlayerRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }
    }
}
