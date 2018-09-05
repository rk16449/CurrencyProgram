using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWPF
{
    class Currency
    {
        public Currency(string id, string currencyName, string currencySymbol)
        {
            this.id = id;
            this.currencyName = currencyName;
            this.currenySymbol = currencySymbol;
        }

        public string toString()
        {
            return "ID: " + this.id + " currencyName: " + currencyName + " currencySymbol: " + currenySymbol;
        }

        private string currencyName;
        private string currenySymbol;
        private string id;

        public string CurrencyName { get => currencyName; set => currencyName = value; }
        public string CurrenySymbol { get => currenySymbol; set => currenySymbol = value; }
        public string Id { get => id; set => id = value; }
    }
}
