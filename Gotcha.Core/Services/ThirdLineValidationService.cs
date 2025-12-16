using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Services
{
    public class ThirdLineValidationService
    {
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsInputClean(string input)
        {
            if(input == null ||
               input == "null" ||
               input == "void" ||
               input.Contains("--") ||
               input.ToUpper().Contains("DROP TABLE") ||
               input.ToUpper().Contains("SELECT * FROM") ||
               input.ToUpper().Contains("INSERT INTO") ||
               input.ToUpper().Contains("DELETE FROM") ||
               input.ToUpper().Contains("UPDATE SET") ||
               input.ToUpper().Contains("ALTER TABLE") ||
               input.ToUpper().Contains("CREATE TABLE") ||
               input.ToUpper().Contains("TRUNCATE TABLE") ||
               input.ToUpper().Contains("EXECUTE") ||
               input.ToUpper().Contains("EXEC") ||
               input.ToUpper().Contains("UNION ALL") ||
               input.Contains(";") ||
               input.Contains("{") ||
               input.Contains("}") ||
               input.Contains("<") ||
               input.Contains(">") ||
               input.Contains("$") ||
               input.Contains("%")
               )
               {
                    return false;
               }
            return true;
        }
    }
}
