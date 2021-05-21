using Stencil.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stencil.Plugins.RestAPI.Controllers
{
    public static class HttpAssert
    {
        public static HttpResponseMessage IsResponse(this object response)
            => Assert.IsType<HttpResponseMessage>(response);

        public static HttpResponseMessage IsSuccess(this object response)
        {
            var httpResponse = response.IsResponse();

            Assert.True(httpResponse.IsSuccessStatusCode);

            return httpResponse;
        }

        public static HttpResponseMessage IsFailure(this object response)
        {
            var httpResponse = response.IsResponse();

            Assert.False(httpResponse.IsSuccessStatusCode);

            return httpResponse;
        }

        public static HttpResponseMessage IsCreatedResponse(this object response)
        {
            var httpResponse = response.IsSuccess();

            Assert.Equal(System.Net.HttpStatusCode.Created, httpResponse.StatusCode);

            return httpResponse;
        }

        public static TContent IsContent<TContent>(this HttpContent content)
        {
            var objectContent = Assert.IsType<ObjectContent<object>>(content);
            return Assert.IsType<TContent>(objectContent.Value);
        }

        public static HttpResponseMessage IsMethodNotAllowed(this object response)
        {
            var httpResponse = response.IsFailure();

            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, httpResponse.StatusCode);

            return httpResponse;
        }

        public static HttpResponseMessage IsUnauthorizedResponse(this object response)
        {
            var httpResponse = response.IsFailure();

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, httpResponse.StatusCode);

            return httpResponse;
        }

        public static ActionResult IsActionResult(HttpContent content)
        {
            var objectContent = Assert.IsType<ObjectContent<object>>(content);
            return Assert.IsType<ActionResult>(objectContent.Value);
        }
    }
}
