using System;
using System.Runtime.InteropServices;

namespace dnYara.Interop
{

#if WIN
    [StructLayout(LayoutKind.Sequential)]
    public struct YR_STOPWATCH
    {
        public ulong frequency;
        public ulong start;
    }
#elif OSX
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
#elif LINUX
    [StructLayout(LayoutKind.Sequential)]
    public struct timeval {
        public long tv_sec;
        public int tc_usec;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct timespec {
        public long tv_sec;
        public int tc_nsec;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct YR_STOPWATCH
    {
        public timeval tv_start;
        public timespec ts_start;
    }
#endif

}
