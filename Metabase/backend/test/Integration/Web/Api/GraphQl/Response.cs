using System.Collections.Generic;
using System.Text.Json;
using Errors = Infrastructure.Errors;

namespace Test.Integration.Web.Api.GraphQl
{
    public class Response<TData, TError>
      : ResponseBase
      where TData : ResponseBase
      where TError : ResponseBase
    {
        public TData? data { get; set; }
        public IReadOnlyList<TError>? errors { get; set; }

        public Response() { }

        public TData EnsureSuccess()
        {
            var nonNullData = EnsureData();
            EnsureNoErrors();
            EnsureNoOverflow();
            nonNullData.EnsureNoOverflow();
            return nonNullData;
        }

        public TData EnsureData()
        {
            if (data is null)
            {
                throw new JsonException("The data value is empty'");
            }
            return data!;
        }

        public Response<TData, TError> EnsureNoData()
        {
            if (data != null)
            {
                throw new JsonException($"The data value is not empty but contains '{JsonSerializer.Serialize(data)}'");
            }
            return this;
        }

        public virtual IReadOnlyList<TError> EnsureFailure()
        {
            var nonNullErrors = EnsureErrors();
            EnsureNoData();
            EnsureNoOverflow();
            foreach (var error in nonNullErrors)
            {
                error.EnsureNoOverflow();
            }
            return nonNullErrors;
        }

        public IReadOnlyList<TError> EnsureErrors()
        {
            if (errors is null)
            {
                throw new JsonException("The errors dictionary is empty'");
            }
            return errors!;
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

    public sealed class Response<TData>
      : Response<TData, Error>
      where TData : ResponseBase
    {
        public override IReadOnlyList<Error> EnsureFailure()
        {
            var nonNullErrors = base.EnsureFailure();
            foreach (var error in nonNullErrors)
            {
                error.EnsureNoOverflow();
            }
            return nonNullErrors;
        }
    }
}