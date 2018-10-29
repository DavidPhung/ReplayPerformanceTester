using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Diagnostics;

namespace PerformanceTester
{
    public interface DatabaseEventExecutionContext
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
        IList<DatabaseEvent> Events { get; }
        IDictionary<int, OdbcConnection> Connections { get; }
        //the stopwatch here gives more control, 
        //i.e when we want to pause the stopwatch during login, logout
        Stopwatch Stopwatch { get; } 
    }
}
