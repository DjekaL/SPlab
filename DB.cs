﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

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
        public void InitialModel(string model, List<string> titles, List<double> values, List<string> units) {
            _connection.Open();
            string query = $"select value, unit, mat_coef.title from mat_set inner join mat_coef on mat_id = mat_coef_id inner join mat_model on mat_model.mat_model_id = mat_set.mat_model_id " +
                $"where mat_model.title = '{model}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    titles.Add(reader["title"].ToString());
                    values.Add(double.Parse(reader["value"].ToString()));
                    units.Add(reader["unit"].ToString());
                }
            }
            _connection.Close();
        }
        public void InsertModel(string modelName, string mu0text, double mu0, string Eatext, double Ea, string Trtext, double Tr, string ntext, double n, string alphaUtext, double alphaU)
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
                $"value('{tmp}', '{modelId}', {mu0.ToString().Replace(",", ".")})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();

            query = $"select mat_id from mat_coef where title = '{Eatext}'";
            command = new MySqlCommand(query, _connection);
            tmp = command.ExecuteScalar().ToString();
            query = $"insert into mat_set(mat_coef_id, mat_model_id, value)  " +
                $"value('{tmp}', '{modelId}', {Ea.ToString().Replace(",", ".")})";
            command = new MySqlCommand(query, _connection);
            command.ExecuteNonQuery();

            query = $"select mat_id from mat_coef where title = '{Trtext}'";
            command = new MySqlCommand(query, _connection);
            tmp = command.ExecuteScalar().ToString();
            query = $"insert into mat_set(mat_coef_id, mat_model_id, value)  " +
                $"value('{tmp}', '{modelId}', {Tr.ToString().Replace(",", ".")})";
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
                $"value('{tmp}', '{modelId}', {alphaU.ToString().Replace(",", ".")})";
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
        public void UpdatePropValue(string material, string propertyName, double value) {
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
        public void UpdateCoeffValue(string model, string coeffName, double value) {
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
        public void InitialMaterial(string material, List<string> titles, List<double> values, List<string> units) {
            _connection.Open();
            string query = $"select value, unit, property.title from material_has_property " +
                $"inner join material on material_id = material_material_id " +
                $"inner join property on prop_id = property_prop_id " +
                $"where material.title = '{material}'";
            MySqlCommand command = new MySqlCommand(query, _connection);
            using (MySqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    titles.Add(reader["title"].ToString());
                    values.Add(double.Parse(reader["value"].ToString()));
                    units.Add(reader["unit"].ToString());
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
    }
}
