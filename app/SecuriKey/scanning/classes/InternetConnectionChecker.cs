using System;
using System.Runtime.InteropServices;

public static class InternetConnectionChecker
{
    [DllImport("wininet.dll")]
    private static extern bool InternetGetConnectedState(out int description, int reservedValue);

    public static bool IsConnectedToInternet()
    {
        int description = 0;
        return InternetGetConnectedState(out description, 0);
    }
}
