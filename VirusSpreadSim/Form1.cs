using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms;//the WinForm wrappers
using LiveCharts.Defaults;
using LiveCharts.Helpers;

namespace VirusSpreadSim
{
    
    public partial class Form1 : Form
    {
        bool firstinfection = true;
        IList<double> Ilst = new List<double>();
        IList<double> Rlst = new List<double>();
        IList<double> Slst = new List<double>();
        bool helpme=false;
        //mathematical model 
        double S;
        double I;
        double R;
        //
        double Snew;
        double Inew;
        double Rnew;
        //
        bool key1 = false;
        bool key2 = false;
        bool firstime = true;
        int speed = 1;
        double population;
        double beta;
        double daycount=0;
        double lifespan;
        public Form1()
        {
            InitializeComponent();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'myDataSet.someTable' table. You can move, or remove it, as needed.
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void daylbl_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            nodayslbl.Text = trackBar1.Value.ToString();
            if(nodayslbl.Text.Equals("1"))
            {
                label21.Text = "day";
            }
            else
            {
                label21.Text = "days";
            }
        }

        private void nodayslbl_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(firstime)
            {

                if (!string.IsNullOrWhiteSpace(popbox.Text)&&!string.IsNullOrWhiteSpace(infectionratebox.Text))
                {
                    lifespan = double.Parse(nodayslbl.Text);
                    if (Double.TryParse(popbox.Text, out population))
                    {
                        if(population<1000||population>1000000000)
                        {
                            MessageBox.Show(" Error:  Population must be between (1000-1000000000) people");

                        }
                        else
                        {

                            key1 = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong Input population");
                    }
                    if (Double.TryParse(infectionratebox.Text,out beta))
                    {
                        if(beta<0||beta>1)
                        {
                            MessageBox.Show(" Error :Please Select an Infection Rate Value Between (0-1)");
                        }
                        else
                        {
                            key2 = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(" Wrong Input affection rate");
                    }
                    //
                    if(key1&&key2)
                    {
                        timer1.Start();
                        button1.Enabled = false;
                        button2.Enabled = true;
                        firstime = !firstime;
                    }
                   
                }
                else
                {
                    MessageBox.Show("Please enter correct data");
                }
              
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = true;
                timer1.Start();
            }
        

            
            
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            speedlbl.Text = speedBar.Value.ToString();
            if (speedlbl.Text.Equals("1"))
            {
                timer1.Interval = 1000;
            }
            else if (speedlbl.Text.Equals("2"))
            {
                speed = 2;
                timer1.Interval = 500;
            }
            else if (speedlbl.Text.Equals("3"))
            {
                speed = 3;
                timer1.Interval = 250;
            }

//maths implementation---------------------------------------------------------------------------------------------
//------------------------------------------------------
//------------------------------------------------------


            int newInf;
            double hS;
            double hI;
            double hR;
            double gamma;
            ///mathmodel
            ///
            if(firstinfection)
            {
               // Random rn = new Random();
                newInf = 2;
                I = newInf + I;
                S = population - I;
                firstinfection = false;
                ilbl.Text = Math.Round(I).ToString();
                slbl.Text = Math.Round(S).ToString();
            }
            //S--------
            hS = -(beta * S * I)/population;
            Snew = S +hS;
            S = Snew;
            //R--------
            gamma = (1.0 / lifespan);
            hR = gamma * I;
            Rnew = R + hR;
            R = Rnew;
            //I--------
            hI= -hS - hR;
            Inew = I + hI;
            I = Inew;
            //Inew = I + hI;
            ilbl.Text = Math.Round(I).ToString();
            rlbl.Text = Math.Round(R).ToString();
            slbl.Text = Math.Round(S).ToString();

            daycount++;
            daylbl.Text = daycount.ToString();

            if (daycount % 7 == 1)
            {
                Ilst.Add(I);

            }

            if (daycount % 7 == 1)
            {
                Rlst.Add(R);

            }
            if (daycount % 7 == 1)
            {
                Slst.Add(S);

            }

            if (daycount%7==0)
            {
                cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Infected",
                    Values = Ilst.AsChartValues(),

                },
                 new LineSeries
                {
                    Title = "Recovered",
                    Values = Rlst.AsChartValues(),
 
                },
                  new LineSeries
                {
                    Title = "Susceptible",
                    Values = Slst.AsChartValues(),
                   
                },
            };
          
            

        }
            if (I < 0.001 || S < 0.001)
            {
                timer1.Stop();
                button1.Hide();
                button2.Hide();
                label5.Visible = true;
            }

        }

        private void speedBar_Scroll(object sender, EventArgs e)
        {
            speedlbl.Text = speedBar.Value.ToString();
            if(speedlbl.Text.Equals("1"))
            {
                speed = 1;
            }
            else if(speedlbl.Text.Equals("2"))
            {
                speed = 2;
            }
            else if(speedlbl.Text.Equals("3"))
            {
                speed = 3;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            timer1.Stop();
        }

       

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newSimulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 NewForm = new Form1();
            NewForm.Show();
            this.Dispose(false);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutVirusSpreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutVirusSpread jk = new AboutVirusSpread();
            jk.Show();
        }
    }
}
