using Newtonsoft.Json;
using RestcountriesRAD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace RestcountriesRAD
{
    public partial class MainPage : ContentPage
    {
        public partial class MainPage : ContentPage
        {
            private const string RestCountriesBaseUrl = "https://restcountries.com/v3.1/";
            private HttpClient httpClient;
            private List<Country> countries;

            public MainPage()
            {
                InitializeComponent();

                // Inicializar el cliente HTTP
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(RestCountriesBaseUrl);

                // Inicializar la lista de países
                countries = new List<Country>();

                // Evento de selección del Spinner/ComboBox
                regionPicker.SelectedIndexChanged += OnRegionSelected;
            }

            private async void OnRegionSelected(object sender, EventArgs e)
            {
                var selectedRegion = (Region)regionPicker.SelectedItem;
                if (selectedRegion != null)
                {
                    string region = selectedRegion.Name.ToLower();

                    // Obtener datos de la API
                    var response = await httpClient.GetAsync($"region/{region}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        countries = JsonConvert.DeserializeObject<List<Country>>(json);

                        // Actualizar el ListView
                        countriesListView.ItemsSource = countries;
                    }
                }
            }

            private void OnCountrySelected(object sender, SelectedItemChangedEventArgs e)
            {
                var selectedCountry = e.SelectedItem as Country;
                if (selectedCountry != null)
                {
                    // Mostrar información en el MapView
                    var location = new Location(0, 0); // Agrega las coordenadas del país seleccionado.
                    var pin = new Pin
                    {
                        Position = new Position(location.Latitude, location.Longitude),
                        Label = selectedCountry.Name,
                        Address = "Country information goes here" // Agrega la información del país.
                    };
                    mapView.Pins.Clear();
                    mapView.Pins.Add(pin);
                    mapView.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(1000)));
                }
            }
        }
    }
}




   
}
