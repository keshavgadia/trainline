using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    /// <summary>
    /// Summary: To refactor this class into clean single responsibilty objects will break the backward compatibility.
    /// The exception handling has been added wherever applicable to make this class clean up the file handles.
    /// </summary>
    public class CSVReaderWriter
    {


        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;
                             

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            // Case statement is more intuitive then if-else-else if
            // The read and write streams cannot be disposed in this method due to the nature of the functionality.
            // Closing the streams just to be on safer side.
            try
            {
                switch (mode)
                {
                    case Mode.Read:
                        _readerStream = File.OpenText(fileName);
                        break;
                    case Mode.Write:
                        _writerStream = new FileInfo(fileName).CreateText();
                        break;
                    default:
                        throw new Exception("Unknown file mode for " + fileName);
                }
            }
            catch// Catch all as a need to handle specific exceptions differently is not envisaged.
            {
                Close();
                throw;
            }
        }

        public void Write(params string[] columns)
        {

            if(_writerStream==null)
            {
                throw new Exception("The writer stream has not been initialised. Please call the Open method");
            }
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        [Obsolete("Deprecated in favour of overloaded method",true)]
        public bool Read(string column1, string column2)
        {
            // Water tight this method if the readerStream has not been initialised.
            if(_readerStream==null)
            {
                throw new Exception("The reader stream has not been initialised. Please call the Open method");
            }
          
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();
            //if (line == null)
            //{
            //    column1 = null;
            //    column2 = null;

            //    return false;
            //}
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string column1, out string column2)
        {

            // If the readerStream has not been initialised raise red flag straightaway.
            if (_readerStream == null)
            {
                throw new Exception("The reader stream has not been initialised. Please call the Open method");
            }

            try
            {
                const int FIRST_COLUMN = 0;
                const int SECOND_COLUMN = 1;

                string line;
                string[] columns;

                char[] separator = { '\t' };

                line = ReadLine();

                if (line == null)
                {
                    column1 = null;
                    column2 = null;

                    return false;
                }

                columns = line.Split(separator);

                if (columns.Length == 0)
                {
                    column1 = null;
                    column2 = null;

                    return false;
                }
                else
                {
                    column1 = columns[FIRST_COLUMN];
                    column2 = columns[SECOND_COLUMN];

                    return true;
                }
            }
            catch
            {
                // The catch block deals with things going wrong. It closes the stream and rethrows the exception to bubble up the stack.
                column1 = null;
                column2 = null;
                CloseReaderStream();
                throw;
            }
        }

        private void WriteLine(string line)
        {
            try
            {
                _writerStream.WriteLine(line);
            }
            catch // Catch all as a need to handle specific exceptions differently is not envisaged.
            {
                CloseWriterStream();// This will flush and close the writer stream.
                throw;
            }
        }

        private string ReadLine()
        {
            //Note: The stream cannot be disposed here due to the nature of the functionality it provides.
            // Close the stream if there is any exception during the ReadLine operation like OutOfMemoryException.
            try
            {
                return _readerStream.ReadLine();
            }
            catch // Catch all as a need to handle specific exceptions differently is not envisaged.
            {
                CloseReaderStream();
                throw;
            }

            
        }

        public void Close()
        {

            CloseWriterStream();
            CloseReaderStream();
            
        }

        private void CloseWriterStream()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

        }

        private void CloseReaderStream()
        {
            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
