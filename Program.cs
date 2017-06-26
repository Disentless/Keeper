using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Keeper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Window());
        }

        class Window : Keeper.Windows.Window
        {
            public Window()
            {
                this.Shown += new EventHandler(OnShow);
                this.FormClosing += new FormClosingEventHandler(OnClose);
            }

            private void OnShow(System.Object sender, System.EventArgs args)
            {
                Elements.Tabs.RecipeTab.RecipeTab tab = new Elements.Tabs.RecipeTab.RecipeTab();
                tab.Parent = this;
                tab.Size = new System.Drawing.Size(this.Width - 10, this.Height - 30);
                tab.Location = new System.Drawing.Point(5, 5);
                tab.Init();

                Elements.Tabs.ContactTab.ContactsTab_v1_0 cont_tab = new Elements.Tabs.ContactTab.ContactsTab_v1_0();
                cont_tab.Parent = this;
                cont_tab.Size = tab.Size;
                cont_tab.Number = tab.Number + 1;
                cont_tab.Location = tab.GetNextTab();
                cont_tab.Init();

                Elements.Tabs.PasswordTab.PassTab pass_tab;
                try
                {
                    pass_tab = new Elements.Tabs.PasswordTab.PassTab();
                    pass_tab.Parent = this;
                    pass_tab.Size = tab.Size;
                    pass_tab.Number = cont_tab.Number + 1;
                    pass_tab.Location = cont_tab.GetNextTab();
                    pass_tab.Init();
                }
                catch (System.Exception)
                {

                }

                Interfaces.IDraggable.FollowNewItem(this);

                Refresh();
            }

            private void OnClose(System.Object sender, System.Windows.Forms.FormClosingEventArgs args)
            {
                Data.DB.Database.Close();
            }
        }
    }
}
