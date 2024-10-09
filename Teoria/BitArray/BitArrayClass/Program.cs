using System.Security.Cryptography;

namespace BitArrayClass
{
    class BitArray
    {
        private const int BITS_PER_VALUE = 32;
        private uint[] bits;
        private long n_bits;

        public long Length { get { return n_bits; } private set { } }

        public BitArray(long n_bits)
        {
            long n_uints = n_bits / BITS_PER_VALUE;
            if (n_bits % BITS_PER_VALUE != 0)
                ++n_uints;  // serve un (uint) in più per i bits residui

            if (n_uints > int.MaxValue - 60)  // il valore massimo è stato trovato per tentativi
                throw new OverflowException("Too many bits");

            this.bits = new uint[n_uints];
            this.n_bits = n_bits;
        }

        public BitArray(long n_bits, bool initial_value) : 
            this(n_bits) { SetAllBits(initial_value); }


        public void SetAllBits(bool value)  // imposta tutti i bit a true o false
        {
            uint _value = 0;
            if (value)
                _value = ~_value;

            for (int i = 0; i < bits.Length; ++i)
                bits[i] = _value;
        }

        public bool GetBit(long bit_index)
        {
            if (bit_index < 0 || bit_index > n_bits) throw new IndexOutOfRangeException("Cannot access bits outside of the array");

            long index = bit_index / BITS_PER_VALUE;
            bit_index %= BITS_PER_VALUE;

            uint mask = (uint)(1 << (BITS_PER_VALUE - 1 - (int)bit_index));
            return (mask & bits[index]) != 0;
        }

        public void SetBit(long bit_index, bool value)
        {
            if (bit_index < 0 || bit_index >= n_bits) throw new IndexOutOfRangeException("Cannot access bits outside of the array");

            long index = bit_index / BITS_PER_VALUE;
            bit_index %= BITS_PER_VALUE;

            uint mask = (uint)(1 << (BITS_PER_VALUE - 1 - (int)bit_index));

            if (value) bits[index] |= mask;
            else bits[index] &= ~mask;
        }

        public bool this[long bit_index] { get => GetBit(bit_index); set => SetBit(bit_index, value); }

        public static BitArray operator ~(BitArray arr){
            BitArray result = new BitArray(arr.Length);
            for (int i = 0; i < arr.Length; ++i)
                result[i] = arr[i] ? false : true;

            return result;
        }

        public static BitArray operator |(BitArray arr1, BitArray arr2)
        {
            BitArray result = new BitArray(long.Max(arr1.Length, arr2.Length));
            for (long i = 0; i < result.Length; ++i)
            {
                result[i] = arr1[i%arr1.Length] || arr2[i%arr2.Length];
            }

            return result;

        }

        public static BitArray operator &(BitArray arr1, BitArray arr2)
        {
            BitArray result = new BitArray(long.Max(arr1.Length, arr2.Length));
            for (long i = 0; i < result.Length; ++i)
            {
                result[i] = arr1[i%arr1.Length] && arr2[i%arr2.Length];
            }

            return result;

        }

        public static BitArray operator ^(BitArray arr1, BitArray arr2)
        {
            BitArray result = new BitArray(long.Max(arr1.Length, arr2.Length));
            for (long i = 0; i < result.Length; ++i)
            {
                result[i] = arr1[i % arr1.Length] ^ arr2[i % arr2.Length];
            }

            return result;

        }

        public override string ToString()
        {
            string result = string.Empty;
            for (int i = 0; i < n_bits; i++)
                result += this[i] ? '1' : '0';
            
            return result;
        }
    }
    internal class Program
    {
        static List<long> EratosthenesSieve(long max_value)
        {
            List<long> primes = new List<long>();

            BitArray sieve = new BitArray(max_value + 1, true);
            for (long n = 2; n <= max_value; ++n)
            {
                if (sieve[n])  // se true, allora n è primo
                {
                    primes.Add(n);
                    for (long mult_n = n + n; mult_n <= max_value; mult_n += n)  // genera tutti i multipli di n, che vanno marcati come non-primi
                    {
                        sieve[mult_n] = false;
                    }
                }
            }

            return primes;
        }
        static void Main(string[] args)
        {

#if true
            List<long> primes = EratosthenesSieve(64_000_000);

            foreach (long n in primes)
                Console.Write($"{n}, ");
            Console.WriteLine();
#endif
        }
    }
}
