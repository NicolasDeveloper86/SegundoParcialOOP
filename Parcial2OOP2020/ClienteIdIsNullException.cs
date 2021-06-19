using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2OOP2020
{
    /// <summary>
    /// Representación en objetos de un cliente con Id nulo.
    /// </summary>
    public class ClienteIdIsNullException : Exception
    {
        public ClienteIdIsNullException()
        {
        }

        public ClienteIdIsNullException(string message) : base(message)
        {
        }
    }
}
