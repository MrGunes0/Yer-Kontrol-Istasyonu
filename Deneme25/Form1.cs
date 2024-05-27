using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using Microsoft.Office.Interop.Excel;
using excel = Microsoft.Office.Interop.Excel;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GMap.NET;
using GMap.NET.MapProviders;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Diagnostics;
using AForge;
//RECORD
using Accord.Video.FFMPEG;
using Accord.Video.VFW;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace Deneme25
{
    public partial class Form1 : Form

    {

        private string data;
        string[] value;
        public static double lat = 37.01, longt = 36.5;
        int zoom = 5;
        int x = 0, y = 0, z = 0;
        bool cx = false, cy = false, cz = false;
        private Stopwatch stopWatch = null;
        public Form1()
        {
            InitializeComponent();
        }
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

        private void silindir(float step, float topla, float radius, float dikey1, float dikey2)
        {
            float eski_step = 0.1f;
            GL.Begin(BeginMode.Quads);//Y EKSEN CIZIM DAİRENİN
            while (step <= 360)
            {
                if (step < 45)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 90)
                    GL.Color3(Color.FromArgb(255, 255, 255));
                else if (step < 135)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 180)
                    GL.Color3(Color.FromArgb(255, 255, 255));
                else if (step < 225)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 270)
                    GL.Color3(Color.FromArgb(255, 255, 255));
                else if (step < 315)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 360)
                    GL.Color3(Color.FromArgb(255, 255, 255));


                float ciz1_x = (float)(radius * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey1, ciz1_y);

                float ciz2_x = (float)(radius * Math.Cos((step + 2) * Math.PI / 180F));
                float ciz2_y = (float)(radius * Math.Sin((step + 2) * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey1, ciz2_y);

                GL.Vertex3(ciz1_x, dikey2, ciz1_y);
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                step += topla;
            }
            GL.End();
            GL.Begin(BeginMode.Lines);
            step = eski_step;
            topla = step;
            while (step <= 180)// UST KAPAK
            {
                if (step < 45)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 90)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 135)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 180)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 225)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 270)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 315)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 360)
                    GL.Color3(Color.FromArgb(250, 250, 200));


                float ciz1_x = (float)(radius * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey1, ciz1_y);

                float ciz2_x = (float)(radius * Math.Cos((step + 180) * Math.PI / 180F));
                float ciz2_y = (float)(radius * Math.Sin((step + 180) * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey1, ciz2_y);

                GL.Vertex3(ciz1_x, dikey1, ciz1_y);
                GL.Vertex3(ciz2_x, dikey1, ciz2_y);
                step += topla;
            }
            step = eski_step;
            topla = step;
            while (step <= 180)//ALT KAPAK
            {
                if (step < 45)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 90)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 135)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 180)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 225)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 270)
                    GL.Color3(Color.FromArgb(250, 250, 200));
                else if (step < 315)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 360)
                    GL.Color3(Color.FromArgb(250, 250, 200));

                float ciz1_x = (float)(radius * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey2, ciz1_y);

                float ciz2_x = (float)(radius * Math.Cos((step + 180) * Math.PI / 180F));
                float ciz2_y = (float)(radius * Math.Sin((step + 180) * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);

                GL.Vertex3(ciz1_x, dikey2, ciz1_y);
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                step += topla;
            }
            GL.End();
        }
        private void koni(float step, float topla, float radius1, float radius2, float dikey1, float dikey2)
        {
            float eski_step = 0.1f;
            GL.Begin(BeginMode.Lines);//Y EKSEN CIZIM DAİRENİN
            while (step <= 360)
            {
                if (step < 45)
                    GL.Color3(1.0, 1.0, 1.0);
                else if (step < 90)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 135)
                    GL.Color3(1.0, 1.0, 1.0);
                else if (step < 180)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 225)
                    GL.Color3(1.0, 1.0, 1.0);
                else if (step < 270)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 315)
                    GL.Color3(1.0, 1.0, 1.0);
                else if (step < 360)
                    GL.Color3(Color.FromArgb(0, 0, 255));


                float ciz1_x = (float)(radius1 * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius1 * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey1, ciz1_y);

                float ciz2_x = (float)(radius2 * Math.Cos(step * Math.PI / 180F));
                float ciz2_y = (float)(radius2 * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                step += topla;
            }
            GL.End();

            GL.Begin(BeginMode.Lines);
            step = eski_step;
            topla = step;
            while (step <= 180)// UST KAPAK ALT KAPAK
            {
                if (step < 45)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 90)
                    GL.Color3(Color.FromArgb(250, 255, 255));
                else if (step < 135)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 180)
                    GL.Color3(Color.FromArgb(250, 255, 255));
                else if (step < 225)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 270)
                    GL.Color3(Color.FromArgb(250, 255, 255));
                else if (step < 315)
                    GL.Color3(Color.FromArgb(0, 0, 255));
                else if (step < 360)
                    GL.Color3(Color.FromArgb(250, 255, 255));


                float ciz1_x = (float)(radius2 * Math.Cos(step * Math.PI / 180F));
                float ciz1_y = (float)(radius2 * Math.Sin(step * Math.PI / 180F));
                GL.Vertex3(ciz1_x, dikey2, ciz1_y);

                float ciz2_x = (float)(radius2 * Math.Cos((step + 180) * Math.PI / 180F));
                float ciz2_y = (float)(radius2 * Math.Sin((step + 180) * Math.PI / 180F));
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);

                GL.Vertex3(ciz1_x, dikey2, ciz1_y);
                GL.Vertex3(ciz2_x, dikey2, ciz2_y);
                step += topla;
            }
            step = eski_step;
            topla = step;
            GL.End();
        }
        private void Pervane(float yukseklik, float uzunluk, float kalinlik, float egiklik)
        {
            float radius = 10, angle = 45.0f;
            GL.Begin(BeginMode.Quads);

            GL.Color3(Color.DarkRed);
            GL.Vertex3(uzunluk, yukseklik, kalinlik);
            GL.Vertex3(uzunluk, yukseklik + egiklik, -kalinlik);
            GL.Vertex3(0.0, yukseklik + egiklik, -kalinlik);
            GL.Vertex3(0.0, yukseklik, kalinlik);

            GL.Color3(Color.DarkRed);
            GL.Vertex3(-uzunluk, yukseklik + egiklik, kalinlik);
            GL.Vertex3(-uzunluk, yukseklik, -kalinlik);
            GL.Vertex3(0.0, yukseklik, -kalinlik);
            GL.Vertex3(0.0, yukseklik + egiklik, kalinlik);

            GL.Color3(Color.White);
            GL.Vertex3(kalinlik, yukseklik, -uzunluk);
            GL.Vertex3(-kalinlik, yukseklik + egiklik, -uzunluk);
            GL.Vertex3(-kalinlik, yukseklik + egiklik, 0.0);//+
            GL.Vertex3(kalinlik, yukseklik, 0.0);//-

            GL.Color3(Color.White);
            GL.Vertex3(kalinlik, yukseklik + egiklik, +uzunluk);
            GL.Vertex3(-kalinlik, yukseklik, +uzunluk);
            GL.Vertex3(-kalinlik, yukseklik, 0.0);
            GL.Vertex3(kalinlik, yukseklik + egiklik, 0.0);
            GL.End();
        }

            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void OpenVideoSource(IVideoSource source)
        {
            //* set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            CloseCurrentVideoSource();

            // start new video source
            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start();

            // reset stop watch


            // start timer
            Zamanlayici.Start();

            this.Cursor = Cursors.Default;
        }

        // Close video source if it is running
        private void CloseCurrentVideoSource()
        {
            if (videoSourcePlayer.VideoSource != null)
            {
                videoSourcePlayer.SignalToStop();

                // wait ~ 3 seconds
                for (int i = 0; i < 30; i++)
                {
                    if (!videoSourcePlayer.IsRunning)
                        break;
                    System.Threading.Thread.Sleep(100);
                }

                if (videoSourcePlayer.IsRunning)
                {
                    videoSourcePlayer.Stop();
                }

                videoSourcePlayer.VideoSource = null;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            captureDevice = new VideoCaptureDeviceForm();
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            string[] portlar = SerialPort.GetPortNames();
            foreach (string porAdi in portlar)
            {
                comboBox1.Items.Add(porAdi);

            }
            OkumaNesnesi.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived);
            GL.ClearColor(Color.FromArgb(181, 181, 181));//Color.FromArgb(143, 212, 150)
            
            //this.dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 11);
            for (int i = 0; i <= 1; i++)
                //this.dataGridView1.Columns[i].HeaderCell.Style.Font = new Font("Tahoma", 11);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.White;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            chart.ChartAreas[0].AxisY2.Minimum = 850;       //İkinci Y ekseni minimum değeri
            chart.ChartAreas[0].AxisY2.Maximum = 1250;      //İkinci Y ekseni maksimum değeri
            chart.ChartAreas[0].AxisY2.Interval = 40;       //İkinci Y ekseni aralığı
            chart.ChartAreas[0].AxisY.Minimum = 0;          //Y ekseni minimum değeri
            chart.ChartAreas[0].AxisY.Maximum = 200;        //Y ekseni maksimum değeri
            chart.ChartAreas[0].AxisY.Interval = 10;        //Y ekseni aralığı
            chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;                  //X ekseni için grid kaldır
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "d/M/yyyy HH:mm:ss";  //X eksenindeki metin formatı
            
            chart1.ChartAreas[0].AxisY2.Minimum = 850;       //İkinci Y ekseni minimum değeri
            chart1.ChartAreas[0].AxisY2.Maximum = 1250;      //İkinci Y ekseni maksimum değeri
            chart1.ChartAreas[0].AxisY2.Interval = 40;       //İkinci Y ekseni aralığı
            chart1.ChartAreas[0].AxisY.Minimum = 0;          //Y ekseni minimum değeri
            chart1.ChartAreas[0].AxisY.Maximum = 850;        //Y ekseni maksimum değeri
            chart1.ChartAreas[0].AxisY.Interval = 100;        //Y ekseni aralığı
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;                  //X ekseni için grid kaldır
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "d/M/yyyy HH:mm:ss";  //X eksenindeki metin formatı
            
            chart2.ChartAreas[0].AxisY2.Minimum = 850;       //İkinci Y ekseni minimum değeri
            chart2.ChartAreas[0].AxisY2.Maximum = 1250;      //İkinci Y ekseni maksimum değeri
            chart2.ChartAreas[0].AxisY2.Interval = 40;       //İkinci Y ekseni aralığı
            chart2.ChartAreas[0].AxisY.Minimum = 0;          //Y ekseni minimum değeri
            chart2.ChartAreas[0].AxisY.Maximum = 1800;        //Y ekseni maksimum değeri
            chart2.ChartAreas[0].AxisY.Interval = 100;        //Y ekseni aralığı
            chart2.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;                  //X ekseni için grid kaldır
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "d/M/yyyy HH:mm:ss";  //X eksenindeki metin formatı
            
            chart3.ChartAreas[0].AxisY2.Minimum = 850;       //İkinci Y ekseni minimum değeri
            chart3.ChartAreas[0].AxisY2.Maximum = 1250;      //İkinci Y ekseni maksimum değeri
            chart3.ChartAreas[0].AxisY2.Interval = 40;       //İkinci Y ekseni aralığı
            chart3.ChartAreas[0].AxisY.Minimum = 0;          //Y ekseni minimum değeri
            chart3.ChartAreas[0].AxisY.Maximum = 200;        //Y ekseni maksimum değeri
            chart3.ChartAreas[0].AxisY.Interval = 20;        //Y ekseni aralığı
            chart3.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;                  //X ekseni için grid kaldır
            chart3.ChartAreas[0].AxisX.LabelStyle.Format = "d/M/yyyy HH:mm:ss";  //X eksenindeki metin formatı
           
            chart4.ChartAreas[0].AxisY2.Minimum = 850;       //İkinci Y ekseni minimum değeri
            chart4.ChartAreas[0].AxisY2.Maximum = 1250;      //İkinci Y ekseni maksimum değeri
            chart4.ChartAreas[0].AxisY2.Interval = 40;       //İkinci Y ekseni aralığı
            chart4.ChartAreas[0].AxisY.Minimum = 0;          //Y ekseni minimum değeri
            chart4.ChartAreas[0].AxisY.Maximum = 200;        //Y ekseni maksimum değeri
            chart4.ChartAreas[0].AxisY.Interval = 20;        //Y ekseni aralığı
            chart4.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;                  //X ekseni için grid kaldır
            chart4.ChartAreas[0].AxisX.LabelStyle.Format = "d/M/yyyy HH:mm:ss";  //X eksenindeki metin formatı

            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
        }

        private void Zamanlayici_Tick(object sender, EventArgs e)
        {
            try
            {
                string[] paket;
                string sonuc = OkumaNesnesi.ReadLine();
                paket = sonuc.Split('/');
                label3.Text = paket[10];
                label4.Text = paket[11];
                label5.Text = paket[12];
                x = Convert.ToInt32(paket[10]);
                y = Convert.ToInt32(paket[11]);
                z = Convert.ToInt32(paket[12]);
                glControl1.Invalidate();
                OkumaNesnesi.DiscardInBuffer();
                
                                        
                DateTime myDateValue = DateTime.Now;    //Güncel zaman bilgisini al
                label2.Text = myDateValue.ToString();  //Güncel zaman bilgisini label8'e yaz
                
                paket = sonuc.Split('/');    //'/' gördüğün yerlerden stringi ayır ve diziye ata
                textBox1.Text = paket[13];
                textBox2.Text = paket[14];
                textBox3.Text = paket[15];
                textBox4.Text = paket[4];
                textBox5.Text = paket[5];
                textBox9.Text = paket[6];
                textBox10.Text = paket[7];
                textBox11.Text = paket[8];
                textBox12.Text = paket[9];
                textBox13.Text = paket[16];
                textBox14.Text = paket[17];
                textBox15.Text = paket[0];
                textBox8.Text = paket[3];
                
                



                //double humidity = Convert.ToDouble(value[0]);    //String değişkenlerini double'a dönüştür
                //double temp = Convert.ToDouble(value[1]);

                int satir = dataGridView1.Rows.Add();
                dataGridView1.Rows[satir].Cells[0].Value = "410772  ";
                dataGridView1.Rows[satir].Cells[1].Value = paket[0];
                dataGridView1.Rows[satir].Cells[2].Value = paket[1];     // Cell sütun oluyor 0 ise 0. sütun rows ise satır 
                dataGridView1.Rows[satir].Cells[3].Value = paket[2];
                dataGridView1.Rows[satir].Cells[4].Value = paket[3];
                dataGridView1.Rows[satir].Cells[5].Value = paket[4];     // Cell sütun oluyor 0 ise 0. sütun rows ise satır 
                dataGridView1.Rows[satir].Cells[6].Value = paket[5];
                dataGridView1.Rows[satir].Cells[7].Value = paket[6];
                dataGridView1.Rows[satir].Cells[8].Value = paket[7];
                dataGridView1.Rows[satir].Cells[9].Value = paket[8];
                dataGridView1.Rows[satir].Cells[10].Value = paket[9];
                dataGridView1.Rows[satir].Cells[11].Value = paket[10];
                dataGridView1.Rows[satir].Cells[12].Value = paket[11];
                dataGridView1.Rows[satir].Cells[13].Value = paket[12];
                dataGridView1.Rows[satir].Cells[14].Value = paket[13];
                dataGridView1.Rows[satir].Cells[15].Value = paket[14];
                dataGridView1.Rows[satir].Cells[16].Value = paket[15];
                dataGridView1.Rows[satir].Cells[17].Value = paket[16];
               // dataGridView1.Rows[satir].Cells[18].Value = paket[17];
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Index;

                satir++;
                OkumaNesnesi.DiscardInBuffer();
                this.chart.Series[0].Points.AddXY(myDateValue.ToString("d/M/yyyy HH:mm:ss"), paket[13]);    //Zaman ve sıcaklık değerlerini eksenlere ata
                //this.chart.Series[1].Points.AddXY(myDateValue.ToString("d/M/yyyy HH:mm:ss"), paket[13]);    //Zaman ve sıcaklık değerlerini eksenlere ata
                this.chart1.Series[0].Points.AddXY(myDateValue.ToString("d/M/yyyy HH:mm:ss"), paket[14]);    //Zaman ve sıcaklık değerlerini eksenlere ata
                this.chart2.Series[0].Points.AddXY(myDateValue.ToString("d/M/yyyy HH:mm:ss"), paket[15]);
                this.chart3.Series[0].Points.AddXY(myDateValue.ToString("d/M/yyyy HH:mm:ss"), paket[16]);    //Zaman ve sıcaklık değerlerini eksenlere ata
                this.chart4.Series[0].Points.AddXY(myDateValue.ToString("d/M/yyyy HH:mm:ss"), paket[17]);

                //Zaman ve sıcaklık değerlerini eksenlere ata

                paket = sonuc.Split('/');
                textBox7.Text = paket[2];
                textBox6.Text = paket[1];

                textBox6.Text = textBox6.Text.ToString().Replace('.', ',');
                textBox7.Text = textBox7.Text.ToString().Replace('.', ',');
                string lats = textBox6.Text.ToString().Replace('.', ',');
                string lots = textBox7.Text.ToString().Replace('.', ',');
                lat = Convert.ToDouble(lats);
                longt = Convert.ToDouble(lots);

                IVideoSource videoSource = videoSourcePlayer.VideoSource;

                if (videoSource != null)
                {
                    // get number of frames since the last timer tick
                    int framesReceived = videoSource.FramesReceived;

                    if (stopWatch == null)
                    {
                        stopWatch = new Stopwatch();
                        stopWatch.Start();
                    }
                    else
                    {
                        stopWatch.Stop();

                        float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;


                        stopWatch.Reset();
                        stopWatch.Start();
                    }
                }
            }

            catch { }
        }

        private FilterInfoCollection VideoCaptureDevices;

        private VideoCaptureDevice FinalVideo = null;
        private VideoCaptureDeviceForm captureDevice;

        private Bitmap video;
        //private AVIWriter AVIwriter = new AVIWriter();
        public VideoFileWriter FileWriter = new VideoFileWriter();
        private SaveFileDialog saveAvi;

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Enable(EnableCap.DepthTest);//sonradan yazdık
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            float step = 1.0f;
            float topla = step;
            float radius = 5.0f;
            float dikey1 = radius, dikey2 = -radius;
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(1.04f, 4 / 3, 1, 10000);
            Matrix4 lookat = Matrix4.LookAt(25, 0, 0, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LoadMatrix(ref lookat);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Rotate(x, 1.0, 0.0, 0.0);//ÖNEMLİ
            GL.Rotate(z, 0.0, 1.0, 0.0);
            GL.Rotate(y, 0.0, 0.0, 1.0);

            silindir(step, topla, radius, 3, -5);
            //silindir(0.01f, topla, 0.5f, 9, 9.7f);
            //silindir(0.01f, topla, 0.1f, 5, dikey1 + 5);
            koni(0.01f, 0.01f, radius, 3.0f, 3, 5);
            koni(0.01f, 0.01f, radius, 2.0f, -5.0f, -10.0f);
            Pervane(9.0f, 11.0f, 0.2f, 0.5f);

            GL.Begin(BeginMode.Lines);

            GL.Color3(Color.FromArgb(250, 0, 0));
            GL.Vertex3(-30.0, 0.0, 0.0);
            GL.Vertex3(30.0, 0.0, 0.0);


            GL.Color3(Color.FromArgb(0, 0, 0));
            GL.Vertex3(0.0, 30.0, 0.0);
            GL.Vertex3(0.0, -30.0, 0.0);


            GL.Color3(Color.FromArgb(0, 0, 250));
            GL.Vertex3(0.0, 0.0, 30.0);
            GL.Vertex3(0.0, 0.0, -30.0);

            GL.End();
            //GraphicsContext.CurrentContext.VSync = true;
            glControl1.SwapBuffers();
        }

        private void TimerXYZ_Tick(object sender, EventArgs e)
        {
            if (cx == true)
            {
                if (x < 360)
                    x += 5;
                else
                    x = 0;
                label3.Text = x.ToString();
            }
            if (cy == true)
            {
                if (y < 360)
                    y += 5;
                else
                    y = 0;
                label4.Text = y.ToString();
            }
            if (cz == true)
            {
                if (z < 360)
                    z += 5;
                else
                    z = 0;
                label5.Text = z.ToString();
            }
            glControl1.Invalidate();
           
            
            
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (cx == false)
                cx = true;
            else
                cx = false;
            TimerXYZ.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cy == false)
                cy = true;
            else
                cy = false;
            TimerXYZ.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cz == false)
                cz = true;
            else
                cz = false;
            TimerXYZ.Start();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            StringBuilder adress = new StringBuilder();
            adress.Append("http://google.com/maps?q=");
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(39.97771200532895, 41.27375838332586);//lat longt
            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 120;
            gMapControl1.Zoom = 10;
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (zoom > 0)
                zoom -= 1;
            gMapControl1.MinZoom = zoom;
            gMapControl1.Zoom = zoom;
        }

        private void Yakınlaştır_Click(object sender, EventArgs e)
        {
            if (zoom < 100)
                zoom += 1;
            gMapControl1.MinZoom = zoom;
            gMapControl1.Zoom = zoom;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void manuelAyrilma_Click(object sender, EventArgs e)
        {
            if (!OkumaNesnesi.IsOpen) OkumaNesnesi.Open();
            OkumaNesnesi.Write("!xT!");
            
        }

        private void KameraAc_Click(object sender, EventArgs e)
        {
            captureDevice = new VideoCaptureDeviceForm();

            if (captureDevice.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                FinalVideo = captureDevice.VideoDevice;

                // open it
                OpenVideoSource(FinalVideo);
                FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                FinalVideo.Start();
            }
        }

        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (KameraKapa.Text == "Kaydı Durdur")
            {
                video = (Bitmap)eventArgs.Frame.Clone();
                //pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
                //AVIwriter.Quality = 0;
                try
                {
                    FileWriter.WriteVideoFrame(video);
                }
                catch (Exception)
                {


                }

                //AVIwriter.AddFrame(video);
            }
            else //Stop
            {
                video = (Bitmap)eventArgs.Frame.Clone();
                //pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            }
        }

        private void Kayıt_Click(object sender, EventArgs e)
        {
            if (KameraKapa.Text == "Kamera Kapa")
            {
                saveAvi = new SaveFileDialog();
                saveAvi.Filter = "Avi Files (*.avi)|*.avi";
                if (saveAvi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int h = captureDevice.VideoDevice.VideoResolution.FrameSize.Height;
                    int w = captureDevice.VideoDevice.VideoResolution.FrameSize.Width;
                    FileWriter.Open(saveAvi.FileName, w, h, 25, VideoCodec.Default, 5000000);
                    FileWriter.WriteVideoFrame(video);

                    KameraKapa.Text = "Kaydı Durdur";
                }
            }
        }

        private void KameraKapa_Click(object sender, EventArgs e)
        {
            if (KameraKapa.Text == "Kaydı Durdur")
            {
                KameraAc.Text = "Kamera Kapa";
                if (FinalVideo == null)
                { return; }
                if (FinalVideo.IsRunning)
                {
                    //this.FinalVideo.Stop();
                    FileWriter.Close();
                    //this.AVIwriter.Close();
                    //pictureBox1.Image = null;
                }
            }
            else
            {
                this.FinalVideo.Stop();
                FileWriter.Close();
                //this.AVIwriter.Close();
                //pictureBox1.Image = null;
            }
        }

        private void chart_Click(object sender, EventArgs e)
        {

        }

        private void videoSourcePlayer_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            serialPort1.Open();
            string veriler = serialPort1.ReadExisting();
            MessageBox.Show("veriler gösteriliyor " + veriler);
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseCurrentVideoSource();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OkumaNesnesi.BaudRate = Convert.ToInt32(texBaundRate.Text);

                OkumaNesnesi.PortName = comboBox1.Text;
                if (!OkumaNesnesi.IsOpen) 
                {
                    Zamanlayici.Start();
                    OkumaNesnesi.Open();
                    baglan.Enabled = false;
                    kes.Enabled = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("BAĞLANTI KURULAMADI");
                kes.Enabled = true;
            }

            
        }

        private void kes_Click(object sender, EventArgs e)
        {
            try
            {
                OkumaNesnesi.Close();              //Seri portu kapa
                kes.Enabled = false;              //"Kes" butonunu tıklanamaz yap
                baglan.Enabled = true;            //"Bağlan" butonunu tıklanabilir yap
                
                label1.Text = "Bağlantı kesildi";
                label1.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message); //Hata mesajı
            }
        }

        private void ExlAktar_Click(object sender, EventArgs e)
        {
            excel.Application app = new excel.Application();
            app.Visible = true;
            Workbook kitap = app.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sayfa = (Worksheet)kitap.Sheets[1];
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                Range alan = (Range)sayfa.Cells[1, 1];
                alan.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    Range alan2 = (Range)sayfa.Cells[j + 1, i + 1];
                    alan2.Cells[2, 1] = dataGridView1[i, j].Value;

                }

            }
        }

        private void displayData_event(object sender, EventArgs e)
        {
            
        }

    }
}
