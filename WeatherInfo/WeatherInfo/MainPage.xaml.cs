using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
namespace WeatherInfo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnGetWeather_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(entPinCode.Text))
            {
                Weather weather = await GetWeather(entPinCode.Text);
                if (weather != null)
                {
                    txtLocation.Text = weather.Title;
                    txtTemperature.Text = weather.Temperature;
                    txtWind.Text = weather.Wind;
                    txtHumidity.Text = weather.Humidity;
                    txtSunrise.Text = weather.Sunrise;
                    txtSunset.Text = weather.Sunset;
                    btnGetWeather.Text = "Search Again";
                }
            }
        }
        public static async Task<Weather> GetWeather(string pinCode)
        {
            string key = "9c7544fa96727804894fa8b735555c63";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?zip=" + pinCode + ",in&appid=" + key + "&units=imperial";
            if (key != "9c7544fa96727804894fa8b735555c63")
            {
                throw new ArgumentException("Get you API key from openweathermap.org/appid and assign it in the 'key' variable.");
            }
            dynamic results = await getDataFromService(queryString).ConfigureAwait(false);
            if (results["weather"] != null)
            {
                Weather weather = new Weather();
                weather.Title = (string)results["name"];
                weather.Temperature = (string)results["main"]["temp"] + " F";
                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                weather.Sunrise = sunrise.ToString() + " UTC";
                weather.Sunset = sunset.ToString() + " UTC";
                weather.Wind = (string)results["wind"]["speed"] + " mph";
                weather.Humidity = (string)results["main"]["humidity"] + " %";
                return weather;
            }
            else
            {
                return null;
            }
        }
        public static async Task<dynamic> getDataFromService(string pQueryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(pQueryString);
            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }
            return data;
        }
    }
}
