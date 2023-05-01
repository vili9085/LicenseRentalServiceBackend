using System;
using System.Linq;
using System.Threading.Tasks;
using LicenseFunctionApp.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs;

namespace LicenseFunctionApp
{
    public class RentalResetFunction
    {
        private readonly Container _cosmosContainer;

        public RentalResetFunction(Container container)
        {
            _cosmosContainer = container;
        }

        /// <summary>
        /// Time triggered function that resets the availability of Licenses when the expiration time passes.
        /// Would preferably be event triggered instead, for example triggered by a scheduled service bus message,
        /// or a feed change event that the function orchestrates to be handled after 15 seconds,
        /// or maybe a stored procedure on the database
        /// </summary>
        /// <param name="myTimer"></param>
        /// <returns></returns>
        [FunctionName("RentalResetFunction")]
        public async Task Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer)
        {
            using (FeedIterator<License> setIterator = _cosmosContainer.GetItemLinqQueryable<License>().Where(x => !x.IsAvailable).ToFeedIterator())
            {
                while (setIterator.HasMoreResults)
                {
                    foreach (var license in await setIterator.ReadNextAsync())
                    {
                        if (license.ExpirationTime <= DateTime.UtcNow)
                        {
                            license.ExpirationTime = null;
                            license.IsAvailable = true;
                            license.Renter = null;
                            await _cosmosContainer.UpsertItemAsync(license);
                            Console.WriteLine($"License expired. Id = {license.id}, Time = {DateTime.UtcNow}");
                        }
                    }
                }
            }

            
        }
    }
}
