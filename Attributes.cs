
namespace Keeper
{
    namespace Attributes
    {
        class SafeUsageAttribute : System.Attribute
        {
            public static void Check(bool IsSafe)
            {
                if (IsSafe == true) return;
                if (Keeper.Security.AskDialog.Ask("Operation above default access. Administrator privilleges are required.") != Security.Admin.Pass)
                {
                    throw new System.Exception();
                }
            }
        }
    }
}
