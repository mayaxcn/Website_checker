using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

public class SQLDB
{
    private static MySqlConnection SqlConn;
    private static MySqlTransaction SqlTrans = null;
    #region 私有实用方法&构造函数 Start

    private void AttachParameters(MySqlCommand command, MySqlParameter[] commandParameters)
    {
        foreach (MySqlParameter p in commandParameters)
        {
            if ((p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
            {

                p.Value = DBNull.Value;
            }
            if (p.MySqlDbType == MySqlDbType.DateTime)
            {
                if (DateTime.Compare(Convert.ToDateTime(p.Value), DateTime.Parse("1901-01-01")) < 0)
                {
                    p.Value = DBNull.Value;
                }
            }

            command.Parameters.Add(p);
        }
    }

    private void AssignParameterValues(MySqlParameter[] commandParameters, object[] parameterValues)
    {
        if ((commandParameters == null) || (parameterValues == null))
        {
            return;
        }

        if (commandParameters.Length != parameterValues.Length)
        {
            throw new ArgumentException("参数个数不匹配");
        }

        for (int i = 0, j = commandParameters.Length; i < j; i++)
        {
            commandParameters[i].Value = parameterValues[i];
        }
    }

    private void PrepareCommand(MySqlCommand command, MySqlConnection connection, MySqlTransaction transaction, CommandType commandType, string commandText, MySqlParameter[] commandParameters)
    {
        //if the provided connection is not open, we will open it
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        //associate the connection with the command
        command.Connection = connection;

        //set the command text (stored procedure name or SQL statement)
        command.CommandText = commandText;

        //if we were provided a transaction, assign it.
        if (transaction != null)
        {
            command.Transaction = transaction;
        }

        //set the command type
        command.CommandType = commandType;

        //attach the command parameters if they are provided
        if (commandParameters != null)
        {
            AttachParameters(command, commandParameters);
        }

        return;
    }
    #endregion 私有实用方法&构造函数 End

    #region DataHelpers

    public string CheckNull(object obj)
    {
        return (string)obj;
    }

    public string CheckNull(DBNull obj)
    {
        return null;
    }

    #endregion

    /// <summary>
    /// 在开始事务后，必须要有一种方法，即使用方法。
    /// 若不使用，则是不使用，则是不使用
    /// 事务也在事务中，不会提交。连接总是打开的。
    /// </summary>
    public void BeginTranscation()
    {
        try
        {
            if (SqlTrans != null) return;
            if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
            SqlTrans = SqlConn.BeginTransaction();
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }

    public void EndTranscation()
    {
        try
        {
            if (SqlTrans == null) return;
            SqlTrans.Commit();
        }
        catch (System.Exception ex)
        {
            SqlTrans.Rollback();
            throw ex;
        }
        finally
        {
            SqlTrans = null;
            SqlConn.Close();
        }
    }

    public void Rollback()
    {
        if (SqlTrans == null) return;
        SqlTrans.Rollback();
        SqlTrans = null;
    }

    /// <summary>
    /// 根据SQL命令返回数据DataSet数据集，其中的表可直接作为dataGridView的数据源。
    /// </summary>
    /// <param name="SQL"></param>
    /// <param name="subtableName">在返回的数据集中所添加的表的名称</param>
    /// <returns></returns>
    public static DataSet SelectToDataSet(string connstring,string sql, string subtableName)
    {
        MySqlConnection Connection = new MySqlConnection(connstring);
        try
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand(sql, Connection);
            adapter.SelectCommand = command;
            DataSet Ds = new DataSet();
            Ds.Tables.Add(subtableName);
            adapter.Fill(Ds, subtableName);
            return Ds;
        }
        catch {
            //MySqlConnection.ClearPool(Connection);
            return null;
        }
        finally
        {
            Connection.Close();
            //Connection.Dispose();
        }
    }


    //insert delete update
    /// <summary>
    /// 执行一个SQL语句，返回受影响的行数
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="cmdType">类型</param>
    /// <param name="pms">参数</param>
    /// <returns>受影响的行数</returns>
    public static int ExecuteNonQuery(string connstring,string sql, CommandType cmdType = CommandType.Text, params MySqlParameter[] pms)
    {
        MySqlConnection Connection = new MySqlConnection(connstring);
        try
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            if (SqlTrans != null)
            {
                MySqlCommand cmd = new MySqlCommand(sql, SqlConn);
                cmd.CommandType = cmdType;
                cmd.Transaction = SqlTrans;
                cmd.CommandTimeout = 10;
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                return cmd.ExecuteNonQuery();
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand(sql, Connection);
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = 10;
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                return cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            MySqlConnection.ClearPool(Connection);
            throw new Exception("自定义：执行时遇到错误", ex);
        }
        finally
        {
            Connection.Close();
            //Connection.Dispose();
        }
    }

    /// <summary>
    /// 执行多个SQL语句。事务处理
    /// </summary>
    /// <param name="sqlStringArray"></param>
    public void ExecuteNonQueryArrayList(string connstring,ArrayList sqlStringArray)
    {
        MySqlConnection Connection = new MySqlConnection(connstring);
        MySqlCommand cmd = new MySqlCommand();
        if (Connection.State == ConnectionState.Closed) Connection.Open();
        cmd.Connection = Connection;
        try
        {
            SqlTrans = Connection.BeginTransaction();
            cmd.Transaction = SqlTrans;
            foreach (string i in sqlStringArray)
            {
                cmd.CommandText = i;
                cmd.ExecuteNonQuery();
            }

            SqlTrans.Commit();
        }
        catch (Exception ex)
        {
            SqlTrans.Rollback();
            //MySqlConnection.ClearPool(Connection);
            throw new Exception("自定义：执行时遇到错误", ex);
        }
        finally
        {
            SqlTrans = null;
            Connection.Close();
            //Connection.Dispose();
        }
    }

    /// <summary>
    /// 执行一个SQL语句，返回单个值
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="cmdType">类型</param>
    /// <param name="pms">参数</param>
    /// <returns>返回值</returns>
    public static string ExecuteScalar(string connstring,string sql, CommandType cmdType = CommandType.Text, params MySqlParameter[] pms)
    {
        MySqlConnection Connection = new MySqlConnection(connstring);
        try
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, Connection);
            cmd.CommandType = cmdType;
            cmd.CommandTimeout = 10;
            if (pms != null)
            {
                cmd.Parameters.AddRange(pms);
            }
            if (SqlTrans != null) cmd.Transaction = SqlTrans;
            object obj = cmd.ExecuteScalar();
            if (obj != null) return Convert.ToString(obj.ToString().Trim());
            else return "-1";
        }
        catch (Exception ex)
        {
            MySqlConnection.ClearPool(Connection);
            throw new Exception("自定义：执行时遇到错误", ex);
        }
        finally
        {
            Connection.Close();
            //Connection.Dispose();
        }
    }
    /// <summary>
    /// 执行一个SQL语句，返回DataReader
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="cmdType">类型</param>
    /// <param name="pms">参数</param>
    /// <returns>DataReader</returns>
    public static MySqlDataReader ExecuteReader(string connstring,string sql, CommandType cmdType = CommandType.Text, params MySqlParameter[] pms)
    {
        MySqlConnection Connection = new MySqlConnection(connstring);
        MySqlCommand cmd = new MySqlCommand(sql, Connection);

        cmd.CommandType = CommandType.Text;
        cmd.CommandTimeout = 10;
        try
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            if (pms != null)
            {
                cmd.Parameters.AddRange(pms);
            }
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch(Exception ex)
        {
            if (Connection.State != ConnectionState.Closed)
            {
                MySqlConnection.ClearPool(Connection);
                Connection.Close();
                //Connection.Dispose();
            }
            throw new Exception("自定义：执行时遇到错误", ex);
        }
    }

    /// <summary>
    /// 执行一个SQL语句，返回DataTable
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="cmdType">类型</param>
    /// <param name="pms">参数</param>
    /// <returns>DataTable</returns>
    public DataTable ExecuteDataTable(string connstring,string sql, CommandType cmdType,  params MySqlParameter[] pms)
    {
        #region 第二版。
        MySqlConnection Connection = new MySqlConnection(connstring);
        DataTable dt = new DataTable();
        try
        {
            //无事物，使用 Connection
            if (SqlTrans == null)
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(sql, Connection);
                adapter.SelectCommand.CommandType = cmdType;
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            else
            {
                //有事物，使用 SqlConn
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(sql, SqlConn);
                adapter.SelectCommand.Transaction = SqlTrans;
                adapter.SelectCommand.CommandType = cmdType;
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Connection.Close();
            //Connection.Dispose();
        }
        #endregion

        return dt;
    }
}