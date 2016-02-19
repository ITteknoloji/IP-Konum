using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace IP_konum_bul
{
    public partial class Splash : SplashScreen
    {
        public Splash()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
           
        }
        int a;
        private void timer1_Tick(object sender, EventArgs e)
        {
            a++;

            if (a == 4) {
                timer1.Stop();
                this.Close();
            }
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            timer1.Start();
            this.BackColor = Color.Black;
            
        }
    }
}