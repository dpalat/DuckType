using Prueba.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demostracion
{
    public class Principal
    {
        static void Main( string[] args )
        {
            Console.WriteLine( "Demostración de tipificación dinamica de objetos" );
            Console.WriteLine( "'Cuando veo un ave que camina como un pato, nada como un" );
            Console.WriteLine( " pato y suena como un pato, a esa ave yo la llamo un pato.'" );
            Console.WriteLine( " - James Whitcomb Riley - " );
            Console.WriteLine( "" );

            ICalculatodora calculator = DuckType.As<ICalculatodora>( new ElementoQueSabeCalcular() );
            var resultado = calculator.Sumar( 2, 3 );
            Console.WriteLine( "El resultado es: " + resultado.ToString() );

            var clienteDynamic = new ClienteDynamic();
            clienteDynamic.Nombre = "Carlos Carrasco";
            ValidadorDeNombres validador = new ValidadorDeNombres();
            Console.WriteLine( validador.Validar( clienteDynamic ) );

            var clienteAnonymousType = new { Nombre = "Jhon Malcovich" };
            Console.WriteLine( validador.Validar( clienteAnonymousType ) );


            var clienteConHijoAnonymousType = new { Nombre = "John Malkovich", Hijo = new { Nombre = "Amandine Malkovich" } };
            Console.WriteLine( validador.ValidarHijo( clienteConHijoAnonymousType ) );

            Console.ReadLine();
        }

        public interface ICalculatodora
        {
            int Sumar( int a, int b );
            int Restar( int a, int b );
        }

        public class ElementoQueSabeCalcular
        {
            public int Sumar( int a, int b )
            { 
                return a + b;
            }
        }

        public class ClienteDynamic
        {
            private string nombre;
            public string Nombre
            {
                get { return nombre; }
                set { nombre = value; }
            }
            
        }

    }
}
