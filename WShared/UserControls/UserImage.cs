using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using NS_AppConfig;
using NS_WUtilities;

namespace NS_UserImage
{
    public partial class UserImage:Form
    {
        /***************************************************************************
        SPECIFICATION: Accessors
        CREATED:       15.04.2021
        LAST CHANGE:   15.04.2021
        ***************************************************************************/
        public Bitmap Picture { get { return m_Picture; } }

        /***************************************************************************
        SPECIFICATION: Members
        CREATED:       17.09.2015
        LAST CHANGE:   11.05.2021
        ***************************************************************************/
        //private Bitmap      m_Bitmap;
        private ImageParams m_Params;
        private Bitmap      m_Picture;
        private Memory      m_Mem;

        /***************************************************************************
        SPECIFICATION: C'tor
        CREATED:       17.09.2015
        LAST CHANGE:   17.09.2015
        ***************************************************************************/
        public UserImage()
        {
            InitializeComponent();
            m_Mem = new Memory();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.09.2015
        LAST CHANGE:   18.09.2015
        ***************************************************************************/
        public void Serialize( ref AppSettings a_Conf )
        {
            if( a_Conf.IsReading )
            {
                a_Conf.DeserializeDialog( this );
            }
            else
            {
                a_Conf.SerializeDialog( this );
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.09.2015
        LAST CHANGE:   11.05.2021
        ***************************************************************************/
        public Bitmap Read16BitGray( List<byte> a_Data, ImageParams a_Parms )
        {
            m_Params = a_Parms;

            int width  = a_Parms.Width;
            int height = a_Parms.Height;
            int i=0;

            List<byte> dat = new List<byte>();

            Rectangle rect = new Rectangle( 0,0,width,height);
            m_Mem.AssignMem( ref a_Data );

            Bitmap result  = new Bitmap( width, height );
            Graphics graph = Graphics.FromImage( result );

            for( int y = 0; y < height; y++ )
            {
                for( int x = 0; x < width; x++ )
                {
                    UInt16 gray = m_Mem.GetMem2( ref i );

                    int gr = gray * 0xff / a_Parms.MaxGrayVal;

                    Color col = Color.FromArgb( gr, gr, gr );
                    result.SetPixel(x,y,col);
                }
            }

            m_Picture = result;
            return result;
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       15.04.2021
        LAST CHANGE:   15.04.2021
        ***************************************************************************/
        public Bitmap Read8BitGray( List<byte> a_Data, ImageParams a_Parms )
        {
            m_Params = a_Parms;

            int width  = a_Parms.Width;
            int height = a_Parms.Height;
            int i=0;

            List<byte> dat = new List<byte>();

            Rectangle rect = new Rectangle( 0,0,width,height);
            m_Mem.AssignMem( ref a_Data );

            Bitmap result  = new Bitmap( width, height );
            Graphics graph = Graphics.FromImage( result );

            for( int y = 0; y < height; y++ )
            {
                for( int x = 0; x < width; x++ )
                {
                    byte gray = m_Mem.GetMem1( ref i );

                    int gr = gray * 0xff / a_Parms.MaxGrayVal;

                    Color col = Color.FromArgb( gr, gr, gr );
                    result.SetPixel(x,y,col);
                }
            }

            m_Picture = result;
            return result;
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.09.2015
        LAST CHANGE:   15.12.2015
        ***************************************************************************/
        private delegate void dl_ShowBmp( Bitmap a_Bmp );

        public void ShowBmp( Bitmap a_Bmp )
        {
            if( this.InvokeRequired )
            {
                dl_ShowBmp d = new dl_ShowBmp( ShowBmp );
                this.Invoke( d, new object[]{ a_Bmp } );
            }
            else
            {
                m_Picture = a_Bmp;
                pictureBox.Image = a_Bmp;
                pictureBox.Show();

                int offsy = this.Size.Height - pictureBox.Size.Height;
                int offsx = this.Size.Width  - pictureBox.Size.Width;

                if (m_Params == null) this.Size = a_Bmp.Size;
                else                  this.Size = new Size( m_Params.Width + offsx, m_Params.Height + offsy );

                Show();
                BringToFront();
            }
        }


        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.09.2015
        LAST CHANGE:   17.09.2015
        ***************************************************************************/
        private void UserImage_FormClosing( object sender, FormClosingEventArgs e )
        {
            e.Cancel = true;
            this.Hide();
        }

        /***************************************************************************
        SPECIFICATION: 
        CREATED:       17.09.2015
        LAST CHANGE:   17.09.2015
        ***************************************************************************/
        private void pictureBox_DragDrop( object sender, DragEventArgs e )
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop,false))
            {
                string[] sa = (string[])e.Data.GetData(DataFormats.FileDrop);
                string   fn = sa[0];
                string   dir = fn;
                if (! Directory.Exists(fn) )
                {
                    dir = Directory.GetParent(fn).FullName;
                }

                //Bitmap.load
            }
        }

    } // class


    /***************************************************************************
    SPECIFICATION: Data types
    CREATED:       17.09.2015
    LAST CHANGE:   15.04.2021
    ***************************************************************************/
    public class ImageParams
    {
        public int Width;
        public int Height;
        public int MaxGrayVal;
        public int XOffs;
        public int YOffs;

        public ImageParams( int a_Width, int a_Height, int a_MaxGrayVal )
        {
            Width      = a_Width;
            Height     = a_Height;
            MaxGrayVal = a_MaxGrayVal;
        }
        public ImageParams( int a_Width, int a_Height, int a_MaxGrayVal, int a_XOffs, int a_YOffs )
        {
            Width      = a_Width;
            Height     = a_Height;
            MaxGrayVal = a_MaxGrayVal;
            XOffs      = a_XOffs;
            YOffs      = a_YOffs; 
        }

    }
} // namespace 
