using LicenseRentalAPI.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using System.Net;

namespace LicenseRentalAPI.Extensions
{
    public static class CosmosContainerExtensions
    {
        /// <summary>
        /// Returns true if database contains object with id
        /// </summary>
        /// <param name="container"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> ContainsIdAsync(this Container container, string id)
        {
            try
            {
                await container.ReadItemAsync<License>(id, new PartitionKey(id));
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Return first object from database matching predicate
        /// </summary>
        /// <param name="container"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<License> FirstOrDefaultAsync(this Container container, Expression<Func<License, bool>> predicate)
        {
            using (FeedIterator<License> setIterator = container.GetItemLinqQueryable<License>().Where(predicate).ToFeedIterator())
            {
                if (setIterator.HasMoreResults)
                {
                    return (await setIterator.ReadNextAsync()).FirstOrDefault();
                }
            }
            return null;
        }
    }
}
