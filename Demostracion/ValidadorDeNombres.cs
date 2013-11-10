using Prueba.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demostracion
{
    public class ValidadorDeNombres
    {
        public string Validar( dynamic clienteDynamic )
        {
            ICliente cliente = DuckType.As<ICliente>( clienteDynamic );
            return "Nombre válido: " + cliente.Nombre;
        }

        internal string ValidarHijo( dynamic clienteConHijoAnonymousType )
        {
            ICliente cliente = DuckType.As<ICliente>( clienteConHijoAnonymousType.Hijo );
            return "Nombre válido: " + cliente.Nombre;
        }
    }
}
