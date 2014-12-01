using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Quaffle : Pelota
    {
        public int enPosesion = 0;
        public Cazador Dueño;
        public Quaffle(int posx, int posy) : base(posx, posy)
        {
            this.forma = '☼';
        }
        public override bool Mover(int pfx, int pfy, long tics)
        {
            if (enPosesion == 1)
            {
                foreach (Entidad i in Mapa.Personajes)
                {
                    if (i is Cazador)
                    {
                        Cazador temp = (Cazador)i;
                        if (posx == temp.posx && posy == temp.posy)
                        {
                            enPosesion = -1;            //Definitivamente alguien la tiene
                            Dueño = temp;
                            temp.tieneQuaffle = true;
                        }
                    }
                }
            }
            else if (enPosesion > 1) //Pelea por la quaffle
            {
                Cazador cazadorsuperior = new Cazador(0,0,false,"dummy",'0'); 
                Cazador cazadortemp;
                bool fp = true;
                int mejordestreza = 0;
                foreach (Entidad i in Mapa.Personajes)
                {
                    if (i is Cazador)
                    {
                        cazadortemp = (Cazador)i;
                        if(cazadortemp.enQuaffle == true)
                        {
                            if(fp == true)
                            {
                                fp = false;
                                cazadorsuperior = (Cazador)i;
                                mejordestreza = cazadorsuperior.destreza;
                            }
                            else
                            {
                                if (mejordestreza < cazadortemp.destreza)
                                {
                                    cazadorsuperior = (Cazador)i;
                                    mejordestreza = cazadorsuperior.destreza;
                                }
                            }
                        }
                    }
                }
                cazadorsuperior.tieneQuaffle = true;

            }
            else if (enPosesion == 0)
            {
            }
            else if (enPosesion == -1)
            {
            }
            return true;
        }
    }
}
