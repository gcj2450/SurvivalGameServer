using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace SurvivalGameServer
{
    internal class Encryption : IDisposable
    {
        private RSAParameters privateKey, publicKey;
        public string publicKeyInString;
        private RSACryptoServiceProvider csp;

        public Encryption()
        {            
            //create keys
            csp = new RSACryptoServiceProvider(2048);
            privateKey = csp.ExportParameters(true);
            publicKey = csp.ExportParameters(false);
            


            //conver key into string
            var sw = new System.IO.StringWriter();
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, publicKey);
            publicKeyInString = sw.ToString();

        }

        public byte[] GetSecretKey(string encoded_bytes)
        {
            //getting back real public key by public key string
            var sr = new System.IO.StringReader(encoded_bytes);
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(byte[]));
            byte[] preres = (byte[])xs.Deserialize(sr);

            byte[] res = csp.Decrypt(preres, false);
            //Console.WriteLine("secret key is - " + FromByteToString(res));
            return res;
        }


        public void Dispose()
        {
            csp.Dispose();
        }

        public byte[] GetByteArrFromCharByChar(string key_in_string)
        {
            if (string.IsNullOrEmpty(key_in_string)) return new byte[1] { 0 };

            List<byte> result = new List<byte>();

            for (int i = 0; i < key_in_string.Length; i++)
            {
                result.Add(Byte.Parse(key_in_string.Substring(i, 1)));
            }

            return result.ToArray();
        }


        public static byte[] TakeSomeToArrayTO(Span<byte> source, int numberToTake)
        {
            if (numberToTake > source.Length)
            {
                numberToTake = source.Length;
            }

            return source.Slice(0, numberToTake).ToArray();
        }

        public static byte[] TakeSomeToArrayFromNumber(ReadOnlySpan<byte> source, int fromNumber)
        {
            if (fromNumber > source.Length)
            {
                fromNumber = source.Length - 1;
            }


            return source.Slice(fromNumber, (source.Length - fromNumber)).ToArray();
        }



        public static void Encode(ref byte[] source, byte[] key, Globals.PacketCode code)
        {
            if (source == null || key == null)
            {
                return;
            }

            int index = source.Length < key.Length ? source.Length : key.Length;

            for (int i = 0; i < index; i++)
            {
                source[i] = (byte)(source[i] + key[i]);
            }
            source = new byte[1] { (byte)code }.Concat(source).ToArray();
        }


        public static void Decode(ref byte[] source, byte[] key)
        {
            if (source.Length == 0 || key.Length == 0) return;

            int index = source.Length < key.Length ? source.Length : key.Length;

            for (int i = 0; i < index; i++)
            {
                source[i] = (byte)(source[i] - key[i]);
            }
        }

        public static string FromByteToString(byte[] data)
        {
            StringBuilder d = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                d.Append(data[i]);
            }

            return d.ToString();
        }

        public static string FromByteToStringWithDelimiter(byte[] data)
        {
            StringBuilder d = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                d.Append(data[i] + " - ");
            }

            return d.ToString();
        }

        public static byte[] GetHash384(string data)
        {
            SHA384 create_hash = SHA384.Create();
            return create_hash.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
