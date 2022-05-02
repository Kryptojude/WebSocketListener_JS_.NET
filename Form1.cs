using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlQueriesNamespace;

namespace webSocketListener
{
    public partial class Form1 : Form
    {
        MySqlQueries mySqlQueries;
        WebBrowser browser = new WebBrowser();

        public Form1()
        {
            InitializeComponent();

            // Test database connection
            mySqlQueries = new MySqlQueries("server=localhost;uid=root;pwd=;database=bitcoin_orderbook");
            if (!mySqlQueries.TestConnection())
                throw new Exception("database connection failed");
            // Clear the tables in database
            else
            {
                AddLog("Database connection established");
                foreach (string table in mySqlQueries.SHOW_TABLES())
                {
                    mySqlQueries.TRUNCATE_TABLE(table);
                    AddLog("table " + table + " in database cleared");
                }
            }

            browser.Navigate(new Uri(Environment.CurrentDirectory + "/webSocketListener.html"));
            browser.DocumentCompleted += Browser_DocumentCompleted;
        }

        private void AddLog(string text)
        {
            log.Text += text + Environment.NewLine;
        }

        /// <summary>
        /// This is called in the beginning and whenever a new query string is attached to the uri
        /// </summary>
        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            AddLog("documentCompleted Event");
            // Get URI
            string uri = browser.Url.ToString();
            // Check if query string attached to uri
            int hashTagIndex = uri.IndexOf('#');
            if (hashTagIndex != -1)
            {
                // Get query string from uri
                string queryStringFromURL = uri.Substring(hashTagIndex + 1);
                // string to object
                HttpQuery queryObjectFromURL = QueryStringToObject(queryStringFromURL);

                // query says to add order
                if (queryObjectFromURL._event_type == "add_order")
                {
                    // Get target table name from query
                    string tableName = queryObjectFromURL.trading_pair + "_" + queryObjectFromURL.order_type;
                    mySqlQueries.INSERT(tableName, queryObjectFromURL.GetFieldNamesArray(), queryObjectFromURL.GetValuesArray());
                }
                // query says to remove order
                else if (queryObjectFromURL._event_type == "remove_order")
                {
                    string tableName = queryObjectFromURL.trading_pair + "_" + queryObjectFromURL.order_type;
                    mySqlQueries.DELETE_WHERE(tableName, "order_id", queryObjectFromURL.order_id);
                }
                else
                    throw new Exception("invalid query string: missing event_type parameter");
            }
        }

        private HttpQuery QueryStringToObject(string query_string)
        {
            HttpQuery query = new HttpQuery();
            // Split at '&'
            string[] keyValuePairs = query_string.Split('&');
            for (int i = 0; i < keyValuePairs.Length; i++)
            {
                // Split at '='
                string[] keyAndValue = keyValuePairs[i].Split('=');
                // Fill value into appropriate field of the order instance
                switch (keyAndValue[0])
                {
                    case "_event_type":
                        query._event_type = keyAndValue[1];
                        break;
                    case "order_type":
                        query.order_type = keyAndValue[1];
                        break;
                    case "order_id":
                        query.order_id = keyAndValue[1];
                        break;
                    case "amount":
                        query.amount = keyAndValue[1];
                        break;
                    case "min_amount":
                        query.min_amount = keyAndValue[1];
                        break;
                    case "price":
                        query.price = keyAndValue[1];
                        break;
                    case "trading_pair":
                        query.trading_pair = keyAndValue[1];
                        break;
                    case "payment_option":
                        query.payment_option = keyAndValue[1];
                        break;
                }
            }

            // TODO: array to object simple conversion
            //Type dataType = typeof(HttpQuery);
            //TypeConverter obj = TypeDescriptor.GetConverter(dataType);
            //object value = obj.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, query_string);

            //return (HttpQuery)value;
            return query;
        }

        private void log_TextChanged(object sender, EventArgs e)
        {
            if (log.Text.Length > 1000)
                log.Text = "";
        }
    }
}
