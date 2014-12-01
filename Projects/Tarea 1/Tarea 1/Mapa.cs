using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tarea_1
{
    public class Mapa
    {
        static public Entidad[,] Cancha = new Entidad[76, 21];
        static public char[,] mapaimp = new char[80, 25];
        static public List<Entidad> Personajes = new List<Entidad>();
        static public int anotacion = 0;
        static public bool atajada = false;
    }
}
