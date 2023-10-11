using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup
{
    internal abstract class BveFileValidatorBase
    {
        protected abstract BveFileError Validate(AssemblyName assemblyName);

        public BveFileError Validate(string path)
        {
            try
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(path);
                BveFileError validatedResult = Validate(assemblyName);

                return validatedResult == BveFileError.None && assemblyName.Name != "BveTs" ? BveFileError.MayNotBve : validatedResult;
            }
            catch
            {
                return BveFileError.InvalidFormat;
            }
        }
    }
}
