using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using GradeBook.Helpers;
using GradeBook.Models;
using Xamarin.Essentials;
using System.Linq;



namespace GradeBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharityPage : ContentView
    {

        List<string> p1List;
        /*
        List<string> p2List;
        Dictionary<string, string> map;
        */

        public CharityPage()
        {
            InitializeComponent();

            p1List = new List<string> { "Elementary", "Middle", "High", "Alt", "Public", "Private" };
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


        public static string OpenEndpoint = "https://api.schooldigger.com/v1.2/schools?";   // "http://api.donorschoose.org/common/json_feed.html?";

        public static string OpenAPIKey = Secrets.APIKEY;
        public static string OpenAPIid = Secrets.AppID;


        public HttpClient client = new HttpClient();
        public string CreateQuery()
        {
            string state = "";
            if (entry.Text != "" && entry.Text != null)
            {
                state = entry.Text;
            }

            string name = "";
            if (entry2.Text != "" && entry2.Text != null)
            {
                name = entry2.Text;
            }

            string levl = "";
            if (p1.SelectedItem.ToString() != "" && p1.SelectedItem != null)
            {
                levl = p1.SelectedItem.ToString();
            }

            string zip = "";
            if (entry3.Text != "" && entry3.Text != null)
            {
                zip = entry3.Text;
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
            string result = null;

            try
            {
                var response = await client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("\t\tERROR {0}", ex.Message);
                Environment.Exit(0);
            }

            return result;
        }

        public void ProcessQuery()
        {

            string response = GetQueryResult().Result;
            Data data = JsonConvert.DeserializeObject<Data>(response);
            lv.ItemsSource = (System.Collections.IEnumerable)data;

        }
    }
    }