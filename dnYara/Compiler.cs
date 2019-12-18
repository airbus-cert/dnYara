using System;
using System.Collections.Generic;
using System.ComponentModel;
using dnYara.Interop;
using dnYara.Exceptions;
using System.IO;

namespace dnYara
{
    /// <summary>
    /// Yara compiler wrapper
    /// </summary>
    public class Compiler 
        : IDisposable
    {
        private IntPtr compilerPtr;

        private List<string> compilationErrors;
        private YR_COMPILER_CALLBACK_FUNC compilerCallback;
        
        public Compiler() 
        {
            ErrorUtility.ThrowOnError(Methods.yr_compiler_create(out compilerPtr));

            compilationErrors = new List<string>();

            compilerCallback = new YR_COMPILER_CALLBACK_FUNC(this.HandleError);

            Methods.yr_compiler_set_callback(compilerPtr, compilerCallback, IntPtr.Zero);
            
        }

        ~Compiler()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!compilerPtr.Equals(IntPtr.Zero))
            {
                var ptr = compilerPtr;
                compilerPtr = IntPtr.Zero;
                Methods.yr_compiler_destroy(ptr);
            }

        }
        
        public void AddRuleFile(string path)
        {
            compilationErrors.Clear();

            try
            {
                PosixFileHandler fw = new PosixFileHandler(path, "r");

                string nullstr = string.Empty;

                string rule = File.ReadAllText(path);
                
                //var errors = Methods.yr_compiler_add_file(
                //    compilerPtr,
                //    fw.FileHandle,
                //    null,
                //    path);

                var errors = Methods.yr_compiler_add_string(
                    compilerPtr,
                    rule,
                    nullstr);

                if (errors != 0)
                    throw new CompilationException(compilationErrors);
            }
            catch (Exception e)
            {
                throw new Win32Exception(e.HResult, e.Message);
            }
        }
        
        public void AddRuleString(string rule)
        {
            compilationErrors.Clear();

            var errors = Methods.yr_compiler_add_string(
                compilerPtr,
                rule,
                string.Empty);

            if (errors != 0)
                throw new CompilationException(compilationErrors);
        }
        
        public CompiledRules Compile()
        {
            IntPtr rulesPtr = new IntPtr();

            ErrorUtility.ThrowOnError(
                Methods.yr_compiler_get_rules(compilerPtr, ref rulesPtr));
            
            return new CompiledRules(rulesPtr);
        }
        
        public static CompiledRules CompileRulesFile(string path)
        {
            Compiler yc = new Compiler();
            yc.AddRuleFile(path);

            return yc.Compile();
        }
        
        public static CompiledRules CompileRulesString(string rule)
        {
            Compiler yc = new Compiler();
            yc.AddRuleString(rule);

            return yc.Compile();
        }

        public void HandleError(
            int errorLevel,
            string fileName,
            int lineNumber,
            string message,
            IntPtr userData)
        {
            var msg = string.Format("Line {1}, file: {2}: {0}",
                message,
                lineNumber,
                string.IsNullOrWhiteSpace(fileName) ? fileName : "[none]");
            
            compilationErrors.Add(msg);
        }
    }
}