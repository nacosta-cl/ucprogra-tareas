using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    abstract public class Entidad   //Clase superior que engloba a cada uno de las entidades presentes enPosesion el mapa
    {
        public int posx;
        public int posy;
        public char forma;
        protected int vel_lineal;
        protected bool material;
        protected int Aturdido = 0;
        public Entidad(int posx, int posy)
        {
            this.posx = posx;
            this.posy = posy;
        }
        public Entidad(int posx, int posy, bool material)
        {
            this.material = material;
        }
        public virtual bool Mover(int pfx, int pfy, long tics)
        {
            int pasos = vel_lineal;
            while(pasos > 0 && Aturdido==0)
            {
                int dx = posx-pfx;
                int dy = posy-pfy;
                if (dx == 0 && dy == 0)
                {
                    return true;
                }
                else if(tics%2 == 0)
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
                pasos --;
            }
            if (Aturdido > 0)       //Suponiendo que se llama una vez por tic
            {
                Aturdido--;
            }
            return false;

        }
        public void Retirar()
        {

        }
    }
}
