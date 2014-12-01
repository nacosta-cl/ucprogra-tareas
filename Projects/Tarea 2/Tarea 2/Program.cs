using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Tarea_2
{
    class Program
    {   //Doge
        static void Main(string[] args)
        {
            bool ingame = true;
            Console.Beep();
            while (ingame == true)
            {
                Console.Title = "Intérprete de tablas Fadic : Selector inicial";
                Console.ForegroundColor = ConsoleColor.Gray;
                string path = Interfaz.abrirArchivo(); //Abre un archivo desde consola. Entrega el path del archivo XML
                bool modo = Interfaz.modo; //true = juego, false = editor
                if (modo)
                {
                    Mapa tabla = new Mapa(path); //Abrir en modo juego
                    ingame = tabla.jugar();
                }
                else
                {
                    Escritor_imagen imagen = new Escritor_imagen(path, "new", 6, 6);
                    Mapa tabla = new Mapa(path, true);  //Crear un mapa nuevo
                    tabla.editar();
                    Console.Clear();
                }
            }
        }
    }
}
