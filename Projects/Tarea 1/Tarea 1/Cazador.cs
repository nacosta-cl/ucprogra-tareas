using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Cazador : Jugador
    {
        public bool tieneQuaffle = false;
        private int precision;
        public int destreza;
        public bool Ofensivo = true;
        public bool enQuaffle = false;

        public Cazador(int posx, int posy, bool equipo, string nombre, char forma) : base(posx, posy, equipo, nombre)
        {
           
            Random rnd = new Random();
            this.forma = forma;
            this.HP = rnd.Next(200, 251);
            this.precision = rnd.Next(50,81);
            this.destreza = rnd.Next(1, 11);
            this.vel_lineal = rnd.Next(4, 7);
            this.agilidad = rnd.Next(40, 61);
        }

        public void recibir_quaffle()
        {

        }

        public override bool Mover(int pfx, int pfy, long tics)
        {
            double minim = 0;
            Quaffle qt = (Quaffle)Mapa.Personajes[12];

            if (tieneQuaffle == true)               //Si tiene la quaffle, debe ver si puede disparar, si no, debe avanzar y cargarla con el
            {
                //Buscar el aro mas cercano
                if (equipo == false)
                {
                    double d1 = Math.Pow(Math.Abs(posx - 1),2) + Math.Pow(Math.Abs(posy - 6),2);      //73 9   73 6 73 12
                    double d2 = Math.Pow(Math.Abs(posx - 1), 2) + Math.Pow(Math.Abs(posy - 9), 2);
                    double d3 = Math.Pow(Math.Abs(posx - 1), 2) + Math.Pow(Math.Abs(posy - 12), 2);
                    minim = Math.Min(d1, d2);
                    minim = Math.Min(minim, d3);

                }
                if (equipo == true)
                {
                    double d1 = Math.Pow(Math.Abs(posx - 73), 2) + Math.Pow(Math.Abs(posy - 6), 2);      //73 9   73 6 73 12
                    double d2 = Math.Pow(Math.Abs(posx - 73), 2) + Math.Pow(Math.Abs(posy - 9), 2);
                    double d3 = Math.Pow(Math.Abs(posx - 73), 2) + Math.Pow(Math.Abs(posy - 12), 2);
                    minim = Math.Min(d1, d2);
                    minim = Math.Min(minim, d3);
                }
                bool exito = this.disparar(minim);
                qt.posx = posx;
                qt.posy = posy;
                return exito;
            }
            else if(qt.enPosesion==0) //si no la tiene, debe ver donde esta, y si alguien mas la tiene
            {
                return seguirQuaffle(qt, tics);
            }
            else if(qt.enPosesion==-1 && tieneQuaffle==false)
            {
                foreach(Entidad i in Mapa.Personajes)
                {
                    if(i is Cazador)
                    {
                        Cazador j = (Cazador)i;
                        if (j.tieneQuaffle)
                        {
                            this.seguir(j.posx, j.posy, tics);
                        }
                    }
                }
            }
            return true;
        }

        public override string ToString()
        {
            return "cazador";
        }

        public void soltarQuaffle(int t) 
        {
            tieneQuaffle = false;
            Aturdido = 2;
        }
        private bool seguir(int pfx, int pfy, long tics)
        {
            int pasos = vel_lineal;
            while (pasos > 0 && Aturdido == 0)
            {
                int dx = posx - pfx;
                int dy = posy - pfy;
                if (Math.Abs(dx) == 1 && Math.Abs(dy) == 1)
                {
                    return true;
                }
                else if (tics % 2 == 0)
                {
                    if (dx != 0)
                    {
                        if (dx < 0) posx++;
                        else posx--;
                    }
                    else if (dy != 0)
                    {
                        if (dy > 0) posy--;
                        else posy++;
                    }
                }
                else if (tics % 2 == 1)
                {
                    if (dy != 0)
                    {
                        if (dy > 0) posy--;
                        else posy++;
                    }
                    else if (dx != 0)
                    {
                        if (dx < 0) posx++;
                        else posx--;
                    }
                }
                pasos--;
            }
            if (Aturdido > 0)       //Suponiendo que se llama una vez por tic
            {
                Aturdido--;
            }
            return false;
        }
        public bool disparar(double distancia)
        {
            Random rnd = new Random();
            Guardian guardian = new Guardian(" ");

            foreach (Entidad i in Mapa.Personajes)
            {
                if (i is Guardian)
                {
                    Guardian j = (Guardian)i;
                    if(j.equipo != this.equipo)
                    {
                        guardian = j;
                    }
                }
            }
            int a = guardian.reflejo;
            double b = distancia;
            int c = precision;
            double d = (guardian.reflejo);
            double e = ((precision));
            double prob = (( d * e )/100)/distancia;
            prob *= 100;
            if (equipo == false)
            {
                if(posx >=0 && posx <= 9 && posy<=18 && posy >=2)
                {
                    int hit = rnd.Next(0, 100);
                    if (hit<=prob)
                    {
                        if(this.equipo == false)
                            Mapa.anotacion +=10;
                        else
                            Mapa.anotacion -=10;
                        return true;
                    }
                    else
                    {
                        tieneQuaffle=false;
                        guardian.tienequaffle = true;
                        return false;
                    }
                }
                else
                {
                    posx -= vel_lineal;
                }
            }
            else  //equipo == true izquierdo
            {
                if (posx >= 67 && posx <= 73 && posy <= 18 && posy >= 2)
                {
                    int hit = rnd.Next(0, 100);
                    if (hit <= prob)
                    {
                        if (this.equipo == false)
                            Mapa.anotacion += 10;
                        else
                            Mapa.anotacion -= 10;
                        return true;
                    }
                    else
                    {
                        tieneQuaffle = false;
                        guardian.tienequaffle = true;
                        return false;
                    }
                }
                else
                {
                    posx += vel_lineal;
                }
            }
            return true;
        }

        public bool seguirQuaffle(Quaffle q, long tics)
        {
            int pasos = vel_lineal;
            int pfx = q.posx;           //Fijando objetivo
            int pfy = q.posy;
            if (q.enPosesion == 0)         //Dirigirse a la quaffle si nadie mas la tiene
            {
                while (pasos > 0 && Aturdido == 0) //Si no está aturdido
                {
                    int dx = posx - pfx;
                    int dy = posy - pfy;
                    if (dx == 0 && dy == 0)     //Llegó a la quaffle
                    {
                        enQuaffle = true;
                        q.enPosesion++;
                        pasos = 0;              //Se detiene
                    }
                    else if (tics % 2 == 0)
                    {
                        if (dx != 0)
                        {
                            if (dx < 0) posx++;
                            else posx--;
                        }
                        else if (dy != 0)
                        {
                            if (dy > 0) posy--;
                            else posy++;
                        }
                    }
                    else if (tics % 2 == 1)
                    {
                        if (dy != 0)
                        {
                            if (dy > 0) posy--;
                            else posy++;
                        }
                        else if (dx != 0)
                        {
                            if (dx < 0) posx++;
                            else posx--;
                        }
                    }
                    pasos--;
                }
                //En el turno de la quaffle, se decide a quien se le entrega
                if (Aturdido > 0)       //Suponiendo que se llama una vez por tic
                {
                    Aturdido--;
                }
            }
        return enQuaffle;
        }

    }
}
