using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.API.Utils
{
    public class EnumHelper
    {
        public static bool DoesStringExistInEnum(Type enumType, string str)
        {
            str = str.ToLower();
            foreach (string item in Enum.GetNames(enumType))
            {
                if (item.ToLower() == str)
                    return true;                
            }
            return false;
        }
    }
}
