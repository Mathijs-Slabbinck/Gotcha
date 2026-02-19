using Gotcha.Core.Data;
using Gotcha.Core.Entities;
using Gotcha.Core.Entities.Logging.LogEntities;
using Gotcha.Core.Enums;
using Gotcha.Core.Interfaces;
using Gotcha.Core.Services.ResultModel;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.Core.Services.Repository
{
    public class GameRepoService : IRepositoryService<Game>
    {
        private readonly GotchaDbContext _gotchaDbContext;
        private readonly LogRepoService _logRepoService;

        public GameRepoService(GotchaDbContext _context, LogRepoService logRepoService)
        {
            _gotchaDbContext = _context;
            _logRepoService = logRepoService;
        }

        public async Task<ResultModel<List<Game>>> GetAllAsync()
        {
            ResultModel<List<Game>> resultModel = new ResultModel<List<Game>>();

            try
            {
                List<Game>? games = await _gotchaDbContext.Games
                                                            .ToListAsync();

                if(games == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't fetch the games from the server. Please try again later!",
                                            "Error in GetAllAsync() in GameRepoService");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = games;
            }

            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "The server took too long to fetch the games. Please try again later!",
                                        "Error in GetAllAsync() in GameRepoService");

                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                        LogSubTypes.Error_DbGet_Exception,
                        "Something went wrong while trying to fetch the games. Please try again later!",
                        "Error in GetAllAsync() in GameRepoService");

                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Game>> GetByIdAsync(Guid id)
        {
            ResultModel<Game> resultModel = new ResultModel<Game>();

            try
            {
                Game? game = await _gotchaDbContext.Games
                                                        .FirstOrDefaultAsync(a => a.Id == id);
                if(game == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                        "Couldn't fetch the game from the server. Please try again later!",
                        $"Error in GetByIdAsync() in GameRepoService. Game id: {id}.");
                    resultModel.Errors.Add(error);
                }

                resultModel.Data = game;
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_TimeOut_Exception,
                                        "Server timed out trying to fetch game from the server. Please try again later!",
                                        $"Error in GetByIdAsync() in GameRepoService. Game id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbGet_Exception,
                                        "Something went wrong while trying to fetch game. Please try again later!",
                                        $"Error in GetByIdAsync() in GameRepoService. Game id: {id}.");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }


        public async Task<ResultModel<Game>> AddAsync(Game game)
        {
            ResultModel<Game> resultModel = new ResultModel<Game>();

            try
            {
                await _gotchaDbContext.Games.AddAsync(game);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = game;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Concurrency_Exception,
                                        "It looks like the game details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in AddAsync() in GameRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_TimeOut_Exception,
                                        "Server timed out trying to add game. Please try again later!",
                                        "Error in AddAsync() in GameRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbAdd_Exception,
                                        "Something went wrong while trying to create this game. Please try again!",
                                        "Error in AddAsync() in GameRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Game>> UpdateAsync(Game game)
        {
            ResultModel<Game> resultModel = new ResultModel<Game>();

            try
            {
                _gotchaDbContext.Games.Update(game);
                await _gotchaDbContext.SaveChangesAsync();
                resultModel.Data = game;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Concurrency_Exception,
                                        "It looks like the game details changed recently. To avoid overwriting other changes, please try again!",
                                        "Error in UpdateAsync() in GameRepoService");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_TimeOut_Exception,
                                        "Server timed out trying to update game. Please try again later!",
                                        "Error in UpdateAsync() in GameRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbUpdate_Exception,
                                        "Something went wrong while trying to update game. Please try again later!",
                                        "Error in UpdateAsync() in GameRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }

        public async Task<ResultModel<Game>> DeleteAsync(Guid id)
        {
            ResultModel<Game> resultModel = new ResultModel<Game>();

            try
            {
                Game? game = await _gotchaDbContext.Games
                                                    .FirstOrDefaultAsync(l => l.Id == id);

                if (game == null)
                {
                    Error error = new Error(LogSubTypes.Error_DbGet_Null,
                                            "Couldn't find game from the server (to delete it). Please try again later!",
                                            $"Error in DeleteAsync() in GameRepoService. Game id: {id}.");
                    resultModel.Errors.Add(error);
                    return resultModel;
                }

                resultModel.Data = game;

                _gotchaDbContext.Games.Remove(game);
                await _gotchaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Concurrency_Exception,
                                        "It looks like the game's details changed recently. Please try again!",
                                        $"Error in DeleteAsync() in GameRepoService. Game id: {id}.");
                resultModel.Errors.Add(error);
            }
            catch (TimeoutException ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_TimeOut_Exception,
                                        "Server timed out trying to delete game. Please try again later!",
                                        "Error in DeleteAsync() in GameRepoService");
                resultModel.Errors.Add(error);
            }
            catch (Exception ex)
            {
                Error error = new Error(ex,
                                        LogSubTypes.Error_DbRemove_Exception,
                                        "Something went wrong while trying to remove game. Please try again later!",
                                        "Error in DeleteAsync() in GameRepoService");
                resultModel.Errors.Add(error);
            }

            await _logRepoService.HandleAddingResultModelToLoggerAsync(resultModel, Guid.NewGuid());
            return resultModel;
        }
    }
}
