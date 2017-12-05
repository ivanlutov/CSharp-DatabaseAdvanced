namespace TeamBuilder.App.Utilities
{
    using System;

    public static class Check
    {
        public static void CheckLength(int expectedLenght, string[] array)
        {
            if (expectedLenght != array.Length)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }
        }

        public static void CheckLengthTwoLengths(int firstExpectedLenght, int secondExpectedLength, string[] array)
        {
            if (firstExpectedLenght != array.Length && secondExpectedLength != array.Length)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }
        }
    }
}