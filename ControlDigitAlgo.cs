using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static IEnumerable<int> ToDigits(this long number)
        {
            var numberString = number.ToString();
            var digits = numberString
                .ToCharArray()
                .Select(GetIntValueFromChar)
                .ToArray();

            return digits;
        }

        private static int GetIntValueFromChar(char value)
        {
            return (int)char.GetNumericValue(value);
        }

        public static int GetTotalSum(this int[] source, Func<int, int> func)
        {
            var sum = 0;

            for (var index = 0; index < source.Length; index++)
            {
                var value = source[index];
                sum += value * func(index);
            }

            return sum;
        }        
    }

    public static class ControlDigitAlgo
    {
        // ReSharper disable once InconsistentNaming
        private const int UPCMultiplier = 3;
        
        public static int Upc(long number)
        {
            var digits = number.ToDigits();
            var reverseDigits = digits.Reverse().ToArray();
            var totalSum = reverseDigits.GetTotalSum(index => index % 2 == 0 ? UPCMultiplier : 1);

            return GetUPCResult(totalSum);
        }

        // ReSharper disable once InconsistentNaming
        private static int GetUPCResult(int totalSum)
        {
            var remainder = totalSum % 10;
            
            return remainder == 0 ? 0 : 10 - remainder;
        }

        public static char Isbn10(long number)
        {
            var digits = number.ToDigits();
            var reverseDigits = digits.Reverse().ToArray();
            var totalSum = reverseDigits.GetTotalSum(index => 2 + index);
            
            return GetIsbn10Result(totalSum);
        }

        private static char GetIsbn10Result(int totalSum)
        {
            var remainder = totalSum % 11;
            if (remainder == 0)
                return '0';
            var result = 11 - remainder;

            return result == 10 ? 'X' : (char)(result + 48);
        }        
        
        public static int Luhn(long number)
        {
            var digits = number.ToDigits();
            var reverseDigits = digits.Reverse().ToArray();
            for (var index = 0; index < reverseDigits.Length; index++)
                if (index % 2 == 0)
                    reverseDigits[index] *= 2;

            return GetLuhnResult(reverseDigits);
        }
        
        private static int GetLuhnResult(int[] digits)
        {
            for (var index = 0; index < digits.Length; index++)
                if (digits[index] >= 10)
                    digits[index] = ((long)digits[index]).ToDigits().Sum();
            var remainder = digits.GetTotalSum(index => 1) % 10;
            if (remainder == 0)
                return 0;
            
            return 10 - remainder;
        }
    }
}
