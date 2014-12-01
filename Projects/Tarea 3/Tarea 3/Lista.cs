using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_3
{

    public class Lista
    {
        private Nodolista head;
        private Nodolista tail;
        private Nodolista nodoactual;
        private int largo = 0;
        private bool celdatipo = false;

        public Lista()
        {
            nodoactual = head;
            head = null;
            tail = null;
        }
        public int Largo
        {
            get { return this.largo; }
            set { }
        }
        /// <summary>
        /// Añade un objeto de cualquier tipo. En caso de ser un tipo Tarea3.Celda, solo se podran agregar objetos de tipo Tarea3.Celda a futuro
        /// </summary>
        /// <param name="contenido"></param>
        public void Add(object contenido)
        {
            if(head == null && contenido is Celda)          //Lista de celdas
            {
                celdatipo = true;
            }
            if (celdatipo == true)
	        {
		        if (contenido is Celda)
	            {
                    Celda ccelda = contenido as Celda;
		            if (head == null)
                    {
                        head = new Nodolista(contenido);
                        head.px=ccelda.px;
                        head.py=ccelda.py;
                        tail = head;
                    }
                    else
                    {
                        tail.next = new Nodolista(contenido);
                        tail = tail.next;
                        tail.px = ccelda.px;
                        tail.py = ccelda.py;
                        tail.next = null;
                    }
	            }
                else Console.WriteLine("ERROR: Agregando nodos del tipo incorrecto"); ;
	        }
            else            //Lista de cualquier otra cosa
	        {
                if (head == null)
                {
                    head = new Nodolista(contenido);
                    tail = head;
                }
                else
                {
                    tail.next = new Nodolista(contenido);
                    tail = tail.next;
                }
	        }
            largo++;
        }
        public object buscar(int indice)
        {
            Nodolista tmp = head;
            int tmpindex = 0;
            for (; tmpindex < indice && tmp != null; tmpindex++, tmp = tmp.next) ;
            if (tmp == null) return -1;
            return tmp.content;
        }
        public Celda buscar(int px, int py)
        {
            if (celdatipo == true)
            {
                Nodolista tmp = head;
                for (; !(tmp.px == px && tmp.py == py) && tmp.next != null; tmp = tmp.next) ;
                if (tmp.px == px && tmp.py == py) return (Celda)tmp.content;
            }
            return null;
        }
        public object buscar(object contenido)
        {
            Nodolista tmp = head;
            for (; contenido == tmp.content && tmp != null; tmp = tmp.next);
            if (contenido != tmp.content) return -1;
            return tmp.content;
        }
    }
}

