using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

public abstract class BaseEntity
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    public virtual PartitionKey GetPartitionKey()
    {
        return new PartitionKey(this.Id);
    }
}