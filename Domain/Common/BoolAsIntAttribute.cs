using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BoolAsIntAttribute : Attribute
    {
    }
}
