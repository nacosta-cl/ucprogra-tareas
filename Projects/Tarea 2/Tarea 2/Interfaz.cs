using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tarea_2
{
    static class Interfaz
    {
        public static bool modo;
        public static string abrirArchivo()
        {
            modo = true;
            bool mdone = false;
            int sy = 3;
            
            while (!mdone)
            {
                Console.Clear();
                Console.Write("Bienvenido a Tarea 2");
                Console.Write("#", 0, 0);
                Console.WriteLine();
                Console.WriteLine("Escoge un modo. Para salir, apreta Ctrl y C al mismo tiempo. Para detener un juego, pulsa escape");
                Console.WriteLine("[ ] Abrir tabla");
                Console.WriteLine("[ ] Crear tabla");
                Console.SetCursorPosition(1,sy);
                Console.CursorSize = 100;
                ConsoleKeyInfo mov = Console.ReadKey();

                if (mov.Key == ConsoleKey.UpArrow && sy == 4)
                {
                    sy--;
                }
                else if (mov.Key == ConsoleKey.DownArrow && sy == 3)
                {
                    sy++;
                }
                if (mov.Key == ConsoleKey.Enter || mov.Key == ConsoleKey.Spacebar)
                {
                    if (sy == 4)
                    {
                        modo = false;
                    }
                    if (sy == 3)
                    {
                        modo = true;
                    }
                    mdone = true;
                }
            }
            if (modo == true)         //Buscandoyj
            {
                int curx = 1;
                int cury = 3;
                Console.SetCursorPosition(curx, cury);
                Console.CursorSize = 100;
                bool pdone = false;             //Termino la seleccion de path
                bool sdone = false;             //Termino la seleccion de lugar temporal
                string pathactual = "./";
                string errmsg = "";
                while (pdone == false)
                {
                    cury = 3;
                    Console.Clear();
                    Console.WriteLine("ESCOJA UN ARCHIVO XML PARA ABRIR");
                    Console.WriteLine(errmsg);
                    Console.WriteLine();
                    sdone = false;
                    string[] dirs = Directory.GetDirectories(pathactual);
                    string[] fils = Directory.GetFiles(pathactual);
                    int ly = 1+dirs.Length + fils.Length;
                    Console.WriteLine("[ ] ../");
                    foreach (string carpeta in dirs)
                    {
                        Console.WriteLine("[ ] <dir> " + carpeta.Substring(pathactual.Length));
                    }
                    foreach (string archivo in fils)
                    {
                        Console.WriteLine("[ ] "+archivo.Substring(pathactual.Length));
                    }
                    while (sdone == false)
                    {
                        Console.SetCursorPosition(curx, cury);
                        ConsoleKeyInfo mov = Console.ReadKey();
                        errmsg = "";
                        if (mov.Key == ConsoleKey.UpArrow && cury > 3)
                        {
                            cury--;
                        }
                        else if (mov.Key == ConsoleKey.DownArrow && cury < ly +2)
                        {
                            cury++;
                        }
                        if (mov.Key == ConsoleKey.Enter || mov.Key == ConsoleKey.Spacebar)
                        {
                            if (cury == 3)
                            {
                                pathactual += "../";
                            }
                            else if (cury > 3 && cury<=3+dirs.Length)
                            {
                                pathactual = dirs[cury-4]+"/";
                            }
                            else
                            {
                                string sel = fils[cury - 4 - dirs.Length];
                                if (sel.Substring(sel.Length - 4) == ".xml")
                                {
                                    return pathactual + sel.Substring(pathactual.Length);
                                }
                                else errmsg = "ERROR: Ese archivo no es un archivo XML. Escoge otro";
                            }
                            sdone = true;
                        }
                    }
                }
            }
            else if (modo == false) //buscando una carpeta para crear un xml
            {
                int curx = 1;
                int cury = 3;
                Console.SetCursorPosition(curx, cury);
                Console.CursorSize = 100;
                bool pdone = false;             //Termino la seleccion de path
                bool sdone = false;             //Termino la seleccion de lugar temporal
                string pathactual = "./";
                while (pdone == false)
                {
                    Console.Clear();
                    Console.WriteLine("ESCOJA UN DIRECTORIO PARA CREAR UN ARCHIVO XML");
                    Console.WriteLine();
                    Console.WriteLine();
                    sdone = false;
                    string[] dirs = Directory.GetDirectories(pathactual);
                    int ly = 2 + dirs.Length;
                    Console.WriteLine("[ ] <dir> ../");
                    Console.WriteLine("[ ] <dir> ./");
                    foreach (string carpeta in dirs)
                    {
                        Console.WriteLine("[ ] <dir> " + carpeta.Substring(pathactual.Length));
                    }
                    while (sdone == false)
                    {
                        Console.SetCursorPosition(curx, cury);
                        ConsoleKeyInfo mov = Console.ReadKey();
                        if (mov.Key == ConsoleKey.UpArrow && cury > 3)
                        {
                            cury--;
                        }
                        else if (mov.Key == ConsoleKey.DownArrow && cury < ly + 2)
                        {
                            cury++;
                        }
                        if (mov.Key == ConsoleKey.Enter || mov.Key == ConsoleKey.Spacebar)
                        {
                            if (cury == 3)
                            {
                                pathactual += "../";
                            }
                            else if (cury == 4)
                            {
                                return pathactual;
                            }
                            else if (cury > 4 && cury <= 4 + dirs.Length)
                            {
                                pathactual = dirs[cury - 5] + "/";
                            }
                            sdone = true;
                            cury = 3;
                            Console.SetCursorPosition(curx, cury);
                        }
                    }
                }
            }
            return "s";
        }
        public static void imprimirTabla(char[,] tabla, bool movmode, Escritor_imagen imagen)       //Este métodoimprime en el bmp, y en la consola, a la vez
        {
            if (Interfaz.modo == true)
            {
                Console.Clear();
            }
            Console.CursorSize = 1;
            Console.BackgroundColor = ConsoleColor.White;
            for (int j = 0; j < tabla.GetLength(1); j++)
            {
                for (int i = 0; i < tabla.GetLength(0); i++)
                {
                    imagen.dibujar_cuadro(i, j, tabla[i, j],tabla.GetLength(0),tabla.GetLength(1));
                }
            }
            for (int j = 0; j < tabla.GetLength(1); j++)
            {
                for (int i = 0; i < tabla.GetLength(0); i++)
                {
                    switch (tabla[i, j])
                    {
                        case 'W':
                            Console.ForegroundColor=ConsoleColor.Gray;
                            Console.Write('█');
                            break;
                        case 'R':
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('█');
                            break;
                        case 'r':
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write('█');
                            break;
                        case 'G':
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write('█');
                            break;
                        case 'g':
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write('█');
                            break;
                        case 'Y':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write('█');
                            break;
                        case 'y':
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write('█');
                            break;
                        case 'P':
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write('█');
                            break;
                        case 'p':
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write('█');
                            break;
                        case 'B':
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write('█');
                            break;
                        case 'b':
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write('█');
                            break;
                        case 'O':
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write('█');
                            break;
                        case 'o':
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write('█');
                            break;
                        default:
                            Console.Write(' ');
                            break;
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            if (Interfaz.modo == true)
            {
                Console.WriteLine();
                Console.Write("Marcando ruta : ");
                Console.Write(movmode);             //Header
                Console.WriteLine();
            }
            else if(Interfaz.modo == false)
            {
                Console.WriteLine();
            }

            Console.CursorSize = 100;
        }
    }
}
