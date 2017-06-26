using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keeper
{
    namespace Elements
    {
        namespace Controls
        {
            class List : System.Windows.Forms.Panel
            {
                private System.Windows.Forms.TextBox[] Lines;
                private System.Windows.Forms.PictureBox[] DeleteButton;
                private System.Windows.Forms.Label AddButton = new System.Windows.Forms.Label();
                private System.Windows.Forms.Label Title = new System.Windows.Forms.Label();
                private int amount = 0;

                private void Init()
                {
                    Lines = new System.Windows.Forms.TextBox[0];
                    DeleteButton = new System.Windows.Forms.PictureBox[0];

                    AddButton.Parent = this;
                    AddButton.Dock = System.Windows.Forms.DockStyle.Bottom;
                    AddButton.Height = 20;
                    AddButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    AddButton.Text = "---Add---";
                    AddButton.Click += new EventHandler(OnAddClick);

                    Title.Parent = this;
                    Title.Dock = System.Windows.Forms.DockStyle.Top;
                    Title.Height = 20;
                    Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                    this.MinimumSize = new System.Drawing.Size(50, 20);

                    Refresh();
                }

                public void SetTitle(string title)
                {
                    Title.Text = title;
                }

                public void FromStringArray(string[] array)
                {
                    this.Controls.Clear();
                    Title.Parent = this;
                    amount = 0;
                    AddButton.Parent = this;
                    Lines = new System.Windows.Forms.TextBox[0];
                    DeleteButton = new System.Windows.Forms.PictureBox[0];
                    for (int i = 0; i < array.Length; ++i)
                    {
                        AddLine();
                        Lines[i].Text = array[i];
                    }

                    Refresh();
                }

                public override void Refresh()
                {
                    base.Refresh();

                    this.Height = 5 + (amount != 0 ? amount * 20 : -5) + AddButton.Height + Title.Height;
                    for (int i = 0; i < Lines.Length; ++i)
                    {
                        if (Lines[i] == null) return;
                        Lines[i].Width = this.Width - 10 - DeleteButton[0].Width;
                        DeleteButton[i].Left = Lines[i].Right;
                    }
                }

                private void OnAddClick(System.Object sender, System.EventArgs args)
                {
                    AddLine();
                }

                public string[] ToStringArray()
                {
                    string[] lines = new string[Lines.Length];
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        if (Lines[i] == null || Lines[i].Text == "") continue;
                        lines[i] = Lines[i].Text;
                    }
                    return lines;
                }

                private void AddLine()
                {
                    int i = Lines.Length;
                    Array.Resize(ref Lines, i + 1);
                    Lines[i] = new System.Windows.Forms.TextBox();
                    Lines[i].Parent = this;
                    Lines[i].Size = new System.Drawing.Size(this.Width - 10 - 20, 20);
                    Lines[i].Location = new System.Drawing.Point(5, 5 + amount * Lines[i].Height + Title.Height);

                    i = DeleteButton.Length;
                    Array.Resize(ref DeleteButton, i + 1);
                    DeleteButton[i] = new System.Windows.Forms.PictureBox();
                    DeleteButton[i].Parent = this;
                    DeleteButton[i].Size = new System.Drawing.Size(20, 20);
                    DeleteButton[i].Location = new System.Drawing.Point(Lines[i].Right, Lines[i].Top);
                    DeleteButton[i].Tag = i;
                    DeleteButton[i].BackColor = System.Drawing.Color.DarkRed;
                    DeleteButton[i].Click += new EventHandler(OnDeleteClick);

                    amount++;
                    Refresh();
                }

                private void OnDeleteClick(System.Object sender, System.EventArgs args)
                {
                    DeleteLine(UInt32.Parse(((System.Windows.Forms.PictureBox)sender).Tag.ToString()));
                }

                private void DeleteLine(uint number)
                {
                    if (amount == 1) return;
                    this.Controls.Clear();
                    Title.Parent = this;
                    int offset = 0;
                    for (int i = 0; i < Lines.Length; ++i)
                    {
                        if (i == Lines.Length - 1 && number == (uint)i) break;
                        if (i == number)
                        {
                            offset = 1;
                            //continue;
                        }
                        if (i != Lines.Length - 1) Lines[i] = Lines[i + offset];
                        Lines[i].Parent = this;
                        Lines[i].Location = new System.Drawing.Point(5, 5 + (i - offset) * 20 + Title.Height);
                        DeleteButton[i].Parent = this;
                        DeleteButton[i].Location = new System.Drawing.Point(Lines[i].Right, Lines[i].Top);
                        DeleteButton[i].Tag = i - offset;
                    }
                    Array.Resize(ref Lines, Lines.Length - 1);
                    Array.Resize(ref DeleteButton, DeleteButton.Length - 1);
                    AddButton.Parent = this;

                    amount--;
                    Refresh();
                }

                public List()
                {
                    Init();
                }
            }
        }

        namespace Tabs
        {
            using Tab = Tab_v1_0;

            namespace Tools
            {
                namespace Contact
                {
                    class ContactAddPanel_v1_0 : System.Windows.Forms.Panel
                    {
                        public ContactAddPanel_v1_0()
                        {

                        }
                    }
                }

                namespace Password
                {
                    class PasswordAddWindow_v1_0 : System.Windows.Forms.Form
                    {
                        System.Windows.Forms.TextBox descr, login, pass;
                        public PasswordAddWindow_v1_0()
                        {
                            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

                            System.Windows.Forms.Label buf = new System.Windows.Forms.Label();
                            buf.Parent = this;
                            buf.Location = new System.Drawing.Point(5, 5);
                            buf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                            buf.Text = "Description: ";
                            buf.AutoSize = true;

                            descr = new System.Windows.Forms.TextBox();
                            descr.Parent = this;
                            descr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                            descr.Location = new System.Drawing.Point(buf.Right + 5, buf.Top);
                            descr.Size = new System.Drawing.Size(200, buf.Height);

                            buf = new System.Windows.Forms.Label();
                            buf.Parent = this;
                            buf.Location = new System.Drawing.Point(5, descr.Bottom + 5);
                            buf.AutoSize = true;
                            buf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                            buf.Text = "Login: ";

                            login = new System.Windows.Forms.TextBox();
                            login.Parent = this;
                            login.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                            login.Location = new System.Drawing.Point(descr.Left, buf.Top);
                            login.Size = descr.Size;

                            buf = new System.Windows.Forms.Label();
                            buf.Parent = this;
                            buf.Location = new System.Drawing.Point(5, login.Bottom + 5);
                            buf.AutoSize = true;
                            buf.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                            buf.Text = "Password: ";

                            pass = new System.Windows.Forms.TextBox();
                            pass.Parent = this;
                            pass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                            pass.Location = new System.Drawing.Point(descr.Left, buf.Top);
                            pass.Size = descr.Size;

                            this.AutoSize = true;
                            this.Refresh();
                        }

                        public Data.Types.Password.Info GetInfo()
                        {
                            Data.Types.Password.Info info = new Data.Types.Password.Info();
                            info.description = descr.Text;
                            info.login = login.Text;
                            info.password = pass.Text;

                            return info;
                        }
                    }
                }

                namespace Recipe
                {
                    class RecipeAddingDialog : Keeper.Windows.Window
                    {
                        private System.Windows.Forms.TextBox title = new System.Windows.Forms.TextBox();
                        private System.Windows.Forms.TextBox[] ingrs = new System.Windows.Forms.TextBox[0];
                        private System.Windows.Forms.Label add_ingr = new System.Windows.Forms.Label();

                        private Controls.List StepsList = new Controls.List();
                        private Controls.List IngrList = new Controls.List();

                        private void Init()
                        {
                            title.Parent = this;
                            title.Location = new System.Drawing.Point(5, 5);

                            StepsList.Parent = this;
                            StepsList.Location = new System.Drawing.Point(title.Left, title.Bottom + 5);
                            StepsList.Width = title.Width;
                            StepsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                            StepsList.SetTitle("Steps");

                            IngrList.Parent = this;
                            IngrList.Location = new System.Drawing.Point(title.Right + 5, title.Top);
                            IngrList.Width = title.Width;
                            IngrList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                            IngrList.SetTitle("Ingridients");

                            Refresh();
                        }

                        public override void Refresh()
                        {
                            base.Refresh();
                            title.Width = (this.Width - 50) / 2;
                            StepsList.Width = title.Width;
                            IngrList.Width = StepsList.Width;
                            IngrList.Left = StepsList.Right + 5;
                            StepsList.Refresh();
                            IngrList.Refresh();
                        }

                        public RecipeAddingDialog()
                            : base()
                        {
                            Init();
                        }

                        public RecipeAddingDialog(Data.Types.Recipes.Info info)
                        {
                            Init();
                            title.Text = info.name;
                            StepsList.FromStringArray(info.steps);
                            IngrList.FromStringArray(info.ingridients);
                        }

                        public Keeper.Data.Types.Recipes.Info GetInfo()
                        {
                            Keeper.Data.Types.Recipes.Info info = new Data.Types.Recipes.Info();
                            info.name = title.Text;
                            info.steps = StepsList.ToStringArray();
                            info.ingridients = IngrList.ToStringArray();
                            info.amount = (uint)info.steps.Length;
                            info.ing_amount = (uint)info.ingridients.Length;

                            if (info.amount == 0)
                            {
                                info.amount = 1;
                                info.steps = new string[1] { "N/D" };
                            }
                            if (info.ing_amount == 0)
                            {
                                info.ing_amount = 1;
                                info.ingridients = new string[1] { "N/D" };
                            }
                            if (info.name == "")
                            {
                                info.name = "N/D";
                            }
                            info.id = Data.Types.Recipes.Info.gen_id++;
                            return info;
                        }
                    }
                }
            }

            abstract class Tab_v1_0
            {
                private System.Windows.Forms.Label caption;
                private System.Windows.Forms.Panel field;
                private int index = 0;

                private static System.Drawing.Point FieldLocation;

                protected uint typeID;

                protected System.Windows.Forms.Panel[] items;
                public int in_row, in_column;

                protected abstract void CheckAttributes();

                public Tab_v1_0(System.String title = "NewTab", uint _typeID = 100)
                {
                    CheckAttributes();

                    caption = new System.Windows.Forms.Label();
                    caption.Text = title;
                    caption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    caption.Size = new System.Drawing.Size(60, 20);
                    caption.ForeColor = System.Drawing.Color.White;
                    caption.Click += new EventHandler(OnCaptionClick);

                    field = new System.Windows.Forms.Panel();
                    field.BackColor = System.Drawing.Color.White;

                    in_row = 3;
                    in_column = 3;

                    typeID = _typeID;

                    Interfaces.IMouseMoveColor.FollowNewControl(caption, System.Drawing.Color.Green, System.Drawing.Color.Black, System.Drawing.Color.DarkGray, System.Drawing.Color.White);
                }

                private void OnCaptionClick(System.Object sender, System.EventArgs args)
                {
                    field.BringToFront();
                }

                public System.Drawing.Size Size
                {
                    set
                    {
                        field.Size = value;
                    }

                    get
                    {
                        return field.Size;
                    }
                }
                public System.Windows.Forms.Control Parent
                {
                    set
                    {
                        field.Parent = value;
                        caption.Parent = value;
                    }
                }
                public System.Drawing.Point Location
                {
                    set
                    {
                        caption.Location = new System.Drawing.Point(value.X, value.Y); ;
                        if (FieldLocation.Y == 0)
                        {
                            FieldLocation = new System.Drawing.Point(value.X, caption.Bottom);
                        }
                        field.Location = FieldLocation;
                    }
                    get
                    {
                        return caption.Location;
                    }
                }

                public int Number
                {
                    set
                    {
                        index = value;
                    }
                    get
                    {
                        return index;
                    }
                }

                public System.Drawing.Point GetNextTab()
                {
                    return new System.Drawing.Point(caption.Right, caption.Top);
                }

                public void BringToFront()
                {
                    field.BringToFront();
                }

                public void AddItems(System.Windows.Forms.Panel[] arr)
                {
                    if (arr == null || arr.Length == 0) return;
                    int width = field.Width / in_row;
                    int height = field.Height / in_column;
                    int offset = (items != null ? items.Length : 0);

                    for (int i = 0; i < arr.Length; ++i)
                    {
                        arr[i].Parent = field;
                        arr[i].Size = new System.Drawing.Size(width, height);
                        arr[i].Location = new System.Drawing.Point((i + offset) % in_row * width, (i + offset) / in_row * height);
                    }
                    if (items == null)
                    {
                        items = arr;
                        return;
                    }
                    items.Concat(arr);
                }

                protected System.Windows.Forms.Control GetParent()
                {
                    return field;
                }

                public virtual void Init()
                {

                }
            }

            namespace ContactTab
            {
                using ContTab = ContactTab.ContactsTab_v1_0;

                class ContactsTab_v1_0 : Tab
                {
                    protected System.Windows.Forms.Label add;

                    protected override void CheckAttributes()
                    {
                        //throw new NotImplementedException();
                    }

                    public ContactsTab_v1_0()
                        : base("Contacts")
                    {
                        add = new System.Windows.Forms.Label();
                        add.Parent = GetParent();
                        add.Dock = System.Windows.Forms.DockStyle.Bottom;
                        add.Height = 5;
                        add.BackColor = System.Drawing.Color.Black;
                        add.Click += new EventHandler(OnAddClick);
                    }

                    private void OnAddClick(System.Object sender, System.EventArgs args)
                    {
                        (new Tools.Contact.ContactAddPanel_v1_0()).Parent = GetParent();
                    }
                }
            }

            namespace PasswordTab
            {
                class PassTab : PasswordTab_v1_0 { }

                class PasswordTab_v1_0 : Tab
                {
                    protected override void CheckAttributes()
                    {
                        if (Keeper.Security.AskDialog.Ask("Opening password tab: Operation above default access. Administrator privilleges are required.") != Security.Admin.Pass)
                        {
                            throw new System.Exception();
                        }
                    }

                    public PasswordTab_v1_0()
                        : base("Passwords")
                    {
                        System.Windows.Forms.Panel h = new System.Windows.Forms.Panel();
                        h.Parent = GetParent();
                        h.Dock = System.Windows.Forms.DockStyle.Bottom;
                        h.Height = 5;
                        h.BackColor = System.Drawing.Color.Green;
                        h.Click += new EventHandler(OnAddClick);
                    }

                    private void OnAddClick(System.Object sender, System.EventArgs args)
                    {
                        (new Tools.Password.PasswordAddWindow_v1_0()).ShowDialog();
                    }
                }
            }

            namespace RecipeTab
            {
                namespace Records
                {
                    class Record : Record_v1_0
                    {
                        public Record(Data.Types.Recipes.Info info) : base(info) { }
                    }

                    class Record_v1_0 : System.Windows.Forms.Panel
                    {
                        private Data.Types.Recipes.Info info;

                        private System.Windows.Forms.Label Title;
                        private System.Windows.Forms.Label[] Steps;
                        private System.Windows.Forms.Label[] Ingrs;

                        private void Init()
                        {
                            Title = new System.Windows.Forms.Label();
                            Title.Parent = this;
                            Title.Dock = System.Windows.Forms.DockStyle.Top;
                            Title.Height = 20;
                            Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                            Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                            Title.MouseClick += new System.Windows.Forms.MouseEventHandler(OnTitleClick);

                            this.BackColor = System.Drawing.Color.Black;
                            this.ForeColor = System.Drawing.Color.White;
                            this.AutoSize = true;
                            this.MinimumSize = new System.Drawing.Size(50, 50);
                        }

                        private void OnTitleClick(System.Object sender, System.Windows.Forms.MouseEventArgs args)
                        {
                            if (args.Button == System.Windows.Forms.MouseButtons.Left)
                            {
                                Tools.Recipe.RecipeAddingDialog dialog = new Tools.Recipe.RecipeAddingDialog(info);
                                dialog.ShowDialog();
                                Data.Types.Recipes.Info newInfo = dialog.GetInfo();
                                Data.DB.Database.UpdateInfo(Data.DB.Database.RecipeID, info.id, newInfo);
                                UpdateInfo(newInfo);
                            }
                            if (args.Button == System.Windows.Forms.MouseButtons.Right)
                            {
                                this.Parent = null;
                                Data.DB.Database.DeleteByID(Data.DB.Database.RecipeID, info.id);
                            }
                        }

                        public Record_v1_0()
                        {
                            Init();
                        }

                        public Record_v1_0(Data.Types.Recipes.Info newInfo)
                        {
                            Init();
                            UpdateInfo(newInfo);
                        }

                        public void UpdateInfo(Data.Types.Recipes.Info newInfo)
                        {
                            info = newInfo;

                            this.Controls.Clear();
                            Title.Parent = this;
                            Title.Text = info.name;

                            Steps = new System.Windows.Forms.Label[info.amount];
                            Ingrs = new System.Windows.Forms.Label[info.ing_amount];
                            int max_width = 0;
                            int max_height = 0;
                            Title.AutoSize = true;
                            if (Title.Width > max_width) max_width = Title.Width + 10;
                            Title.AutoSize = false;

                            int max_left = 0;

                            for (int i = 0; i < Ingrs.Length; ++i)
                            {
                                Ingrs[i] = new System.Windows.Forms.Label();
                                Ingrs[i].Parent = this;
                                Ingrs[i].MaximumSize = new System.Drawing.Size(100, 40);
                                Ingrs[i].AutoEllipsis = true;
                                Ingrs[i].AutoSize = true;
                                Ingrs[i].Location = new System.Drawing.Point(5, (i != 0 ? Ingrs[i - 1].Bottom : Title.Bottom + 5));
                                Ingrs[i].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                                Ingrs[i].Text = "- " + info.ingridients[i];
                                Ingrs[i].Refresh();

                                if (Ingrs[i].Bottom > max_height) max_height = Ingrs[i].Bottom;
                                if (Ingrs[i].Right > max_left) max_left = Ingrs[i].Right;
                            }

                            for (int i = 0; i < Steps.Length; ++i)
                            {
                                Steps[i] = new System.Windows.Forms.Label();
                                Steps[i].Parent = this;
                                Steps[i].MaximumSize = new System.Drawing.Size(190, 40);
                                Steps[i].AutoEllipsis = true;
                                Steps[i].AutoSize = true;
                                Steps[i].Location = new System.Drawing.Point(max_left + 5, (i != 0 ? Steps[i - 1].Bottom : Title.Bottom + 5));
                                Steps[i].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                                Steps[i].Text = (i + 1).ToString() + ") " + info.steps[i];
                                Steps[i].Refresh();

                                if (Steps[i].Right > max_width) max_width = Steps[i].Right;
                                if (Steps[i].Bottom > max_height) max_height = Steps[i].Bottom;
                            }

                            this.Size = new System.Drawing.Size(max_width + 5, max_height + 5);
                            this.Refresh();
                        }
                    }
                }

                class RecipeTab : RecipeTab_v1_0 { }

                class RecipeTab_v1_0 : Tab
                {
                    protected override void CheckAttributes()
                    {
                        //throw new NotImplementedException();
                    }

                    private Records.Record[] list = new Records.Record[0];

                    public override void Init()
                    {
                        System.Windows.Forms.Panel _add = new System.Windows.Forms.Panel();
                        _add.Parent = GetParent();
                        _add.Dock = System.Windows.Forms.DockStyle.Bottom;
                        _add.BackColor = System.Drawing.Color.Red;
                        _add.Click += new EventHandler(OnAddClick);
                        _add.Height = 5;

                        Data.DB.Database.InitByID(Data.DB.Database.RecipeID);

                        int amount = Data.DB.Database.GetAmountForID(typeID);
                        for (int i = 0; i < amount; ++i)
                        {
                            AddNewRecord(new Records.Record((Data.Types.Recipes.Info)Data.DB.Database.GetInfoByIDS(typeID, i)));
                        }
                    }

                    private void AddNewRecord(Records.Record rec)
                    {
                        Array.Resize(ref list, list.Length + 1);
                        list[list.Length - 1] = rec;
                        rec.Parent = GetParent();

                        rec.Location = list.Length != 1 ? new System.Drawing.Point(list[list.Length - 2].Right + 5, list[list.Length - 2].Top) : new System.Drawing.Point(5, 5);
                        if (rec.Right > GetParent().Width)
                        {
                            rec.Left = 5;
                        }
                        for (int k = 0; k < list.Length - 1; ++k)
                        {
                            if ((list[k].Right + 5 > rec.Left && list[k].Right <= rec.Right) || (list[k].Left >= rec.Left && list[k].Left <= rec.Right))
                            {
                                if (list[k].Bottom + 5 > rec.Top)
                                    rec.Top = list[k].Bottom + 5;
                            }
                        }
                    }

                    public void Refresh()
                    {
                        System.Windows.Forms.Control parent = GetParent();

                        int offset = 0;
                        for (int i = 0; i < list.Length; ++i)
                        {
                            if (list[i].Parent == null)
                            {
                                offset++;
                                continue;
                            }
                            list[i] = list[i + offset];
                        }
                        Array.Resize(ref list, list.Length - offset);

                        if (list.Length == 0) return;
                        list[0].Location = new System.Drawing.Point(5, 5);
                        for (int i = 1; i < list.Length; ++i)
                        {
                            list[i].Location = new System.Drawing.Point(list[i - 1].Right + 5, list[i - 1].Top);
                            if (list[i].Right > parent.Width)
                            {
                                list[i].Left = 5;
                            }
                            for (int k = 0; k < i; ++k)
                            {
                                if ((list[k].Right + 5 >= list[i].Left && list[k].Right <= list[i].Right + 5) || (list[k].Left >= list[i].Left && list[k].Left <= list[i].Right))
                                {
                                    list[i].Top = list[k].Bottom + 5;
                                }
                            }
                        }
                    }

                    private void OnAddClick(System.Object sender, System.EventArgs args)
                    {
                        Tools.Recipe.RecipeAddingDialog dialog = new Tools.Recipe.RecipeAddingDialog();
                        dialog.ShowDialog();
                        int index = Data.DB.Database.AddInfoForID(typeID, dialog.GetInfo());
                        AddNewRecord(new Records.Record((Data.Types.Recipes.Info)Data.DB.Database.GetInfoByIDS(typeID, index)));
                    }

                    public RecipeTab_v1_0()
                        : base("Recipes", Data.DB.Database.RecipeID)
                    {
                        //Init();
                    }
                }
            }
        }
    }
}
