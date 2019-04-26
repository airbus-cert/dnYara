using dnYara.Interop;
using dnYara.Exceptions;

namespace dnYara
{
    public class ScanHelper
    {
        public static YARA_ERROR CheckRule(string ruleFile)
        {
            YARA_ERROR error = YARA_ERROR.SUCCESS;
            Compiler comp = new Compiler();

            try
            {
                comp.AddRuleFile(ruleFile);
            }
            catch (YaraException e)
            {
                error = e.YRError;
            }
            catch
            {
                error = YARA_ERROR.ERROR_INVALID_FILE;
            }

            comp.Dispose();
            return error;
        }
    }
}
