using System;

namespace AddressProcessing.CSV
{
    public class CSVReaderWriterAdapter : ICSVReaderWriter
    {
        private readonly CSVReaderWriter _inner = new CSVReaderWriter();

        public void Open(string fileName, CSVReaderWriter.Mode mode) => _inner.Open(fileName, mode);

        public void Write(params string[] columns) => _inner.Write(columns);

        public bool Read(out string column1, out string column2) => _inner.Read(out column1, out column2);

        public void Close() => _inner.Close();

        public void Dispose() => Close();
    }
}
