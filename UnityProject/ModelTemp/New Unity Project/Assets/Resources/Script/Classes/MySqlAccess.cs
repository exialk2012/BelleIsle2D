using mysql.proxy.client;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using UnityEngine;

namespace Script.Classes
{
    public class MySqlAccess
    {
        
        /// <summary>
        /// 构造函数，需要赋予IP、端口、用户名、密码、要处理的数据库名称，之后会自动连接对应的数据库
        /// </summary>
        /// <param name="_host">IP地址</param>
        /// <param name="_port">端口号</param>
        /// <param name="_userName">用户名</param>
        /// <param name="_passwd">密码</param>
        /// <param name="_database">要处理的数据库名称</param>
        public MySqlAccess(string _host,string _port,string _userName,string _passwd,string _database)
        {
            host = _host;
            port = _port;
            userName = _userName;
            passwd = _passwd;
            database = _database;
            OpenSql();
        }
        /// <summary>
        /// 连接数据库函数
        /// </summary>
        public static void OpenSql()
        {
            try
            {
                string mySqlString = $"server={host};port={port};user={userName};password={passwd}; database={database};";
                mySqlConnection = new MySqlConnection(mySqlString);
                mySqlConnection.Open();
                Debug.Log("服务器连接成功");
            }
            catch (Exception e)
            {
                Debug.LogError($"服务器连接失败,{e.Message.ToString()}");
            }
        }

        /// <summary>
        /// 创建表函数
        /// </summary>
        /// <param name="name">要创建的数据表名</param>
        /// <param name="colName">数据表的字段名称</param>
        /// <param name="colType">数据表的字段类型</param>
        /// <returns></returns>
        public DataSet CreateTable(string name,string[] colName,string[] colType)
        {
            if (colName.Length!=colType.Length)
            {
                throw new Exception("输入不正确，字段名称的数量不同于字段类型的数量");
            }
            string sqlString = $"CREATE TABLE {name} ({colName[0]} {colType[0]}";
            for (int i = 1; i < colName.Length; i++)
            {
                sqlString += $",{colName[i]} {colType[i]}";
            }
            sqlString += ")";
            return QuerySet(sqlString);
        }
        /// <summary>
        /// 创建自动增加ID并且有主键的表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="colName"></param>
        /// <param name="colType"></param>
        /// <returns></returns>
        public DataSet CreateTableAutoId(string name,string[] colName,string[] colType)
        {
            if (colName.Length!=colType.Length)
            {
                throw new Exception("字段数与字段类型数不一致");
            }
            string sqlString = $"CREATE TABLE {name} ({colName[0]} {colType[0]} NOT NULL AUTO_INCREMENT";
            for (int i = 1; i < colName.Length; i++)
            {
                sqlString += $", {colName[i]} {colType[i]}";
            }
            sqlString += $",PRIMARY KEY ({colName[0]}))";
            return QuerySet(sqlString);
        }
        /// <summary>
        /// 插入一条数据函数（不支持自动递增所创建的表）
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataSet InsertInto(string tableName,string[] values)
        {
            string sqlString = $"REPLACE INTO {tableName} VALUES ('{values[0]}'";
            for (int i = 1; i <values.Length; i++)
            {
                sqlString += $", '{values[i]}'";
            }
            sqlString += ")";
            return QuerySet(sqlString);
        }
        /// <summary>
        /// 在一条数据的部分字段中插入值
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataSet InsertInto(string tableName,string[] colName,string[] values)
        {
            if (colName.Length!=values.Length)
            {
                throw new Exception("要插入的数据数与字段数不一致");
            }
            string sqlString = $"INSERT INTO {tableName} ({colName[0]}";
            for (int i = 1; i < colName.Length; i++)
            {
                sqlString += $", {colName[i]}";
            }
            sqlString += $") VALUES ('{values[0]}'";
            for (int i = 1; i < values.Length; i++)
            {
                sqlString += $", '{values[i]}'";
            }
            sqlString += ")";
            return QuerySet(sqlString);
        }
        /// <summary>
        /// 查询数据库函数
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="items"></param>
        /// <param name="whereColName"></param>
        /// <param name="operation"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DataSet Select(string tableName,string[] items,string[] whereColName,string[] operation,string[] values)
        {
            if (whereColName.Length!=operation.Length||operation.Length!=values.Length)
            {
                throw new Exception("输入不正确：字段长度不等于操作符长度不等于值长度");
            }
            string sqlString = $"SELECT {items[0]}";
            for (int i = 1; i < items.Length; i++)
            {
                sqlString += $",{items[i]}";
            }
            sqlString += $" FROM {tableName} WHERE {whereColName[0]}{operation[0]} '{values[0]}'";
            for (int i = 1; i < whereColName.Length; i++)
            {
                sqlString += $" AND {whereColName[i]}{operation[i]} '{values[i]}'";
            }
            return QuerySet(sqlString);
        }
        /// <summary>
        /// 更新数据函数
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colNames"></param>
        /// <param name="colValues"></param>
        /// <param name="selectKey"></param>
        /// <param name="selectValue"></param>
        /// <returns></returns>
        public DataSet UpdateInto(string tableName,string[] colNames,string[] colValues,string selectKey,string selectValue)
        {
            string sqlString = $"UPDATE {tableName} SET {colNames[0]} = {colValues[0]}";
            for (int i = 1; i < colValues.Length; i++)
            {
                sqlString += $", {colNames[i]} = {colValues[i]}";
            }
            sqlString += $" WHERE {selectKey} = {selectValue} ";
            return QuerySet(sqlString);
        }
        /// <summary>
        /// 删除数据函数
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colNames"></param>
        /// <param name="colValues"></param>
        /// <returns></returns>
        public DataSet Delete(string tableName,string[] colNames,string[] colValues)
        {
            string sqlString = $"DELETE FROM {tableName} WHERE {colNames[0]} = {colValues[0]}";
            for (int i = 1; i < colValues.Length; i++)
            {
                sqlString += $" or {colNames[i]} = {colValues[i]}";
            }
            return QuerySet(sqlString);
        }
        /// <summary>
        /// 关闭数据库函数
        /// </summary>
        public void Close()
        {
            if (mySqlConnection!=null)
            {
                mySqlConnection.Close();
                mySqlConnection.Dispose();
                mySqlConnection=null;
            }
            Debug.Log("数据库关闭成功");
        }
        /// <summary>
        /// 执行SQL语句函数
        /// </summary>
        /// <param name="sqlString">要执行的SQL语句</param>
        /// <returns></returns>
        public static DataSet QuerySet(string sqlString)
        {
            //Debug.Log(mySqlConnection.State);
            if (mySqlConnection.State==ConnectionState.Open)
            {
                DataSet dataSet = new DataSet();
                try
                {
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
                    mySqlDataAdapter.Fill(dataSet);
                }
                catch (Exception e)
                {

                    throw new Exception($"SQL:{sqlString}\n{e.Message.ToString()}");
                }
                finally
                {

                }
                Debug.Log("执行SQL语句成功");
                return dataSet;
            }
            Debug.Log("执行SQL语句失败");

            return null;
        }


        public static MySqlConnection mySqlConnection;
        private static string host;
        private static string port;
        private static string userName;
        private static string passwd;
        private static string database;

       
    }
}

