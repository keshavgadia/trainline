using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressProcessing.CSV
{
    public sealed class CsvDataRecordMap : CsvClassMap<CsvDataRecord>
    {
        public CsvDataRecordMap()
        {
            Map(m => m.LineNumber).ConvertUsing(row => ((CsvHelper.CsvReader)row).Parser.Row);
            Map(m => m.Column1).Index(0);
            Map(m => m.Column2).Index(1);
        }
    }
}
