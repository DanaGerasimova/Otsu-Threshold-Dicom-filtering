using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using SharpGL.Enumerations;
using Dicom;
using SharpGL.SceneGraph.Assets;


namespace cartesian_spherical
{
    public partial class Form1 : Form
    {
        OpenGL gl;

        Texture ish = new Texture();
        Texture otsu_segment = new Texture();

        List<ushort> pixels = new List<ushort>();
        DicomDecoder dd = new DicomDecoder();
        byte[] pix = new byte[1];

        public Form1()
        {
            InitializeComponent();
            gl = (gl == null) ? this.openGLControl1.OpenGL : gl;
            dd.DicomFileName = "DICOM_Image_for_Lab_5.dcm";
            dd.GetPixels16(ref pixels);

            ish.Create(gl, pixels.Bitmap(dd.width, dd.height));
            otsu_segment.Create(gl, pixels.Bitmap(dd.width, dd.height).otsu());
        }

        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            gl = (gl == null) ? this.openGLControl1.OpenGL : gl;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            gl.Viewport(0, 0, this.Width, this.Height);
            gl.MatrixMode(MatrixMode.Projection);
            gl.Ortho(-this.Width / 2, this.Width / 2, -this.Height / 2, this.Height / 2, -1, 1);

            gl.Enable(OpenGL.GL_TEXTURE_2D);

            gl.Begin(BeginMode.Quads);
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(-dd.width / 2, -dd.height / 2);
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(dd.width / 2, -dd.height / 2);
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(dd.width / 2, dd.height / 2);
            gl.TexCoord(0.0f, 0.0f); gl.Vertex(-dd.width / 2, dd.height / 2);
            gl.End();
            gl.Flush();
        }

        private void openGLControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.A))
            {
                Text = "ish";
                ish.Bind(gl);
            }
            if (e.KeyChar == Convert.ToChar(Keys.S))
            {
                Text = "otsu_segmentation";
                otsu_segment.Bind(gl);
            }
            if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                Close();
            }
        }
    }

}
