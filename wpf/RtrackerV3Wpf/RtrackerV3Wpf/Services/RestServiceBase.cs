using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RtrackerV3Wpf.Models;

namespace RtrackerV3Wpf.Services
{
    abstract class RestServiceBase<T>
    {
        protected TT EnsureValidResponse<TT>(IRestResponse<TT> response) where TT : class
        {
            if (!response.IsSuccessful || response.ErrorException != null || !string.IsNullOrWhiteSpace(response.ErrorMessage))
                throw new Exception("Request unsuccessful");
            return response.Data;
        }

        protected void  EnsureValidResponse(IRestResponse response)
        {
            if (!response.IsSuccessful || response.ErrorException != null || !string.IsNullOrWhiteSpace(response.ErrorMessage))
                if (response.ErrorException != null)
                    throw response.ErrorException;
                else
                {
                    throw  new Exception("Response error.");
                }
        }
    }
}
