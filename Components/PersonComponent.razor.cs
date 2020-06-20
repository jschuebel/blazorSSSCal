using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

using System.Net.Http;
using System.Net.Http.Json;
using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using SSSCalBlazor.Client.Models;


namespace SSSCalBlazor.Client.Components
{
    public partial class PersonComponent : ComponentBase
    {
        [Inject]
        public HttpClient client { get; set; }

        [Inject]
        public IJSRuntime jsRuntime { get; set; }


 


        public string selectedCountryID { get; set; }
        public List<PeopleModel> peopleData = new List<PeopleModel>();
        public int selectedID { get; set; }
        public int currentPage = 1, TotalRows = 0, pageSize = 10;
        protected string sortKey = "name", sortDirection ="asc";
        bool isOpened = false;
        PeopleModel selectedPerson = new PeopleModel();


        protected async Task Search() {
            var filterParams = $"page={currentPage}&pageSize={pageSize}&sort[0][field]={sortKey}&sort[0][dir]={sortDirection}";
//            peopleData = await client.GetFromJsonAsync<List<PeopleModel>>($"http://www.schuebelsoftware.com/SSSCalCoreApi/api/person?{filterParams}");
            var httpResponse = await client.GetAsync($"http://www.schuebelsoftware.com/SSSCalCoreApi/api/person?{filterParams}", HttpCompletionOption.ResponseHeadersRead);

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
                peopleData = JsonConvert.DeserializeObject<List<PeopleModel>>(streamReader.ReadToEnd());
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Search();
        }

        async Task PagingHandler(int page)
        {
            currentPage = page;
            await Search();
            //            ((IJSInProcessRuntime)jsRuntime).InvokeVoid("alert", newMessage);
        }

        protected void ShowPop(MouseEventArgs e, PeopleModel p)
        {
            selectedPerson = p;
            isOpened = true;

        }

        
        void SaveModal()
        {
            isOpened = false;
            ((IJSInProcessRuntime)jsRuntime).InvokeVoid("alert", $"Updating({selectedPerson.name}, {selectedPerson.dateOfBirth}, {selectedPerson.homePhone}). Are you sure?");
        }

        void CloseModal()
        {
            isOpened = false;
        }

        private async Task OnTestClick()
        {
            await jsRuntime.InvokeVoidAsync("alert", "selected val" + selectedCountryID);

            //        ((IJSInProcessRuntime)jsRuntime).InvokeVoid("ShowAlert", "selected val" + selectedCountryID);

        }

  
     }
}