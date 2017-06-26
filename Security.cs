namespace Keeper
{
    namespace Security
    {
        abstract class Admin
        {
            static public string Pass = "admin";
        }

        class AskDialog : System.Windows.Forms.Form
        {
            static public string Ask(string caption)
            {
                AskDialog dialog = new AskDialog();
                dialog.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                dialog.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                dialog.BackColor = System.Drawing.Color.Black;
                dialog.ForeColor = System.Drawing.Color.White;
                dialog.MinimumSize = new System.Drawing.Size(0, 0);

                System.Windows.Forms.Label text = new System.Windows.Forms.Label();
                text.Parent = dialog;
                text.Dock = System.Windows.Forms.DockStyle.Top;
                text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                text.Text = caption;
                text.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif.ToString(), 10, System.Drawing.FontStyle.Bold);
                text.Height = 60;
                text.Refresh();

                System.Windows.Forms.TextBox box = new System.Windows.Forms.TextBox();
                box.Parent = dialog;
                box.Dock = System.Windows.Forms.DockStyle.Bottom;
                box.Top = text.Bottom;
                box.Font = text.Font;
                box.PasswordChar = '•';
                box.UseSystemPasswordChar = false;
                box.Multiline = false;
                box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                box.BackColor = dialog.BackColor;
                box.ForeColor = dialog.ForeColor;
                box.BorderStyle = System.Windows.Forms.BorderStyle.None;

                dialog.Size = new System.Drawing.Size(box.Right, text.Bottom + box.Height);
                dialog.KeyPreview = true;
                dialog.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);
                dialog.ShowDialog();

                return box.Text;
            }

            static private void OnKeyDown(System.Object sender, System.Windows.Forms.KeyEventArgs args)
            {
                args.Handled = true;
                if (args.KeyCode == System.Windows.Forms.Keys.Escape || args.KeyCode == System.Windows.Forms.Keys.Enter)
                {
                    ((System.Windows.Forms.Form)sender).Close();
                }
            }
        }
    }
}
