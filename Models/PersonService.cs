using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Json;
using System.IO;
using Newtonsoft.Json;

namespace SSSCalBlazor.Client.Models
{
    public interface IPersonService
    {
        Task<Tuple<int, List<PeopleModel>>> GetPeople(int currentPage, int pageSize, string sortKey, string sortDirection, string searchString);
    }
    public class PersonService : IPersonService
    {
        public HttpClient _client { get; set; }

        public int TotalRows = 0;

        public PersonService(HttpClient client) { _client = client;  }

        public async Task<Tuple<int, List<PeopleModel>>> GetPeople(int currentPage, int pageSize, string sortKey, string sortDirection, string searchString)
        {
            var filterParams = new System.Text.StringBuilder($"page={currentPage}&pageSize={pageSize}&sort[0][field]={sortKey}&sort[0][dir]={sortDirection}");
            if (!string.IsNullOrEmpty(searchString))
                filterParams.Append(searchString);

            var httpResponse = await _client.GetAsync($"https://www.schuebelsoftware.com/SSSCalWebAPI/api/person?{filterParams}", HttpCompletionOption.ResponseHeadersRead);
            List<PeopleModel> lst = null;
            Tuple<int, List<PeopleModel>> retVal = null;

            //client.DefaultRequestHeaders.Authorization=new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", savedToken);
            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                if (httpResponse.Headers.Contains("Paging-TotalRecords"))
                {
                    var hdrRecordCount = httpResponse.Headers.GetValues("Paging-TotalRecords").FirstOrDefault();
                    TotalRows = int.Parse(hdrRecordCount);
                }

                var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                var streamReader = new StreamReader(contentStream);
                lst=JsonConvert.DeserializeObject<List<PeopleModel>>(streamReader.ReadToEnd());
                retVal = new Tuple<int, List<PeopleModel>>(TotalRows, lst);
            }
            return retVal;

        }
    }
}
