namespace Shos.Reversi.Core.Helpers
{
    public struct Coset
    {
        public int Modulo;
        public int Value ;

        public Coset(int modulo = 2, int value = 0)
        {
            Modulo = modulo;
            Value  = value ;
            Normalize();
        }

        public static Coset operator ++(Coset coset) => new Coset(coset.Modulo, coset.Value + 1);
        public static Coset operator --(Coset coset) => new Coset(coset.Modulo, coset.Value - 1);

        public void Normalize()
        {
            while (Value <  0     ) Value += Modulo;
            while (Value >= Modulo) Value -= Modulo;
        }
    }
}
