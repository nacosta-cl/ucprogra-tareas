using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Seccion de iniciacion              
            Random rnd = new Random();
            bool ingame = true;
            string[] lisnom = { "Doomers", "Zealots", "Cats", "DCC", "Lordaeron","Kalimdor","Isengard", "Smith_Agents","Sanctuary","HDusers",""};
            Nomop operadorequipos = new Nomop(lisnom);
            string equipo1 = operadorequipos.getnom();
            string equipo2 = operadorequipos.getnom();
            long tics = 0;
            Nomop operadornombre = new Nomop();
            Buscador be1 = new Buscador(29, 9, true, operadornombre.getnom(), '♀');
            Buscador be2 = new Buscador(42, 9, false, operadornombre.getnom(), '♀');
            Bludger b1 = new Bludger(0, 0, true); //36,16
            Bludger b2 = new Bludger(36, 2, false);
            Snitch s = new Snitch(36, 11);
            //Mapa
            Mapa m = new Mapa();
            //Operaciones de mapa
            Mapaops Operadormapa = new Mapaops();
            //No implementado
            #region
            //Pelotas
            //Mapa.Cancha[36, 16] = new Bludger(36, 16 , true);
            //Mapa.Cancha[36, 2] = new Bludger(36, 2, false);
            //Mapa.Cancha[36, 9] = new Quaffle(36, 9);
            //Mapa.Cancha[36, 11] = new Snitch(36, 11);
            //Snitch s = (Snitch)Mapa.Cancha[36, 11];
            ////Equipo 1
            //Mapa.Cancha[12, 3] = new Cazador(12, 3, true, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)16);
            //Mapa.Cancha[12, 9] = new Cazador(15, 9, true, Nombres[rnd.Next(0,Nombres.Length-1)], (char)16);
            //Mapa.Cancha[12, 15] = new Cazador(17, 15, true, Nombres[rnd.Next(0,Nombres.Length-1)], (char)16);
            //Mapa.Cancha[17,3] = new Golpeador(17, 3, true, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)121);
            //Mapa.Cancha[17,15] = new Golpeador(17, 15, true, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)121);
            //Mapa.Cancha[19,9] = new Buscador(29, 9, true, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)12);
            //Mapa.Cancha[1, 9] = new Guardian(1, 9, true, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)18);
            ////Equipo 2
            //Mapa.Cancha[61, 3] = new Cazador(61, 3, false, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)17);
            //Mapa.Cancha[61, 9] = new Cazador(61, 9, false, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)17);
            //Mapa.Cancha[61, 15] = new Cazador(61, 15, false, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)17);
            //Mapa.Cancha[56, 3] = new Golpeador(56, 3, false, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)122);
            //Mapa.Cancha[56, 15] = new Golpeador(56, 15, false, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)122);
            //Mapa.Cancha[42, 9] = new Buscador(42, 9, false, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)12);
            //Mapa.Cancha[72, 9] = new Guardian(73, 9, false, Nombres[rnd.Next(0, Nombres.Length - 1)], (char)18);
            #endregion //NI
            Operadormapa.reestablecer();
            Mapa.Personajes.Add(s);
            Mapa.Personajes.Add(be1);
            Mapa.Personajes.Add(be2);
            Mapa.Personajes.Add(b1);
            Mapa.Personajes.Add(b2);
            int puntosA = 0;
            int puntosB = 0;
            Console.Clear();
            Console.Beep();
            string alerta = "";
            //Simulación
            while (ingame == true)
            {
                tics += 1;
                Thread.Sleep(300);
                #region;
                Operadormapa.actualizar();
                foreach (Entidad i in Mapa.Personajes)
                {
                    i.Mover(0, 0, tics);
                }
                Operadormapa.mostrarenconsola(tics, puntosA, puntosB, equipo1, equipo2, alerta);
                if (alerta != "")
                {
                    Thread.Sleep(500);
                }
                alerta = "";
                #endregion
                //Console.ReadKey();
                if (Mapa.anotacion != 0)
                {
                    Operadormapa.reestablecer();
                    if (Mapa.anotacion < 0)
                    {
                        puntosB -= Mapa.anotacion;
                        alerta = equipo2 + " anotó 10 puntos";
                    }
                    else
                    {
                        puntosA += Mapa.anotacion;
                        alerta = equipo1 + " anotó 10 puntos";
                    }
                    Mapa.anotacion = 0;
                    Operadormapa.reestablecer();
                }
                if (Mapa.atajada == true)
                {
                    Mapa.atajada = false;
                    Operadormapa.reestablecer();
                }
                if (s.enPosesion == true)
                {
                    ingame = false;
                }
            }
            Console.ReadKey();
        }
    }
}