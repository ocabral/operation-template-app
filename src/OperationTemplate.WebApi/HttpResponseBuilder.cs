using Newtonsoft.Json;
using StoneCo.Buy4.OperationTemplate.WebApi.Settings;
using StoneCo.Buy4.OperationTemplate.DataContracts.V1;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace StoneCo.Buy4.OperationTemplate.WebApi
{
    /// <summary>
    /// Class responsible for building http responses.
    /// </summary>
    public static class HttpResponseBuilder
    {
        /// <summary>
        /// Extension Method for build http response messages from OperationResponseBase.
        /// </summary>
        /// <param name="responseBase">Operation response</param>
        /// <param name="enumSerializationOptions">Enum serialization option.</param>
        /// <returns>Http response message</returns>
        public static HttpResponseMessage BuildHttpResponse(this OperationResponseBase responseBase, EnumSerializationOptions enumSerializationOptions = EnumSerializationOptions.Undefined)
        {
            JsonSerializerSettings settings = SerializationSettings.GetJsonSerializationSettings(enumSerializationOptions);

            HttpResponseMessage httpResponse = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(responseBase, settings), Encoding.UTF8, "application/json")
            };

            if (responseBase != null)
            {
                if (responseBase.Success && responseBase.Errors != null && responseBase.Errors.Any())
                {
                    httpResponse.StatusCode = HttpStatusCode.InternalServerError;
                }
                else
                {
                    httpResponse.StatusCode = responseBase.HttpStatusCode;
                }
            }
            else
            {
                httpResponse.StatusCode = HttpStatusCode.InternalServerError;
            }

            return httpResponse;
        }
    }
}
