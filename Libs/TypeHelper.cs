using System.ComponentModel;

namespace Libs
{
    public static class TypeHelper
    {

        // List of numeric types
        private static readonly Type[] NumericTypes =
        {
        typeof(byte), typeof(sbyte), typeof(short), typeof(ushort),
        typeof(int), typeof(uint), typeof(long), typeof(ulong),
        typeof(float), typeof(double), typeof(decimal)
    };

        /// <summary>
        /// Verifica se o valor fornecido é numérico.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>Retorna verdadeiro se o valor for numérico; caso contrário, falso.</returns>
        public static bool IsNumber(object value)
        {
            if (value == null)
                return false;

            var type = value.GetType();

            if (NumericTypes.Contains(type))
                return true;

            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                return converter.CanConvertTo(typeof(decimal));
            }
            catch
            {
                throw new Exception("Erro ao determinar o tipo");
            }
        }

        /// <summary>
        /// Verifica se o valor fornecido é uma string.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>Retorna verdadeiro se o valor for uma string; caso contrário, falso.</returns>
        public static bool IsString(object value)
        {
            if (value == null)
                return false;

            return value.GetType() == typeof(string);
        }

        /// <summary>
        /// Verifica se o valor fornecido é um decimal.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>Retorna verdadeiro se o valor for um decimal; caso contrário, falso.</returns>
        public static bool IsDecimal(object value)
        {
            if (value == null)
                return false;

            return value.GetType() == typeof(decimal);
        }

        /// <summary>
        /// Verifica se o valor fornecido é um long.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>Retorna verdadeiro se o valor for um decimal; caso contrário, falso.</returns>
        public static bool IsLong(object value)
        {
            if (value == null)
                return false;

            return value.GetType() == typeof(long);
        }

        /// <summary>
        /// Verifica se o valor fornecido é um int.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>Retorna verdadeiro se o valor for um decimal; caso contrário, falso.</returns>
        public static bool IsInt(object value)
        {
            if (value == null)
                return false;

            return value.GetType() == typeof(int);
        }
    }
}
