using System;
using System.Collections.Generic;

namespace ClienteCacheRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCacheRedis service = new ServiceCacheRedis();
            string fin = "y";
            while (fin.ToLower() != "n")
            {
                List<Producto> favoritos = service.GetFavoritosCache();
                if (favoritos == null)
                {
                    Console.WriteLine("No existen productos favoritos");
                }
                else
                {
                    int i = 1;
                    foreach (Producto producto in favoritos)
                    {
                        Console.WriteLine(i + ".- " + producto.Nombre);
                        i += 1;
                    }
                    Console.WriteLine("---------------------------");
                }
                Console.WriteLine("¿Desea seguir cargando productos? (y/n)");
                fin = Console.ReadLine().ToLower();
                Console.WriteLine("Fin de programa");
            }
        }
    }
}
