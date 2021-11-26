using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GradeBook.Helpers;
using Newtonsoft.Json;
using System.Diagnostics;


namespace GradeBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataPage : ContentPage
    {

        List<string> p1List;
        public static string OpenEndpoint;  // "http://api.donorschoose.org/common/json_feed.html?";

        public static string OpenAPIKey; 
        public static string OpenAPIid; 


        public HttpClient client; 

        public DataPage()
        {
       
                InitializeComponent();

            client = new HttpClient();
            OpenAPIid = Secrets.AppID;
            OpenAPIKey = Secrets.APIKEY;
            OpenEndpoint = "https://api.schooldigger.com/v1.2/schools?";

            p1List = new List<string> { "Elementary", "Middle", "High", "Alt", "Public", "Private" };
            p1.ItemsSource = p1List;
                /*
                p1List = new List<string> { "Music & the Arts", "Health & Sports", "Language", "History & Civics", "Math & Science", "Special Needs", "Applied Learning" };
                p2List = new List<string> { "Some Funding", "No Funding Yet", "High Poverty", "Highest Poverty", "Never Before Funded" };
                map = new Dictionary<string, string>();
                map.Add("Music & the Arts", "subject1=-1");
                map.Add("Health & Sports", "subject2=-2");
                map.Add("Language", "subject6=-6");
                map.Add("History & Civics", "subject3=-3");
                map.Add("Math & Science", "subject4=-4");
                map.Add("Special Needs", "subject7=-7");
                map.Add("Applied Learning", "subject5=-5");
                */
            }



        public string CreateQuery()
        {
            string state = "";
            if (entry.Text != null)
            {
                state = entry.Text.ToString();
            }

            string name = "";
            if (entry2.Text != null)
            {
                name = entry2.Text.ToString();
            }

            string levl = "";
            if (p1.SelectedItem != null)
            {
                levl = p1.SelectedItem.ToString();
            }

            string zip = "";
            if (entry3.Text != null)
            {
                zip = entry3.Text.ToString();
            }

            string requestUri = OpenEndpoint;
            if (state.Length > 0)
            {
                requestUri += "st=" + state + "&";
            }
            if (name.Length > 0)
            {
                requestUri += "q=" + name + "&";
            }
            if (levl.Length > 0)
            {
                requestUri += "level=" + levl + "&";
            }
            if (zip.Length > 0)
            {
                requestUri += "zip=" + zip + "&";
            }

            requestUri += $"appID={OpenAPIid}&appKey={OpenAPIKey}";

            return requestUri;

            /*
            string search = "";
            string strp1 = "";
            string id1 = "";
            string id2 = "";
            string strp2 = "";
            if(entry.Text != "" && entry.Text != null)
            {
                string x = "";
                search = entry.Text;
                if(search.Contains(" "))
                x = search.Substring(search.IndexOf(" "));
                search = search.Substring(0, search.IndexOf(" "));
                search += "+" + x;
            }
            if (p1.SelectedItem != null)
            {
                strp1 = p1.SelectedItem.ToString();
                id1 = map[strp1];

            }
            if (p2.SelectedItem != null)
            {
                strp2 = p2.SelectedItem.ToString();
            }

            string requestUri = OpenEndpoint;
            if (search.Length > 0)
            {
                requestUri += "keywords=" + search + "&";

            }
            if (strp1.Length > 0)
            {
                requestUri += id1 + "&";

            }
            if (strp2.Length > 0)
            {
                requestUri += "";
            }
            requestUri += $"APIKey={OpenAPIKey}";

            return requestUri;
            */
        }

        public async Task<string> GetQueryResult()
        {

            string query = CreateQuery();
            Uri q = new Uri(query);
            string result = null;
            testLab.Text = "22222222222222222222";
            try
            {
                testLab.Text = "NOW";
                //var response = await client.GetAsync(query);
                var response = await client.GetAsync(q); //var response = Task.Run(() => requestTask);
                testLab.Text = "NOW2";

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    testLab.Text = "made it here";
                }
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("\t\tERROR {0}", ex.Message);
                testLab.Text = "whats going on";
                Environment.Exit(0);
            }

            return result;
        }

        public void ProcessQuery()
        {
            testLab.Text = "Poop";
              string response = GetQueryResult().Result;
            var data = JsonConvert.DeserializeObject<List<Data>>(response);
            lv.ItemsSource = (System.Collections.IEnumerable)data;

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //DataPage dp = new DataPage();
            testLab.Text = "Poop00";

            ProcessQuery();
        }
    }
}