// See https://aka.ms/new-console-template for more information
using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

Dictionary<string, string> DB = new Dictionary<string, string>(){
    {"photo1.jpg","Data"},
    {"photo2.jpg","Data"},
    {"photo3.jpg","Data"}
};

string? connectionStr = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
BlobServiceClient serviceClient = new BlobServiceClient(connectionStr);
string textConteiner = "home";
BlobContainerClient container = serviceClient.GetBlobContainerClient(textConteiner);

List<string> path = new List<string>();


foreach (var el in DB)
{
    path.Add(Path.Combine(el.Value, el.Key));
}

await container.CreateIfNotExistsAsync();
await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

foreach (var el in DB)
{
    foreach (var el2 in path)
    {
        BlobClient blobClient = container.GetBlobClient(el.Key);
        using (FileStream uploaderFile = File.OpenRead(el2))
        {
            await blobClient.UploadAsync(uploaderFile, overwrite: true);
        }
    }
}

