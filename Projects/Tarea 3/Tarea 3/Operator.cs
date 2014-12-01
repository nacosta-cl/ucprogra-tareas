using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Tarea_3
{
    public class Operator : LibreriaT3.IFractal
    {
        public LibreriaT3.UI interfaz = opinterfaz.UI;
        public Lista etapas = new Lista();
        public bool mapacargado = false;
        public bool win = false;
        public bool reload = false;
        public bool defeat = false;
        public Etapa etapaActual;
        public Operator()
        {

        }
        public void Undo()
        {
            opinterfaz.nextstage = !opinterfaz.UI.ShowPrompt("Pasar a la siguiente etapa");
            Console.WriteLine("undo");
        }
        public void Redo()
        {
            Console.WriteLine("redo");
            this.win = true;
        }
        public void Push(int h,int d)
        {
            if (etapaActual.pushesrestantes > 0)
            {
                LibreriaT3.PetalColor nextcolor = etapaActual.obtenerPush();
                etapaActual.Listanodos.buscar(d, h).push0(nextcolor, LibreriaT3.PetalType.Normal);
                int blooms = etapaActual.checkblooms(false);
                while (blooms != 0)
                {
                    blooms = etapaActual.checkblooms(true);
                }
            }
            else
            {
                opinterfaz.UI.ShowMessage("Te has quedado sin movimientos");
            }
            this.etapaActual.resetmfd();
            if (etapaActual.objetivocumplido())
            {
                this.win = true;
            }
        }
        public void OpenFile(string path) //Armar todas las estructuras, y luego ir presentandolas
        {
            
            Console.WriteLine("55");
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(path);
                this.reload = true;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("ERROR - error 00002");
                return;
            }
            XmlNodeList dat = xDoc.SelectNodes("//stageList/stage");
            
            //Asigancion de nodos a sus posiciones iniciales...
            foreach (XmlNode stage in dat) //Leer etapas, crear una estructura hex para cada etapa, se cargan en una lista, luego se cargan individualmente
            {
                int i = 0;
                XmlNodeList Compstage = stage.ChildNodes;
                XmlNodeList pushList = Compstage[2].ChildNodes;
                LibreriaT3.PetalColor[] tmppushes = new LibreriaT3.PetalColor[pushList.Count];
                //Cargando los pushes
                #region
                foreach (XmlNode push in pushList)
                {
                    switch (push.Attributes[0].Value)
                    {
                        case "blue":
                            tmppushes[i] = LibreriaT3.PetalColor.Blue;
                            break;
                        case "pink":
                            tmppushes[i] = LibreriaT3.PetalColor.Pink;
                            break;
                        case "green":
                            tmppushes[i] = LibreriaT3.PetalColor.Green;
                            break;
                        case "orange":
                            tmppushes[i] = LibreriaT3.PetalColor.Orange;
                            break;
                        default:
                            Console.WriteLine("[!] No se reconoció un pushpetal en el XML");
                            break;
                    }
                    i++;
                }
                #endregion
                Etapa tempetapa = new Etapa(int.Parse(stage.Attributes[1].Value), stage.Attributes[0].Value,Compstage[3].ChildNodes[0].Attributes[0].Value, tmppushes);
                etapas.Add(tempetapa);
                XmlNodeList petalList = Compstage[0].ChildNodes;
                //Cargando los petalos
                #region
                foreach (XmlNode petal in petalList)
                {
                    int bx = int.Parse(petal.Attributes[2].Value); //d=x
                    int by = int.Parse(petal.Attributes[1].Value); //h=y
                    Celda celdatmp = tempetapa.Listanodos.buscar(bx,by);
                    switch(petal.Attributes[0].Value)
                    {
                        case "bluePetal":
                            celdatmp.pcolor = LibreriaT3.PetalColor.Blue;
                            celdatmp.ptipo = LibreriaT3.PetalType.Normal;
                            break;
                        case "greenPetal":
                            celdatmp.pcolor = LibreriaT3.PetalColor.Green;
                            celdatmp.ptipo = LibreriaT3.PetalType.Normal;
                            break;
                        case "pinkPetal":
                            celdatmp.pcolor = LibreriaT3.PetalColor.Pink;
                            celdatmp.ptipo = LibreriaT3.PetalType.Normal;
                            break;
                        case "orangePetal":
                            celdatmp.pcolor = LibreriaT3.PetalColor.Orange;
                            celdatmp.ptipo = LibreriaT3.PetalType.Normal;
                            break;
                        case "bomb":
                            celdatmp.ptipo = LibreriaT3.PetalType.Explosive;
                            celdatmp.pcolor = null;
                            break;
                        case "rasho":
                            celdatmp.ptipo = LibreriaT3.PetalType.Lightning;
                            celdatmp.pcolor = null;
                            break;
                        case "gravity":
                            celdatmp.ptipo = LibreriaT3.PetalType.Gravity;
                            celdatmp.pcolor = null;
                            break;
                    }
                }
                #endregion
                XmlNodeList cellList = Compstage[1].ChildNodes;
                //Cargando las celdas especiales
                #region
                foreach (XmlNode celda in cellList)
                {
                    int by = int.Parse(celda.Attributes[2].Value);
                    int bx = int.Parse(celda.Attributes[1].Value);
                    Celda celdatmp = tempetapa.Listanodos.buscar(bx, by);
                    switch (celda.Attributes[0].Value)
                    {
                        case "ArrowS":
                            celdatmp.celdatipo = LibreriaT3.CellType.ArrowS;
                            break;
                        case "ArrowNE":
                            celdatmp.celdatipo = LibreriaT3.CellType.ArrowNE;
                            break;
                        case "ArrowNW":
                            celdatmp.celdatipo = LibreriaT3.CellType.ArrowNW;
                            break;
                        case "ArrowSE":
                            celdatmp.celdatipo = LibreriaT3.CellType.ArrowSE;
                            break;
                        case "ArrowSW":
                            celdatmp.celdatipo = LibreriaT3.CellType.ArrowSW;
                            break;
                        case "ArrowN":
                            celdatmp.celdatipo = LibreriaT3.CellType.ArrowN;
                            break;
                        case "PortalO":
                            celdatmp.celdatipo = LibreriaT3.CellType.PortalO;
                            tempetapa.portalO = celdatmp;
                            break;
                        case "PortalB":
                            celdatmp.celdatipo = LibreriaT3.CellType.PortalB;
                            tempetapa.portalB = celdatmp;
                            break;
                        case "Hole":
                            celdatmp.celdatipo = LibreriaT3.CellType.Hole;
                            break;
                    }
                }
                #endregion
            }
            this.mapacargado = true;
       }
        public void Exit()
        {
            opinterfaz.UI.Close();

        }
    }
}
