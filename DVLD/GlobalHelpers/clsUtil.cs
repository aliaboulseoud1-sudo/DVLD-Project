using System;

namespace DVLD.GlobalHelpers
{
    public static class clsUtil
    {
        public static string GenerateGUID()
        {
            return Guid.NewGuid().ToString();
        }


    }
}
