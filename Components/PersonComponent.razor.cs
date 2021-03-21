using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

//using System.Net.Http;
//using System.Net.Http.Json;
using System;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
//using System.IO;
using System.Linq;

//using Newtonsoft.Json;
using SSSCalBlazor.Client.Models;
using SSSCalBlazor.Client.Store;
using Fluxor;

namespace SSSCalBlazor.Client.Components
{
    public partial class PersonComponent : ComponentBase //Fluxor.Blazor.Web.Components.FluxorComponent
    {
        //[Inject]
        //public HttpClient client { get; set; }

        [Inject]
        public SSSCalBlazor.Client.Models.IPersonService svc { get; set; }


        [Inject]
        public IJSRuntime jsRuntime { get; set; }

/*Fluxor state changes
        @inherits Fluxor.Blazor.Web.Components.FluxorComponent
<h1>Sel Name: @pState.Value.peopleModel.name</h1>


        [Inject]
        public IState<PersonState> pState { get; set; }

        [Inject]
        public IDispatcher dispatch {get;set;}
*/        


        public string selectedCountryID { get; set; }
        public List<PeopleModel> peopleData = new List<PeopleModel>();
        public int selectedID { get; set; }
        public int currentPage = 1, TotalRows = 0, pageSize = 10;
        protected string sortKey = "name", activeKey = "name", sortDirection ="asc";
        bool isOpened = false;
        PeopleModel selectedPerson = new PeopleModel();


        protected async Task Search(string searchString=null) {

            var retv = await svc.GetPeople(currentPage, pageSize, sortKey, sortDirection, searchString);
            peopleData = retv.Item2;
            TotalRows = retv.Item1;
            /*  ==========>REAL 
            //https://www.schuebelsoftware.com/SSSCalWebAPI/api/person?page=1&pageSize=10&sort[0][field]=name&sort[0][dir]=asc&filter[logic]=and&filter[filters][0][field]=name&%20filter[filters][0][operator]=contains&filter[filters][0][value]=am
            var filterParams = new System.Text.StringBuilder($"page={currentPage}&pageSize={pageSize}&sort[0][field]={sortKey}&sort[0][dir]={sortDirection}");
            if (!string.IsNullOrEmpty(searchString))
                filterParams.Append(searchString);

            //            peopleData = await client.GetFromJsonAsync<List<PeopleModel>>($"http://www.schuebelsoftware.com/SSSCalCoreApi/api/person?{filterParams}");
            //var httpResponse = await client.GetAsync($"http://api.schuebelsoftware.com/api/person?{filterParams}", HttpCompletionOption.ResponseHeadersRead);
            //Azure requires SSL


             var httpResponse = await client.GetAsync($"https://www.schuebelsoftware.com/SSSCalWebAPI/api/person?{filterParams}", HttpCompletionOption.ResponseHeadersRead);

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
                peopleData = JsonConvert.DeserializeObject<List<PeopleModel>>(streamReader.ReadToEnd());
            }
            */
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
/*Fluxor state changes
            dispatch.Dispatch(new AddPerson(p));
            return;
*/
            selectedPerson = p;
            isOpened = true;

        }

        async Task Filter(Tuple<string, string, string> searchv)
        {
            //ColumnName, ColumnType, _searchString
            Console.WriteLine($" column({searchv.Item1})  columnType({searchv.Item2}) searchString({searchv.Item3})");
            //https://www.schuebelsoftware.com/SSSCalWebAPI/api/person?page=1&pageSize=10&sort[0][field]=name&sort[0][dir]=asc&filter[logic]=and&filter[filters][0][field]=homePhone&filter[filters][0][operator]=contains&filter[filters][0][value]=678
            string srch =null;
            if (!string.IsNullOrEmpty(searchv.Item3))
            {
                if (searchv.Item2 == "string")
                    srch = $"&filter[logic]=and&filter[filters][0][field]={searchv.Item1}&filter[filters][0][operator]=contains&filter[filters][0][value]={searchv.Item3}";
                if (searchv.Item2 == "date")
                {
                    srch = $"&filter[logic]=and&filter[filters][0][field]={searchv.Item1}&filter[filters][0][operator]=gte&filter[filters][0][value]={searchv.Item3}";
                    // &filter[filters][1][field]=Date&filter[filters][1][operator]=lte&filter[filters][1][value]=Mon+Apr+30+2018+00%3A00%3A00+GMT-0500+(Central+Daylight+Time)&_=1562110553341";
                }
                if (searchv.Item2 == "int")
                    srch = $"&filter[logic]=and&filter[filters][0][field]={searchv.Item1}&filter[filters][0][operator]=eq&filter[filters][0][value]={searchv.Item3}";
            }
            await Search(srch);
        }

        async Task Sort(string fldName)
        {

            if (sortKey == fldName) {
                if (sortDirection == "asc")
                    sortDirection = "desc";
                else sortDirection = "asc";
            }
            else
            {
                sortKey = fldName;
                sortDirection = "asc";
            }
            Console.WriteLine($"fldname({fldName}) sortdir({sortDirection})");
            await Search();
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