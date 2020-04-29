using System;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{

#if win
    [StructLayout(LayoutKind.Sequential)]
    public struct YR_STOPWATCH
    {
        public ulong frequency;
        public ulong start;
    }
#elif osx
    [StructLayout(LayoutKind.Sequential)]
    public struct mach_timebase_info
    {
        public uint numer;
        public uint denom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct YR_STOPWATCH
    {
        public mach_timebase_info timebase;
        public ulong start;
    }
#elif linux
    [StructLayout(LayoutKind.Sequential)]
    public struct timeval {
        public int64 tv_sec;
        public int32 tc_usec;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct timespec {
        public int64 tv_sec;
        public int32 tc_nsec;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct YR_STOPWATCH
    {
        public timeval tv_start;
        public timespec ts_start;
    }
#endif

}
