using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobiFlight.Scripts
{
    internal class ProcessExecutor
    {
        public Process ProcessObject { get; set; }

        public ProcessExecutor(Process processObject)
        {
            ProcessObject = processObject;               
        }
    }
}
