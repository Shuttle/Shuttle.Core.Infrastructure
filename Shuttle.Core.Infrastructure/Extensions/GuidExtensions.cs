using System;

namespace Shuttle.Core.Infrastructure
{
    public static class GuidExtensions
    {
        public static bool TryParse(string guid, out Guid result)
        {
            try
            {
                result = new Guid(guid);

                return true;
            }
            catch
            {
                result = Guid.Empty;
            }

            return false;
        }
    }
}
