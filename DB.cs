using MySql.Data.MySqlClient;
using System.Collections.Generic;

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
        public void InitialMaterial(List<string> list, string query)
        {
            //string query = $"SELECT title FROM flowmodel.material";
            MySqlCommand command = new MySqlCommand(query, _connection);
            _connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(reader["title"].ToString());
                }
            }

            /* _connection.Close();
             query = $"SELECT title FROM flowmodel.mat_model order by mat_model_id asc";
             command = new MySqlCommand(query, _connection);
             _connection.Open();
             using (MySqlDataReader reader = command.ExecuteReader())
             {
                 while (reader.Read())
                 {
                     models.Add(reader["title"].ToString());
                 }
             */
            _connection.Close();

        }
        public void UpdateModel(string modelComboBoxSelectedItem, string mu0text, int  mu0, string Eatext, int Ea, string Trtext, int Tr, string ntext, double n, string alphaUtext, int alphaU)
        {
            _connection.Open();
            string query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {mu0} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{mu0text}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {Ea} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{Eatext}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {Tr} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{Trtext}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {n.ToString().Replace(",", ".")} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{ntext}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {alphaU} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{alphaUtext}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void InitialModel(string modelComboBoxSelectedItem, string mu0text, string Eatext, string Trtext, string ntext, string alphaUtext, List<string> modelCoeffs)
        {
            _connection.Open();
            string query = $"select value from mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{mu0text}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    modelCoeffs.Add(reader["value"].ToString());
                }
            }
            query = $"select value from mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{Eatext}'";
            command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    modelCoeffs.Add(reader["value"].ToString());
                }
            }
            query = $"select value from mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{Trtext}'";
            command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    modelCoeffs.Add(reader["value"].ToString());
                }
            }
            query = $"select value from mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{ntext}'";
            command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    modelCoeffs.Add(reader["value"].ToString());
                }
            }
            query = $"select value from mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{alphaUtext}'";
            command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    modelCoeffs.Add(reader["value"].ToString());
                }
            }
            _connection.Close();
        }
        public void InsertModel(string modelName, string mu0text, int mu0, string Eatext, int Ea, string Trtext, int Tr, string ntext, double n, string alphaUtext, int alphaU)
        {
            _connection.Open();
            string query = $"insert into mat_model(title) value('{modelName}')";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"select mat_model_id from mat_model where title = '{modelName}'";
            command = new MySqlCommand(query, _connection);
            string modelId = command.ExecuteScalar().ToString();

            query = $"select mat_id from mat_coef where title = '{mu0text}'";
            command = new MySqlCommand(query, _connection);
            string tmp = command.ExecuteScalar().ToString();
            query = $"insert into mat_set(mat_coef_id, mat_model_id, value)  " +
                $"value('{tmp}', '{modelId}', {mu0})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();

            query = $"select mat_id from mat_coef where title = '{Eatext}'";
            command = new MySqlCommand(query, _connection);
            tmp = command.ExecuteScalar().ToString();
            query = $"insert into mat_set(mat_coef_id, mat_model_id, value)  " +
                $"value('{tmp}', '{modelId}', {Ea})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();

            query = $"select mat_id from mat_coef where title = '{Trtext}'";
            command = new MySqlCommand(query, _connection);
            tmp = command.ExecuteScalar().ToString();
            query = $"insert into mat_set(mat_coef_id, mat_model_id, value)  " +
                $"value('{tmp}', '{modelId}', {Tr})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();

            query = $"select mat_id from mat_coef where title = '{ntext}'";
            command = new MySqlCommand(query, _connection);
            tmp = command.ExecuteScalar().ToString();
            query = $"insert into mat_set(mat_coef_id, mat_model_id, value)  " +
                $"value('{tmp}', '{modelId}', {n.ToString().Replace(",", ".")})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();

            query = $"select mat_id from mat_coef where title = '{alphaUtext}'";
            command = new MySqlCommand(query, _connection);
            tmp = command.ExecuteScalar().ToString();
            query = $"insert into mat_set(mat_coef_id, mat_model_id, value)  " +
                $"value('{tmp}', '{modelId}', {alphaU})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void DeleteModel(string modelName)
        {
            _connection.Open();
            string query = $"select mat_model_id from mat_model where title = '{modelName}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            string modelId = command.ExecuteScalar().ToString();

            query = $"delete from mat_set where mat_model_id = {modelId}";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"delete from mat_model where title = '{modelName}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();   
        }
    }


    
}
