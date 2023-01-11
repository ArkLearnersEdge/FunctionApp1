using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
           log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            JArray Arr = JArray.Parse(requestBody);
            CloudStorageAccount storageAcc = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mgrtstorageacc;AccountKey=RCsd0jseggFD37Uflq3KmzsIGCFWvHb+vFlcnLwVRP9+eVkiJD80Fb/G/XsV6Lg6TpYfCgGJUmDI+AStmKAvLw==;EndpointSuffix=core.windows.net");
            CloudBlobClient BlobClient = storageAcc.CreateCloudBlobClient();
            CloudBlobContainer container = BlobClient.GetContainerReference("testmgrt");
            await container.CreateIfNotExistsAsync();
            CloudBlockBlob blockBlob;
            foreach (var item in Arr)
            {
                blockBlob = container.GetBlockBlobReference(item["fileName"].ToString());
                await blockBlob.UploadTextAsync(item["content"].ToString());
            }

                return new OkObjectResult("Success");
        }
    }
}
