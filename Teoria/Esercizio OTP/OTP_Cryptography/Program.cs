using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OTP_Cryptography
{
    class Crypto
    {
        private byte[] _key;
        public byte[] Key { get => _key; set { if (value.Length == 0) throw new ArgumentException("Key cannot have a length of 0"); else _key = value; } }
        private byte[] _cyphertext;

        public Crypto(byte[] key) => _key = key;

        public Crypto(string key) => _key = GetBytes(key);

        static public byte[] GetBytes(string data) => System.Text.Encoding.ASCII.GetBytes(data);
        static public string GetString(byte[] data) => System.Text.Encoding.ASCII.GetString(data);
        static public byte[] Padding(byte[] data, int length)
        {
            byte[] res = new byte[length];
            for (int i = 0; i < length; i++)
                res[i] = data[i%data.Length];

            return res;
        }


        public byte[] Cifra(byte[] plaintext)
        {
            byte[] cypherText = new byte[plaintext.Length];

            if (Key.Length < cypherText.Length)
                Key = Padding(plaintext, cypherText.Length);

            for (int i = 0; i < cypherText.Length; i++)
                cypherText[i] = (byte)(plaintext[i] ^ Key[i]);

            _cyphertext = cypherText;

            return cypherText;
        }

        public byte[] Cifra(string plaintext) => Cifra(GetBytes(plaintext));

        public byte[] Decifra() => Cifra(_cyphertext);

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Crypto OTP = new Crypto("funghetto allucinogeno");
            OTP.Cifra("Sono bellissimo");
            Console.WriteLine(Crypto.GetString(OTP.Decifra()));
        }
    }
}
