using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AddressProcessing.CSV;
using System.IO;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        #region Tests for Open
        [Test]
        public void Open_For_Read_When_File_DoesNot_Exists_Throws_FileNotFoundException()
        {
            var csvReadWriter = new CSVReaderWriter();
            Assert.Throws<FileNotFoundException>(() => csvReadWriter.Open("test.file", CSVReaderWriter.Mode.Read));
        }

        [Test]
        public void Open_For_Read_When_File_Exists_DoesNotThrowException()
        {
            var csvReadWriter = new CSVReaderWriter();
            Assert.DoesNotThrow(() => csvReadWriter.Open(@"test_data\contacts.csv", CSVReaderWriter.Mode.Read));
        }
        #endregion

        #region Tests for Read

        [Test]
        public void Read_With_Out_Parameters_When_ReaderStream_Not_Initialized_Throws_Exception()
        {
            string column1 = null;
            string column2 = null;
            var csvReadWriter = new CSVReaderWriter();
            Assert.Throws<Exception>(()=> csvReadWriter.Read(out column1, out column2));

        }

        [Test]
        public void Read_With_Out_Parameters_When_ReaderStream_Initialized_Reads_Data()
        {
            string column1 = null;
            string column2 = null;
            var csvReadWriter = new CSVReaderWriter();
            csvReadWriter.Open(@"test_data\contacts.csv", CSVReaderWriter.Mode.Read);
            csvReadWriter.Read(out column1, out column2);
            Assert.NotNull(column1);
            Assert.NotNull(column2);


        }

        [Ignore("This test cannot be written without significant refactoring.")]
        public void Read_With_Out_Parameters_When_StreamReader_ReadLine_Throws_Exception_Closes_The_Stream()
        {

        }


        [Test]
        public void Read_When_Only_First_Column_Exists_Throws_Exception()
        {
            var csvReadWriter = new CSVReaderWriter();
            csvReadWriter.Open(@"test_data\test.txt", CSVReaderWriter.Mode.Write);
            csvReadWriter.Write("test line 1 Column 1");// The Read functionality forces to create the file with two columns which is quite rigid.
            csvReadWriter.Close();
            string column1 = null;
            string column2 = null;

            csvReadWriter.Open(@"test_data\test.txt", CSVReaderWriter.Mode.Read);
                      
            Assert.Throws<IndexOutOfRangeException>(()=> csvReadWriter.Read(out column1, out column2));


        }

        #endregion


        #region Tests for Write

        [Test]
        public void Open_For_Write_When_WriterStream_Not_Initialized_Throws_Exception()
        {
            var csvReadWriter = new CSVReaderWriter();
            Assert.Throws<Exception>(() => csvReadWriter.Write("test"));
        }

        [Test]
        public void Open_For_Write_When_WriterStream_Initialized_Write_Data()
        {
            var csvReadWriter = new CSVReaderWriter();
            csvReadWriter.Open(@"test_data\test.txt", CSVReaderWriter.Mode.Write);
            csvReadWriter.Write("test line 1 Column 1", "test line 1 Column 2");// The functionality forces to create the file with two columns which is quite rigid.
            csvReadWriter.Close();
            string column1 = null;
            string column2 = null;

            csvReadWriter.Open(@"test_data\test.txt", CSVReaderWriter.Mode.Read);
            csvReadWriter.Read(out column1, out column2);
            csvReadWriter.Close();
            Assert.AreEqual("test line 1 Column 1", column1);
            Assert.AreEqual("test line 1 Column 2", column2);

        }

        
        #endregion


        #region Cleanup
        [TearDown]
        public void Cleanup()
        {
            File.Delete(@"test_data\test.txt");
        }
        #endregion


    }
}

