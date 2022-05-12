using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayLib.Utils
{
    public static class MiscUtils
    {
        public static T[] ShiftArray<T>(this T[] array, T newFirst)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                array[i] = array[i - 1];
            }
            array[0] = newFirst;
            return array;
        }

        public static T[] FillArray<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }
    }
}
