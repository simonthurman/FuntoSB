using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FuntoSB
{
    [ServiceBusAccount("sbandfun")]
    public static class FuntoBus
    {
        [FunctionName("FuntoBus")]
        [return: ServiceBus("outqueue", Connection = "sbandfun_RootManageSharedAccessKey_SERVICEBUS")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string msg = req.Query["msg"];

            log.LogInformation($"message to write to queue, {msg}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            msg = msg ?? data?.msg;

            string responseMessage = $"This HTTP triggered function executed successfully, {msg}";

            return new OkObjectResult(responseMessage);

        }
    }
}
