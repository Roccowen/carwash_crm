using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace carwash.Interfaces
{
    public interface ILogService
    {
        void Initialize(Assembly assembly, string assemblyName);
    }
}
