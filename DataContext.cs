using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Enumerations;
using Common.Interfaces;

namespace Common.Data
{
	public class DataContext<ConnectionT, CommandT, DataAdapterT> : IDataContext
		where ConnectionT : IDbConnection
		where CommandT : IDbCommand
		where DataAdapterT : IDbDataAdapter
	{
		#region Construction

		public DataContext(IConnectionStringProvider connectionStringProvider)
		{
			this.ConnectionStringProvider = connectionStringProvider;
		}

		#endregion

		#region IDataContext

		public IConnectionStringProvider ConnectionStringProvider { get; protected set; }

		public IDbConnection CreateConnection(string connectionString)
		{
			IDbConnection Conn = Activator.CreateInstance<ConnectionT>();
			Conn.ConnectionString = connectionString;
			return Conn;
		}

		// TODO:  Allowing CA2100 to keep showing up here just to keep folks attention and not create a problem.
		public IDbCommand CreateCommand(string commandText, IDbConnection connection)
		{
			IDbCommand Cmd = Activator.CreateInstance<CommandT>();

			Cmd.Connection = connection;
			Cmd.CommandText = commandText;

			return Cmd;
		}

		public IDbDataAdapter CreateDataAdapter(AdapterCommandTypes adapterCommandType, IDbCommand command)
		{
			IDbDataAdapter Adapter = Activator.CreateInstance<DataAdapterT>();

			switch (adapterCommandType)
			{
				case AdapterCommandTypes.Delete:
					Adapter.DeleteCommand = command;
					break;
				case AdapterCommandTypes.Insert:
					Adapter.InsertCommand = command;
					break;
				case AdapterCommandTypes.Select:
					Adapter.SelectCommand = command;
					break;
				case AdapterCommandTypes.Update:
					Adapter.UpdateCommand = command;
					break;
			}

			return Adapter;
		}

		#endregion
	}
}
