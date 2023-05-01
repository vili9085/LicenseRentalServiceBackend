namespace LicenseRentalAPI.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        /// <summary>
        /// Get name from identity. "No name" as default name for testing if name is not provided
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRenter(this HttpContext context)
        {
            return context?.User?.Identity?.Name ?? "No name";
        }
    }
}
