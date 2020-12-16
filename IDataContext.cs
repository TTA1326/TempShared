using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Common.Enumerations;

namespace Common.Interfaces
{
	public interface IDataContext
	{
		IConnectionStringProvider ConnectionStringProvider { get; }

		IDbConnection CreateConnection(string connectionString);

		IDbCommand CreateCommand(string commandText, IDbConnection connection);

		IDbDataAdapter CreateDataAdapter(AdapterCommandTypes adapterCommandType, IDbCommand command);
	}
}
