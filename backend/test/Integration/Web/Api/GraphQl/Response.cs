using System.Collections.Generic;
using Startup = Icon.Startup;
using GrantType = IdentityServer4.Models.GrantType;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Xunit;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;
using Configuration = Icon.Configuration;
using TokenResponse = IdentityModel.Client.TokenResponse;
using TokenRequest = IdentityModel.Client.TokenRequest;
using IdentityModel.Client;
using JsonExtensionDataAttribute = System.Text.Json.Serialization.JsonExtensionDataAttribute;

namespace Test.Integration.Web.Api.GraphQl
{
    public class Response<TData, TError>
      : ResponseBase
      where TData : ResponseBase
      where TError : ResponseBase
    {
        public TData data { get; set; }
        public IReadOnlyList<TError> errors { get; set; }

        public Response<TData, TError> EnsureSuccess()
        {
            EnsureData();
            EnsureNoErrors();
            EnsureNoOverflow();
            data.EnsureNoOverflow();
            return this;
        }

        public Response<TData, TError> EnsureData()
        {
            if (data == null)
            {
                throw new JsonException("The data value is empty'");
            }
            return this;
        }

        public Response<TData, TError> EnsureNoData()
        {
            if (data != null)
            {
                throw new JsonException($"The data value is not empty but contains '{JsonSerializer.Serialize(data)}'");
            }
            return this;
        }

        public Response<TData, TError> EnsureFailure()
        {
            EnsureErrors();
            EnsureNoData();
            EnsureNoOverflow();
            foreach (var error in errors)
            {
                error.EnsureNoOverflow();
            }
            return this;
        }

        public Response<TData, TError> EnsureErrors()
        {
            if (errors == null)
            {
                throw new JsonException("The errors dictionary is empty'");
            }
            return this;
        }

        public Response<TData, TError> EnsureNoErrors()
        {
            if (errors != null)
            {
                throw new JsonException($"The errors dictionary is not empty but contains '{JsonSerializer.Serialize(errors)}'");
            }
            return this;
        }
    }
}