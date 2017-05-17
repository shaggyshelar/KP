using System;  
using System.Data;  
using System.Data.SqlClient; 

namespace ESPL.KP.DapperRepo
{
    public class BaseRepository: IDisposable
    {
       protected IDbConnection con;
        public BaseRepository()
        {
            string connectionString = "Data Source=(localdb)\\v11.0;Initial Catalog=KPDB;Persist Security Info=True;User ID=manish;Password=espl@123";
            con = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}