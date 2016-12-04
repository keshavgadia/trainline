using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressProcessing.CSV
{
    public class CsvDataRecord
    {
        public int LineNumber { get; set; }

        public string Column1 { get; set; }
        public string Column2 { get; set; }

        public override string ToString()
        {
            return string.Concat(Column1, "|", Column2);
        }
    }
}
