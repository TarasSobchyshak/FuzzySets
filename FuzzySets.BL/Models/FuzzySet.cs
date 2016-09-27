using System;
using static System.Math;

namespace FuzzySets.Logic.Models
{
    public class FuzzySet
    {
        private Func<double, double> _membershipFunction;

        public FuzzySet(Func<double, double> membershipFunction)
        {
            Mf = membershipFunction;
        }

        /// <summary>
        /// Fuzzy set membership function.
        /// </summary>
        public Func<double, double> Mf
        {
            get { return _membershipFunction; }
            set { _membershipFunction = value; }
        }

        #region Operations
        public double this[double x] => Mf(x);

        /// <summary>
        /// Standard complement. Mf = 1 - A[x]
        /// </summary>
        public static FuzzySet operator !(FuzzySet A) => new FuzzySet(x => 1 - A[x]);

        /// <summary>
        /// Concentration. Mf = A[x] * A[x]
        /// </summary>
        public static FuzzySet operator ~(FuzzySet A) => new FuzzySet(x => Pow(A[x], 2));

        /// <summary>
        /// Intersection of first type. Mf = Min(A[x], B[x])
        /// </summary>
        public static FuzzySet operator *(FuzzySet A, FuzzySet B)
            => new FuzzySet(x => Min(A[x], B[x]));

        /// <summary>
        /// Intersection of second type. Mf = Max(0, A[x] + B[x] - 1)
        /// </summary>
        public static FuzzySet operator &(FuzzySet A, FuzzySet B)
            => new FuzzySet(x => Max(0, A[x] + B[x] - 1));

        /// <summary>
        /// Intersection of third type. Mf = A[x] * B[x]
        /// </summary>
        public static FuzzySet operator ^(FuzzySet A, FuzzySet B)
            => new FuzzySet(x => A[x] * B[x]);

        /// <summary>
        /// Union of first type. Mf = Max(A[x], B[x])
        /// </summary>
        public static FuzzySet operator +(FuzzySet A, FuzzySet B)
            => new FuzzySet(x => Max(A[x], B[x]));

        /// <summary>
        /// Union of second type. Mf = Min(1, A[x] + B[x])
        /// </summary>
        public static FuzzySet operator |(FuzzySet A, FuzzySet B)
            => new FuzzySet(x => Min(1, A[x] + B[x]));

        /// <summary>
        /// Union of third type. Mf = A[x] + B[x] - A[x] * B[x]
        /// </summary>
        public static FuzzySet operator %(FuzzySet A, FuzzySet B)
            => new FuzzySet(x => A[x] + B[x] - A[x] * B[x]);

        /// <summary>
        /// Difference. Max(0, A[x] - B[x])
        /// </summary>
        public static FuzzySet operator /(FuzzySet A, FuzzySet B)
            => new FuzzySet(x => Max(0, A[x] - B[x]));
        #endregion
    }
}
