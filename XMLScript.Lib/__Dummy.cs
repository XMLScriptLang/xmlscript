using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace XMLScript.Lib
{
    public class __Dummy
    {
        public __Dummy() { }
        public void __DummyMethod() {
            // whilst acting as dummy method to load XMLScript.Lib into memory, we should also load other .NET assemblies into memory here
            // System.Numerics
            new BigInteger(1);
        }
    }
}
