using System;
using System.Linq;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        // Вспомогательные методы-расширения поместите в этот класс.
        // Они должны быть понятны и потенциально полезны вне контекста задачи расчета контрольных разрядов.
        
        public static int[] ToDigits(this long number)
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
            throw new NotImplementedException();
        }
    }
}
