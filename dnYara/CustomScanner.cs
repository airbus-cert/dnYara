using dnYara.Interop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace dnYara
{
    public class CustomScanner
    {
        private const int YR_TIMEOUT = 10000;

        private IntPtr customScannerPtr = IntPtr.Zero;

        public CustomScanner(CompiledRules rules, int flags = 0, int timeout = YR_TIMEOUT)
        {
            CreateNewScanner(rules, (YR_SCAN_FLAGS)flags, timeout);
        }

        ~CustomScanner()
        {
            if (customScannerPtr != IntPtr.Zero)
            {
                Release();
            }
        }

        //must be called before the context is destroyed (ie: falling out of a using())
        public void Release()
        {
            Methods.yr_scanner_destroy(customScannerPtr);
            customScannerPtr = IntPtr.Zero;
        }

        private void CreateNewScanner(CompiledRules rules, YR_SCAN_FLAGS flags, int timeout)
        {
            ErrorUtility.ThrowOnError(
                Methods.yr_scanner_create(rules.BasePtr, out IntPtr newScanner));

            customScannerPtr = newScanner;

            SetFlags(flags);
            SetTimeout(timeout);
        }

        public virtual void SetFlags(YR_SCAN_FLAGS flags) => Methods.yr_scanner_set_flags(customScannerPtr, (int)flags);
        public virtual void SetTimeout(int timeout) => Methods.yr_scanner_set_timeout(customScannerPtr, timeout);

        private bool TestAllVariablesUnique(ExternalVariables externalVariables, out string duplicatesListString)
        {
            duplicatesListString = "";

            List<string> allKeys = externalVariables.StringVariables.Keys.ToList();
            allKeys.AddRange(externalVariables.IntVariables.Keys.ToList());
            allKeys.AddRange(externalVariables.FloatVariables.Keys.ToList());
            allKeys.AddRange(externalVariables.BoolVariables.Keys.ToList());

            var duplicates = allKeys.GroupBy(_ => _).Where(_ => _.Count() > 1).ToList();

            if (duplicates.Count == 0) return true;

            for (var i = 0; i < duplicates.Count; i++)
            {
                duplicatesListString += $"{duplicates[i].Key}";
                if (i < (duplicates.Count - 1))
                    duplicatesListString += ", ";
            }

            return false;
        }

        private void SetExternalVariables(ExternalVariables externalVariables)
        {
            if (!TestAllVariablesUnique(externalVariables, out string duplicates))
            {
                throw new InvalidDataException("Duplicate external variable names declared: " + duplicates);
            }

            foreach (KeyValuePair<string, string> variable in externalVariables.StringVariables)
                ErrorUtility.ThrowOnError(
                     Methods.yr_scanner_define_string_variable(customScannerPtr, variable.Key, variable.Value));

            foreach (KeyValuePair<string, long> variable in externalVariables.IntVariables)
                ErrorUtility.ThrowOnError(
                    Methods.yr_scanner_define_integer_variable(customScannerPtr, variable.Key, variable.Value));

            foreach (KeyValuePair<string, double> variable in externalVariables.FloatVariables)
                ErrorUtility.ThrowOnError(
                    Methods.yr_scanner_define_float_variable(customScannerPtr, variable.Key, variable.Value));

            foreach (KeyValuePair<string, bool> variable in externalVariables.BoolVariables)
                ErrorUtility.ThrowOnError(
                    Methods.yr_scanner_define_boolean_variable(customScannerPtr, variable.Key, variable.Value == true ? 1 : 0));
        }

        //YARA doesnt allow deletion of variables, this cleans them us as much as practical but
        //a new scanner should be created if it's imporant for them not to exist
        private void ClearExternalVariables(ExternalVariables externalVariables)
        {
            foreach (KeyValuePair<string, string> variable in externalVariables.StringVariables)
                ErrorUtility.ThrowOnError(
                    Methods.yr_scanner_define_string_variable(customScannerPtr, variable.Key, String.Empty));

            foreach (KeyValuePair<string, long> variable in externalVariables.IntVariables)
                ErrorUtility.ThrowOnError(
                    Methods.yr_scanner_define_integer_variable(customScannerPtr, variable.Key, long.MinValue));

            foreach (KeyValuePair<string, double> variable in externalVariables.FloatVariables)
                ErrorUtility.ThrowOnError(
                    Methods.yr_scanner_define_float_variable(customScannerPtr, variable.Key, float.NegativeInfinity));

            foreach (KeyValuePair<string, bool> variable in externalVariables.BoolVariables)
                ErrorUtility.ThrowOnError(
                    Methods.yr_scanner_define_boolean_variable(customScannerPtr, variable.Key, 0));
        }


        public virtual List<ScanResult> ScanFile(string path, ExternalVariables externalVariables)
        {
            if (customScannerPtr == IntPtr.Zero)
                throw new NullReferenceException("Custom Scanner has not been initialised");

            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            SetExternalVariables(externalVariables);

            YR_CALLBACK_FUNC scannerCallback = new YR_CALLBACK_FUNC(HandleMessage);
            List<ScanResult> scanResults = new List<ScanResult>();
            GCHandleHandler resultsHandle = new GCHandleHandler(scanResults);
            Methods.yr_scanner_set_callback(customScannerPtr, scannerCallback, resultsHandle.GetPointer());

            ErrorUtility.ThrowOnError(
                Methods.yr_scanner_scan_file(
                    customScannerPtr,
                    path
                    ));

            ClearExternalVariables(externalVariables);

            return scanResults;
        }

        public virtual List<ScanResult> ScanString(
            string text,
            ExternalVariables externalVariables,
            Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.ASCII;

            byte[] buffer = encoding.GetBytes(text);

            return ScanMemory(ref buffer, externalVariables, YR_SCAN_FLAGS.None);
        }

        public virtual List<ScanResult> ScanStream(
            Stream stream,
            ExternalVariables externalVariables)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byte[] buffer = ms.ToArray();

                return ScanMemory(ref buffer, externalVariables, YR_SCAN_FLAGS.None);
            }
        }

        public virtual List<ScanResult> ScanMemory(
            ref byte[] buffer,
            ExternalVariables externalVariables)
        {
            return ScanMemory(ref buffer, externalVariables, YR_SCAN_FLAGS.None);
        }

        public List<ScanResult> ScanMemory(
            ref byte[] buffer,
            ExternalVariables externalVariables,
            YR_SCAN_FLAGS flags)
        {
            if (buffer.Length == 0)
                return new List<ScanResult>();

            return ScanMemory(ref buffer, buffer.Length, externalVariables, flags);
        }

        internal List<ScanResult> ScanMemory(
            IntPtr buffer,
            int length,
            ExternalVariables externalVariables)
        {
            return ScanMemory(buffer, length, externalVariables, YR_SCAN_FLAGS.None);
        }

        internal List<ScanResult> ScanMemory(
            IntPtr buffer,
            int length,
            ExternalVariables externalVariables,
            YR_SCAN_FLAGS flags)
        {
            byte[] res = new byte[length - 1];
            Marshal.Copy(buffer, res, 0, length);
            return ScanMemory(ref res, length, externalVariables, flags);
        }

        public virtual List<ScanResult> ScanMemory(
            ref byte[] buffer,
            int length,
            ExternalVariables externalVariables,
            YR_SCAN_FLAGS flags)
        {
            YR_CALLBACK_FUNC scannerCallback = new YR_CALLBACK_FUNC(HandleMessage);
            List<ScanResult> scanResults = new List<ScanResult>();
            GCHandleHandler resultsHandle = new GCHandleHandler(scanResults);
            Methods.yr_scanner_set_callback(customScannerPtr, scannerCallback, resultsHandle.GetPointer());

            SetFlags(flags);
            SetExternalVariables(externalVariables);

            IntPtr btCpy = Marshal.AllocHGlobal(buffer.Length); ;
            Marshal.Copy(buffer, 0, btCpy, (int)buffer.Length);

            ErrorUtility.ThrowOnError(
                Methods.yr_scanner_scan_mem(
                    customScannerPtr,
                    btCpy,
                    (ulong)length
                    ));

            ClearExternalVariables(externalVariables);

            return scanResults;
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
