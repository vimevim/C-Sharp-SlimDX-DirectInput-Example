using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.DirectInput;

namespace SlimDXExamples
{
    public partial class GamePadMouse : Form
    {
        //bu sayfada direkt olarak SlimDX.DirectInput kullanılmıştır.
        //SlimDX NuGet and Packages kısmından yazılarak indirilebilir.
        //https://youtu.be/rtnLGfAj7W0 --> bu projenin yapılış videosu

        public GamePadMouse()
        {
            InitializeComponent();
            Sticks = GetSticks();
            mouseTimer.Enabled = true;
        }

        DirectInput Input = new DirectInput();
        SlimDX.DirectInput.Joystick stick;
        Joystick[] Sticks;
        bool mouseClicked = false;
        int leftValueY = 0;
        int leftValueX = 0;
        int rightValueX = 0;
        int rightValueY = 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern void mouse_event(uint flag, uint _x, uint _y, uint btn, uint exInfo);
        private const int MOUSEEVENT_LEFTDOWN = 0x02;//Left click pressed
        private const int MOUSEEVENT_LEFTUP = 0x04;//Left click released
        private const int MOUSEEVENT_RIGHTDOWN = 0x08; //Right click pressed
        private const int MOUSEEVENT_RIGHTUP = 0x10; //Right click released


        public Joystick[] GetSticks()
        {
            List<SlimDX.DirectInput.Joystick> Sticks = new List<SlimDX.DirectInput.Joystick>();
            foreach (DeviceInstance device in Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    stick = new SlimDX.DirectInput.Joystick(Input, device.InstanceGuid);
                    stick.Acquire();

                    foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                        {
                            stick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
                        }
                    }
                    Sticks.Add(stick);
                }
                catch (DirectInputException)
                {

                }
            }
            return Sticks.ToArray();
        }

        void stickHandle(Joystick stick, int id)
        {
            JoystickState state = new JoystickState();
            state = stick.GetCurrentState();

            leftValueY = state.Y;
            leftValueX = state.X;
            rightValueX = state.Z;
            rightValueY = state.RotationZ;
            MouseMoveLeft(leftValueX, leftValueY);
            MouseMoveRight(rightValueX, rightValueY);

            bool[] buttons = state.GetButtons();

            if (id == 0)
            {
                if (buttons[1])
                {
                    if (mouseClicked == false)
                    {
                        mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, 0);
                        mouseClicked = true;
                    }
                }
                else
                {
                    if (mouseClicked == true)
                    {
                        mouse_event(MOUSEEVENT_LEFTUP, 0, 0, 0, 0);
                        mouseClicked = false;

                    }
                }

                if (buttons[0])
                {
                    if (mouseClicked == false)
                    {
                        mouse_event(MOUSEEVENT_RIGHTDOWN, 0, 0, 0, 0);
                        mouseClicked = true;
                    }
                }
                else
                {
                    if (mouseClicked == true)
                    {
                        mouse_event(MOUSEEVENT_RIGHTUP, 0, 0, 0, 0);
                        mouseClicked = false;

                    }
                }
            }
        }
        public void MouseMoveLeft(int posx, int posy)
        {
            Cursor.Position = new Point(Cursor.Position.X + posx / 3, Cursor.Position.Y + posy / 3);
            Cursor.Clip = new Rectangle(Screen.PrimaryScreen.WorkingArea.Location, Screen.PrimaryScreen.WorkingArea.Size);
            this.WindowState = FormWindowState.Minimized;
        }
        public void MouseMoveRight(int posx, int posy)
        {
            Cursor.Position = new Point(Cursor.Position.X + posx / 7, Cursor.Position.Y + posy / 7);
            Cursor.Clip = new Rectangle(Screen.PrimaryScreen.WorkingArea.Location, Screen.PrimaryScreen.WorkingArea.Size);
            this.WindowState = FormWindowState.Minimized;
        }
        private void mouseTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Sticks.Length; i++)
            {
                stickHandle(Sticks[i], i);
            }
        }
    }
}
