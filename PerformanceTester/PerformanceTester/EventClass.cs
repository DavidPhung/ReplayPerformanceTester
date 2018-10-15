using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTester
{
    class EventClass
    {
        public const int RPC_STARTING = 11;
        public const int SQL_BATCH_STARTING = 13;
        public const int AUDIT_LOGIN = 14;
        public const int AUDIT_LOGOUT = 15;
        public const int EXISTING_CONNECTION = 17;
    }
}
