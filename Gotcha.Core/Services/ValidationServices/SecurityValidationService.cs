using System.Text.RegularExpressions;

namespace Gotcha.Core.Services.ValidationServices
{
    public static class SecurityValidationService
    {
        #region Suspicious Input Detection
        // Detection patterns — strings we scan for in user input to catch
        // SQL injection, XSS, path traversal, and other attack attempts.
        // These are NOT executed — they are compared against user input.

        private static readonly string[] SqlInjectionPatterns = new[]
        {
            "--", ";--", "/*", "*/", "xp_", "sp_",
        };

        private static readonly string[] SqlInjectionKeywords = new[]
        {
            "SELECT", "INSERT", "UPDATE", "DELETE", "DROP",
            "UNION", "ALTER", "CREATE", "TRUNCATE",
            "INFORMATION_SCHEMA", "SYSOBJECTS", "SYSCOLUMNS",
            "TABLE", "FROM", "WHERE",
            "OR 1=1", "OR 1 = 1", "OR '1'='1'",
            "AND 1=1", "AND 1 = 1",
            "WAITFOR DELAY", "HAVING", "GROUP BY", "ORDER BY",
            "EXEC(", "EXECUTE(",
            "BENCHMARK(", "SLEEP(", "CHAR(",
            "CONCAT(", "CAST(", "CONVERT(",
        };

        private static readonly string[] XssPatterns = new[]
        {
            "<script", "</script",
            "javascript:", "vbscript:",
            "onerror=", "onload=", "onclick=", "onmouseover=",
            "onfocus=", "onblur=", "onsubmit=", "onchange=",
            "oninput=", "onkeydown=", "onkeyup=", "onkeypress=",
            "<iframe", "<object", "<embed", "<form",
            "<img", "<svg", "<math", "<link", "<meta", "<base",
            "document" + ".cookie", "document" + ".write", "window" + ".location",
            "expression(",
        };

        private static readonly string[] OtherAttackPatterns = new[]
        {
            "../", "..\\",
            "/etc/passwd", "/etc/shadow",
            "| ls", "| dir", "| cat", "| whoami",
            "; ls", "; dir", "; cat", "; whoami",
            "${", "#{", "{{", "%{",
            "\x00", "%00",
        };

        public static string? FindSuspiciousInput(params string?[] inputs)
        {
            foreach (string? input in inputs)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                string lowerInput = input.ToLowerInvariant();

                foreach (string pattern in SqlInjectionPatterns)
                {
                    if (lowerInput.Contains(pattern.ToLowerInvariant()))
                    {
                        return input;
                    }
                }

                foreach (string keyword in SqlInjectionKeywords)
                {
                    if (ContainsKeyword(lowerInput, keyword.ToLowerInvariant()))
                    {
                        return input;
                    }
                }

                foreach (string pattern in XssPatterns)
                {
                    if (lowerInput.Contains(pattern.ToLowerInvariant()))
                    {
                        return input;
                    }
                }

                foreach (string pattern in OtherAttackPatterns)
                {
                    if (lowerInput.Contains(pattern.ToLowerInvariant()))
                    {
                        return input;
                    }
                }

                // Check URL-encoded variants (%3C = <, %27 = ', etc.)
                if (Regex.IsMatch(input, @"%[0-9a-fA-F]{2}"))
                {
                    string decoded = Uri.UnescapeDataString(input);

                    if (decoded != input)
                    {
                        string? decodedResult = FindSuspiciousInput(decoded);

                        if (decodedResult != null)
                        {
                            return input;
                        }
                    }
                }
            }

            return null;
        }

        private static bool ContainsKeyword(string lowerInput, string lowerKeyword)
        {
            int index = lowerInput.IndexOf(lowerKeyword);

            while (index >= 0)
            {
                bool startBound = index == 0 || !char.IsLetterOrDigit(lowerInput[index - 1]);
                int endIndex = index + lowerKeyword.Length;
                bool endBound = endIndex >= lowerInput.Length || !char.IsLetterOrDigit(lowerInput[endIndex]);

                if (startBound && endBound)
                {
                    return true;
                }

                index = lowerInput.IndexOf(lowerKeyword, index + 1);
            }

            return false;
        }
        #endregion

        public static bool IsValidIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            string[] ipAsArray = ip.Split(".");

            if (ipAsArray.Length != 4)
                return false;

            foreach (string ipBlock in ipAsArray)
            {
                if (string.IsNullOrEmpty(ipBlock) || ipBlock.Length > 3 || !int.TryParse(ipBlock, out int num) || num < 0 || num > 255)
                    return false;

                // Reject leading zeros unless the octet is exactly "0"
                if (ipBlock.Length > 1 && ipBlock.StartsWith("0"))
                    return false;
            }

            return true;
        }
    }
}
