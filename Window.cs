using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keeper
{
    namespace Windows
    {
        public class Window : Window_v1_0 { }

        public class Window_v1_0 : System.Windows.Forms.Form
        {
            private System.Windows.Forms.Label close, max_min;

            public Window_v1_0()
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Size = new System.Drawing.Size(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 2, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 2);

                this.BackColor = System.Drawing.Color.Black;
                this.ForeColor = System.Drawing.Color.White;

                close = new System.Windows.Forms.Label();
                close.Parent = this;
                close.AutoSize = true;
                close.Text = "Close";
                close.Click += new EventHandler(OnCloseClick);

                max_min = new System.Windows.Forms.Label();
                max_min.Parent = this;
                max_min.AutoSize = true;
                max_min.Text = "Maximize";
                max_min.Click += new EventHandler(OnMaxMinClick);

            }

            private void OnCloseClick(System.Object sender, System.EventArgs args)
            {
                this.Close();
            }
            private void OnMaxMinClick(System.Object sender, System.EventArgs args)
            {
                if (this.WindowState == System.Windows.Forms.FormWindowState.Maximized)
                    this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                else
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                Refresh();
            }

            public override void Refresh()
            {
                base.Refresh();

                close.Left = this.Width - close.Width - 5;
                max_min.Left = this.Width - max_min.Width - 5;
                close.Top = 5;
                max_min.Top = close.Bottom + 5;
            }

        }
    }
}
