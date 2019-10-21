//using System;

//namespace Shos.Reversi.Core.Helpers
//{
//    public class MersenneTwister : Random
//    {
//        /// <summary>
//        /// 内部状態ベクトル総数
//        /// </summary>
//        protected const int N = 624;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const int M = 397;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const uint MATRIX_A = 0x9908b0dfU;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const uint UPPER_MASK = 0x80000000U;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const uint LOWER_MASK = 0x7fffffffU;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const uint TEMPER1 = 0x9d2c5680U;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const uint TEMPER2 = 0xefc60000U;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const int TEMPER3 = 11;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const int TEMPER4 = 7;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const int TEMPER5 = 15;
//        /// <summary>
//        /// MTを決定するパラメーターの一つ。
//        /// </summary>
//        protected const int TEMPER6 = 18;

//        /// <summary>
//        /// 内部状態ベクトル。
//        /// </summary>
//        protected UInt32[] mt;
//        /// <summary>
//        /// 内部状態ベクトルのうち、次に乱数として使用するインデックス。
//        /// </summary>
//        protected int mti;
//        private UInt32[] mag01;

//        /// <summary>
//        /// 現在時刻を種とした、MersenneTwister擬似乱数ジェネレーターを初期化します。
//        /// </summary>
//        public MersenneTwister() : this(Environment.TickCount) { }

//        /// <summary>
//        /// seedを種とした、MersenneTwister擬似乱数ジェネレーターを初期化します。
//        /// </summary>
//        public MersenneTwister(int seed)
//        {
//            mt = new UInt32[N];
//            mti = N + 1;
//            mag01 = new UInt32[] { 0x0U, MATRIX_A };
//            //内部状態配列初期化
//            mt[0] = (UInt32)seed;
//            for (int i = 1; i < N; i++)
//                mt[i] = (UInt32)(1812433253 * (mt[i - 1] ^ (mt[i - 1] >> 30)) + i);
//        }

//        /// <summary>
//        /// 符号あり32bitの擬似乱数を取得します。
//        /// </summary>
//        public override int Next() => (int)NextUInt();

//        /// <summary>
//        /// 符号なし64bitの擬似乱数を取得します。
//        /// </summary>
//        public virtual UInt64 NextUInt64() => ((UInt64)NextUInt() << 32) | NextUInt();

//        /// <summary>
//        /// 符号あり64bitの擬似乱数を取得します。
//        /// </summary>
//        public virtual Int64 NextInt64() => ((Int64)NextUInt() << 32) | NextUInt();

//        /// <summary>
//        /// 擬似乱数列を生成し、バイト配列に順に格納します。
//        /// </summary>
//        public override void NextBytes(byte[] buffer)
//        {
//            int i = 0;
//            UInt32 r;
//            while (i + 4 <= buffer.Length) {
//                r = NextUInt();
//                buffer[i++] = (byte)r;
//                buffer[i++] = (byte)(r >> 8);
//                buffer[i++] = (byte)(r >> 16);
//                buffer[i++] = (byte)(r >> 24);
//            }
//            if (i >= buffer.Length)
//                return;
//            r = NextUInt();
//            buffer[i++] = (byte)r;
//            if (i >= buffer.Length)
//                return;
//            buffer[i++] = (byte)(r >> 8);
//            if (i >= buffer.Length)
//                return;
//            buffer[i++] = (byte)(r >> 16);
//        }

//        /// <summary>
//        /// [0,1)の擬似乱数を取得します。
//        /// [0,1)を2^53個に均等にわけ、そのうち一つを返します。
//        /// NextUInt32を2回呼び出します。
//        /// </summary>
//        public override double NextDouble()
//            => (NextUInt() * (double)(2 << 11) + NextUInt()) / (double)(2 << 53);

//        /// <summary>
//        /// 符号なし32bitの擬似乱数を取得します。
//        /// </summary>
//        public virtual uint NextUInt()
//        {
//            if (mti >= N) {
//                gen_rand_all();
//                mti = 0;
//            }
//            var y = mt[mti++];
//            y ^= (y >> TEMPER3);
//            y ^= (y << TEMPER4) & TEMPER1;
//            y ^= (y << TEMPER5) & TEMPER2;
//            y ^= (y >> TEMPER6);
//            return y;
//        }

//        public virtual uint NextUInt(uint maxValue) => (uint)(NextUInt() / ((double)uint.MaxValue / maxValue));

//        /// <summary>
//        /// 内部状態ベクトルを更新します。
//        /// </summary>
//        void gen_rand_all()
//        {
//            int kk = 1;
//            uint p;
//            var y = mt[0] & UPPER_MASK;
//            do {
//                p = mt[kk];
//                mt[kk - 1] = mt[kk + (M - 1)] ^ ((y | (p & LOWER_MASK)) >> 1) ^ mag01[p & 1];
//                y = p & UPPER_MASK;
//            } while (++kk < N - M + 1);
//            do {
//                p = mt[kk];
//                mt[kk - 1] = mt[kk + (M - N - 1)] ^ ((y | (p & LOWER_MASK)) >> 1) ^ mag01[p & 1];
//                y = p & UPPER_MASK;
//            } while (++kk < N);
//            p = mt[0];
//            mt[N - 1] = mt[M - 1] ^ ((y | (p & LOWER_MASK)) >> 1) ^ mag01[p & 1];
//        }
//    }

