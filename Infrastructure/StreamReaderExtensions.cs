using System.IO;
using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Geometry
{
    public static class StreamReaderExtensions
    {
        static int DigitStartPosition = 48;

        public static string Read(this StreamReader reader, int count)
        {
            reader.ValidateStreamNotReachedEnd();

            var sb = new StringBuilder();
            for (int i = 0; i < count && !reader.EndOfStream; i++)
            {
                sb.Append((char)reader.Read());
            }

            return sb.ToString();
        }

        public static void TrimWhitespaces(this StreamReader reader)
        {
            reader.ValidateStreamNotReachedEnd();
            
            while ((char)reader.Peek() == ' ')
            {
                reader.Read();
            }
        }

        public static string ReadToSymbols(this StreamReader reader, params char[] symbols)
        {
            reader.ValidateStreamNotReachedEnd();

            var sb = new StringBuilder();
            sb.Append((char)reader.Read());

            var peek = (char)reader.Peek();

            while (!symbols.Contains(peek) && !reader.EndOfStream) 
            {
                sb.Append((char)reader.Read());
                peek = (char)reader.Peek();
            }

            return sb.ToString();
        }

        public static void SkipToSymbols(this StreamReader reader, params char[] symbols)
        {
            reader.ValidateStreamNotReachedEnd();

            while (!symbols.Contains((char)reader.Peek()) && !reader.EndOfStream)
            {
                reader.Read();
            }
        }

        public static void SkipToSymbolsNoExcept(this StreamReader reader, params char[] symbols)
        {
            while (!symbols.Contains((char)reader.Peek()) && !reader.EndOfStream)
            {
                reader.Read();
            }
        }

        public static float ReadFloat32(this StreamReader reader)
        {
            reader.ValidateStreamNotReachedEnd();
            reader.TrimWhitespaces();

            bool isNegative = false;

            if ((char)reader.Peek() == '-')
            {
                isNegative = true;
                reader.Read();
            }

            float value = 0;

            foreach (var digit in reader.ReadDigits())
                value = checked(value * 10 + digit);

            if ((char)reader.Peek() == '.')
            {
                reader.Read();

                float power = 10;
                int k = 1;

                foreach (var digit in reader.ReadDigits())
                {
                    //Round here in case of problems with accurance (disabling it will increase prefomance)
                    value = (float)Math.Round(checked(value + (float)digit / power), k);
                    power *= 10;
                    k++;
                }
            }

            return isNegative ? -value : value;
        }

        public static int ReadInt32(this StreamReader reader)
        {
            reader.ValidateStreamNotReachedEnd();
            reader.TrimWhitespaces();

            bool isNegative = false;

            if ((char)reader.Peek() == '-') 
            {
                isNegative = true;
                reader.Read();
            }

            int value = 0;

            foreach(var digit in reader.ReadDigits())
                value = checked(value * 10 + digit);

            return isNegative ? -value : value;
        }

        private static IEnumerable<int> ReadDigits(this StreamReader reader)
        {
            reader.ValidateStreamNotReachedEnd();

            while(char.IsDigit((char)reader.Peek()) && !reader.EndOfStream)
                yield return (char)reader.Read() - DigitStartPosition;
        }

        private static void ValidateStreamNotReachedEnd(this StreamReader reader)
        {
            if(reader.EndOfStream) throw new EndOfStreamException(nameof(reader));
        }
    }
}