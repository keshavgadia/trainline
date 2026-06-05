using System;
using System.IO;
using NUnit.Framework;
using AddressProcessing.Address;
using AddressProcessing.Address.v1;

namespace AddressProcessing.Tests
{
    [TestFixture]
    public class AddressFileProcessorExtraTests
    {
        [Test]
        public void Constructor_NullMailShot_ThrowsArgumentNullException()
        {
            Assert.That(() => new AddressFileProcessor(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Process_MissingFile_Throws_FileNotFoundException()
        {
            var fake = new FakeMailShot();
            var processor = new AddressFileProcessor(fake);
            string missing = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".csv");
            Assert.That(() => processor.Process(missing), Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public void Process_EmptyFile_DoesNotSendAnyMail()
        {
            var fake = new FakeMailShot();
            var processor = new AddressFileProcessor(fake);
            var tmp = Path.GetTempFileName();
            try
            {
                // ensure file is empty
                File.WriteAllText(tmp, string.Empty);
                processor.Process(tmp);
                Assert.That(fake.Counter, Is.EqualTo(0));
            }
            finally
            {
                File.Delete(tmp);
            }
        }

        internal class FakeMailShot : IMailShot
        {
            public int Counter { get; private set; }
            public void SendMailShot(string name, string address) => Counter++;
        }
    }
}
