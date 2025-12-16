using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Repositories
{
    public class DatabseConfig
    {
           private string _connectionString;
        public String ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
    }
}
