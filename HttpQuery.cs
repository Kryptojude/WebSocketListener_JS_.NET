using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace webSocketListener
{
    class HttpQuery
    {
        public string _event_type;
        public string order_type;
        public string amount;
        public string min_amount;
        public string price;
        public string order_id;
        public string trading_pair;
        public string payment_option;

        public string[] GetFieldNamesArray()
        {
            string[] fieldNames = new string[] { "amount", "min_amount", "price", "order_id", "payment_option" };
            return fieldNames;
        }

        public string[] GetValuesArray()
        {
            string[] fieldValues = new string[] { amount, min_amount, price, order_id, payment_option };
            return fieldValues;
        }
    }
}
