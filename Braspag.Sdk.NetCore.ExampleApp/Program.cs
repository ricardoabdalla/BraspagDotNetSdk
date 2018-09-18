using System;

namespace Braspag.Sdk.NetCore.ExampleApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Braspag SDK Example App - C# / .NET Core");
            Console.WriteLine();

            /* Exemplo de uso do SDK para as APIs do Pagador */
            PagadorDemo.Run();

            /* Exemplo de uso do SDK para as APIs do Cartão Protegido */
            CartaoProtegidoDemo.Run();

            /* Exemplo de uso do SDK para as APIs do Velocity */
            VelocityDemo.Run();

            Console.WriteLine();
            Console.WriteLine("Press any key to close the app...");
            Console.ReadKey();
        }
    }
}
