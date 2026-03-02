namespace Gotcha.Core.Services.ValidationServices
{
    public static class SecurityValidationService
    {
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
