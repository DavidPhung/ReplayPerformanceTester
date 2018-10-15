﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace PerformanceTester
{
    public class ExistingConnectionEvent : DatabaseEvent
    {
        public ExistingConnectionEvent(DatabaseEventExecutionContext context, int spid, string text)
            : base(context, -1, "", spid, text, null, null)
        { }

        public override void Execute()
        {
            string connString = OdbcUtils.CreateConnectionString(Context.ConnectionInfo);
            OdbcConnection conn = new OdbcConnection(connString);
            conn.Open();
            Context.Connections.Add(Spid, conn);
        }
    }
}
