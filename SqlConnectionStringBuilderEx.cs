using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;

namespace Common.Helpers
{
    public class SqlConnectionStringBuilderEx : ISqlConnectionStringBuilderEx
    {
        public SqlConnectionStringBuilderEx(string connectionString)
        {
            _Builder = new SqlConnectionStringBuilder(connectionString);
        }

        public string ConnectionString() { return _Builder.ToString(); }

        public string MaskedConnectionString()
        {
            string UserID = _Builder.UserID;
            string Password = _Builder.Password;

            _Builder.UserID  = _Builder.Password = string.Concat(Enumerable.Repeat("*", 7));

            string Results = ConnectionString();

            _Builder.UserID = UserID;
            _Builder.Password = Password;

            return Results;
        }

        public object this[string keyword] { get { return _Builder?[keyword]; } }

        protected SqlConnectionStringBuilder _Builder;
    }
}
