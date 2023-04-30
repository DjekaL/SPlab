using MySql.Data.MySqlClient;

namespace Don_tKnowHowToNameThis
{
    public class DB
    {
        public MySqlConnection _connection;
        public DB(string host, int port, string database, string username, string password)
        {
            string connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;
            _connection = new MySqlConnection(connString);
        }
    }
}
