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
        public void InitialComboBox(List<string> list, string query, string type)
        {
            //string query = $"SELECT title FROM flowmodel.material";
            MySqlCommand command = new MySqlCommand(query, _connection);
            _connection.Open();
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(reader[type].ToString());
                }
            }


            _connection.Close();
        }
        public void UpdateModel(string modelComboBoxSelectedItem, string mu0text, int mu0, string Eatext, int Ea, string Trtext, int Tr, string ntext, double n, string alphaUtext, int alphaU)
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
        public void UpdateMaterial(string materialName, string pName, double p, string cName, double c, string T0Name, double T0)
        {
            _connection.Open();
            string query = $"update material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"set value = {p.ToString().Replace(",", ".")} " +
                $"where material.title = '{materialName}' and property.title = '{pName}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"set value = {c.ToString().Replace(",", ".")} " +
                $"where material.title = '{materialName}' and property.title = '{cName}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"set value = {T0.ToString().Replace(",", ".")} " +
                $"where material.title = '{materialName}' and property.title = '{T0Name}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void DeleteMaterial(string materialName)
        {
            _connection.Open();
            string query = $"select material_id from material where title = '{materialName}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            string materialId = command.ExecuteScalar().ToString();

            query = $"delete from material_has_property where material_material_id = {materialId}";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"delete from material where title = '{materialName}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void InitialMaterial(string materialComboBoxSelectedItem, string pName, string cName, string T0Name, List<string> matParams)
        {
            _connection.Open();
            string query = $"select value from material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"where material.title = '{materialComboBoxSelectedItem}' and property.title = '{pName}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    matParams.Add(reader["value"].ToString());
                }
            }
            query = $"select value from material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"where material.title = '{materialComboBoxSelectedItem}' and property.title = '{cName}'";
            command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    matParams.Add(reader["value"].ToString());
                }
            }
            query = $"select value from material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"where material.title = '{materialComboBoxSelectedItem}' and property.title = '{T0Name}'";
            command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    matParams.Add(reader["value"].ToString());
                }
            }
            _connection.Close();
        }
        public void InsertMaterial(string materialName, string pName, double p, string cName, double c, string T0Name, double T0)
        {
            _connection.Open();
            string query = $"insert into material(title) value('{materialName}')";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"select material_id from material where title = '{materialName}'";
            command = new MySqlCommand(query, _connection);
            string materialId = command.ExecuteScalar().ToString();

            query = $"select prop_id from property where title = '{pName}'";
            command = new MySqlCommand(query, _connection);
            string tmp = command.ExecuteScalar().ToString();
            query = $"insert into material_has_property(material_material_id, property_prop_id, value)  " +
                $"value('{materialId}', '{tmp}', {p.ToString().Replace(",", ".")})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();

            query = $"select prop_id from property where title = '{cName}'";
            command = new MySqlCommand(query, _connection);
            tmp = command.ExecuteScalar().ToString();
            query = $"insert into material_has_property(material_material_id, property_prop_id, value)  " +
                $"value('{materialId}', '{tmp}', {c.ToString().Replace(",", ".")})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();

            query = $"select prop_id from property where title = '{T0Name}'";
            command = new MySqlCommand(query, _connection);
            tmp = command.ExecuteScalar().ToString();
            query = $"insert into material_has_property(material_material_id, property_prop_id, value)  " +
                $"value('{materialId}', '{tmp}', {T0.ToString().Replace(",", ".")})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();


            _connection.Close();
        }
        public void DeleteUser(string login)
        {
            _connection.Open();
            string query = $"delete from user where login = '{login}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void InsertUser(string login, string password)
        {
            _connection.Open();
            string query = $"insert into user(login, password, category_cat_id) value('{login}', '{password}', 2)";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void ChangePassword(string login, string oldPas, string newPas)
        {
            _connection.Open();
            string query = $"SELECT password FROM flowmodel.user where login = '{login}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            string currentPas = command.ExecuteScalar().ToString();

            if (currentPas == oldPas)
            {
                query = $"update user set password = '{newPas}' where login = '{login}'";
                command = new MySqlCommand(query, _connection);
                command.ExecuteNonQuery();
            }
            _connection.Close();
        }
        public string GetUserId(string login)
        {
            _connection.Open();
            List<string> id = new List<string>();
            string query = $"select user_id from user where login = '{login}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id.Add(reader["user_id"].ToString());
                }
            }
            if(id.Count > 0)
            {
                _connection.Close();
                return id[0];
            }
            _connection.Close();
            return "";
        }
    }
}
