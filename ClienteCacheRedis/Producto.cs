﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteCacheRedis
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public string Imagen { get; set; }
    }
}
