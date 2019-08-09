using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.IO;

namespace Lab4_new_sdk_WF
{
    public partial class Form1 : Form
    {
        //Initial
        public Form1()
        {
            InitializeComponent();
        }

        //Kinect set
        KinectSensor sensor = KinectSensor.KinectSensors[0];
        Skeleton[] skeletons;
        byte[] pixelData;
        //RS232 set
        byte[] tx = new byte[1];
        //Initial
        uint GameStartFlag = 0;
        //timer
        int TextChangeFlag = 0;
        //GameTime
        int GameTimeValued = 30;
        int GameOver=0;
        //Hip
        int[] HipCenter_In = new int[3];
        int[] HipCenter_Comp = new int[3];
        int HipCenter_Flag = 0;
        byte status = 0;
        int StartButtonFlag = 0;

        //Load
        private void Form1_Load(object sender, EventArgs e) //Window Initial
        {
            //kinect
            sensor.Start();
            sensor.ElevationAngle = -10;
            sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(sensor_SkeletonFrameReady);
            sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);
            sensor.ColorStream.Enable();
            sensor.SkeletonStream.Enable();
            //serial
            serialPort1.Open();
            //Timer
            TextChangeTimer.Start();
            //Initial
            GameTimeValue.Text = Convert.ToString(GameTimeValued);
            StatusValue.Text = "Waiting";
        }

        List<JointCollection> jcs = new List<JointCollection>();

        void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {

            jcs.Clear();
            bool receivedData = false;

            using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
            {
                if (SFrame == null)
                {
                    // The image processing took too long. More than 2 frames behind.
                }
                else
                {
                    skeletons = new Skeleton[SFrame.SkeletonArrayLength];
                    SFrame.CopySkeletonDataTo(skeletons);
                    receivedData = true;
                }
            }

            if (receivedData)
            {
                foreach (Skeleton Data in skeletons)
                {
                    if (Data.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        jcs.Add(Data.Joints);
                    }
                }
                pictureBox1.Invalidate();
            }
        }

        void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            bool receivedData = false;

            using (ColorImageFrame CFrame = e.OpenColorImageFrame())
            {
                if (CFrame == null)
                {
                    // The image processing took too long. More than 2 frames behind.
                }
                else
                {
                    pixelData = new byte[CFrame.PixelDataLength];
                    CFrame.CopyPixelDataTo(pixelData);
                    receivedData = true;
                }
            }

