using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

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
        public void UpdateModel(string modelComboBoxSelectedItem, string mu0text, double mu0, string Eatext, double Ea, string Trtext, double Tr, string ntext, double n, string alphaUtext, double alphaU)
        {
            _connection.Open();
            string query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {mu0.ToString().Replace(",", ".")} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{mu0text}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {Ea.ToString().Replace(",", ".")} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{Eatext}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {Tr.ToString().Replace(",", ".")} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{Trtext}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {n.ToString().Replace(",", ".")} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{ntext}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            query = $"update mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id set value = {alphaU.ToString().Replace(",", ".")} " +
                $"where mat_model.title = '{modelComboBoxSelectedItem}' and mat_coef.title = '{alphaUtext}'";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void InitialModel(string model, List<string> titles, List<string> values, List<string> units) {
            _connection.Open();
            string query = $"select value, unit, mat_coef.title from mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id " +
                $"where mat_model.title = '{model}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    titles.Add(reader["title"].ToString());
                    values.Add(reader["value"].ToString());
                    units.Add(reader["unit"].ToString());
                }
            }
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
        public void UpdatePropValue(string material, string propertyName, string value) {
            _connection.Open();
            string query = $"update material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"set value = {value.ToString().Replace(",", ".")} " +
                $"where material.title = '{material}' and property.title = '{propertyName}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
        public void UpdateCoeffValue(string model, string coeffName, string value) {
            _connection.Open();
            string query = $"update mat_set " +
                $"inner join mat_coef on mat_id = mat_coef_id " +
                $"inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id " +
                $"set value = {value.ToString().Replace(",", ".")} " +
                $"where mat_model.title = '{model}' and mat_coef.title = '{coeffName}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
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
        public void InitialMaterial(string material, List<string> titles, List<string> values, List<string> units) {
            _connection.Open();
            string query = $"select value, unit, property.title from material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"where material.title = '{material}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    titles.Add(reader["title"].ToString());
                    values.Add(reader["value"].ToString());
                    units.Add(reader["unit"].ToString());
                }
            }
            _connection.Close();
        }
        public bool InsertMaterial(string materialName, List<Property> materialProperties, List<Property> modelCoeffs)
        {
            bool isErrors = true;
            _connection.Open();
            MySqlTransaction transaction;
            transaction = _connection.BeginTransaction();
            string query = "";
            MySqlCommand command = new MySqlCommand(query, _connection, transaction);
            try {
                query = $"insert into mat_model(title) values('{materialName}')";
                command.CommandText = query;
                command.ExecuteNonQuery();

                query = $"select mat_model_id from mat_model where title = '{materialName}'";
                command.CommandText = query;
                int modelId = int.Parse(command.ExecuteScalar().ToString());

                query = $"insert into material(title, material_model) values('{materialName}', {modelId})";
                command.CommandText = query;
                command.ExecuteNonQuery();

                query = $"select material_id from material where title = '{materialName}'";
                command.CommandText = query;
                int materialId = int.Parse(command.ExecuteScalar().ToString());

                foreach (Property property in materialProperties) {
                    query = $"insert into material_has_property(material_material_id, property_prop_id, value) " +
                        $"values({materialId}, {property.Id}, {property.Value.ToString().Replace(",", ".")})";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                foreach (Property coeff in modelCoeffs) {
                    query = $"insert into mat_set(mat_coef_id, mat_model_id, value) " +
                        $"values({coeff.Id}, {modelId}, {coeff.Value.ToString().Replace(",", ".")})";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                isErrors = false;
            }
            catch {
                transaction.Rollback();
                isErrors = true;
            }
            finally {
                _connection.Close();
            }
            return isErrors;
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
        public bool ChangePassword(string login, string oldPas, string newPas)
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
                _connection.Close();
                return true;
            }
            else
            {
                _connection.Close();
                return false;
            }
        }
        public string GetId(string query, string type)
        {
            _connection.Open();
            List<string> id = new List<string>();
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id.Add(reader[type].ToString());
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
        public string GetModelTitleFromMaterial(string material) {
            string title = "";
            _connection.Open();
            string query = $"SELECT mat_model.title FROM material INNER JOIN mat_model ON material_model = mat_model_id WHERE material.title = '{material}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    title = reader["title"].ToString();
                }
            }
            _connection.Close();
            return title;
        }
        public void DataBaseExport(string user, string password)
        {
            string commands = @"cd C:\Program Files\MySQL\MySQL Server 8.0\bin && mysqldump.exe -h127.0.0.1 " +
                @$"-u{user} -p{password} --add-drop-database --databases flowmodel > {Environment.CurrentDirectory}\dump.sql";
            string batPath = Path.Combine(Path.GetTempPath(), "dump.bat");
            File.WriteAllText(batPath, commands);
            Process cmd = Process.Start(batPath);
            cmd.WaitForExit();
            File.Delete(batPath);
        }
        public void GetProperties(List<string> propTitles, List<int> propIds, List<string> propUnits) {
            _connection.Open();
            string query = $"SELECT prop_id, title, unit FROM property";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    propIds.Add(int.Parse(reader["prop_id"].ToString()));
                    propTitles.Add(reader["title"].ToString());
                    propUnits.Add(reader["unit"].ToString());
                }
            }
            _connection.Close();
        }
        public void GetUnits(List<string> units) {
            _connection.Open();
            string query = $"SELECT unit_title FROM unit";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    units.Add(reader["unit_title"].ToString());
                }
            }
            _connection.Close();
        }
        public bool InsertUnit(string unit) {
            _connection.Open();
            bool isError = false;
            try {
                string query = $"insert into unit(unit_title) values('{unit}')";
                MySqlCommand command = new MySqlCommand(query, _connection);
                command.ExecuteNonQuery();
            }
            catch {
                isError = true;
            }
            finally {
                _connection.Close();
            }
            return isError;
        }
        public bool InsertProperty(string unit, string property) {
            _connection.Open();
            bool isError = false;
            try {
                string query = $"insert into property(unit, title) values('{unit}', '{property}')";
                MySqlCommand command = new MySqlCommand(query, _connection);
                command.ExecuteNonQuery();
            }
            catch {
                isError = true;
            }
            finally { 
                _connection.Close();
            }
            return isError;
        }
        public void GetCoeffs(List<string> coeffTitles, List<int> coeffIds, List<string> coeffUnits) {
            _connection.Open();
            string query = $"SELECT mat_id, title, unit FROM mat_coef";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    coeffIds.Add(int.Parse(reader["mat_id"].ToString()));
                    coeffTitles.Add(reader["title"].ToString());
                    coeffUnits.Add(reader["unit"].ToString());
                }
            }
            _connection.Close();
        }
    }
}
