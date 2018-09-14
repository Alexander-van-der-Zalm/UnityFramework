using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Utility
{
    public static class EnumExt
    {
        /// <summary>
        /// Check to see if a flags enumeration has a specific flag set.
        /// </summary>
        /// <param name="variable">Flags enumeration to check</param>
        /// <param name="value">Flag to check for</param>
        /// <returns></returns>
        public static bool HasFlag(this Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException("value");

            // Not as good as the .NET 4 version of this function, but should be good enough
            if (!Enum.IsDefined(variable.GetType(), value))
            {
                throw new ArgumentException(string.Format(
                    "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                    value.GetType(), variable.GetType()));
            }

            int num = (int)(object)(value);
            return ((int)(object)variable & num) == num;

        }
        //From:
        //https://stackoverflow.com/questions/9207612/extend-enum-with-flag-methods
        /// <summary>
        /// Adds a flag value to enum.
        /// Please note that enums are value types so you need to handle the RETURNED value from this method.
        /// Example: myEnumVariable = myEnumVariable.AddFlag(CustomEnumType.Value1);
        /// </summary>
        public static T AddFlag<T>(this Enum type, T enumFlag)
        {
            try
            {
                return (T)(object)((int)(object)type | (int)(object)enumFlag);
            }            
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not append flag value {0} to enum {1}", enumFlag, typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Removes the flag value from enum.
        /// Please note that enums are value types so you need to handle the RETURNED value from this method.
        /// Example: myEnumVariable = myEnumVariable.RemoveFlag(CustomEnumType.Value1);
        /// </summary>
        public static T RemoveFlag<T>(this Enum type, T enumFlag)
        {
            try
            {
                return (T)(object)((int)(object)type & ~(int)(object)enumFlag);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not remove flag value {0} from enum {1}", enumFlag, typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Sets flag state on enum.
        /// Please note that enums are value types so you need to handle the RETURNED value from this method.
        /// Example: myEnumVariable = myEnumVariable.SetFlag(CustomEnumType.Value1, true);
        /// </summary>
        public static T SetFlag<T>(this Enum type, T enumFlag, bool value)
        {
            return value ? type.AddFlag(enumFlag) : type.RemoveFlag(enumFlag);
        }

    }
}
