using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Mapaops
    {
        public char[,] mapaimp;
        public Mapaops()
        {
            this.crearMapa();
        }
        private void crearMapa()
        {
            this.mapaimp = new char[80, 25];
            this.mapaimp[0, 0] = ' ';
            this.mapaimp[3, 9] = 'O';
            this.mapaimp[3, 12] = 'O';
            this.mapaimp[3, 15] = 'O';
            this.mapaimp[76, 9] = 'O';
            this.mapaimp[76, 12] = 'O';
            this.mapaimp[76, 15] = 'O';
            for (int i = 2; i < 78; i++)
            {
                this.mapaimp[i, 1] = '■';
                this.mapaimp[i, 23] = '■';
            }
            for (int j = 2; j < 23; j++)
            {
                this.mapaimp[2, j] = '█';
                this.mapaimp[77, j] = '█';
            }
        }
        public void vaciar()
        {
            this.mapaimp = new char[80, 25];
        }
        public void reestablecer()
        {
            Random rnd = new Random();
            Mapa.Personajes = new List<Entidad>();
            //Implementar clase que obtenga nombres
            //Nombres Reponom = new Nombres({,,,});
            //Reponom.sacarNombre();
            Nomop nombres = new Nomop();
            //Pelotas

            //Equipo 1 izquierda + true
            Cazador c1e1 = new Cazador(12, 3, true, nombres.getnom(), '►');
            Cazador c2e1 = new Cazador(12, 9, true, nombres.getnom(), '►');
            Cazador c3e1 = new Cazador(12, 15, true, nombres.getnom(), '►');
            Golpeador g1e1 = new Golpeador(17, 3, true, nombres.getnom(), '♠');
            Golpeador g2e1 = new Golpeador(17, 15, true, nombres.getnom(), '♠');

            Guardian ge1 = new Guardian(1, 9, true, nombres.getnom(), (char)18);
            //Equipo 2 derecha - false
            Cazador c1e2 = new Cazador(61, 3, false, nombres.getnom(), '◄');
            Cazador c2e2 = new Cazador(61, 9, false, nombres.getnom(), '◄');
            Cazador c3e2 = new Cazador(61, 15, false, nombres.getnom(), '◄');
            Golpeador g1e2 = new Golpeador(56, 3, false, nombres.getnom(), '♥');
            Golpeador g2e2 = new Golpeador(56, 15, false, nombres.getnom(), '♥');
            Guardian ge2 = new Guardian(72, 9, false, nombres.getnom(), (char)18);
            //Mas pelotas
            Quaffle q = new Quaffle(36, 9);
            #region

            Mapa.Personajes.Add(c1e1);
            Mapa.Personajes.Add(c2e1);
            Mapa.Personajes.Add(c3e1);
            Mapa.Personajes.Add(g1e1);
            Mapa.Personajes.Add(g2e1);

            Mapa.Personajes.Add(ge1);
            Mapa.Personajes.Add(c1e2);
            Mapa.Personajes.Add(c2e2);
            Mapa.Personajes.Add(c3e2);
            Mapa.Personajes.Add(g1e2);
            Mapa.Personajes.Add(g2e2);

            Mapa.Personajes.Add(ge2);

            Mapa.Personajes.Add(q);

            #endregion
        }
        public void actualizar()
        {
            this.crearMapa();
            int x = 0;
            int y = 0;
            foreach (Entidad sujeto in Mapa.Personajes)
            {
                x = sujeto.posx;
                y = sujeto.posy;
                this.mapaimp[x + 3, y + 2] = sujeto.forma;
            }
        }
        public void mostrarenconsola(long tics, int puntosA, int puntosB, string nombreA, string nombreB, string alertas)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Quidditch - tics = ");
            Console.Write(tics);
            Console.Write(" | ");
            Console.Write(nombreB);
            Console.Write(" : ");
            Console.Write(puntosB);
            Console.Write(" - ");
            Console.Write(nombreA);
            Console.Write(" : ");
            Console.WriteLine(puntosA);
            for (int j = 1; j < 24; j++)
            {
                for (int i = 0; i < 80; i++)
                {
                    Console.Write(this.mapaimp[i, j]);
                }
            }
            if (alertas!="")
            Console.WriteLine(alertas);
        }
    }
}
