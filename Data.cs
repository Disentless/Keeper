
namespace Keeper
{
    namespace Data
    {
        class DataFunctions
        {
            static public int GetNotNullAmount(object[] arr)
            {
                int amount = 0;
                for (int i = 0; i < arr.Length; ++i)
                {
                    if (arr[i] != null) amount++;
                }
                return amount;
            }
        }

        namespace Types
        {
            using ContactInfo = Contacts.Info_v1_0;

            namespace Contacts
            {
                struct Info_v1_0
                {
                    string name1, name2, name3;
                    long birthday;
                    string vk;
                    string mobile;
                }
            }
            namespace Password
            {
                class Info : Info_v1_0 { }

                class Info_v1_0
                {
                    public string description, login, password;
                }
            }

            namespace Recipes
            {
                // Documentation:
                // One record should be saved as following:
                // [4 bytes] (uint) number of steps
                // [unlimited] (string) recipe name
                // [unlimited] (string[]) steps
                //--------------------------------
                // Reference to ingridient is %%ingID%%

                class Info_v1_0
                {
                    static public int gen_id = 0;
                    public int id;
                    public string name;
                    public string[] steps;
                    public string[] ingridients;
                    public uint amount;
                    public uint ing_amount;
                }

                class Info : Info_v1_0 { }
            }

            namespace Events
            {
                class EventInfo : EventInfo_v1_0
                {

                }

                class EventInfo_v1_0
                {
                    public uint ID = 0;
                    public string Caption = null;
                    public bool IsFinished = false;

                    public System.DateTime StartTime = System.DateTime.Now;
                    public System.DateTime EndTime = System.DateTime.Now;
                    public System.DateTime FinishTime = System.DateTime.Now;
                }
            }
        }

