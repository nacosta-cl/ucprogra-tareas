using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Nomop
    {
        private string[] almacen;
        public Nomop(string[] nombres)
        {
            almacen = nombres;
        }
        public Nomop()
        {
            string[] n = { "Steve", "Dell_Conhagen", "Xan", "Toolman", "Lightman", "Oso", "Fenix", "Tassadar", "Kachinsky", "Edd_Stark", "Gordon Freeman", "Voldomort", "Giskard", "Hari_Seldon", "Tyrion", "Ursa", "Arthas", "Illidan", "Uther", "Marth", "Epharim", "Beer", "Elli", "Imp", "1010011010", "Deckard" };
            almacen = n;
        }
        public string getnom()
        {
            Random n = new Random();
            int num = n.Next(0, almacen.Length);
            string escogido = almacen[num];
            bool listo = false;
            while (listo == false)
            {
                int a = escogido.Length;
                num = n.Next(0,almacen.Length);
                escogido = almacen[num];
                if (escogido != "O")
                {
                    listo = true;
                }

            }
            almacen[num]="O";
            return escogido;
        }
    }
}
