// Ik hoop dat deze code werkt.
// Met dank aan https://forum.unity.com/threads/openfiledialog-in-runtime.459474/
// en de hardwerkende Chinezen voor het maken van deze classes.

using System;
using System.Runtime.InteropServices;
 
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
 
public class OpenFileName 
{
    public int      structSize = 0;
    public IntPtr   dlgOwner = IntPtr.Zero;
    public IntPtr   instance = IntPtr.Zero;
    public string   filter = null;
    public string   customFilter = null;
    public int      maxCustFilter = 0;
    public int      filterIndex = 0;
    public string   file = null;
    public int      maxFile = 0;
    public string   fileTitle = null;
    public int      maxFileTitle = 0;
    public string   initialDir = null;
    public string   title = null;
    public int      flags = 0;
    public short    fileOffset = 0;
    public short    fileExtension = 0;
    public string   defExt = null;
    public IntPtr   custData = IntPtr.Zero;
    public IntPtr   hook = IntPtr.Zero;
    public string   templateName = null;
    public IntPtr   reservedPtr = IntPtr.Zero;
    public int      reservedInt = 0;
    public int      flagsEx = 0;
}
 
public class DllTest 
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn) {
        return GetOpenFileName(ofn);
    }
}