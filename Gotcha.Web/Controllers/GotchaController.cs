using Gotcha.Core.Entities.Logging.Models;
using Gotcha.Core.Enums;
using Gotcha.Core.Services.Repository;
using Gotcha.Core.Services.ResultModel;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers;

public class GotchaController : Controller
{
    private readonly AttackerRepoService _attackerRepoService;
    private readonly LogRepoService _logRepoService;

    public GotchaController(AttackerRepoService attackerRepoService, LogRepoService logRepoService)
    {
        _attackerRepoService = attackerRepoService;
        _logRepoService = logRepoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Read TempData set by the controller that caught the suspicious input
        string? originalPath = TempData["GotchaOriginalPath"] as string;
        string? invalidInput = TempData["GotchaInvalidInput"] as string;

        bool isDirectNavigation = string.IsNullOrEmpty(originalPath);

        // Build the Attacker entity from HttpContext
        Attacker attacker = new Attacker
        {
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
            Referer = HttpContext.Request.Headers.Referer.ToString(),
            Path = isDirectNavigation ? "/Gotcha" : originalPath!,
            InvalidInput = invalidInput
        };

        // Save the attacker — silent failure, the page always renders
        ResultModel<Attacker> attackerResult = await _attackerRepoService.AddAsync(attacker);

        if (attackerResult.Success && attackerResult.Data != null)
        {
            LogSubTypes logSubType;
            string message;

            if (isDirectNavigation)
            {
                logSubType = LogSubTypes.HackAttempt_DirectlyNavigated;
                message = "Someone navigated directly to /Gotcha";
            }
            else
            {
                logSubType = LogSubTypes.HackAttempt_Any;
                message = $"Suspicious input caught from {originalPath}";
            }

            Log log = new Log
            {
                Id = Guid.NewGuid(),
                LogType = LogTypes.HackAttempt,
                LogSubType = logSubType,
                Message = message,
                AttackerId = attackerResult.Data.Id,
                Attacker = attackerResult.Data,
                TimeStamp = attackerResult.Data.TimeStamp,
                LogGroupId = Guid.NewGuid(),
            };

            await _logRepoService.AddAsync(log);
        }

        return View();
    }

    [HttpGet]
    public IActionResult TakeMeBack()
    {
        TempData["ContactReason"] = "Other";
        TempData["ContactMessage"] = "I am so sorry for trying to break your amazing project. I promise I won't do it again!";

        return RedirectToAction("Index", "Contact");
    }
}
