using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations.Storage;
using System.Windows.Documents;
using RestSharp;
using RestSharp.Authenticators;
using RtrackerV3Wpf.Models;

namespace RtrackerV3Wpf.Services
{
    class DocumentService : RestServiceBase<DocumentModel>
    {
        private readonly RestClient _restClient;

        public DocumentService()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.REMOTE_HOST))
                return;
            try
            {
                _restClient = new RestClient(Properties.Settings.Default.REMOTE_HOST);
                _restClient.Authenticator = new HttpBasicAuthenticator(Properties.Settings.Default.USERNAME, Properties.Settings.Default.PASSWORD);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                _restClient = new RestClient();
            }
        }

        public async Task<List<DocumentModel>> FetchDocumentsAsync()
        {
            var req = new RestRequest("/reports", Method.GET, DataFormat.Json);
            var r = await _restClient.ExecuteTaskAsync<List<DocumentModel>>(req, Method.GET);
            return EnsureValidResponse(r);
        }

        public async Task DoAction(int documentId, string action)
        {
            var req = new RestRequest("/reports/", Method.PATCH, DataFormat.Json);
            req.AddParameter("DocumentId", documentId);
            req.AddParameter("Action", action);
            var r = await _restClient.ExecuteTaskAsync(req);
        }

        public async Task<ROUserModel> EnsureAuthenticated()
        {
            var req = new RestRequest("/check_auth");
            var r = await _restClient.ExecuteTaskAsync<ROUserModel>(req);

            if (!r.IsSuccessful || r.ErrorException != null ||
               r.StatusCode == HttpStatusCode.Forbidden)
                throw new AuthenticationException();

            return r.Data;
        }

        public async Task Enroll(DocumentModel documentModel)
        {
            var req = new RestRequest("/reports/", Method.POST);
            req.AddBody(documentModel);
            var r = await _restClient.ExecuteTaskAsync(req);
            EnsureValidResponse(r);
        }
    }
}
