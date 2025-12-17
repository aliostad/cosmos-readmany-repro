using Microsoft.Azure.Cosmos;
using System;

namespace CosmosReadManyApp;

class Program
{
    // Replace these with your actual Cosmos DB connection details
    private const string CosmosEndpoint = "https://whatisyourname.documents.azure.com:443/";
    private const string CosmosKey = "*****************************";
    private const string DatabaseName = "hcp";
    private const string ContainerName = "hcp";
    private const bool CreateItems = false; // Set to true to create items

    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Cosmos DB ReadMany Demo with Hierarchical Partition Keys...\n");
        var random = new Random();

        // Initialize Cosmos Client
        using var cosmosClient = new CosmosClient(CosmosEndpoint, CosmosKey);

        try
        {
            // Create database if it doesn't exist
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseName);
            Console.WriteLine($"Database '{DatabaseName}' created or already exists.\n");

            // Define hierarchical partition key paths
            var partitionKeyPaths = new List<string> { "/KeyValue", "/KeyName" };

            // Create container with hierarchical partition keys if it doesn't exist
            var containerProperties = new ContainerProperties
            {
                Id = ContainerName,
                PartitionKeyPaths = partitionKeyPaths
            };

            var container = await database.Database.CreateContainerIfNotExistsAsync(
                containerProperties,
                throughput: 400
            );
            Console.WriteLine($"Container '{ContainerName}' created with HPK: {string.Join(", ", partitionKeyPaths)}\n");

            var createdItems = new List<CosmosItem>();

            if (CreateItems)
            {
                // Create 10000 items
                Console.WriteLine("Creating 10000 items...");
                
                for (int i = 5; i <= 10000; i++)
                {
                    var item = new CosmosItem
                    {
                        Id = "item-chippa",
                        KeyValue = $"value-{i}", // Will have value-1, value-2, or value-3
                        KeyName = $"name-{i}",   // Will have name-1 or name-2
                        Data = $"This is item number {i}",
                        PermId = random.Next(10000000, 999999999).ToString()
                    };

                    var partitionKey = item.GetPartitionKey();

                    var response = await container.Container.CreateItemAsync(item, partitionKey);
                    createdItems.Add(item);
                    if (i % 500 == 0)
                    {
                        Console.WriteLine($"  Created {i} items so far...");
                    }
                }

                Console.WriteLine($"\nSuccessfully created {createdItems.Count} items.\n");
            }
            else
            {
                Console.WriteLine("Skipping item creation (CreateItems flag is false).\n");
                
                // For ReadMany demo, manually specify some existing items
                // Replace these with actual IDs and partition keys from your container
                createdItems.Add(new CosmosItem 
                { 
                    Id = "item-chippa", 
                    KeyValue = "value-2000", 
                    KeyName = "name-2000" 
                });
                createdItems.Add(new CosmosItem 
                { 
                    Id = "item-chippa", 
                    KeyValue = "value-3000", 
                    KeyName = "name-3000" 
                });
                createdItems.Add(new CosmosItem 
                { 
                    Id = "item-chippa", 
                    KeyValue = "value-5000", 
                    KeyName = "name-5000" 
                });
        
            }

            // Use ReadMany to read 10 specific items
            Console.WriteLine("Using ReadMany to retrieve 10 items...");
            
            // Select first 10 items to read
            var itemsToRead = createdItems.Take(10).ToList();
            
            var readManyItems = itemsToRead.Select(item => 
                (item.Id, item.GetPartitionKey())
            ).ToList();

            foreach (var (id, pk) in readManyItems)
            {
                Console.WriteLine($"  - Preparing to read Item ID: {id} with PK: {pk.ToString()}");
            }

            var requestOptions = new ReadManyRequestOptions
            {
                ConsistencyLevel = ConsistencyLevel.Strong
            };

            var readManyResponse = await container.Container.ReadManyItemsAsync<CosmosItem>(readManyItems, requestOptions);

            Console.WriteLine($"\nReadMany retrieved {readManyResponse.Count} items (with Strong Consistency):");
            foreach (var item in readManyResponse)
            {
                Console.WriteLine($"  - ID: {item.Id}, KeyValue: {item.KeyValue}, KeyName: {item.KeyName}, Data: {item.Data}");
            }

            Console.WriteLine($"\nRequest Charge for ReadMany: {readManyResponse.RequestCharge} RUs");
            Console.WriteLine("\nDemo completed successfully!");
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Cosmos DB Error: {ex.StatusCode} - {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
