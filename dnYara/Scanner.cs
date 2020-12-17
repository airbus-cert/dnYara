using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dnYara.Interop;

namespace dnYara
{
    public class Scanner
    {
        private const int YR_TIMEOUT = 10000;

        private YR_CALLBACK_FUNC callbackPtr;

        public Scanner()
        {
            callbackPtr = new YR_CALLBACK_FUNC(HandleMessage);
        }

        public virtual List<ScanResult> ScanFile(string path, CompiledRules rules)
        {
            return ScanFile(path, rules, YR_SCAN_FLAGS.None);
        }

        public virtual List<ScanResult> ScanFile(
            string path,
            CompiledRules rules,
            YR_SCAN_FLAGS flags)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            var results = new List<ScanResult>();
            var nativePath = path;

            GCHandleHandler resultsHandle = new GCHandleHandler(results);

            ErrorUtility.ThrowOnError(
                Methods.yr_rules_scan_file(
                    rules.BasePtr,
                    nativePath,
                    (int)flags,
                    callbackPtr,
                    resultsHandle.GetPointer(),
                    YR_TIMEOUT));

            resultsHandle.Dispose();

            return results;
        }

        public virtual List<ScanResult> ScanProcess(int processId, CompiledRules rules)
        {
            return ScanProcess(processId, rules, YR_SCAN_FLAGS.None);
        }

        public virtual List<ScanResult> ScanProcess(
            int processId,
            CompiledRules rules,
            YR_SCAN_FLAGS flags)
        {
            var results = new List<ScanResult>();
            GCHandleHandler resultsHandle = new GCHandleHandler(results);

            ErrorUtility.ThrowOnError(
                Methods.yr_rules_scan_proc(
                    rules.BasePtr,
                    processId,
                    (int)flags,
                    callbackPtr,
                    resultsHandle.GetPointer(),
                    YR_TIMEOUT));

            return results;
        }

        public virtual List<ScanResult> ScanString(
            string text,
            CompiledRules rules,
            Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.ASCII;

            byte[] buffer = encoding.GetBytes(text);

            return ScanMemory(ref buffer, rules, YR_SCAN_FLAGS.None);
        }

        public virtual List<ScanResult> ScanStream(
            Stream stream,
            CompiledRules rules)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byte[] buffer = ms.ToArray();

                return ScanMemory(ref buffer, rules, YR_SCAN_FLAGS.None);
            }
        }

        public virtual List<ScanResult> ScanMemory(
            ref byte[] buffer,
            CompiledRules rules)
        {
            return ScanMemory(ref buffer, rules, YR_SCAN_FLAGS.None);
        }

        public List<ScanResult> ScanMemory(
            ref byte[] buffer,
            CompiledRules rules,
            YR_SCAN_FLAGS flags)
        {
            if (buffer.Length == 0)
                return new List<ScanResult>();

            return ScanMemory(ref buffer, buffer.Length, rules, flags);
        }

        internal List<ScanResult> ScanMemory(
            IntPtr buffer,
            int length,
            CompiledRules rules)
        {
            return ScanMemory(buffer, length, rules, YR_SCAN_FLAGS.None);
        }

        internal List<ScanResult> ScanMemory(
            IntPtr buffer,
            int length,
            CompiledRules rules,
            YR_SCAN_FLAGS flags)
        {
            byte[] res = new byte[length - 1];
            Marshal.Copy(buffer, res, 0, length);
            return ScanMemory(ref res, length, rules, flags);
        }

        public virtual List<ScanResult> ScanMemory(
            ref byte[] buffer,
            int length,
            CompiledRules rules,
            YR_SCAN_FLAGS flags)
        {
            var results = new List<ScanResult>();
            GCHandleHandler resultsHandle = new GCHandleHandler(results);

            IntPtr btCpy = Marshal.AllocHGlobal(buffer.Length); ;
            Marshal.Copy(buffer, 0, btCpy, (int)buffer.Length);

            ErrorUtility.ThrowOnError(
                Methods.yr_rules_scan_mem(
                    rules.BasePtr,
                    btCpy,
                    (ulong)length,
                    (int)flags,
                    callbackPtr,
                    resultsHandle.GetPointer(),
                    YR_TIMEOUT));

            return results;
        }

        private YR_CALLBACK_RESULT HandleMessage(
            IntPtr context,
            int message,
            IntPtr message_data,
            IntPtr user_data)
        {
            if (message == Constants.CALLBACK_MSG_RULE_MATCHING)
            {
                var resultsHandle = GCHandle.FromIntPtr(user_data);
                var results = (List<ScanResult>)resultsHandle.Target;

                YR_RULE rule = Marshal.PtrToStructure<YR_RULE>(message_data);
                results.Add(new ScanResult(context, rule));
            }

            return YR_CALLBACK_RESULT.Continue;
        }
    }
}