        namespace DB
        {
            static class Database
            {
                private class TypeInfo<T> : Interfaces.IInstanceCount
                    where T : new()
                {
                    public bool IsLoaded;
                    public uint TypeID;
                    public string FileName;
                    public string FolderName;
                    public T[] List;
                    public System.IO.FileStream Thread;

                    public string GetFileName()
                    {
                        return default_path + "\\" + FolderName + "\\" + FileName;
                    }
                    public string GetFolderName()
                    {
                        return default_path + "\\" + FolderName;
                    }

                    private uint GetNotNullAmount()
                    {
                        uint amount = 0;
                        for (int i = 0; i < List.Length; ++i)
                        {
                            if (List[i] != null) amount++;
                        }
                        return amount;
                    }

                    private void Init()
                    {
                        if (IsLoaded == true) return;
                        try
                        {
                            Thread = System.IO.File.Open(GetFileName(), System.IO.FileMode.OpenOrCreate);
                        }
                        catch (System.IO.DirectoryNotFoundException)
                        {
                            System.IO.Directory.CreateDirectory(GetFolderName());
                            try
                            {
                                Thread = System.IO.File.Open(GetFileName(), System.IO.FileMode.OpenOrCreate);
                            }
                            catch (System.Exception e)
                            {
                                System.Windows.Forms.MessageBox.Show("Unknown error, details: " + e.ToString() + " File thread cannot be initialized.", "Notification", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                                Thread = null;
                            }
                        }
                        if (Thread == null) throw new System.Exception();
                        List = new T[ReadUint32(ref Thread)];
                        for (int i = 0; i < List.Length; ++i)
                        {
                            List[i] = LoadOne();
                        }
                        IsLoaded = true;
                    }

                    protected virtual T LoadOne() { return new T(); }
                    protected virtual void SaveOne() { }

                    public int Add(T info)
                    {
                        System.Array.Resize(ref List, List.Length + 1);
                        List[List.Length - 1] = info;
                        return List.Length - 1;
                    }

                    public T Get(uint id)
                    {
                        return List[id];
                    }

                    public void Save()
                    {
                        try
                        {
                            if (Thread != null) Thread.Close();
                            Thread = System.IO.File.Open(GetFileName(), System.IO.FileMode.Truncate);
                        }
                        catch (System.Exception)
                        {
                            Thread = null;
                        }
                        if (Thread == null) throw new System.Exception();
                        WriteUint32(ref Thread, (uint)GetNotNullAmount());
                        foreach (T t in List)
                        {
                            if (t == null) continue;
                            SaveOne();
                        }
                        Thread.Close();
                    }

                    public TypeInfo(uint _typeID, string file_name, string folder_name) : base(1)
                    {
                        IsLoaded = false;
                        TypeID = _typeID;
                        FileName = file_name;
                        FolderName = folder_name;
                        List = new T[0];
                        Init();
                    }
                }

                private const string list_extension = "_list.kil";

                private class Recipe : TypeInfo<Data.Types.Recipes.Info>
                {
                    public Recipe()
                        : base(RecipeID, "recipe" + list_extension, "recipes")
                    {

                    }


                }

                public const uint RecipeID = 1;
                private static Recipe RecipeTypeInfo = new Recipe();
                public const uint EventID = 2;
                private static TypeInfo<Data.Types.Events.EventInfo> EventTypeInfo = new TypeInfo<Types.Events.EventInfo>(EventID, "event_list.kil", "events",
                    LoadEvent, AddEvent, GetEventByID, WriteEvent, null);


                const string default_path = "data";
                static string user_path = null;

                static private bool[] idInitialized = new bool[11]
                {
                    false, false, false, 
                    false, false, false,
                    false, false, false,
                    false, false
                };
                //
                // General functions
                //
                static private string GetFileNameByID(uint id)
                {
                    if (user_path == null)
                    {
                        if (id == RecipeTypeInfo.TypeID) return default_path + "\\" + RecipeTypeInfo.FolderName + "\\" + RecipeTypeInfo.FileName;
                    }
                    return null;
                }
                static private string GetFolderNameByID(uint id)
                {
                    if (user_path == null)
                    {
                        if (id == RecipeTypeInfo.TypeID) return default_path + "\\" + RecipeTypeInfo.FolderName;
                    }
                    return null;
                }
                //
                // Working with file stream
                //
                static private string[] ReadStringArray(ref System.IO.FileStream stream, uint amount)
                {
                    if (amount == 0) return new string[1] { "" };
                    string[] array = new string[amount];
                    byte[] buf;
                    for (uint i = 0; i < amount; ++i)
                    {
                        array[i] = "";
                        buf = new byte[sizeof(char)];
                        do
                        {
                            stream.Read(buf, 0, buf.Length);
                            if (System.BitConverter.ToChar(buf, 0) == (char)0) break;
                            array[i] += System.BitConverter.ToChar(buf, 0);
                        } while (true);
                    }
                    return array;
                }
                static private void WriteStringArray(ref System.IO.FileStream stream, string[] array)
                {
                    for (int i = 0; i < array.Length; ++i)
                    {
                        string line = array[i];
                        if (line == null) line = "N/D";
                        for (int k = 0; k < line.Length; ++k)
                        {
                            stream.Write(System.BitConverter.GetBytes(line[k]), 0, sizeof(char));
                        }
                        stream.Write(System.BitConverter.GetBytes(0), 0, sizeof(char));
                    }
                }
                //
                static private uint ReadUint32(ref System.IO.FileStream stream)
                {
                    byte[] buf = new byte[sizeof(uint)];
                    stream.Read(buf, 0, buf.Length);
                    return System.BitConverter.ToUInt32(buf, 0);
                }
                static private void WriteUint32(ref System.IO.FileStream stream, uint k)
                {
                    byte[] buf = System.BitConverter.GetBytes(k);
                    stream.Write(buf, 0, buf.Length);
                }
                //
                static private bool ReadBool(ref System.IO.FileStream stream)
                {
                    byte[] buf = new byte[sizeof(bool)];
                    stream.Read(buf, 0, buf.Length);
                    return System.BitConverter.ToBoolean(buf, 0);
                }
                static private void WriteBool(ref System.IO.FileStream stream, bool value)
                {
                    byte[] buf = new byte[sizeof(bool)];
                    System.BitConverter.GetBytes(value);
                    stream.Write(buf, 0, buf.Length);
                }
                //
                static private long ReadLong(ref System.IO.FileStream stream)
                {
                    byte[] buf = new byte[sizeof(long)];
                    stream.Read(buf, 0, buf.Length);
                    return System.BitConverter.ToInt64(buf, 0);
                }
                static private void WriteLong(ref System.IO.FileStream stream, long value)
                {
                    byte[] buf = new byte[sizeof(long)];
                    System.BitConverter.GetBytes(value);
                    stream.Write(buf, 0, buf.Length);
                }
                //
                static private void InitFileThread(uint typeID, ref System.IO.FileStream stream)
                {
                    try
                    {
                        stream = System.IO.File.Open(GetFileNameByID(typeID), System.IO.FileMode.OpenOrCreate);
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        System.IO.Directory.CreateDirectory(GetFolderNameByID(typeID));
                        try
                        {
                            stream = System.IO.File.Open(GetFileNameByID(typeID), System.IO.FileMode.OpenOrCreate);
                        }
                        catch (System.Exception e)
                        {
                            System.Windows.Forms.MessageBox.Show("Unknown error, details: " + e.ToString() + " File thread cannot be initialized.", "Notification", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                            throw;
                        }
                    }
                }
                static private void RewriteFileThread(string location, ref System.IO.FileStream stream)
                {
                    try
                    {
                        if (stream != null) stream.Close();
                        stream = System.IO.File.Open(location, System.IO.FileMode.Truncate);
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        throw;
                    }
                }

                //===========================================================|
                // Private collection of functions for every supported type =|
                //===========================================================|
                //
                // Load functions
                //
                static private void LoadRecipe(ref System.IO.FileStream cur)
                {
                    if (cur == null) throw new System.Exception();
                    byte[] buf = new byte[sizeof(uint)];
                    cur.Read(buf, 0, buf.Length);
                    uint l = System.BitConverter.ToUInt32(buf, 0);
                    RecipeTypeInfo.List = new Types.Recipes.Info[l];
                    for (int i = 0; i < l; ++i)
                    {
                        Data.Types.Recipes.Info info = new Types.Recipes.Info();

                        info.id = i;
                        Data.Types.Recipes.Info.gen_id = info.id + 1;
                        info.amount = ReadUint32(ref cur);
                        info.ing_amount = ReadUint32(ref cur);
                        info.name = ReadStringArray(ref cur, 1)[0];
                        info.steps = ReadStringArray(ref cur, info.amount);
                        info.ingridients = ReadStringArray(ref cur, info.ing_amount);

                        RecipeTypeInfo.List[i] = info;
                    }
                }
                static private void LoadEvent(ref System.IO.FileStream cur)
                {
                    if (cur == null) throw new System.Exception();
                    EventTypeInfo.List = new Types.Events.EventInfo[ReadUint32(ref cur)];
                    for (int i = 0; i < EventTypeInfo.List.Length; ++i)
                    {
                        Data.Types.Events.EventInfo info = new Types.Events.EventInfo();

                        info.ID = ReadUint32(ref cur);
                        info.Caption = ReadStringArray(ref cur, 1)[0];
                        info.IsFinished = ReadBool(ref cur);

                        info.StartTime = System.DateTime.FromBinary(ReadLong(ref cur));
                        info.EndTime = System.DateTime.FromBinary(ReadLong(ref cur));
                        info.FinishTime = System.DateTime.FromBinary(ReadLong(ref cur));

                        EventTypeInfo.List[i] = info;
                    }
                }
                //
                // Add functions
                //
                static private int AddRecipe(Data.Types.Recipes.Info info)
                {
                    System.Array.Resize(ref RecipeTypeInfo.List, RecipeTypeInfo.List.Length + 1);
                    RecipeTypeInfo.List[RecipeTypeInfo.List.Length - 1] = info;
                    return RecipeTypeInfo.List.Length - 1;
                }
                static private int AddEvent(Data.Types.Events.EventInfo info)
                {
                    System.Array.Resize(ref EventTypeInfo.List, EventTypeInfo.List.Length + 1);
                    EventTypeInfo.List[EventTypeInfo.List.Length - 1] = info;
                    return EventTypeInfo.List.Length - 1;
                }
                //
                // Delete functions
                //
                static private void DeleteRecipe(int id)
                {
                    if (id >= RecipeTypeInfo.List.Length) return;
                    RecipeTypeInfo.List[id] = null;
                }
                static private void DeleteEvent(int id)
                {
                    if (id >= EventTypeInfo.List.Length) return;
                    EventTypeInfo.List[id] = null;
                }
                //
                // Get functions
                //
                static private Data.Types.Recipes.Info GetRecipeInfoByID(int id)
                {
                    if (id >= RecipeTypeInfo.List.Length) return null;
                    return RecipeTypeInfo.List[id];
                }
                static private Data.Types.Events.EventInfo GetEventByID(int id)
                {
                    if (id >= EventTypeInfo.List.Length) return null;
                    return EventTypeInfo.List[id];
                }
                //
                // Save functions
                //
                static private void WriteRecipe(ref System.IO.FileStream stream)
                {
                    if (stream == null) throw new System.Exception();
                    byte[] buf = System.BitConverter.GetBytes(DataFunctions.GetNotNullAmount((object[])RecipeTypeInfo.List));
                    stream.Write(buf, 0, buf.Length);
                    for (int i = 0; i < RecipeTypeInfo.List.Length; ++i)
                    {
                        if (RecipeTypeInfo.List[i] == null) continue;
                        WriteUint32(ref stream, RecipeTypeInfo.List[i].amount);
                        WriteUint32(ref stream, RecipeTypeInfo.List[i].ing_amount);
                        WriteStringArray(ref stream, new string[1] { RecipeTypeInfo.List[i].name });
                        WriteStringArray(ref stream, RecipeTypeInfo.List[i].steps);
                        WriteStringArray(ref stream, RecipeTypeInfo.List[i].ingridients);
                    }
                }
                static private void WriteEvent(ref System.IO.FileStream stream)
                {
                    if (stream == null) throw new System.Exception();
                    WriteUint32(ref stream, (uint)DataFunctions.GetNotNullAmount((object[])EventTypeInfo.List));
                    for (int i = 0; i < EventTypeInfo.List.Length; ++i)
                    {
                        if (EventTypeInfo.List[i] == null) continue;
                        WriteUint32(ref stream, EventTypeInfo.List[i].ID);
                        WriteStringArray(ref stream, new string[1] { EventTypeInfo.List[i].Caption });
                        WriteBool(ref stream, EventTypeInfo.List[i].IsFinished);

                        WriteLong(ref stream, EventTypeInfo.List[i].StartTime.ToBinary());
                        WriteLong(ref stream, EventTypeInfo.List[i].EndTime.ToBinary());
                        WriteLong(ref stream, EventTypeInfo.List[i].FinishTime.ToBinary());
                    }
                }
                //
                // Update functions
                //
                static private void UpdateRecipe(int id, Data.Types.Recipes.Info newInfo)
                {
                    RecipeTypeInfo.List[id] = newInfo;
                }

                //==========================|
                // Public access functions =|
                //==========================|
                /// <summary>Initializing function.</summary>
                static public void InitByID(uint typeID)
                {
                    if (idInitialized[typeID] == true) return;

                    try
                    {
                        if (typeID == RecipeTypeInfo.TypeID)
                        {
                            InitFileThread(RecipeTypeInfo.TypeID, ref RecipeTypeInfo.Thread);
                            RecipeTypeInfo.Load(ref RecipeTypeInfo.Thread);
                            RecipeTypeInfo.IsLoaded = true;
                            idInitialized[typeID] = true;
                            return;
                        }
                        if (typeID == EventTypeInfo.TypeID)
                        {
                            InitFileThread(EventTypeInfo.TypeID, ref EventTypeInfo.Thread);
                            EventTypeInfo.Load(ref EventTypeInfo.Thread);
                            EventTypeInfo.IsLoaded = true;
                            idInitialized[typeID] = true;
                            return;
                        }
                    }
                    catch (System.Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("Error during initializing database.");
                        return;
                    }
                }

                /// <summary>Add function.</summary>
                /// <returns>Index for newly added element.</returns>
                static public int AddInfoForID(uint typeID, System.Object info)
                {
                    if (typeID == RecipeTypeInfo.TypeID) return RecipeTypeInfo.AddItem((Types.Recipes.Info)info);
                    return -1;
                }

                /// <summary>Get function.</summary>
                /// <returns></returns>
                static public System.Object GetInfoByIDS(uint typeID, int infoID)
                {
                    if (typeID == RecipeTypeInfo.TypeID) return RecipeTypeInfo.GetInfoByID(infoID);
                    return null;
                }

                /// <summary>Save function.</summary>
                static private void SaveByID(uint typeID)
                {
                    if (typeID == RecipeTypeInfo.TypeID)
                    {
                        RecipeTypeInfo.Save(ref RecipeTypeInfo.Thread);
                    }
                }

                /// <summary>Gets number of elements.</summary>
                static public int GetAmountForID(uint typeID)
                {
                    if (typeID == RecipeTypeInfo.TypeID) return RecipeTypeInfo.List.Length;
                    return 0;
                }

                /// <summary>Updates information for type.</summary>
                static public void UpdateInfo(uint typeID, int id, System.Object newInfo)
                {
                    if (typeID == RecipeID) UpdateRecipe(id, (Data.Types.Recipes.Info)newInfo);
                }

                /// <summary>Deletes certains item from database.</summary>
                static public void DeleteByID(uint typeID, int id)
                {
                    if (typeID == RecipeID) DeleteRecipe(id);
                }

                /// <summary>Saves changes.</summary>
                static public void Close()
                {
                    RewriteFileThread(GetFileNameByID(RecipeTypeInfo.TypeID), ref RecipeTypeInfo.Thread);
                    RecipeTypeInfo.Save(ref RecipeTypeInfo.Thread);
                }
            }

        }
    }
}