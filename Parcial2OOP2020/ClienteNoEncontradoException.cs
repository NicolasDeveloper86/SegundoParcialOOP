using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2OOP2020
{
    /// <summary>
    /// Representación en objetos de un cliente no encontrado.
    /// </summary>
    public class ClienteNoEncontradoException : Exception
    {
        public ClienteNoEncontradoException()
        {
        }

        public ClienteNoEncontradoException(string message) : base(message)
        {
        }
    }
}
