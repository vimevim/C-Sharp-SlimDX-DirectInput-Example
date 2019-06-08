using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.DirectInput;

namespace SlimDXExamples
{
    public partial class GamePadValues : Form
    {
        DirectInput Input = new DirectInput();
        SlimDX.DirectInput.Joystick stick;
        SlimDX.DirectInput.Joystick[] Sticks;
        public GamePadValues()
        {
            InitializeComponent();
            Sticks = GetSticks();
        }
        public SlimDX.DirectInput.Joystick[] GetSticks()
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
                    lbJoystick.Text = "Joystick (Connected)";
                    joystickTimer.Enabled = true;
                }
                catch (DirectInputException ex)
                {
                    lbJoystick.Text = "Joystick (No Connected)";
                }
            }
            return Sticks.ToArray();
        }

        string labelDegeri = "";

        private void joystickTimer_Tick(object sender, EventArgs e)
        {
            JoystickState state = new JoystickState();
            state = stick.GetCurrentState();

            int leftStickX = state.X;
            int leftStickY = state.Y;
            int rightStickX = state.Z;
            int rightStickY = state.RotationZ;
            this.Text = state.RotationX.ToString();
            //stick simülasyonları
            radioButton1.Left = leftStickX + 100;//left left-right
            radioButton1.Top = leftStickY + 100;//left up-down
            radioButton2.Left = rightStickX + 100;//right left-right
            radioButton2.Top = rightStickY + 100;//right up-down

            if (state.IsPressed(10))
            {
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Checked = true;
            }
            if (state.IsPressed(11))
            {
                radioButton2.Checked = false;
            }
            else
            {
                radioButton2.Checked = true;
            }



            if (state.IsPressed(9))
            {
                labelDegeri = "START";
            }
            if (state.IsPressed(8))
            {
                labelDegeri = "SELECT";
            }
            if (state.IsPressed(7))
            {
                labelDegeri = "R2";
            }
            if (state.IsPressed(6))
            {
                labelDegeri = "L2";
            }
            if (state.IsPressed(5))
            {
                labelDegeri = "R1";
            }
            if (state.IsPressed(4))
            {
                labelDegeri = "L1";
            }
            if (state.IsPressed(3))
            {
                labelDegeri = "Y";
            }
            if (state.IsPressed(2))
            {
                labelDegeri = "X";
            }
            if (state.IsPressed(1))
            {
                labelDegeri = "B";
            }
            if (state.IsPressed(0))
            {
                labelDegeri = "A";
            }
            label1.Text = labelDegeri;
        }
    }
}