            if (receivedData)
            {
                IntPtr ctpr = Marshal.UnsafeAddrOfPinnedArrayElement(pixelData, 0);
                Bitmap bm = new Bitmap(640, 480, 640 * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, ctpr);

                pictureBox1.Image = bm;
            }

        }

        //Get Player Position
        Point GetDisplayPosition(SkeletonPoint jointPosition)
        {
            ColorImagePoint jPoint;

            jPoint = sensor.MapSkeletonPointToColor(jointPosition, ColorImageFormat.RgbResolution640x480Fps30);

            return new Point((int)(jPoint.X * pictureBox1.Width / 640), (int)(jPoint.Y * pictureBox1.Height / 480 ));
        }


        //Body Joint set
        JointType[] body = { JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter, JointType.Head};
        JointType[] leftArm = { JointType.ShoulderCenter, JointType.ShoulderLeft, 
                                  JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft};
        JointType[] rightArm = { JointType.ShoulderCenter, JointType.ShoulderRight, 
                                   JointType.ElbowRight, JointType.WristRight, JointType.HandRight };
        JointType[] leftLeg = { JointType.HipCenter, JointType.HipLeft, JointType.KneeLeft, 
                                  JointType.AnkleLeft, JointType.FootLeft};
        JointType[] rightLeg = { JointType.HipCenter, JointType.HipRight, JointType.KneeRight,
                                   JointType.AnkleRight, JointType.FootRight};
        //Draw All Joint and Body
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            JointType[][] lines = { body, leftArm, rightArm, leftLeg, rightLeg };

            Pen pen = new Pen(new SolidBrush(Color.FromArgb(0, 255, 0)), 5);
            Pen StartButton = new Pen(new SolidBrush(Color.FromArgb(50, 200, 50, 0)), 10);

            foreach (JointCollection jc in jcs)
            {

                //畫出光劍
                /*
                Point pHandLeft = GetDisplayPosition(jc[JointType.HandLeft].Position);
                Point pHandRight = GetDisplayPosition(jc[JointType.HandRight].Position);

                int cx = (pHandLeft.X + pHandRight.X) / 2;
                int cy = (pHandLeft.Y + pHandRight.Y) / 2;
                int dx = pHandRight.X - cx;
                int dy = pHandRight.Y - cy;
                int r = (int)Math.Sqrt(dx * dx + dy * dy);                
                r = Math.Max(1, r);

                double scale = 1.5;                
                
                int xRight = (int)(cx + 200 * scale * dx / r);
                int yRight = (int)(cy + 200 * scale * dy / r);
                int xLeft = (int)(cx - 200 * scale * dx / r);
                int yLeft = (int)(cy - 200 * scale * dy / r);

                Pen pSword = new Pen(Color.Yellow, 10);
                e.Graphics.DrawLine(pSword, xLeft, yLeft, xRight, yRight);
                
                */

                //畫出所有身體,手,腳
                /*
                for (int i = 0; i < lines.Length; i++)
                {
                    List<Point> points = new List<Point>();
                    foreach (JointType jid in lines[i])
                        points.Add(GetDisplayPosition(jc[jid].Position));
                    e.Graphics.DrawLines(pen, points.ToArray());
                }

                //畫出所有關節
                foreach (JointType jid in Enum.GetValues(typeof(JointType)))
                {
                    Point p = GetDisplayPosition(jc[jid].Position);
                    e.Graphics.FillEllipse(Brushes.Red, p.X - 8, p.Y - 8, 17, 17);
                /*
                    tx[0] = (byte)(p.X>>8);
                    tx[1] = (byte)p.X;
                    serialPort1.Write(tx, 0, 2);
                
                }*/
                Point pHandRight = GetDisplayPosition(jc[JointType.HandRight].Position);
                Point pHandLeft = GetDisplayPosition(jc[JointType.HandLeft].Position);
                Point pHipCenter = GetDisplayPosition(jc[JointType.HipCenter].Position);
                SkeletonPoint HR = jc[JointType.HandRight].Position;
                SkeletonPoint HL = jc[JointType.HandLeft].Position;
                SkeletonPoint HC = jc[JointType.HipCenter].Position;
                HR.Z = (int)(HR.Z * 100);
                HL.Z = (int)(HL.Z * 100);
                HC.Z = (int)(HC.Z * 100);
                //textBox1.Text = Convert.ToString((int)(pHandRight.X));
                //textBox2.Text = Convert.ToString((int)(pHandRight.Y));
                //textBox2.Text = Convert.ToString((int)(HR.Z));
                if (pHandRight.X <= 590 && pHandRight.X >= 490 && pHandRight.Y <= 100 && pHandRight.Y >= 0
                    && pHandLeft.X <= 150 && pHandLeft.X >= 50 && pHandLeft.Y <= 100 && pHandLeft.Y >= 0)
                {
                    StartButtonTimer.Start();
                }
                else{
                    StartButtonTimer.Stop();
                }
                if(GameStartFlag==0)               
                {
                    GameStartFlag = 0;
                    GameTimeValued = 30;
                    GameOver = 0;
                    GameTimeValue.Text = Convert.ToString(GameTimeValued);
                    StatusValue.ForeColor = Color.Black;
                    StatusValue.Text = "Waitting";
                    e.Graphics.FillEllipse(Brushes.Purple, 540 - 50, 50 - 50, 101, 101);
                    e.Graphics.FillEllipse(Brushes.Purple, 100 - 50, 50 - 50, 101, 101);
                    HipCenter_Flag = 0;
                    StartButtonFlag = 0;
                }
                if (GameStartFlag == 1)
                {
                    if (HipCenter_Flag == 0)
                    {
                        HipCenter_In[0] = (int)pHipCenter.X;
                        HipCenter_In[1] = (int)pHipCenter.Y;
                        HipCenter_In[2] = (int)HC.Z;
                        HipCenter_Flag = 1;
                    }
                    HipCenter_Comp[0] = (int)pHipCenter.X;
                    HipCenter_Comp[1] = (int)pHipCenter.Y;
                    HipCenter_Comp[2] = (int)HC.Z;
                    if ((HipCenter_In[2] - HipCenter_Comp[2]) > 10) status = 1;
                    if ((HipCenter_In[2] - HipCenter_Comp[2]) < -10) status = 2;
                    if ((HipCenter_In[0] - HipCenter_Comp[0]) < -15) status = 3;
                    if ((HipCenter_In[0] - HipCenter_Comp[0]) > 15) status = 4;
                    if (((HipCenter_In[2] - HipCenter_Comp[2]) < 10) &&(
                        (HipCenter_In[2] - HipCenter_Comp[2]) > -10) &&(
                        (HipCenter_In[0] - HipCenter_Comp[0]) > -15) &&(
                        (HipCenter_In[0] - HipCenter_Comp[0]) < 15)) status = 0;
                    tx[0] = status;
                    serialPort1.Write(tx, 0, 1);
                    if (StartButtonFlag == 0)
                    {
                        e.Graphics.FillEllipse(Brushes.Yellow, 540 - 50, 50 - 50, 101, 101);
                        e.Graphics.FillEllipse(Brushes.Yellow, 100 - 50, 50 - 50, 101, 101);
                        ClearStart.Start();
                    }
                    StatusValue.Text = "Playing";
                    GameTimer.Start();
                }
                //tx[0] = (byte)pHandRight.X;
                //tx[1] = (byte)(pHandRight.X >> 8);
                // serialPort1.Write(tx, 0, 2);
            }
        }

        //Form close
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sensor.Stop();
            serialPort1.Close();
        }

        private void StartButtonTimer_Tick(object sender, EventArgs e)
        {
            GameStartFlag = 1;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            GameTimeValued--;
            GameTimeValue.Text = Convert.ToString(GameTimeValued);
            if (GameTimeValued == 0)
            {
                GameOver = 1;
                tx[0] = 0;
                serialPort1.Write(tx, 0, 1);
                GameTimer.Stop();
                GameStartFlag = 0;
            }
        }

        private void TextChangeTimer_Tick(object sender, EventArgs e)
        {
            if (TextChangeFlag == 0 && GameStartFlag==1)
            {
                StatusValue.ForeColor = Color.Red;
            }
            if (TextChangeFlag == 1 && GameStartFlag==1)
            {
                StatusValue.ForeColor = Color.Black;
            }
            TextChangeFlag++;
            if (TextChangeFlag == 2) { TextChangeFlag = 0; }
        }

        private void ClearStart_Tick(object sender, EventArgs e)
        {
            StartButtonFlag = 1;
        }
    }
}
