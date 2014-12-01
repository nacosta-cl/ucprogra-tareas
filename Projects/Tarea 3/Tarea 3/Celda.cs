using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_3
{
    public class Celda
    {
        private Celda NW = null;          //Nodos presentes en cada una de las direcciones, inicialmente nulos
        private Celda NN = null;
        private Celda NE = null;
        private Celda SW = null;
        private Celda SS = null;
        private Celda SE = null;
        public LibreriaT3.PetalColor? pcolor = null;   //Contenido por defecto
        public LibreriaT3.PetalType? ptipo = null;
        public LibreriaT3.CellType celdatipo = LibreriaT3.CellType.Normal;
        public int px;
        public int py;
        private Lista listanodos;
        private bool mfd = false;
        /// <summary>
        /// Esta celda se detruirá en la siguiente comprobación
        /// </summary>
        public bool MFD
        {
            get { return mfd; }
            set { mfd = value; }
        }

        public Celda(int px, int py, Lista listanodos)
        {
            this.px = px;
            this.py = py;
            this.listanodos = listanodos;
        }
        /// <summary>
        /// Este metodo solo es funcional si es llamado desde una celda base (h = 0, d = 0). En otro caso, originará una estructura erronea
        /// </summary>
        /// <param name="radio"></param>
        public void crearborde(int radio) //Creador del borde
        {
            #region
            if (radio > 0)
            {
                radio--;
                //----------
                if (NN == null)
                {
                    NN = new Celda(this.px, this.py + 1, listanodos);
                    NN.celdatipo = LibreriaT3.CellType.Hole;
                    listanodos.Add(NN);
                }
                //----------
                if (NW == null)
                {
                    NW = new Celda(this.px - 1, this.py + 1, listanodos);
                    NW.celdatipo = LibreriaT3.CellType.Hole;
                    listanodos.Add(NW);
                }
                //----------
                if (NE == null)
                {
                    NE = new Celda(this.px + 1, this.py, listanodos);
                    NE.celdatipo = LibreriaT3.CellType.Hole;
                    listanodos.Add(NE);
                }
                //----------
                if (SS == null)
                {
                    SS = new Celda(this.px, this.py - 1, listanodos);
                    SS.celdatipo = LibreriaT3.CellType.Hole;
                    listanodos.Add(SS);
                }
                //----------
                if (SW == null)
                {
                    SW = new Celda(this.px - 1, this.py, listanodos);
                    SW.celdatipo = LibreriaT3.CellType.Hole;
                    listanodos.Add(SW);
                }
                //----------
                if (SE == null)
                {
                    SE = new Celda(this.px + 1, this.py - 1, listanodos);
                    SE.celdatipo = LibreriaT3.CellType.Hole;
                    listanodos.Add(SE);
                }
                //----------
                //Asignacion direcciones a los diversos nodos CW
                SW.SE = SS;
                SS.NE = SE;
                SE.NN = NE;
                NE.NW = NN;
                NN.SW = NW;
                NW.SS = SW;
                //Asignacion de direcciones a los diversos nodos CCW
                SW.NN = NW;
                NW.NE = NN;
                NN.SE = NE;
                NE.SS = SE;
                SE.SW = SS;
                SS.NW = SW;
                //Devoluciones al nodo original
                NN.SS = this;
                NE.SW = this;
                SE.NW = this;
                SS.NN = this;
                SW.NE = this;
                NW.SE = this;
                //Órdenes de crecimiento
                NN.crearborde(radio);
                NW.crearborde(radio);
                NE.crearborde(radio);
                SE.crearborde(radio);
                SS.crearborde(radio);
                SW.crearborde(radio);
            }
            #endregion
        }
        /// <summary>
        /// Crea una estructura de radio r a partir de una celda. Este metodo solo es funcional si es llamado desde una celda base (h = 0, d = 0). En otro caso, originará una estructura erronea.
        /// </summary>
        /// <param name="radio"></param>
        public void crecer(int radio)
        {
            if (radio > 0)
            {
                radio--;
                //----------
                if (NN == null)
                {
                    NN = new Celda(this.px, this.py + 1, listanodos);
                    listanodos.Add(NN);
                }
                //----------
                if (NW == null)
                {
                    NW = new Celda(this.px - 1, this.py + 1, listanodos);
                    listanodos.Add(NW);
                }
                //----------
                if (NE == null)
                {
                    NE = new Celda(this.px + 1, this.py, listanodos);
                    listanodos.Add(NE);
                }
                //----------
                if (SS == null)
                {
                    SS = new Celda(this.px, this.py - 1, listanodos);
                    listanodos.Add(SS);
                }
                //----------
                if (SW == null)
                {
                    SW = new Celda(this.px - 1, this.py, listanodos);
                    listanodos.Add(SW);
                }
                //----------
                if (SE == null)
                {
                    SE = new Celda(this.px + 1, this.py - 1, listanodos);
                    listanodos.Add(SE);
                }
                //----------
                //Asignacion direcciones a los diversos nodos CW
                SW.SE = SS;
                SS.NE = SE;
                SE.NN = NE;
                NE.NW = NN;
                NN.SW = NW;
                NW.SS = SW;
                //Asignacion de direcciones a los diversos nodos CCW
                SW.NN = NW;
                NW.NE = NN;
                NN.SE = NE;
                NE.SS = SE;
                SE.SW = SS;
                SS.NW = SW;
                //Devoluciones al nodo original
                NN.SS = this;
                NE.SW = this;
                SE.NW = this;
                SS.NN = this;
                SW.NE = this;
                NW.SE = this;
                //Órdenes de crecimiento
                NN.crecer(radio);
                NW.crecer(radio);
                NE.crecer(radio);
                SE.crecer(radio);
                SS.crecer(radio);
                SW.crecer(radio);
            }
        }
        /// <summary>
        /// Primer push: Solo se puede hacer en una celda que no contenga un pétalo
        /// </summary>
        /// <param name="colorIn"></param>
        /// <param name="tipoIn"></param>
        /// <returns></returns>
        public bool push0(LibreriaT3.PetalColor colorIn, LibreriaT3.PetalType tipoIn)
        {
            Celda[] celdas = { NN, NW, NE, SW, SE, SS };
            if ((this.celdatipo == LibreriaT3.CellType.Normal || this.celdatipo == LibreriaT3.CellType.Hole) && this.pcolor == null)
            {
                foreach (Celda cell in celdas)
                {
                    if (cell != null)
                    {
                        if ((cell.celdatipo == LibreriaT3.CellType.Normal) && (cell.ptipo == LibreriaT3.PetalType.Normal && cell.pcolor != null)) //Solo crea petalo si ya hay otro 
                        {
                            cell.push1(this, colorIn, tipoIn);
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Crea una celda del color correspondiente aqui mismo y empuja hasta pillar celda vacia, o elimina la celda en un agujero negro.
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="colorIn"></param>
        /// <param name="tipoIn"></param>
        public void push1(Celda origen, LibreriaT3.PetalColor colorIn, LibreriaT3.PetalType tipoIn)
        {
            #region
            if (origen == this.NN) //Base, modificar y copiar segun cosas
            {
                if (this.SS != null)
                {
                    if (this.SS.pcolor != null && this.SS.ptipo == LibreriaT3.PetalType.Normal && this.SS.celdatipo==LibreriaT3.CellType.Normal)
                    {
                        this.SS.push2(this,false);      //envia la informacion que debe adoptar al siguiente nodo...
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px);
                    }
                    else if (this.SS.celdatipo == LibreriaT3.CellType.Hole) //Hay un agujero abajo
                    {
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px);
                        System.Threading.Thread.Sleep(100);
                        opinterfaz.UI.DeleteHex(this.py - 1, this.px);
                    }
                    else if (this.SS.pcolor == null && this.SS.ptipo == null && this.SS.celdatipo==LibreriaT3.CellType.Normal) //Hay un nada abajo
                    {
                        this.SS.pcolor = this.pcolor;
                        this.SS.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px);
                    }
                    else if (this.SS.celdatipo == LibreriaT3.CellType.PortalB || this.SS.celdatipo == LibreriaT3.CellType.PortalO) //Hay un portal abajo
                    {
                        if (this.SS.celdatipo == LibreriaT3.CellType.PortalB) //Entra en B, sale en O
                        {
                            opinterfaz.portalO.pcolor = this.pcolor;
                            opinterfaz.portalO.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalO.py, opinterfaz.portalO.px);
                            opinterfaz.portalO.pushportal("SS");
                        }
                        else // vic
                        {
                            opinterfaz.portalB.pcolor = this.pcolor;
                            opinterfaz.portalB.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalB.py, opinterfaz.portalB.px);
                            System.Threading.Thread.Sleep(100);
                            opinterfaz.portalB.pushportal("SS");
                        }
                    }
                    else if (this.SS.celdatipo == LibreriaT3.CellType.ArrowN || this.SS.celdatipo == LibreriaT3.CellType.ArrowNE || this.SS.celdatipo == LibreriaT3.CellType.ArrowNW || this.SS.celdatipo == LibreriaT3.CellType.ArrowS || this.SS.celdatipo == LibreriaT3.CellType.ArrowSW || this.SS.celdatipo == LibreriaT3.CellType.ArrowSE) //Hay una flecha abajo. Las flechas nunca traen petalos encima. Se mueve el petalo a la flecha, se le entrega la informcion, y el metodo push flecha decide que hacer con ella
                    {
                        this.SS.pcolor = this.pcolor;
                        this.SS.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px);
                        this.SS.pushflecha(this);
                    }
                    this.pcolor = colorIn;
                    this.ptipo = tipoIn;
                    opinterfaz.UI.CreateHex(this.py, this.px, (LibreriaT3.PetalType)this.ptipo, (LibreriaT3.PetalColor)this.pcolor);
                }
            }
            else if (origen == this.NW) // SS = SE , this.px -> this.px + 1, this.py -> this.py - 1
            {
                if (this.SE != null)
                {
                    if (this.SE.pcolor != null && this.SE.ptipo == LibreriaT3.PetalType.Normal && this.SE.celdatipo == LibreriaT3.CellType.Normal)
                    {
                        this.SE.push2(this,false);      //envia la informacion que debe adoptar al siguiente nodo...
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px +1);
                    }
                    else if (this.SE.celdatipo == LibreriaT3.CellType.Hole) //Hay un agujero abajo
                    {
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px + 1);
                        System.Threading.Thread.Sleep(100);
                        opinterfaz.UI.DeleteHex(this.py - 1, this.px + 1);
                    }
                    else if (this.SE.pcolor == null && this.SE.ptipo == null && this.SE.celdatipo == LibreriaT3.CellType.Normal) //Hay un nada abajo
                    {
                        this.SE.pcolor = this.pcolor;
                        this.SE.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px + 1);
                    }
                    else if (this.SE.celdatipo == LibreriaT3.CellType.PortalB || this.SE.celdatipo == LibreriaT3.CellType.PortalO) //Hay un portal abajo
                    {
                        if (this.SE.celdatipo == LibreriaT3.CellType.PortalB) //Entra en B, sale en O
                        {
                            opinterfaz.portalO.pcolor = this.pcolor;
                            opinterfaz.portalO.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalO.py, opinterfaz.portalO.px);
                            opinterfaz.portalO.pushportal("SE");
                        }
                        else // vic
                        {
                            opinterfaz.portalB.pcolor = this.pcolor;
                            opinterfaz.portalB.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalB.py, opinterfaz.portalB.px);
                            System.Threading.Thread.Sleep(100);
                            opinterfaz.portalB.pushportal("SE");
                        }
                    }
                    else if (this.SE.celdatipo == LibreriaT3.CellType.ArrowN || this.SE.celdatipo == LibreriaT3.CellType.ArrowNE || this.SE.celdatipo == LibreriaT3.CellType.ArrowNW || this.SE.celdatipo == LibreriaT3.CellType.ArrowS || this.SE.celdatipo == LibreriaT3.CellType.ArrowSW || this.SE.celdatipo == LibreriaT3.CellType.ArrowSE) //Hay una flecha abajo. Las flechas nunca traen petalos encima. Se mueve el petalo a la flecha, se le entrega la informcion, y el metodo push flecha decide que hacer con ella
                    {
                        this.SE.pcolor = this.pcolor;
                        this.SE.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px + 1);
                        this.SE.pushflecha(this);
                    }
                    this.pcolor = colorIn;
                    this.ptipo = tipoIn;
                    opinterfaz.UI.CreateHex(this.py, this.px, (LibreriaT3.PetalType)this.ptipo, (LibreriaT3.PetalColor)this.pcolor);
                }
            }
            else if (origen == this.NE) //SS = SW , this.px -> this.px - 1, this.py -> this.py
            {
                if (this.SW != null)
                {
                    if (this.SW.pcolor != null && this.SW.ptipo == LibreriaT3.PetalType.Normal && this.SW.celdatipo == LibreriaT3.CellType.Normal)
                    {
                        this.SW.push2(this,false);      //envia la informacion que debe adoptar al siguiente nodo...
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px - 1);
                    }
                    else if (this.SW.celdatipo == LibreriaT3.CellType.Hole) //Hay un agujero abajo
                    {
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px - 1);
                        System.Threading.Thread.Sleep(100);
                        opinterfaz.UI.DeleteHex(this.py, this.px - 1);
                    }
                    else if (this.SW.pcolor == null && this.SW.ptipo == null && this.SW.celdatipo == LibreriaT3.CellType.Normal) //Hay un nada abajo
                    {
                        this.SW.pcolor = this.pcolor;
                        this.SW.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px - 1);
                    }
                    else if (this.SW.celdatipo == LibreriaT3.CellType.PortalB || this.SW.celdatipo == LibreriaT3.CellType.PortalO) //Hay un portal abajo
                    {
                        if (this.SW.celdatipo == LibreriaT3.CellType.PortalB) //Entra en B, sale en O
                        {
                            opinterfaz.portalO.pcolor = this.pcolor;
                            opinterfaz.portalO.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalO.py, opinterfaz.portalO.px);
                            opinterfaz.portalO.pushportal("SW");
                        }
                        else // vic
                        {
                            opinterfaz.portalB.pcolor = this.pcolor;
                            opinterfaz.portalB.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalB.py, opinterfaz.portalB.px);
                            System.Threading.Thread.Sleep(100);
                            opinterfaz.portalB.pushportal("SW");
                        }
                    }
                    else if (this.SW.celdatipo == LibreriaT3.CellType.ArrowN || this.SW.celdatipo == LibreriaT3.CellType.ArrowNE || this.SW.celdatipo == LibreriaT3.CellType.ArrowNW || this.SW.celdatipo == LibreriaT3.CellType.ArrowS || this.SW.celdatipo == LibreriaT3.CellType.ArrowSW || this.SW.celdatipo == LibreriaT3.CellType.ArrowSW) //Hay una flecha abajo. Las flechas nunca traen petalos encima. Se mueve el petalo a la flecha, se le entrega la informcion, y el metodo push flecha decide que hacer con ella
                    {
                        this.SW.pcolor = this.pcolor;
                        this.SW.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px - 1);
                        this.SW.pushflecha(this);
                    }
                    this.pcolor = colorIn;
                    this.ptipo = tipoIn;
                    opinterfaz.UI.CreateHex(this.py, this.px, (LibreriaT3.PetalType)this.ptipo, (LibreriaT3.PetalColor)this.pcolor);
                }
            }
            else if (origen == this.SW) //SS = NE, this.px -> this.px + 1, this.py -> this.py
            {
                if (this.NE != null)
                {
                    if (this.NE.pcolor != null && this.NE.ptipo == LibreriaT3.PetalType.Normal && this.NE.celdatipo == LibreriaT3.CellType.Normal)
                    {
                        this.NE.push2(this,false);      //envia la informacion que debe adoptar al siguiente nodo...
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px + 1);
                    }
                    else if (this.NE.celdatipo == LibreriaT3.CellType.Hole) //Hay un agujero abajo
                    {
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px + 1);
                        System.Threading.Thread.Sleep(100);
                        opinterfaz.UI.DeleteHex(this.py, this.px + 1);
                    }
                    else if (this.NE.pcolor == null && this.NE.ptipo == null && this.NE.celdatipo == LibreriaT3.CellType.Normal) //Hay un nada abajo
                    {
                        this.NE.pcolor = this.pcolor;
                        this.NE.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px + 1);
                    }
                    else if (this.NE.celdatipo == LibreriaT3.CellType.PortalB || this.NE.celdatipo == LibreriaT3.CellType.PortalO) //Hay un portal abajo
                    {
                        if (this.NE.celdatipo == LibreriaT3.CellType.PortalB) //Entra en B, sale en O
                        {
                            opinterfaz.portalO.pcolor = this.pcolor;
                            opinterfaz.portalO.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalO.py, opinterfaz.portalO.px);
                            opinterfaz.portalO.pushportal("NE");
                        }
                        else // vic
                        {
                            opinterfaz.portalB.pcolor = this.pcolor;
                            opinterfaz.portalB.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalB.py, opinterfaz.portalB.px);
                            System.Threading.Thread.Sleep(100);
                            opinterfaz.portalB.pushportal("NE");
                        }
                    }
                    else if (this.NE.celdatipo == LibreriaT3.CellType.ArrowN || this.NE.celdatipo == LibreriaT3.CellType.ArrowNE || this.NE.celdatipo == LibreriaT3.CellType.ArrowNW || this.NE.celdatipo == LibreriaT3.CellType.ArrowS || this.NE.celdatipo == LibreriaT3.CellType.ArrowSW || this.NE.celdatipo == LibreriaT3.CellType.ArrowNE) //Hay una flecha abajo. Las flechas nunca traen petalos encima. Se mueve el petalo a la flecha, se le entrega la informcion, y el metodo push flecha decide que hacer con ella
                    {
                        this.NE.pcolor = this.pcolor;
                        this.NE.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px + 1);
                        this.NE.pushflecha(this);
                    }
                    this.pcolor = colorIn;
                    this.ptipo = tipoIn;
                    opinterfaz.UI.CreateHex(this.py, this.px, (LibreriaT3.PetalType)this.ptipo, (LibreriaT3.PetalColor)this.pcolor);
                }
            }
            else if (origen == this.SS) //SS = NN, this.px -> this.px, this.py -> this.py + 1
            {
                if (this.NN != null)
                {
                    if (this.NN.pcolor != null && this.NN.ptipo == LibreriaT3.PetalType.Normal && this.NN.celdatipo == LibreriaT3.CellType.Normal)
                    {
                        this.NN.push2(this,false);      //envia la informacion que debe adoptar al siguiente nodo...
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px);
                    }
                    else if (this.NN.celdatipo == LibreriaT3.CellType.Hole) //Hay un agujero abajo
                    {
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px);
                        System.Threading.Thread.Sleep(100);
                        opinterfaz.UI.DeleteHex(this.py + 1, this.px);
                    }
                    else if (this.NN.pcolor == null && this.NN.ptipo == null && this.NN.celdatipo == LibreriaT3.CellType.Normal) //Hay un nada abajo
                    {
                        this.NN.pcolor = this.pcolor;
                        this.NN.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px);
                    }
                    else if (this.NN.celdatipo == LibreriaT3.CellType.PortalB || this.NN.celdatipo == LibreriaT3.CellType.PortalO) //Hay un portal abajo
                    {
                        if (this.NN.celdatipo == LibreriaT3.CellType.PortalB) //Entra en B, sale en O
                        {
                            opinterfaz.portalO.pcolor = this.pcolor;
                            opinterfaz.portalO.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalO.py, opinterfaz.portalO.px);
                            opinterfaz.portalO.pushportal("NN");
                        }
                        else // vic
                        {
                            opinterfaz.portalB.pcolor = this.pcolor;
                            opinterfaz.portalB.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalB.py, opinterfaz.portalB.px);
                            System.Threading.Thread.Sleep(100);
                            opinterfaz.portalB.pushportal("NN");
                        }
                    }
                    else if (this.NN.celdatipo == LibreriaT3.CellType.ArrowN || this.NN.celdatipo == LibreriaT3.CellType.ArrowNE || this.NN.celdatipo == LibreriaT3.CellType.ArrowNW || this.NN.celdatipo == LibreriaT3.CellType.ArrowS || this.NN.celdatipo == LibreriaT3.CellType.ArrowSW || this.NN.celdatipo == LibreriaT3.CellType.ArrowN) //Hay una flecha abajo. Las flechas nunca traen petalos encima. Se mueve el petalo a la flecha, se le entrega la informcion, y el metodo push flecha decide que hacer con ella
                    {
                        this.NN.pcolor = this.pcolor;
                        this.NN.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px);
                        this.NN.pushflecha(this);
                    }
                    this.pcolor = colorIn;
                    this.ptipo = tipoIn;
                    opinterfaz.UI.CreateHex(this.py, this.px, (LibreriaT3.PetalType)this.ptipo, (LibreriaT3.PetalColor)this.pcolor);
                }
            }
            else if (origen == this.SE) //SS = NW , this.px -> this.px - 1, this.py -> this.py + 1
            {
                if (this.NW != null)
                {
                    if (this.NW.pcolor != null && this.NW.ptipo == LibreriaT3.PetalType.Normal && this.NW.celdatipo == LibreriaT3.CellType.Normal)
                    {
                        this.NW.push2(this,false);      //envia la informacion que debe adoptar al siguiente nodo...
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px - 1);
                    }
                    else if (this.NW.celdatipo == LibreriaT3.CellType.Hole) //Hay un agujero abajo
                    {
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px - 1);
                        System.Threading.Thread.Sleep(100);
                        opinterfaz.UI.DeleteHex(this.py + 1, this.px - 1);
                    }
                    else if (this.NW.pcolor == null && this.NW.ptipo == null && this.NW.celdatipo == LibreriaT3.CellType.Normal) //Hay un nada abajo
                    {
                        this.NW.pcolor = this.pcolor;
                        this.NW.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px - 1);
                    }
                    else if (this.NW.celdatipo == LibreriaT3.CellType.PortalB || this.NW.celdatipo == LibreriaT3.CellType.PortalO) //Hay un portal abajo
                    {
                        if (this.NW.celdatipo == LibreriaT3.CellType.PortalB) //Entra en B, sale en O
                        {
                            opinterfaz.portalO.pcolor = this.pcolor;
                            opinterfaz.portalO.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalO.py, opinterfaz.portalO.px);
                            opinterfaz.portalO.pushportal("NW");
                        }
                        else // vic
                        {
                            opinterfaz.portalB.pcolor = this.pcolor;
                            opinterfaz.portalB.ptipo = this.ptipo;
                            opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalB.py, opinterfaz.portalB.px);
                            System.Threading.Thread.Sleep(100);
                            opinterfaz.portalB.pushportal("NW");
                        }
                    }
                    else if (this.NW.celdatipo == LibreriaT3.CellType.ArrowN || this.NW.celdatipo == LibreriaT3.CellType.ArrowNE || this.NW.celdatipo == LibreriaT3.CellType.ArrowNW || this.NW.celdatipo == LibreriaT3.CellType.ArrowS || this.NW.celdatipo == LibreriaT3.CellType.ArrowSW || this.NW.celdatipo == LibreriaT3.CellType.ArrowNW) //Hay una flecha abajo. Las flechas nunca traen petalos encima. Se mueve el petalo a la flecha, se le entrega la informcion, y el metodo push flecha decide que hacer con ella
                    {
                        this.NW.pcolor = this.pcolor;
                        this.NW.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px - 1);
                        this.NW.pushflecha(this);
                    }
                    this.pcolor = colorIn;
                    this.ptipo = tipoIn;
                    opinterfaz.UI.CreateHex(this.py, this.px, (LibreriaT3.PetalType)this.ptipo, (LibreriaT3.PetalColor)this.pcolor);
                }
            }
            #endregion
        }
        /// <summary>
        /// Empuje característico de las flechas
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="tipoIn"></param>
        /// <param name="colorIn"></param>
        public void pushflecha(Celda origen) // Recibida ya la informacion, empuja este pétalo
        {
            switch (this.celdatipo)
            {
                case LibreriaT3.CellType.ArrowN:
                    this.NN.push2(this,false);
                    this.NN.pcolor = this.pcolor;
                    this.NN.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case LibreriaT3.CellType.ArrowNE:
                    this.NE.push2(this,false);
                    this.NE.pcolor = this.pcolor;
                    this.NE.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px + 1);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case LibreriaT3.CellType.ArrowNW:
                    this.NW.push2(this,false);
                    this.NW.pcolor = this.pcolor;
                    this.NW.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px -1);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case LibreriaT3.CellType.ArrowS:
                    this.SS.push2(this,false);
                    this.SS.pcolor = this.pcolor;
                    this.SS.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case LibreriaT3.CellType.ArrowSE:
                    this.SE.push2(this,false);
                    this.SE.pcolor = this.pcolor;
                    this.SE.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px + 1);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case LibreriaT3.CellType.ArrowSW:
                    this.SW.push2(this,false);
                    this.SW.pcolor = this.pcolor;
                    this.SW.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px - 1);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                default:
                    Console.WriteLine("ERROR : 000001");
                    break;
            }
        }
        public bool rodeada()
        {
            this.mfd = false;
            if (this.pcolor != null && this.NN.pcolor == this.pcolor && this.NW.pcolor == this.pcolor && this.NE.pcolor == this.pcolor && this.SW.pcolor == this.pcolor && this.SE.pcolor == this.pcolor && this.SS.pcolor == this.pcolor)
            {
                this.mfd = true;
            }

            return this.mfd;
        }
        public void boom()
        {
            Celda[] celdas = {this.NN, this.NW, this.NE, this.SS, this.SE, this.SW };
            if (mfd)
            {
                bool bloomsolo = true;
                if (bloomsolo)
                {
                    foreach (Celda celdamfd in celdas)
                    {
                        celdamfd.push2(this,true);
                    }
                }
            }
        }
        public void pushportal(string origen)
        {
            switch (origen)
            {
                case "NN":
                    this.NN.push2(this,false);
                    this.NN.pcolor = this.pcolor;
                    this.NN.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case "NE":
                    this.NE.push2(this,false);
                    this.NE.pcolor = this.pcolor;
                    this.NE.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px + 1);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case "NW":
                    this.NW.push2(this,false);
                    this.NW.pcolor = this.pcolor;
                    this.NW.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py + 1, this.px - 1);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case "SS":
                    this.SS.push2(this,false);
                    this.SS.pcolor = this.pcolor;
                    this.SS.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case "SE":
                    this.SE.push2(this,false);
                    this.SE.pcolor = this.pcolor;
                    this.SE.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py - 1, this.px + 1);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                case "SW":
                    this.SW.push2(this,false);
                    this.SW.pcolor = this.pcolor;
                    this.SW.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, this.py, this.px - 1);
                    this.ptipo = null;
                    this.pcolor = null;
                    break;
                default:
                    Console.WriteLine("ERROR : 000001");
                    break;
            }
        }
        /// <summary>
        /// Envia los datos a la siguiente celda, Empuja la siguiente celda
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="tipoIn"></param>
        /// <param name="colorIn"></param>
        public void push2(Celda origen, bool destruir)
        {
            if (this.pcolor == null && this.ptipo == null) 
            { }
            #region
            Celda tmp = null;
            int nx = 0;
            int ny = 0;
            string dest = null;
            if (origen == this.NN)
            {
                tmp = this.SS;
                 ny = this.py - 1;
                 nx = this.px;
                 dest = "SS";
            }
            else if(origen == this.NW) // SS = SE , this.px -> this.px + 1, this.py -> this.py - 1
            {
                tmp = this.SE;
                 ny = this.py - 1;
                 nx = this.px + 1;
                 dest = "SE";
            }
            else if (origen == this.NE) //SS = SW , this.px -> this.px - 1, this.py -> this.py
            {
                tmp = this.SW;
                 ny = this.py;
                 nx = this.px - 1;
                 dest = "SW";
            }
            else if (origen == this.SW) //SS = NE, this.px -> this.px + 1, this.py -> this.py
            {
                tmp = this.NE;
                 ny = this.py;
                 nx = this.px + 1;
                 dest = "NE";
            }
            else if (origen == this.SS) //SS = NN, this.px -> this.px, this.py -> this.py + 1
            {
                tmp = this.NN;
                 ny = this.py + 1;
                 nx = this.px;
                 dest = "NN";
            }
            else if (origen == this.SE) //SS = NW , this.px -> this.px - 1, this.py -> this.py + 1
            {
                tmp = this.NW;
                ny = this.py + 1;
                nx = this.px - 1;
                dest = "SW";
            }
            if (tmp != null) //generalizar
            {
                if (tmp.pcolor != null && tmp.ptipo == LibreriaT3.PetalType.Normal && tmp.celdatipo == LibreriaT3.CellType.Normal) //Hay un petalo comun abajo. Se empuja el siguiente, se le envia la informacion de este petalo y se le entrega el petalo
                {
                    tmp.push2(this,false);      //envia la informacion que debe adoptar al siguiente nodo...
                    tmp.pcolor = this.pcolor;
                    tmp.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, ny,nx);
                }
                else if (tmp.pcolor == null && tmp.celdatipo == LibreriaT3.CellType.Normal) // Hay un nada abajo, se le entrega informacion y luego el pétalo
                {
                    tmp.pcolor = this.pcolor;
                    tmp.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, ny,nx);
                }
                else if (tmp.celdatipo == LibreriaT3.CellType.Hole) //Hay un agujero abajo. Se entrega el pétalo, y luego se elimina
                {
                    opinterfaz.UI.MoveHex(this.py, this.px, ny,nx);
                    opinterfaz.UI.DeleteHex(ny,nx);
                }
                else if (tmp.celdatipo == LibreriaT3.CellType.PortalB || tmp.celdatipo == LibreriaT3.CellType.PortalO) // Hay un portal abajo
                {
                    if (tmp.celdatipo == LibreriaT3.CellType.PortalB) //Entra en B, sale en O
                    {
                        opinterfaz.portalO.pcolor = this.pcolor;
                        opinterfaz.portalO.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalO.py, opinterfaz.portalO.px);
                        opinterfaz.portalO.pushportal(dest);
                    }
                    else // vic
                    {
                        opinterfaz.portalB.pcolor = this.pcolor;
                        opinterfaz.portalB.ptipo = this.ptipo;
                        opinterfaz.UI.MoveHex(this.py, this.px, opinterfaz.portalB.py, opinterfaz.portalB.px);
                        System.Threading.Thread.Sleep(100);
                        opinterfaz.portalB.pushportal(dest);
                    }
                }
                else if (tmp.celdatipo == LibreriaT3.CellType.ArrowN || tmp.celdatipo == LibreriaT3.CellType.ArrowNE || tmp.celdatipo == LibreriaT3.CellType.ArrowNW || tmp.celdatipo == LibreriaT3.CellType.ArrowS || tmp.celdatipo == LibreriaT3.CellType.ArrowSW || tmp.celdatipo == LibreriaT3.CellType.ArrowSE) //Hay una flecha abajo. Las flechas nunca traen petalos encima. Se mueve el petalo a la flecha, se le entrega la informcion, y el metodo push flecha decide que hacer con ella
                {
                    tmp.pcolor = this.pcolor;
                    tmp.ptipo = this.ptipo;
                    opinterfaz.UI.MoveHex(this.py, this.px, ny,nx);
                    pushflecha(this);
                }
                if (destruir == true)
                {
                    opinterfaz.UI.HighLight(true, ny, nx);
                    opinterfaz.UI.DeleteHex(ny, nx);
                    tmp.pcolor = null;
                    tmp.ptipo = null;
                }
            }
            #endregion

        }
    }
}
