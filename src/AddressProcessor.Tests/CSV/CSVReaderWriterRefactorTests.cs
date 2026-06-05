using System;
using NUnit.Framework;
using AddressProcessing.CSV;
using AddressProcessing.Address;
using AddressProcessing.Address.v1;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterRefactorTests
    {
        [Test]
        public void AddressFileProcessor_Closes_Reader_When_Read_Throws()
        {
            var fakeReader = new FakeCSVReaderWriter()
            {
                ThrowOnRead = true
            };

            var factory = new Func<ICSVReaderWriter>(() => fakeReader);
            var mailShot = new NoopMailShot();
            var processor = new AddressFileProcessor(mailShot, factory);

            Assert.Throws<Exception>(() => processor.Process("doesnotmatter.csv"));
            Assert.That(fakeReader.Closed, Is.True);
        }

        [Test]
        public void AddressFileProcessor_Closes_Reader_When_MailShot_Throws()
        {
            var fakeReader = new FakeCSVReaderWriter()
            {
                Lines = new[] { Tuple.Create("name","address") }
            };

            var factory = new Func<ICSVReaderWriter>(() => fakeReader);
            var mailShot = new ThrowingMailShot();
            var processor = new AddressFileProcessor(mailShot, factory);

            Assert.Throws<InvalidOperationException>(() => processor.Process("ignored.csv"));
            Assert.That(fakeReader.Closed, Is.True);
        }

        class NoopMailShot : IMailShot { public void SendMailShot(string name, string address) { } }

        class ThrowingMailShot : IMailShot { public void SendMailShot(string name, string address) { throw new InvalidOperationException("boom"); } }

        class FakeCSVReaderWriter : ICSVReaderWriter
        {
            public bool ThrowOnRead { get; set; }
            public bool Closed { get; private set; }
            public Tuple<string,string>[] Lines { get; set; } = Array.Empty<Tuple<string,string>>();
            private int _index = 0;

            public void Open(string fileName, CSVReaderWriter.Mode mode) { }
            public void Write(params string[] columns) { }

            public bool Read(out string column1, out string column2)
            {
                if (ThrowOnRead) throw new Exception("read failure");
                if (_index >= Lines.Length)
                {
                    column1 = null; column2 = null; return false;
                }
                column1 = Lines[_index].Item1; column2 = Lines[_index].Item2; _index++; return true;
            }

            public void Close() { Closed = true; }
            public void Dispose() { Close(); }
        }
    }
}
