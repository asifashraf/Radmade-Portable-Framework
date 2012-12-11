using System;
public static class IntEx
    {
		#region Methods (3) 

		// Public Methods (3) 

        /// <summary>
        /// tests if an integer is EVEN
        /// </summary>
        /// <param name="garbage"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static bool IsEven(this int garbage, int amount)
        {
            return amount % 2 == 0;
        }

		public static bool IsNullOrNotPositive(this int? item)
				{
					bool skip = false;

					if (item == null)
						skip = true;
					else
						if (item < 1)
							skip = true;

					return skip;
				}

        /// <summary>
        /// tests if the given integer is ODD
        /// </summary>
        /// <param name="garbage"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static bool IsOdd(this int garbage, int amount)
        {
            return !(amount % 2 == 0);
        }

		public static bool IsPositive(this int integer)
		{
			return integer > 0;
		}
		public static bool IsZero(this int integer)
		{
			return integer == 0;
		}
		public static bool IsNegative(this int integer)
		{
			return integer < 0;
		}
        public static int GetRandomNumber(this int minimum, int maximum)
        {
            var random = new Random();
            return random.Next(minimum, maximum + 1);
        }
		#endregion Methods 
    }

