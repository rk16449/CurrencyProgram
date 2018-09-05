using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Currency> list = new List<Currency>();

        internal List<Currency> CurrencyList { get => list; set => list = value; }

        public MainWindow()
        {
            InitializeComponent();

            // Loads CurrencyList with available currencies
            LoadList();

            // Fill up Comboboxes with currency list
            firstComboBox.ItemsSource = CurrencyList;
            secondComboBox.ItemsSource = CurrencyList;
        }

        // Loads ComboBoxes with available currencies
        private void LoadList()
        {
            // Load up currencies into list
            string url = "http://free.currencyconverterapi.com/api/v6/currencies";
            string jsonString;

            // List of currencies


            using (var webClient = new WebClient())
            {
                jsonString = webClient.DownloadString(url);

                // parse jsonString
                JObject json = JObject.Parse(jsonString);
                // Count how many objects are here (root) = results
                Console.WriteLine(json.Count);

                // Count how many objects inside results
                JObject results = (JObject)json["results"];
                Console.WriteLine(results.Count);



                // Loop through each object
                foreach (JProperty x in (JToken)results)
                {
                    JToken value = x.Value;

                    // Convert back to JObject
                    JObject newVal = (JObject)value;

                    // Get the values
                    string currencyName = GetJArrayValue(newVal, "currencyName");
                    string currencySymbol = GetJArrayValue(newVal, "currencySymbol");

                    // Create objects of each currency
                    CurrencyList.Add(new Currency(x.Name, currencyName, currencySymbol));
                }

            }

        }

        // Gets key values (method taken from stack over flow)
        private string GetJArrayValue(JObject yourJArray, string key)
        {
            foreach (KeyValuePair<string, JToken> keyValuePair in yourJArray)
            {
                if (key == keyValuePair.Key)
                {
                    return keyValuePair.Value.ToString();
                }
            }

            return "";
        }

        // When we press the convert button, attempt to convert
        private void onConvert(object sender, RoutedEventArgs e)
        {
            Currency firstCurrency = (Currency)firstComboBox.SelectedValue;
            Currency secondCurrency = (Currency)secondComboBox.SelectedValue;

            // Make sure we have something selected
            if (firstCurrency == null || secondCurrency == null)
            {
                MessageBox.Show("You need to select two currencies!");
                // Exit this entire method (could have else)
                return;
            }

            // Validate that our currencies exist in the List
            if (ValidateCurrencyID(firstCurrency.Id) && ValidateCurrencyID(secondCurrency.Id))
            {
                // Do a calculation on these then update the TextBox
                string conversion = getConversionRate(firstCurrency.Id, secondCurrency.Id);

                // Try and calculate the sum
                double sum;
                if (double.TryParse(multiplyTextBox.Text, out sum))
                {
                    sum = Convert.ToDouble(conversion) * Convert.ToDouble(multiplyTextBox.Text);
                    // Change the text
                    conversionTextBox.Text = sum.ToString();
                }
                else
                {
                    MessageBox.Show("Make sure TextBoxes are numbers!");
                }
            }

        }

        // Validates our currencies
        private Boolean ValidateCurrencyID(string ID)
        {
            // Loop through currencies and try to find a matching ID
            foreach (var currency in CurrencyList)
            {
                if (currency.Id.Equals(ID))
                {
                    return true;
                }
            }

            // Not valid
            return false;
        }

        // Gets the conversion rate between two currencies
        static string getConversionRate(string firstID, string secondID)
        {
            string CONVERSION = firstID + "_" + secondID;
            string url = "https://free.currencyconverterapi.com/api/v6/convert?q=" + CONVERSION + "&compact=ultra";

            using (var webClient = new WebClient())
            {
                string jsonString = webClient.DownloadString(url);
                JObject json = JObject.Parse(jsonString);

                if (json != null)
                {
                    return json.GetValue(CONVERSION).ToString();
                }
            }

            return "INVALID";
        }
    }
}