//    //public class MersenneTwister : Random
//    //{
//    //    // Period parameters
//    //    const int N = 624;
//    //    const int M = 397;
//    //    const uint MATRIX_A = 0x9908b0df;   // constant vector a
//    //    const uint UPPER_MASK = 0x80000000; // most significant w-r bits
//    //    const uint LOWER_MASK = 0x7fffffff; // least significant r bits

//    //    // Tempering parameters
//    //    const uint TEMPERING_MASK_B = 0x9d2c5680;
//    //    const uint TEMPERING_MASK_C = 0xefc60000;

//    //    static uint TEMPERING_SHIFT_U(uint y) { return (y >> 11); }
//    //    static uint TEMPERING_SHIFT_S(uint y) { return (y <<  7); }
//    //    static uint TEMPERING_SHIFT_T(uint y) { return (y << 15); }
//    //    static uint TEMPERING_SHIFT_L(uint y) { return (y >> 18); }

//    //    uint[] mt = new uint[N]; // the array for the state vector

//    //    short mti;

//    //    static uint[] mag01 = { 0x0, MATRIX_A };

//    //    // initializing the array with a NONZERO seed
//    //    public MersenneTwister(uint seed)
//    //    {
//    //        //setting initial seeds to mt[N] using the generator Line 25 of Table 1 in [KNUTH 1981, The Art of Computer Programming Vol. 2(2nd Ed.), pp102]                  
//    //        mt[0] = seed & 0xffffffffU;
//    //        for (mti = 1; mti < N; mti++)
//    //            mt[mti] = (69069 * mt[mti - 1]) & 0xffffffffU;
//    //    }

//    //    public MersenneTwister() : this((uint)Environment.TickCount/*4357*/) // a default initial seed is used
//    //    {}

//    //    protected uint GenerateUInt()
//    //    {
//    //        uint y;

//    //        /* mag01[x] = x * MATRIX_A  for x=0,1 */
//    //        if (mti >= N) { // generate N words at one time
//    //            short kk = 0;

//    //            for (; kk < N - M; ++kk) {
//    //                y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
//    //                mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1];
//    //            }

//    //            for (; kk < N - 1; ++kk) {
//    //                y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
//    //                mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1];
//    //            }

//    //            y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
//    //            mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1];

//    //            mti = 0;
//    //        }

//    //        y = mt[mti++];
//    //        y ^= TEMPERING_SHIFT_U(y);
//    //        y ^= TEMPERING_SHIFT_S(y) & TEMPERING_MASK_B;
//    //        y ^= TEMPERING_SHIFT_T(y) & TEMPERING_MASK_C;
//    //        y ^= TEMPERING_SHIFT_L(y);

//    //        return y;
//    //    }

//    //    public virtual uint NextUInt() => GenerateUInt();

//    //    public virtual uint NextUInt(uint maxValue) => (uint)(GenerateUInt() / ((double)uint.MaxValue / maxValue));

//    //    //public virtual uint NextUInt(uint minValue, uint maxValue) // throws ArgumentOutOfRangeException
//    //    //{
//    //    //    if (minValue > maxValue)
//    //    //        throw new ArgumentOutOfRangeException();
//    //    //    return (uint)(this.GenerateUInt() / ((double)uint.MaxValue / (maxValue - minValue)) + minValue);
//    //    //}

//    //    public override int Next() => Next(int.MaxValue);

//    //    public override int Next(int maxValue) // throws ArgumentOutOfRangeException
//    //    {
//    //        if (maxValue <= 1) {
//    //            if (maxValue < 0)
//    //                throw new ArgumentOutOfRangeException();
//    //            return 0;
//    //        }
//    //        return (int)(this.NextDouble() * maxValue);
//    //    }

//    //    public new int Next(int minValue, int maxValue)
//    //    {
//    //        if (maxValue < minValue)
//    //            throw new ArgumentOutOfRangeException();
//    //        return maxValue == minValue ? minValue
//    //                                    : Next(maxValue - minValue) + minValue;
//    //    }

//    //    public override void NextBytes(byte[] buffer) // throws ArgumentNullException
//    //    {
//    //        if (buffer == null)
//    //            throw new ArgumentNullException();
//    //        for (var index = 0; index < buffer.Length; index++)
//    //            buffer[index] = (byte)this.Next(256);
//    //    }

//    //    public override double NextDouble() => (double)this.GenerateUInt() / ((ulong)uint.MaxValue + 1);
//    //}
//}
