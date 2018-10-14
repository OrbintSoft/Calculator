namespace Calculator
{
    /// <summary>
    /// Supported operations
    /// </summary>
    public enum OperationsEnum
    {
        Addition = 0,
        Substraction = 1,
        Multiplication = 2,
        Division = 3,
        Power = 4,
        Root = 5,
        Logarithm = 6,
        Expression = 7
    }

    /// <summary>
    /// Extend OperationsEnum provinding useful methods for conversion
    /// </summary>
    public static class OperationsEnumExtensions
    {
        /// <summary>
        /// Return the opcode mapped to the operation
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <returns>Opcode</returns>
        public static int GetOpCode(this OperationsEnum operation)
        {
            return (int)operation;
        }

        /// <summary>
        /// Returns the localized name of the operation
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <returns>localized name string</returns>
        public static string ToString(this OperationsEnum operation)
        {
            switch (operation)
            {
                case OperationsEnum.Addition:
                    return LocalizationManager.GetString("Addition");
                case OperationsEnum.Substraction:
                    return LocalizationManager.GetString("Substraction");
                case OperationsEnum.Multiplication:
                    return LocalizationManager.GetString("Multiplication");
                case OperationsEnum.Division:
                    return LocalizationManager.GetString("Division");
                case OperationsEnum.Power:
                    return LocalizationManager.GetString("Power");
                case OperationsEnum.Root:
                    return LocalizationManager.GetString("Root");
                case OperationsEnum.Logarithm:
                    return LocalizationManager.GetString("Logarithm");
                case OperationsEnum.Expression:
                    return LocalizationManager.GetString("Expression");
                default:
                    return LocalizationManager.GetString("Error");
            }
        }
    }
}
