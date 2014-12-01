using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tarea_2
{
    class Escritor_imagen
    {
        private bool mododoge;
        private string pathimg;
        private string nombre;
        private int desfase;
        private byte[] emptycell = new byte[3675];
        private byte[] emptynod = new byte[3675];
        private byte[] rednod = new byte[3675];
        private byte[] redcell = new byte[3675];
        private byte[] grenod = new byte[3675];
        private byte[] grecell = new byte[3675];
        private byte[] blunod = new byte[3675];
        private byte[] blucell = new byte[3675];
        private byte[] ornod = new byte[3675];
        private byte[] orcell = new byte[3675];
        private byte[] purnod = new byte[3675];
        private byte[] purcell = new byte[3675];
        private byte[] yelnod = new byte[3675];
        private byte[] yelcell = new byte[3675];

        private void cargarImagenes()
        {
            byte[][] imgs = { this.emptycell, this.emptynod, this.grecell, this.grenod, this.blucell, this.blunod, this.redcell, this.rednod, this.purcell, this.purnod, this.orcell, this.ornod, this.yelcell, this.yelnod };
            string[] noms = { "emptycell.bmp", "emptynode.bmp", "greencell.bmp", "greennode.bmp", "bluecell.bmp", "bluenode.bmp", "redcell.bmp", "rednode.bmp", "purplecell.bmp", "purplenode.bmp", "orangecell.bmp", "orangenode.bmp", "yellowcell.bmp", "yellownode.bmp" };
            
            //Cargador universal de imágenes

            for(int sel = 0; sel<imgs.Length;sel++)
            {
                byte[] alm = imgs[sel];
                string nom = noms[sel];
                FileStream tstr = new FileStream("../../Imagenes/"+nom, FileMode.Open);
                BinaryReader tlec = new BinaryReader(tstr);
                tstr.Position = 54;
                for (int i = 0; i < 35; i++)
                {
                    byte[] buffer = tlec.ReadBytes(105);
                    for (int j = 0; j < 105; j++)
                    {
                        alm[(i * 105) + j] = buffer[j];
                    }
                    tstr.Position += 3;
                }
                tstr.Close();
            }
        }


        public Escritor_imagen(string path, string nombre, int largo, int ancho) //Creador de imagen base
        {
            cargarImagenes();
            this.pathimg = path;
            this.nombre = nombre;
            FileStream imagen = new FileStream(pathimg+nombre+".bmp", FileMode.Create);
            //imagen en negro basica, sin nada
            BinaryWriter escritor = new BinaryWriter(imagen);
            #region
            //Crear header
            //Cada cuadro mide 35*35 pixeles
            int ival = 0;
            short sval = 0;
            sval = 19778;
            escritor.Write(sval);        //primera parte del header
            this.desfase = (ancho * 3) % 4;
            ival = 35 * 35 * largo * ancho * 3 + 54;            //Tamaño total
            escritor.Write(ival);
            sval = 0;
            escritor.Write(sval);        //Header reservado
            escritor.Write(sval);        //Header reservado
            ival = 54;                  //offset a data
            escritor.Write(ival);
            //header v1
            ival = 40;          //Tamaño header
            escritor.Write(ival);
            ival = (ancho * 35);
            escritor.Write(ival);
            ival = (largo * 35);
            escritor.Write(ival);
            sval = 1;       //planos de color
            escritor.Write(sval);
            sval = 24;
            escritor.Write(sval); //bits usados 24
            ival = 0;
            escritor.Write(ival); // Compresion = 0
            ival = 0;//Size imagen
            escritor.Write(ival); //Size image
            ival = 0;
            escritor.Write(ival);
            ival = 0;
            escritor.Write(ival);
            ival = 0;
            escritor.Write(ival);
            ival = 0;
            escritor.Write(ival);
            byte bval = 0;
            for (int i = 0; i < (35 * largo); i++)
            {
                for (int j = 0; j < (35 * ancho); j++)
                {
                    escritor.Write(bval);
                    escritor.Write(bval);
                    escritor.Write(bval);
                }
                for (int k = 0; k < this.desfase; k++)
                {
                    escritor.Write((byte)0);
                }
            }
            #endregion
            escritor.Close();
            imagen.Close();
        }

        public void dibujar_cuadro (int relx, int rely, char color, int lx, int ly)
        {
            return;
            /*
            FileStream imagen = new FileStream(pathimg + nombre + ".bmp", FileMode.Open);
            int bx = relx * 35;
            int by = rely * 35;
            BinaryWriter escritor = new BinaryWriter(imagen);
            imagen.Position=54;
            switch (color)
            {
                case ' ':
                    for (int j = 0; j < 35; j++)
                    {
                        emptycell.
                        for (int i = 0; i < 105; i++)
                        {
                            imagen.Write(
                        }
                    }
                    break;
                    //for (int j = 0; j < 35; j++)
                    //{
                    //    for (int i = 0; i < 105; i++)
                    //    {
                    //        escritor.Write(emptycell, (j * i) + j, 105);
                    //    }
                    //}
                    //break;
                case 'W':
                    break;
                case 'R':
                    break;
                case 'r':
                    break;
                case 'G':
                    break;
                case 'g':
                    break;
                case 'Y':
                    break;
                case 'y':
                    break;
                case 'P':
                    break;
                case 'p':
                    break;
                case 'B':
                    break;
                case 'b':
                    break;
                case 'O':
                    break;
                case 'o':
                    break;
                default:
                    break;
            }
            escritor.Close();
            imagen.Close();
             */
        }
    }
}
