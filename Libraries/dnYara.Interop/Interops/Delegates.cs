using System;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int YaraScanCallback(
        int message,
        IntPtr data,
        IntPtr context);
}
