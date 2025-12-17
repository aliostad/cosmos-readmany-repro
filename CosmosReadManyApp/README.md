# Cosmos DB ReadMany with Hierarchical Partition Keys Demo

This is a .NET 8 console application that demonstrates how to work with Azure Cosmos DB using Hierarchical Partition Keys (HPK) and the ReadMany operation.

## Features

- Creates a Cosmos DB database and container with hierarchical partition keys
- Uses `/keyValue` and `/keyName` as the hierarchical partition key paths
- Inserts 10 items into the container
- Demonstrates the `ReadMany` operation to efficiently retrieve 2 specific items

## Prerequisites

- .NET 8 SDK
- Azure Cosmos DB account (or use the Cosmos DB Emulator)

## Configuration

Before running the application, update the connection details in `Program.cs`:

```csharp
private const string CosmosEndpoint = "YOUR_COSMOS_ENDPOINT";
private const string CosmosKey = "YOUR_COSMOS_KEY";
```

### Using Cosmos DB Emulator

If you're using the Cosmos DB Emulator locally:

```csharp
private const string CosmosEndpoint = "https://localhost:8081";
private const string CosmosKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
```

## Running the Application

```bash
cd CosmosReadManyApp
dotnet run
```

## What the Application Does

1. **Creates Database**: Creates a database named `TestDatabase` if it doesn't exist
2. **Creates Container**: Creates a container named `TestContainer` with hierarchical partition keys
3. **Inserts 10 Items**: Creates 10 items with different combinations of partition key values
4. **ReadMany Operation**: Uses the ReadMany API to efficiently retrieve 2 specific items by their IDs and partition keys

## Hierarchical Partition Keys

This application uses a two-level hierarchical partition key:
- **Level 1**: `/keyValue` (e.g., "value-1", "value-2", "value-3")
- **Level 2**: `/keyName` (e.g., "name-1", "name-2")

This allows for better data distribution and more efficient queries across related data.

## ReadMany Benefits

The `ReadMany` operation is optimized for retrieving multiple items by their IDs and partition keys in a single request, which is more efficient than making multiple point reads.
