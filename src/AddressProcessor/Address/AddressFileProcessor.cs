using System;
using AddressProcessing.Address.v1;
using AddressProcessing.CSV;

namespace AddressProcessing.Address
{
    public class AddressFileProcessor
    {
        private readonly IMailShot _mailShot;
        private readonly Func<ICSVReaderWriter> _csvFactory;

        public AddressFileProcessor(IMailShot mailShot)
            : this(mailShot, () => new CSVReaderWriterAdapter())
        {
        }

        // Constructor overload to allow injecting a CSV reader/writer factory for testing
        public AddressFileProcessor(IMailShot mailShot, Func<ICSVReaderWriter> csvFactory)
        {
            if (mailShot == null) throw new ArgumentNullException("mailShot");
            if (csvFactory == null) throw new ArgumentNullException("csvFactory");
            _mailShot = mailShot;
            _csvFactory = csvFactory;
        }

        public void Process(string inputFile)
        {
            var reader = _csvFactory();
            try
            {
                reader.Open(inputFile, CSVReaderWriter.Mode.Read);

                string column1, column2;

                while (reader.Read(out column1, out column2))
                {
                    _mailShot.SendMailShot(column1, column2);
                }
            }
            finally
            {
                // Ensure resources are cleaned up even if exceptions occur
                try { reader.Close(); } catch { }
            }
        }
    }
}
