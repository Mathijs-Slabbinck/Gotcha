using Gotcha.Core.Entities.Logging.LogEntities;
using Gotcha.Core.Enums;
using Gotcha.Core.Services.ResultModel.Base;
using System.Collections;

namespace Gotcha.Core.Services.ResultModel
{
    public class ResultModel<T> : BaseResultModel
    {
        private T? _data;

        public T? Data
        {
            get { return _data; }
            set
            {
                CheckIfEmptyList(value);
                _data = value;
            }
        }

        private void CheckIfEmptyList(T? value)
        {
            // Skip if value is null or a plain string
            if (value == null || value is string)
                return;

            // If the data is a list/collection, warn when it's empty
            if (value is ICollection collection && collection.Count == 0)
            {
                Warning warning = new Warning(
                    LogSubTypes.Warning_DbGet_Empty,
                    "Data in ResultModel is an empty list",
                    $"Requested Data: {typeof(T).Name}");
                Warnings.Add(warning);
            }
        }
    }
}
