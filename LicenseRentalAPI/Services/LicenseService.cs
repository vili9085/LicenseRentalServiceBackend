using LicenseRentalAPI.Extensions;
using LicenseRentalAPI.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace LicenseRentalAPI.Services
{
    public class LicenseService
    {
        private readonly Container _cosmosContainer;

        public LicenseService(Container container) 
        {
            _cosmosContainer = container;
        }

        /// <summary>
        /// Add license as available to database if id does not already exist
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns></returns>
        public async Task<bool> AddLicense(string licenseId)
        {
            if (!await _cosmosContainer.ContainsIdAsync(licenseId))
            {
                var license = new License
                {
                    id = licenseId,
                    IsAvailable = true
                };
                await _cosmosContainer.CreateItemAsync(license);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Rent license if one is available and set it as not available
        /// </summary>
        /// <param name="renter"></param>
        /// <returns></returns>
        public async Task<License> RentLicense(string renter)
        {
            var license = await _cosmosContainer.FirstOrDefaultAsync(x => x.IsAvailable);
            if (license != null)
            {
                license.IsAvailable = false;
                license.Renter = renter;
                license.ExpirationTime = DateTime.UtcNow.AddSeconds(15);
                await _cosmosContainer.UpsertItemAsync(license);
                return license;
            }

            return null;
        }

        /// <summary>
        /// Get all licenses from database by iterating through data
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<License>> GetAllLicenses()
        {
            var licenses = new List<License>();
            using (FeedIterator<License> setIterator = _cosmosContainer.GetItemLinqQueryable<License>().ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        licenses.Add(item);
                    }
                }
            }

            return licenses;
        }
    }
}
