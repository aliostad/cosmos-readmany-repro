
from azure.cosmos import CosmosClient, exceptions
import time

# 1. Setup Connection Metadata
URL = "https://whatisyourname.documents.azure.com:443/"
KEY = "**************"
DATABASE_NAME = "hcp"
CONTAINER_NAME = "hcp"

client = CosmosClient(URL, credential=KEY)
container = client.get_database_client(DATABASE_NAME).get_container_client(CONTAINER_NAME)

def read_many_items_example():
    # 1. Define the items (id, partition_key)
    # Note: If your partition key is a list/hierarchical, pass it as a list
    items_to_read = [
        ("item-chippa", ["value-1000", "name-1000"])
    ]

    t = time.time()
    # 2. Call read_items directly (No .by_page() needed)
    results = container.read_items(items=items_to_read)
    print(time.time() - t)
    
    # 3. Immediately capture the RU charge from the client_connection
    # The header is populated by the most recent network call
    request_charge = container.client_connection.last_response_headers.get("x-ms-request-charge")

    # 4. Results
    print(f"Retrieved {len(results)} items.")
    print(f"RU Charge for this operation: {request_charge} RUs")
    
    for item in results:
        print(f"Found Item: {item.get('id')}")

if __name__ == '__main__':
    read_many_items_example()
    