using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_4_back
{
    [Serializable]
    public class Mapa
    {
        private nodonivel[,] nivel;
        private Random rnd = new Random();
        private int size;
        private int puntaje;
        public int Puntaje
        {
            get { return this.puntaje; }
            set { ; }
        }
        public int Size
        {
            get { return this.size; }
            set { ; }
        }
        /// <summary>
        /// Crea un mapa a partir de los parametros seleccionados
        /// </summary>
        /// <param name="n">Tamaño</param>
        /// <param name="imgNormal">Tipo de imagen</param>
        public Mapa(int n)
        {
            nivel = new nodonivel[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    nivel[i, j] = new nodonivel(i, j, 0);
                }
            }
            size = n;
        }
        /// <summary>
        /// Toma un arreglo de potencias de dos, y lo convierte en nodos utlizables
        /// </summary>
        /// <param name="repr">Arreglo de enteros</param>
        public void numerosEnMapa(int[,] repr)
        {
            int n = size;
            nivel = new nodonivel[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    nivel[i, j] = new nodonivel(i, j, repr[i,j]);
                }
            }
        }
        /// <summary>
        /// Entrega un arreglo de numeros, basado en la informacion contenida en el arreglo de nodos
        /// </summary>
        /// <returns></returns>
        public int[,] mapaEnNumeros()
        {
            int[,] ret = new int[nivel.GetLength(0),nivel.GetLength(1)];
            for(int i = 0;i<nivel.GetLength(0);i++)//x
            {
                for(int j = 0;j<nivel.GetLength(1);j++)//x
                {
                    ret[i,j]=nivel[i,j].valor;
                }
            }
            return ret;
        }
        /// <summary>
        /// Retorna arrays con datos para mover los rectangulos con la siguiente forma [xi,yi,xf,yf,vcf], (vcf = valor final celda final)en orden temporal.
        /// </summary>
        /// <param name="dir">Direccion de movimiento 0=abajo CCW</param>
        /// <returns></returns>
        public int[][] Mover(int dir)
        {
            List<int[]> movimientos = new List<int[]>();
            int lxmap = nivel.GetLength(0);
            int lymap = nivel.GetLength(1);
            for (int p = 0; p < nivel.GetLength(0)*nivel.GetLength(0); p++) //son n pasadas del algoritmo
            {
                if (dir == 0) //Hacia abajo
                {
                    for (int j = lymap-1; j > 0; j--) //j parte desde abajo y queda en 1 para que no quede "nivel[i,-1]"
                    {
                        for (int i = 0; i < lxmap; i++) //Recorrer arreglo por filas
                        {
                            nodonivel nodoactual = nivel[i, j];
                            nodonivel nodocomp = nivel[i, j - 1]; //el de arriba
                            int vi = nodocomp.valor;
                            if (nodocomp.valor == 0) ; //no hay movimiento
                            else if (nodocomp.valor != 0 && nodoactual.valor == 0) //se puede, este esta vacio, y baja aqui
                            {
                                nodoactual.valor = nodocomp.valor;
                                nodocomp.valor = 0;
                                int[] exp = { nodocomp.px, nodocomp.py, nodoactual.px, nodoactual.py, nodoactual.valor ,vi};
                                movimientos.Add(exp);
                            }
                            else if (nodocomp.valor == nodoactual.valor && nodoactual.sumable == true && nodocomp.sumable == true) //se puede y se suma
                            {
                                nodoactual.valor = nodoactual.valor + nodocomp.valor;
                                nodocomp.valor = 0;
                                nodoactual.sumable = false;
                                int[] exp = { nodocomp.px, nodocomp.py, nodoactual.px, nodoactual.py, nodoactual.valor ,vi};
                                movimientos.Add(exp);
                                puntaje += nodoactual.valor;
                            }
                            else if (nodocomp.valor == nodoactual.valor && nodoactual.sumable == false || nodocomp.sumable == false) ; //no se puede...
                        }
                    }
                } //done
                else if (dir == 1) //hacia la derecha
                {
                    for (int i = lxmap -1; i > 0; i--) //i parte desde la derecha 
                    {
                        for (int j = 0; j < lymap; j++)//Recorrer arreglo por columnas
                        {
                            nodonivel nodoactual = nivel[i, j];
                            nodonivel nodocomp = nivel[i-1, j];
                            int vi = nodocomp.valor;
                            if (nodocomp.valor == 0) ; //no hay movimiento
                            else if (nodocomp.valor != 0 && nodoactual.valor == 0) //se puede, este esta vacio, y baja aqui
                            {
                                nodoactual.valor = nodocomp.valor;
                                nodocomp.valor = 0;
                                int[] exp = { nodocomp.px, nodocomp.py, nodoactual.px, nodoactual.py, nodoactual.valor ,vi};
                                movimientos.Add(exp);
                            }
                            else if (nodocomp.valor == nodoactual.valor && nodoactual.sumable == true && nodocomp.sumable == true) //se puede y se suma
                            {
                                nodoactual.valor = nodoactual.valor + nodocomp.valor;
                                nodocomp.valor = 0;
                                nodoactual.sumable = false;
                                int[] exp = { nodocomp.px, nodocomp.py, nodoactual.px, nodoactual.py, nodoactual.valor ,vi };
                                movimientos.Add(exp);
                                puntaje += nodoactual.valor;
                            }
                            else if (nodocomp.valor == nodoactual.valor && nodoactual.sumable == false || nodocomp.sumable == false) ; //no se puede...
                        }
                    }
                }
                else if (dir == 2) // hacia arriba
                {
                    for (int j = 0; j < lymap-1; j++) //j parte desde arriba
                    {
                        for (int i = 0; i < lxmap; i++) //Recorrer arreglo por filas
                        {
                            nodonivel nodoactual = nivel[i, j];
                            nodonivel nodocomp = nivel[i, j + 1];
                            int vi = nodocomp.valor;
                            if (nodocomp.valor == 0) ; //no hay movimiento
                            else if (nodocomp.valor != 0 && nodoactual.valor == 0) //se puede, este esta vacio, y baja aqui
                            {
                                nodoactual.valor = nodocomp.valor;
                                nodocomp.valor = 0;
                                int[] exp = { nodocomp.px, nodocomp.py, nodoactual.px, nodoactual.py, nodoactual.valor ,vi};
                                movimientos.Add(exp);
                            }
                            else if (nodocomp.valor == nodoactual.valor && nodoactual.sumable == true && nodocomp.sumable == true) //se puede y se suma
                            {
                                nodoactual.valor = nodoactual.valor + nodocomp.valor;
                                nodocomp.valor = 0;
                                nodoactual.sumable = false;
                                int[] exp = { nodocomp.px, nodocomp.py, nodoactual.px, nodoactual.py, nodoactual.valor,vi };
                                movimientos.Add(exp);
                                puntaje += nodoactual.valor;
                            }
                            else if (nodocomp.valor == nodoactual.valor && nodoactual.sumable == false || nodocomp.sumable == false) ; //no se puede...
                        }
                    }
                }
                else if (dir == 3) //izquierda
                {
                    for (int i = 0; i < lxmap-1; i++)
                    {
                        for (int j = 0; j < lymap; j++) //Recorrer arreglo por columnas
                        {
                            nodonivel nodoactual = nivel[i, j];
                            nodonivel nodocomp = nivel[i + 1, j];
                            int vi = nodocomp.valor;
                            if (nodocomp.valor == 0); //no hay movimiento
                            else if (nodocomp.valor != 0 && nodoactual.valor == 0) //se puede, este esta vacio, y baja aqui
                            {
                                nodoactual.valor = nodocomp.valor;
                                nodocomp.valor = 0;
                                int[] exp = { nodocomp.px, nodocomp.py, nodoactual.px, nodoactual.py, nodoactual.valor,vi};
                                movimientos.Add(exp);
                            }
                            else if (nodocomp.valor == nodoactual.valor && nodoactual.sumable == true && nodocomp.sumable == true) //se puede y se suma
                            {
                                nodoactual.valor = nodoactual.valor + nodocomp.valor;
                                nodocomp.valor = 0;
                                nodoactual.sumable = false;
                                int[] exp = { nodocomp.px, nodocomp.py, nodoactual.px, nodoactual.py, nodoactual.valor ,vi};
                                movimientos.Add(exp);
                                puntaje += nodoactual.valor;
                            }
                            else if (nodocomp.valor == nodoactual.valor && nodoactual.sumable == false || nodocomp.sumable == false) ; //no se puede...
                        }
                    }
                }
            }
            //resetear todo lo que no era sumable
            for (int i = 0; i < nivel.GetLength(0); i++)//x
            {
                for (int j = 0; j < nivel.GetLength(1); j++)//x
                {
                    nivel[i, j].sumable=true;
                }
            }
            return movimientos.ToArray();
        }
        /// <summary>
        /// Añadir un cuadro en una posicion aleatoria de valor 2
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        public int[] addrandom(bool init)
        {
            Random rnd = new Random();
            int cho = rnd.Next(0,100);
            if (cho<66 || init) //2
            {
                cho = 2;
            }
            else
            {
                cho = 4;
            }
            bool ndone = true;
            int intento = 0;
            while (ndone && checkespacio())
            {
                intento++;
                int x = rnd.Next(0,nivel.GetLength(0));
                int y = rnd.Next(0,nivel.GetLength(1));
                if(nivel[x,y].valor==0)
                {
                    nivel[x,y].valor=cho;
                    ndone = false;
                }
            }
            return new int[1];
        }
        private bool checkespacio()
        {
            bool valor = false;
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    if (nivel[i, j].valor == 0) valor = true;
                }
            }
            return valor;
        }
        /// <summary>
        /// Este metodo se invoca después de haber hecho el met mover, y asume que todos los bloques son sumables
        /// </summary>
        /// <returns></returns>
        public bool noMovs()
        {
            int[,]nnivel = this.mapaEnNumeros();
            bool derrota;
            int lxmap = nnivel.GetLength(0);
            int lymap = nnivel.GetLength(1);
            int mcount = 0;
            for (int j = lymap-1; j > 0; j--) //j parte desde abajo y queda en 1 para que no quede "nnivel[i,-1]"
            {
                for (int i = 0; i < lxmap; i++) //Recorrer arreglo por filas
                {
                    int nodoactual = nnivel[i, j];
                    int nodocomp = nnivel[i, j - 1]; //el de arriba
                        
                    if (nodocomp == nodoactual || nodoactual == 0) //se puede
                    {
                        mcount++;
                    }
                }
            }
                for (int i = lxmap -1; i > 0; i--) //i parte desde la derecha 
                {
                    for (int j = 0; j < lymap; j++)//Recorrer arreglo por columnas
                    {
                        int nodoactual = nnivel[i, j];
                        int nodocomp = nnivel[i-1, j];
                        if (nodocomp == nodoactual || nodoactual == 0) //se puede
                        {
                            mcount++;
                        }
                    }
                }
                for (int j = 0; j < lymap-1; j++) //j parte desde arriba
                {
                    for (int i = 0; i < lxmap; i++) //Recorrer arreglo por filas
                    {
                        int nodoactual = nnivel[i, j];
                        int nodocomp = nnivel[i, j + 1];
                        if (nodocomp == nodoactual || nodoactual == 0) //se puede
                        {
                            mcount++;
                        }
                    }
                }
                for (int i = 0; i < lxmap-1; i++)
                {
                    for (int j = 0; j < lymap; j++) //Recorrer arreglo por columnas
                    {
                        int nodoactual = nnivel[i, j];
                        int nodocomp = nnivel[i + 1, j];
                        if (nodocomp == nodoactual || nodoactual == 0) //se puede
                        {
                            mcount++;
                        }
                    }
                }
                if (mcount == 0)
                {
                    derrota = true;
                }
                else
                {
                    derrota = false;
                }
                return derrota;
        }
    }
}
