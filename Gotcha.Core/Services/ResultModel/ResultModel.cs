// ResultModel.cs
using Gotcha.Core.Entities.Logging.LogEntities;
using Gotcha.Core.Enums;
using Gotcha.Core.Services.ResultModel.Base;

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
                ValidateData(value);
                _data = value;
            }
        }

        private void ValidateData(T? value)
        {
            if (value is System.Collections.IEnumerable enumerable && value is not string)
            {
                var enumerator = enumerable.GetEnumerator();
                try
                {
                    if (!enumerator.MoveNext())
                    {
                        Warning warning = new Warning(
                            LogSubTypes.Warning_DbGet_Empty,
                            "Data in ResultModel is an empty list",
                            $"Requested Data: ({GetFriendlyTypeName(typeof(T))})");
                        Warnings.Add(warning);
                    }
                }
                finally
                {
                    // Dispose enumerator if it's IDisposable
                    (enumerator as IDisposable)?.Dispose();
                }
            }
        }

        private string GetFriendlyTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetFriendlyTypeName));
                return $"{type.Name.Split('`')[0]}<{genericArgs}>";
            }
            return type.Name;
        }
    }
}