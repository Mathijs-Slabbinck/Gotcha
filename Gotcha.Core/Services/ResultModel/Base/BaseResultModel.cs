using Gotcha.Core.Entities.Logging.LogEntities;

namespace Gotcha.Core.Services.ResultModel.Base
{
    public abstract class BaseResultModel
    {
        public List<Error> Errors { get; set; } = new List<Error>();
        public bool HasErrors => Errors.Any();
        public List<Warning> Warnings { get; set; } = new List<Warning>();
        public bool HasWarnings => Warnings.Any();
        public bool Success => !HasErrors;

        // no errors, no warnings
        public bool FullSuccess => !HasErrors && !HasWarnings;
    }
}
