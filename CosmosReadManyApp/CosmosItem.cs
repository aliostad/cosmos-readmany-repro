using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CosmosReadManyApp;

public class CosmosItem : BaseEntity
{
    [JsonProperty("KeyValue")]
    public string KeyValue { get; set; } = string.Empty;

    [JsonProperty("KeyName")]
    public string KeyName { get; set; } = string.Empty;

    [JsonProperty("data")]
    public string Data { get; set; } = string.Empty;

    [JsonProperty("PermId")]
    public string PermId { get; set; } = string.Empty;


    public override PartitionKey GetPartitionKey()
    {
        return new PartitionKeyBuilder()
            .Add(this.KeyValue)
            .Add(this.KeyName)
            .Build();
    }
}
