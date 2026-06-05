using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using AddressProcessing.CSV;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterExtraTests
    {
        [Test]
        public void Open_With_InvalidMode_ThrowsExceptionWithUnknownModeMessage()
        {
            var csv = new CSVReaderWriter();
            var tmp = Path.GetTempFileName();
            try
            {
                var ex = Assert.Throws<Exception>(() => csv.Open(tmp, (CSVReaderWriter.Mode)0));
                Assert.That(ex.Message, Does.Contain("Unknown file mode"));
            }
            finally
            {
                File.Delete(tmp);
            }
        }

        [Test]
        public void Read_On_EmptyFile_ReturnsFalse_And_OutputsNullColumns()
        {
            var csv = new CSVReaderWriter();
            var tmp = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tmp, string.Empty);
                csv.Open(tmp, CSVReaderWriter.Mode.Read);
                string c1, c2;
                var result = csv.Read(out c1, out c2);
                Assert.That(result, Is.False);
                Assert.That(c1, Is.Null);
                Assert.That(c2, Is.Null);
                csv.Close();
            }
            finally
            {
                File.Delete(tmp);
            }
        }

        [Test]
        public void Close_Is_Idempotent()
        {
            var csv = new CSVReaderWriter();
            var tmp = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tmp, string.Empty);
                csv.Open(tmp, CSVReaderWriter.Mode.Read);
                csv.Close();
                // calling Close twice should not throw
                Assert.DoesNotThrow(() => csv.Close());
            }
            finally
            {
                File.Delete(tmp);
            }
        }

        [Test]
        public void Open_Write_InvalidDirectory_Throws_DirectoryNotFoundException()
        {
            var csv = new CSVReaderWriter();
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "file.txt");
            Assert.That(() => csv.Open(path, CSVReaderWriter.Mode.Write), Throws.TypeOf<DirectoryNotFoundException>());
        }

        [Test]
        public void Obsolete_Read_Method_Has_ObsoleteAttribute_With_IsError_True()
        {
            var type = typeof(CSVReaderWriter);
            var method = type.GetMethod("Read", new[] { typeof(string), typeof(string) });
            Assert.That(method, Is.Not.Null);
            var attr = method.GetCustomAttributes(typeof(ObsoleteAttribute), false).OfType<ObsoleteAttribute>().SingleOrDefault();
            Assert.That(attr, Is.Not.Null);
            Assert.That(attr.IsError, Is.True);
        }
    }
}
