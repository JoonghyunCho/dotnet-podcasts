namespace Microsoft.NetConf2021.Maui;

public static class Config
{
    public static bool ListenTogetherIsVisible => true;

    public static bool Desktop
    {
        get
        {
#if WINDOWS || MACCATALYST
            return true;
#else
            return false;
#endif
        }
    }

    public static string BaseWeb = $"{Base}:5002/listentogether";
    public static string Base = "http://10.113.165.98";
    public static string APIUrl = $"{Base}:5000/v1/";
    public static string ListenTogetherUrl = $"{Base}:5001/listentogether";
}
