using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;
using System;
using System.Threading.Tasks;

namespace ConsoleApp13
{
    public class DeleteStatus
    {
        public int Deleted { get; set; }
        public bool Continuation { get; set; }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cosmosClient = new CosmosClient("EnterConnectionStringHere",
                new CosmosClientOptions
                {
                    ApplicationName = "Bulk Delete",
                    AllowBulkExecution = true
                });
            Container _container = cosmosClient.GetContainer("DbName","ContainerName");
            await BulkDelete(_container);
        }

        private static async Task BulkDelete(Container container)
        {
            bool resume = true;
            do
            {
                string query = "Enter Query";
                try
                {
                    StoredProcedureExecuteResponse<DeleteStatus> result = await container.Scripts.ExecuteStoredProcedureAsync<DeleteStatus>("EnterStoredProcedureName", new PartitionKey("EnterPartitionKey"), new dynamic[] { query });
                    await Console.Out.WriteLineAsync($"Batch Delete Completed.\tDeleted: {result.Resource.Deleted}\tContinue: {result.Resource.Continuation}");
                    resume = result.Resource.Continuation;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            while (resume);
        }
    }
}
