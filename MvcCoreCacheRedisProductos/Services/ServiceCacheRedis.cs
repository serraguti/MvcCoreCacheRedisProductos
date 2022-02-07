using MvcCoreCacheRedisProductos.Helpers;
using MvcCoreCacheRedisProductos.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreCacheRedisProductos.Services
{
    public class ServiceCacheRedis
    {
        private IDatabase database;

        public ServiceCacheRedis()
        {
            //ALT + ENTER
            this.database =
                CacheRedisMultiplexer.GetConnection.GetDatabase();
        }

        //NECESITAMOS ALMACENAR VARIOS PRODUCTOS.
        //TENDREMOS UN METODO QUE RECIBIRA UN PRODUCTO Y LO GUARDARA EN UNA COLECCION
        public void AlmacenarFavoritoCache(Producto producto)
        {
            //CACHE REDIS FUNCIONA CON CLAVES DENTRO DE SU INTERIOR
            //DICHAS CLAVES DEBEN SER UNICAS, SINO SOBRESCRIBEN EL VALOR
            //LO QUE ALMACENEMOS DEBE SER EN FORMATO JSON
            //DEBEMOS RECUPERAR EL JSON QUE REPRESENTA TODOS LOS PRODUCTOS
            //FAVORITOS
            string jsonProductos = this.database.StringGet("favoritos");
            //NOSOTROS ALMACENAREMOS UNA COLECCION DE PRODUCTOS
            List<Producto> favoritos;
            //SI TODAVIA NO HEMOS ALMACENADO FAVORITOS jsonProductos
            //NO TENDRA VALOR
            if (jsonProductos == null)
            {
                //CREAMOS LA NUEVA LISTA DE PRODUCTOS FAVORITOS
                favoritos = new List<Producto>();
            }
            else
            {
                //YA TENEMOS PRODUCTOS EN CACHE REDIS, POR LO QUE LOS 
                //EXTRAEMOS
                favoritos =
                    JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
            }
            //AÑADIMOS EL NUEVO PRODUCTO A CACHE DE FAVORITOS
            favoritos.Add(producto);
            //CONVERTIMOS LOS NUEVOS FAVORITOS A FORMATO JSON
            jsonProductos = JsonConvert.SerializeObject(favoritos);
            //ALMACENAMOS DE NUEVO EN CACHE LOS DATOS
            this.database.StringSet("favoritos", jsonProductos);
        }

        public List<Producto> GetFavoritosCache()
        {
            string jsonProductos = this.database.StringGet("favoritos");
            if (jsonProductos == null)
            {
                return null;
            }
            else
            {
                List<Producto> favoritos =
                    JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
                return favoritos;
            }
        }

        public void EliminarFavoritoCache(int idproducto)
        {
            string jsonProductos = this.database.StringGet("favoritos");
            if (jsonProductos != null)
            {
                //SI TENEMOS CONTENIDO, PODEMOS ELIMINAR
                List<Producto> favoritos =
                    JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
                Producto favorito =
                    favoritos.FirstOrDefault(x => x.IdProducto == idproducto);
                //ELIMINAMOS EL FAVORITO
                favoritos.Remove(favorito);
                //DEBEMOS COMPROBAR SI SEGUIMOS TENIENDO PRODUCTOS
                if (favoritos.Count == 0)
                {
                    //YA NO HAY FAVORITOS, ELIMINAMOS LA KEY DE CACHE REDIS
                    this.database.KeyDelete("favoritos");
                }
                else
                {
                    //CONVERTIMOS LA LISTA ACTUALIZADA Y ALMACENAMOS DE NUEVO EN 
                    //CACHE
                    jsonProductos = JsonConvert.SerializeObject(favoritos);
                    this.database.StringSet("favoritos", jsonProductos, TimeSpan.FromMinutes(15));
                }
            }
        }
    }
}
