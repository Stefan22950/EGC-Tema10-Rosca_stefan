using System;
using System.Drawing;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using OpenTK3_StandardTemplate_WinForms.helpers;
using OpenTK3_StandardTemplate_WinForms.objects;

namespace OpenTK3_StandardTemplate_WinForms
{
    public partial class MainForm : Form
    {
        private Axes mainAxis;
        private Rectangles re;
        private Camera cam;
        private Scene scene;

        private Point mousePosition;
        private Vector3 paralPosition = Vector3.Zero;

        private bool paralSelected=false;
        private bool rombSelected=true;

        public MainForm()
        {   
            // general init
            InitializeComponent();

            // init VIEWPORT
            scene = new Scene();

            scene.GetViewport().Load += new EventHandler(this.mainViewport_Load);
            scene.GetViewport().Paint += new PaintEventHandler(this.mainViewport_Paint);
            scene.GetViewport().MouseMove += new MouseEventHandler(this.mainViewport_MouseMove);

            this.Controls.Add(scene.GetViewport());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // init RNG
            Randomizer.Init();

            // init CAMERA/EYE
            cam = new Camera(scene.GetViewport());

            // init AXES
            mainAxis = new Axes(showAxes.Checked);
            re = new Rectangles();
        }

        private void showAxes_CheckedChanged(object sender, EventArgs e)
        {
            mainAxis.SetVisibility(showAxes.Checked);

            scene.Invalidate();
        }

        private void changeBackground_Click(object sender, EventArgs e)
        {
            GL.ClearColor(Randomizer.GetRandomColor());

            scene.Invalidate();
        }

        private void resetScene_Click(object sender, EventArgs e)
        {
            showAxes.Checked = true;
            mainAxis.SetVisibility(showAxes.Checked);
            scene.Reset();
            cam.Reset();

            scene.Invalidate();
        }

        private void mainViewport_Load(object sender, EventArgs e)
        {
            scene.Reset();
        }

        private void mainViewport_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mousePosition = new Point(e.X, e.Y);
            scene.Invalidate();
        }

        private void mainViewport_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            

            cam.SetView();

            if (enableRotation.Checked == true)
            {
                // Doar după axa Ox.
                GL.Rotate(Math.Max(mousePosition.X, mousePosition.Y), 1, 1, 1);
            }

            // GRAPHICS PAYLOAD
            mainAxis.Draw();

            objectSelected(); //Verifica care obiect este selectat

            rotateObject();

            MoveObject();


            scene.GetViewport().SwapBuffers();
        }


        private void MoveObjectbackend()
        {
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Key.Escape))
            {
                Close();
            }

            if (keyboard.IsKeyDown(Key.Left))
                paralPosition.X -= 0.1f;

            if (keyboard.IsKeyDown(Key.Right))
                paralPosition.X += 0.1f;

            if (keyboard.IsKeyDown(Key.Up))
                paralPosition.Y += 0.1f;

            if (keyboard.IsKeyDown(Key.Down))
                paralPosition.Y -= 0.1f;

            scene.Invalidate();
        }

        private void MoveObject()
        {
            if (mvCheck.Checked == true && paralSelected==true)
            {
                // Rotatie a obiectului
                GL.PushMatrix();
                MoveObjectbackend();
                GL.Translate(paralPosition);
                re.DrawParal();
                GL.PopMatrix();
            }
            else
            {
                re.DrawParal();
            }

            if (mvCheck.Checked == true && rombSelected == true)
            {
                // Rotatie a obiectului
                GL.PushMatrix();
                MoveObjectbackend();
                GL.Translate(paralPosition);
                re.DrawRomb();
                GL.PopMatrix();
            }
            else
            {
                re.DrawRomb();
            }
        }

        private void rotateObject()
        {
            if (enableObjectRotation.Checked == true && paralSelected==true)
            {
                // Rotatie a obiectului
                GL.PushMatrix();
                GL.Rotate(Math.Max(mousePosition.X, mousePosition.Y), 1, 1, 1);
                re.DrawParal();
                GL.PopMatrix();
            }
            else
            {
                re.DrawParal();
                
            }

            if (enableObjectRotation.Checked == true && rombSelected == true)
            {
                // Rotatie a obiectului
                GL.PushMatrix();
                GL.Rotate(Math.Max(mousePosition.X, mousePosition.Y), 1, 1, 1);
                re.DrawRomb();
                GL.PopMatrix();
            }
            else
            {
                re.DrawRomb();

            }

        }

        private void objectSelected()
        {
            var mousePosition = Mouse.GetState();

            if (mousePosition.X >=-35 && mousePosition.X <= -5 && mousePosition.Y >=5 && mousePosition.Y <=40) 
            {
                paralSelected = true;
                rombSelected = false;
            }
            else if(mousePosition.X >=0 && mousePosition.X<=20 && mousePosition.Y>=-20 && mousePosition.Y <=20)
            {
                paralSelected=false;
                rombSelected = true;

            }
            

        }

    }
}
