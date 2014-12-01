using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Tarea_2
{
    class Mapa
    {
        public char[,] Mapasig;           //Guarda signos representativos
        private int lx;     //0,0 es arriba izq
        private int ly;
        private int nconex = 0;
        private string nombre;
        private Escritor_imagen imagen;
        private string path;
        public Mapa(string path)                //Inicializado como objeto abierto
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);                //Load path
            Console.WriteLine("Archivo bien cargado");
            XmlNodeList nnstage = xDoc.SelectNodes("//stage");
            XmlNode nstage = nnstage[0];
            string nombre = path.Split('/').Last();
            lx = Convert.ToInt32(nstage.Attributes["width"].Value);      //obten
            ly = Convert.ToInt32(nstage.Attributes["height"].Value);
            imagen = new Escritor_imagen(path.Substring(0, path.Length - nombre.Length), nombre.Substring(0, nombre.Length - 4), lx, ly);
            XmlNodeList dat = xDoc.SelectNodes("//stage/nodeList/node");   //Cargar los nodos
            Mapasig = new char[lx, ly];
            for (int j = 0; j < ly; j++)
            {
                for (int i = 0; i < lx; i++)
                {
                    Mapasig[i, j] = ' ';
                }
            }
            int[] nnodos = { 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < dat.Count; i++)
            {
                string contenido = dat[i].Attributes["content"].Value;  //obtengo el tipo de nodo
                int x = Convert.ToInt32(dat[i].Attributes["i"].Value);  //y su posicion
                int y = Convert.ToInt32(dat[i].Attributes["j"].Value);
                if (contenido == "wall")
                {
                    Mapasig[x, y] = 'W';
                }
                else if (contenido == "blue")
                {
                    Mapasig[x, y] = 'B';
                    nnodos[0]++;
                }
                else if (contenido == "red")
                {
                    Mapasig[x, y] = 'R';
                    nnodos[1]++;
                }
                else if (contenido == "purple")
                {
                    Mapasig[x, y] = 'P';
                    nnodos[2]++;
                }
                else if (contenido == "yellow")
                {
                    Mapasig[x, y] = 'Y';
                    nnodos[3]++;
                }
                else if (contenido == "orange")
                {
                    Mapasig[x, y] = 'O';
                    nnodos[4]++;
                }
                else if (contenido == "green")
                {
                    Mapasig[x, y] = 'G';
                    nnodos[5]++;
                }
            }
            foreach (int numno in nnodos)
            {
                if (numno == 2)
                {
                    nconex += 1;              //Formula para las conexiones posibles de los nodos....
                }
                if (numno >= 3)
                {
                    nconex += numno - 1;
                }
            }
            Interfaz.imprimirTabla(Mapasig, false, imagen);
        }
        public Mapa(string pathcreacion, bool nulo)     //Crear un archivo xml aqui
        {
            Console.Clear();
            bool pass = false;
            Console.WriteLine("Ingrese un nombre adecuado para su nuevo mapa. Este no puede comenzar ni con tabulación, ni con espacios");     //Cuidado con los tabs, y otros caracteres raros
            while (pass == false)
            {
                this.nombre = Console.ReadLine();
                if (nombre.Length == 0)
                {
                    Console.WriteLine("ERROR: Debe escribir un nombre");
                }
                else
                {
                    if (nombre[0] == ' ' || nombre[0] == '\t')
                    {
                        Console.WriteLine("ERROR: Su nombre no puede contener espacios ni tabulaciones al comienzo");
                    }
                    else
                    {
                        pass = true;
                    }
                }
            }
            Console.WriteLine("Introduzca un ancho en formato numerico. Ancho máximo: 79");
            pass = false;
            while (pass == false)
            {
                try
                {
                    string input = Console.ReadLine();
                    this.lx = int.Parse(input);
                    pass = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: Introduzca un formato numérico");
                }
                finally
                {
                    if (pass == true && lx > 79)
                    {
                        Console.WriteLine("ERROR: El ancho debe ser inferior a 79");
                        pass = false;
                    }
                }
            }
            Console.WriteLine("Introduzca un largo en formato numerico");
            pass = false;
            while (pass == false)
            {
                try
                {
                    string input = Console.ReadLine();
                    this.ly = int.Parse(input);
                    pass = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error en el formato: Introduzca en formato numérico");
                }
            }
            Console.WriteLine("Parametros basicos introducidos correctamente");
            this.path = pathcreacion;
        }
        private void imprimirCuadritoColor(string texto)
        {
            switch(texto)
            {
                case "Purple":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "Blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "Red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "Yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "Orange":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "Green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "Wall":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "Void":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.Write('█');
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void editar()
        {
            Escritor_imagen imagen = new Escritor_imagen(path, nombre, lx, ly);
            Console.WriteLine("Iniciando editor");
            bool finedit = false;       //Finalizó editor
            Mapasig = new char[lx, ly];
            for (int j = 0; j < ly; j++)
            {
                for (int i = 0; i < lx; i++)
                {
                    Mapasig[i, j] = ' ';
                }
            }
            int posx = 0;
            int posy = 3;
            string modo = "Void";
            while (finedit == false)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Title = ("Nombre = " + nombre + ".xml | " + "Modo cursor : " + modo);
                Console.WriteLine("Nombre = " + nombre + ".xml | Largo = " + lx + " - Ancho = " + ly);
                Console.Write("Modo cursor : |");
                //Selector de color para el cuadrito pequeño
                imprimirCuadritoColor(modo);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("| " + modo);
                Console.WriteLine("[ ] Abrir paleta visual de colores");
                Interfaz.imprimirTabla(Mapasig, false, imagen);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(posx, posy);
                Console.CursorSize = 100;
                ConsoleKeyInfo tecla = Console.ReadKey();
                if (tecla.Key == ConsoleKey.UpArrow && posy >= 3)
                {
                    if (posy == 3)
                    {
                        posy--;
                        posx = 1;
                    }
                    else
                    {
                        posy--;
                    }
                }
                else if (tecla.Key == ConsoleKey.DownArrow && posy < ly + 3 - 1)
                {
                    posy++;
                }
                else if (tecla.Key == ConsoleKey.RightArrow && posx < lx)
                {
                    posx++;
                }
                else if (tecla.Key == ConsoleKey.LeftArrow && posx > 0)
                {
                    posx--;
                }
                else if (tecla.Key == ConsoleKey.Spacebar)
                {
                    if (posx == 1 && posy == 2)     //Vamos a la paleta de colores
                    {
                        bool s2done = false;
                        posy = 2;
                        while (s2done == false)
                        {
                            Console.Clear();
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("Seleccione un elemento para posicionar en su mapa");
                            Console.WriteLine();
                            #region
                            Console.Write("[ ] ");
                            imprimirCuadritoColor("Void");
                            Console.WriteLine(" Void");

                            Console.Write("[ ] ");
                            imprimirCuadritoColor("Wall");
                            Console.WriteLine(" Wall");

                            Console.Write("[ ] ");
                            imprimirCuadritoColor("Purple");
                            Console.WriteLine(" Purple");

                            Console.Write("[ ] ");
                            imprimirCuadritoColor("Blue");
                            Console.WriteLine(" Blue");

                            Console.Write("[ ] ");
                            imprimirCuadritoColor("Red");
                            Console.WriteLine(" Red");

                            Console.Write("[ ] ");
                            imprimirCuadritoColor("Yellow");
                            Console.WriteLine(" Yellow");

                            Console.Write("[ ] ");
                            imprimirCuadritoColor("Orange");
                            Console.WriteLine(" Orange (Cyan)");

                            Console.Write("[ ] ");
                            imprimirCuadritoColor("Green");
                            Console.WriteLine(" Green");
                            #endregion
                            Console.SetCursorPosition(1, posy);
                            Console.CursorSize = 100;
                            ConsoleKeyInfo tecla2 = Console.ReadKey();
                            if (tecla2.Key == ConsoleKey.UpArrow && posy > 2)
                            {
                                posy--;
                            }
                            else if (tecla2.Key == ConsoleKey.DownArrow && posy < 9)
                            {
                                posy++;
                            }
                            else if (tecla2.Key == ConsoleKey.Spacebar)
                            {
                                switch (posy - 2)
                                {
                                    case 0:
                                        modo = "Void";
                                        break;
                                    case 1:
                                        modo = "Wall";
                                        break;
                                    case 2:
                                        modo = "Purple";
                                        break;
                                    case 3:
                                        modo = "Blue";
                                        break;
                                    case 4:
                                        modo = "Red";
                                        break;
                                    case 5:
                                        modo = "Yellow";
                                        break;
                                    case 6:
                                        modo = "Orange";
                                        break;
                                    case 7:
                                        modo = "Green";
                                        break;
                                }
                                s2done = true;
                            }

                        }
                    }
                    else if (posx < lx && posy < ly + 3)
                    {
                        int relx = posx;
                        int rely = posy - 3;
                        if (modo == "Wall" || modo == "Void")            //Poner una muralla  o un espacio es muy simple, solo hemos de ponerla
                        {
                            switch (modo)
                            {
                                case "Wall":
                                    Mapasig[relx, rely] = 'W';
                                    break;
                                case "Void":
                                    Mapasig[relx, rely] = ' ';
                                    break;
                            }
                        }
                        if (modo != "Wall" && modo != "Void")           //Algun nodo de color...
                        {
                            bool otronododone = false;
                            int porx = posx;
                            int pory = posy;
                            switch (modo)
                            {
                                case "Purple":
                                    Mapasig[relx, rely] = 'P';
                                    break;
                                case "Blue":
                                    Mapasig[relx, rely] = 'B';
                                    break;
                                case "Red":
                                    Mapasig[relx, rely] = 'R';
                                    break;
                                case "Yellow":
                                    Mapasig[relx, rely] = 'Y';
                                    break;
                                case "Orange":
                                    Mapasig[relx, rely] = 'O';
                                    break;
                                case "Green":
                                    Mapasig[relx, rely] = 'G';
                                    break;
                            }
                            while (otronododone == false)           //Modo seleccion del otro par: asi siempre tenemos un par de nodos
                            {
                                Console.Clear();
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Title = ("Nombre = " + nombre + ".xml | " + "Modo cursor : " + modo);
                                Console.WriteLine("Nombre = " + nombre + ".xml | Largo = " + lx + " - Ancho = " + ly);
                                Console.Write("Modo cursor : |");
                                //Selector de color para el cuadrito pequeño
                                imprimirCuadritoColor(modo);
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine("| " + modo);
                                Console.WriteLine("Seleccione otro lugar para poner el otro nodo del mismo color"); // modos disponibles: pintando x color, eliminando, seleccionando
                                Interfaz.imprimirTabla(Mapasig, false, imagen);
                                Console.WriteLine("NOTA: No puede atravesar murallas; si puede sobreescribir nodos ya existentes");
                                Console.SetCursorPosition(posx, posy);
                                Console.CursorSize = 100;
                                ConsoleKeyInfo tecla2 = Console.ReadKey();
                                if (tecla2.Key == ConsoleKey.UpArrow && posy > 3)
                                {
                                    if (Mapasig[posx, posy - 3 - 1] != 'W')
                                    {
                                        posy--;
                                    }
                                }
                                else if (tecla2.Key == ConsoleKey.DownArrow && posy < ly + 2)
                                {
                                    if (Mapasig[posx, posy - 3 + 1] != 'W')
                                    {
                                        posy++;
                                    }
                                }
                                else if (tecla2.Key == ConsoleKey.RightArrow && posx < lx - 1)
                                {
                                    if (Mapasig[posx + 1, posy - 3] != 'W')
                                    {
                                        posx++;
                                    }
                                }
                                else if (tecla2.Key == ConsoleKey.LeftArrow && posx > 0)
                                {
                                    if (Mapasig[posx - 1, posy - 3] != 'W')
                                    {
                                        posx--;
                                    }
                                }
                                else if (tecla2.Key == ConsoleKey.Spacebar && !(posx == porx && pory == posy))
                                {
                                    relx = posx;
                                    rely = posy - 3;
                                    switch (modo)
                                    {
                                        case "Purple":
                                            Mapasig[relx, rely] = 'P';
                                            break;
                                        case "Blue":
                                            Mapasig[relx, rely] = 'B';
                                            break;
                                        case "Red":
                                            Mapasig[relx, rely] = 'R';
                                            break;
                                        case "Yellow":
                                            Mapasig[relx, rely] = 'Y';
                                            break;
                                        case "Orange":
                                            Mapasig[relx, rely] = 'O';
                                            break;
                                        case "Green":
                                            Mapasig[relx, rely] = 'G';
                                            break;
                                    }
                                    otronododone = true;
                                }
                            }
                        }
                    }
                }
                switch (tecla.Key)  //Es posible seleccionar mediante la paleta en pantalla, o con las teclas asignadas a cada pincel
                {
                    case ConsoleKey.P:
                        modo = "Purple";
                        break;
                    case ConsoleKey.B:
                        modo = "Blue";
                        break;
                    case ConsoleKey.R:
                        modo = "Red";
                        break;
                    case ConsoleKey.Y:
                        modo = "Yellow";
                        break;
                    case ConsoleKey.O:
                        modo = "Orange";
                        break;
                    case ConsoleKey.G:
                        modo = "Green";
                        break;
                    case ConsoleKey.W:
                        modo = "Wall";
                        break;
                    case ConsoleKey.V:
                        modo = "Void";
                        break;
                    case ConsoleKey.Escape:             //Comando de salida
                        finedit = true;
                        break;
                }
            }
            Console.WriteLine("Escribiendo archivo XML");
            this.ImprimirXML();
            Console.WriteLine("Edición finalizada. Pulse cualquier tecla para continuar");
        }
        private void ImprimirXML()
        {
            XmlWriterSettings sett = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
            XmlWriter escritor = XmlTextWriter.Create(path+nombre+".xml", sett);
            escritor.WriteStartDocument();
            escritor.WriteStartElement("stage");
            escritor.WriteAttributeString("name", nombre);
            escritor.WriteAttributeString("width", "" + lx);
            escritor.WriteAttributeString("height", "" + ly);
            escritor.WriteStartElement("nodeList");
            
            for (int j = 0; j<ly;j++ )
            {
                for (int i = 0; i<lx ;i++)
                {
                    switch (Mapasig[i,j])
                    {
                        case 'B':
                            escritor.WriteStartElement("node");
                            escritor.WriteAttributeString("content","blue");
                            escritor.WriteAttributeString("i", "" + i);
                            escritor.WriteAttributeString("j", "" + j);
                            escritor.WriteEndElement();
                            break;
                        case 'P':
                            escritor.WriteStartElement("node");
                            escritor.WriteAttributeString("content","purple");
                            escritor.WriteAttributeString("i", "" + i);
                            escritor.WriteAttributeString("j", "" + j);
                            escritor.WriteEndElement();
                            break;
                        case 'Y':
                            escritor.WriteStartElement("node");
                            escritor.WriteAttributeString("content","yellow");
                            escritor.WriteAttributeString("i", "" + i);
                            escritor.WriteAttributeString("j", "" + j);
                            escritor.WriteEndElement();
                            break;
                        case 'O':
                            escritor.WriteStartElement("node");
                            escritor.WriteAttributeString("content","orange");
                            escritor.WriteAttributeString("i", "" + i);
                            escritor.WriteAttributeString("j", "" + j);
                            escritor.WriteEndElement();
                            break;
                        case 'G':
                            escritor.WriteStartElement("node");
                            escritor.WriteAttributeString("content","green");
                            escritor.WriteAttributeString("i", "" + i);
                            escritor.WriteAttributeString("j", "" + j);
                            escritor.WriteEndElement();
                            break;
                        case 'R':
                            escritor.WriteStartElement("node");
                            escritor.WriteAttributeString("content","red");
                            escritor.WriteAttributeString("i", "" + i);
                            escritor.WriteAttributeString("j", "" + j);
                            escritor.WriteEndElement();
                            break;
                        case 'W':
                            escritor.WriteStartElement("node");
                            escritor.WriteAttributeString("content","wall");
                            escritor.WriteAttributeString("i", "" + i);
                            escritor.WriteAttributeString("j", "" + j);
                            escritor.WriteEndElement();
                            break;
                        case ' ':
                            break;
                    }
                }
            }
            escritor.WriteEndElement();
            escritor.WriteEndElement();
            escritor.WriteEndDocument();
            escritor.Close();
        }
        public bool jugar()
        {
            int posx = 0;
            int posy = 0;
            bool gano = false;
            bool movmode = false;
            bool quit = false;
            while (gano == false && quit == false)
            {
                Interfaz.imprimirTabla(Mapasig, movmode, imagen);
                Console.WriteLine("Conexiones restantes = " + nconex);
                Console.SetCursorPosition(posx, posy);
                Console.CursorSize = 100;
                ConsoleKeyInfo tecla = Console.ReadKey();
                //Insertar algo para cambiar a WASD...
                if (tecla.Key == ConsoleKey.Escape)
                {
                    quit = true;
                }
                else if (tecla.Key == ConsoleKey.UpArrow && posy > 0)
                {
                    posy--;
                }
                else if (tecla.Key == ConsoleKey.DownArrow && posy < ly - 1)
                {
                    posy++;
                }
                else if (tecla.Key == ConsoleKey.RightArrow && posx < lx - 1)
                {
                    posx++;
                }
                else if (tecla.Key == ConsoleKey.LeftArrow && posx > 0)
                {
                    posx--;
                }
                else if (tecla.Key == ConsoleKey.Spacebar) //Seleccionamos un nodo, y entramos al modo movimiento
                {
                    char a = Mapasig[posx, posy];
                    if (Mapasig[posx, posy] != (char)32 && Mapasig[posx, posy] != (char)87 && (int)Mapasig[posx, posy] < 91)
                    {
                        movmode = true;
                        char letraor = Mapasig[posx, posy];
                        char letramarcada = (char)((int)Mapasig[posx, posy] + 32);
                        int norigenx = posx;
                        int norigeny = posy;
                        List<int[]> tracert = new List<int[]>();
                        while (movmode == true)                        //Modo movimiento
                        {
                            Interfaz.imprimirTabla(Mapasig, movmode, imagen);
                            Console.Write("Conexiones restantes = ");
                            Console.WriteLine(nconex);
                            Console.SetCursorPosition(posx, posy);
                            tecla = Console.ReadKey();
                            if (tecla.Key == ConsoleKey.UpArrow && posy > 0)
                            {
                                if (Mapasig[posx, posy - 1] == letraor && !(posy - 1 == norigeny && posx == norigenx))
                                {
                                    posy--;
                                    movmode = false;
                                    nconex -= 1;
                                }
                                else if (Mapasig[posx, posy - 1] == ' ')
                                {
                                    posy--;
                                    Mapasig[posx, posy] = letramarcada;
                                    int[] temp = { posx, posy };
                                    tracert.Add(temp);
                                }
                            }
                            else if (tecla.Key == ConsoleKey.DownArrow && posy < ly - 1)
                            {

                                if (Mapasig[posx, posy + 1] == letraor && !(posy + 1 == norigeny && posx == norigenx))
                                {
                                    posy++;
                                    movmode = false;
                                    nconex -= 1;
                                }
                                else if (Mapasig[posx, posy + 1] == ' ')
                                {
                                    posy++;
                                    Mapasig[posx, posy] = letramarcada;
                                    int[] temp = { posx, posy };
                                    tracert.Add(temp);
                                }
                            }
                            else if (tecla.Key == ConsoleKey.RightArrow && posx < lx - 1)
                            {
                                if (Mapasig[posx + 1, posy] == letraor && !(posy == norigeny && posx + 1 == norigenx))
                                {
                                    posx++;
                                    movmode = false;
                                    nconex -= 1;
                                }
                                else if (Mapasig[posx + 1, posy] == ' ')
                                {
                                    posx++;
                                    Mapasig[posx, posy] = letramarcada;
                                    int[] temp = { posx, posy };
                                    tracert.Add(temp);
                                }
                            }
                            else if (tecla.Key == ConsoleKey.LeftArrow && posx > 0)
                            {

                                if (Mapasig[posx - 1, posy] == letraor && !(posy == norigeny && posx - 1 == norigenx))
                                {
                                    posx--;
                                    movmode = false;
                                    nconex -= 1;
                                }
                                else if (Mapasig[posx - 1, posy] == ' ')
                                {
                                    posx--;
                                    Mapasig[posx, posy] = letramarcada;
                                    int[] temp = { posx, posy };
                                    tracert.Add(temp);
                                }
                            }
                            else if (tecla.Key == ConsoleKey.Spacebar)
                            {
                                movmode = false;
                                foreach (int[] pto in tracert)
                                {
                                    Mapasig[pto[0], pto[1]] = ' ';
                                }
                            }
                        }
                    }
                    else
                    {
                        movmode = false;
                    }
                }
                if (nconex == 0)
                {
                    Interfaz.imprimirTabla(Mapasig, true, imagen);
                    bool lleno = true;
                    foreach (char obj in Mapasig)
                    {
                        if (obj == ' ')
                        {
                            lleno = false;
                        }

                    }
                    if (lleno == true)
                    {
                        gano = true;
                    }
                    else
                    {
                        gano = false;
                        quit = true;

                    }
                }
            }
            if (gano == true)
            {
                Console.Clear();
                Console.WriteLine("¡Has resuelto una tabla Fadic! El emperador Shibe está orgulloso de ti");
                Console.WriteLine("Pulsa cualquier tecla para continuar");
                Console.ReadKey();
                Console.Clear();
                return true;
            }
            if (quit == true)
            {
                Console.Clear();
                Console.WriteLine("Apretaste esc");
                Console.WriteLine("Pulsa ESC de nuevo para salir. Pulsa cualquier otra tecla para volver al menú");
                ConsoleKeyInfo sel = Console.ReadKey();
                Console.Clear();
                if (sel.Key== ConsoleKey.Escape) return false;
                else return true;
            }
            return true;
        }
    }
}


