using dnYara.Interop;

namespace dnYara
{
    public class ProfilingInfo
    {
        public ProfilingInfo()
        {

        }

        public ProfilingInfo(YR_PROFILING_INFO prof)
        {
            AtomMatches = prof.atom_matches;
            MatchTimeNanos = prof.match_time;
            ExecTimeNanos = prof.exec_time;
        }

        /// Number of times that some atom belonging to the rule matched. Each
        /// matching atom means a potential string match that needs to be verified.
        public uint AtomMatches { get; private set; }

        // Amount of time (in nanoseconds) spent verifying atom matches for
        // determining if the corresponding string actually matched or not. This
        // time is not measured for all atom matches, only 1 out of 1024 matches
        // are actually measured.
        public ulong MatchTimeNanos { get; private set; }

        // Amount of time (in nanoseconds) spent evaluating the rule condition.
        public ulong ExecTimeNanos { get; private set; }
    }
}
