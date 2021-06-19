using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2OOP2020
{
    public class ClienteConDeudaException : Exception
    {
        public ClienteConDeudaException()
        {
        }

        public ClienteConDeudaException(string message) : base(message)
        {
        }
    }
}
