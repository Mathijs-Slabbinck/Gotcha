using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Services.ResultModel.Base
{
    public class BaseResult
    {
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public bool hasErrors => Errors.Any();
        private bool hasWarnings => Warnings.Any();

        // no errors, warnings are allowed by logged
        public bool Succes
        {
            get
            {
                foreach (string warning in Warnings)
                {
                    Console.WriteLine($"WARNING: {warning}");
                }
                return hasErrors;
            }
        }

        // no errors, no warnings
        public bool FullSucces => !hasErrors && !hasWarnings;
    }
}
