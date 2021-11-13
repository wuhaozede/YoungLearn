using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace YoungLearn.Utility
{
    class SQLclass
    {
        public string sqlname;

        public DataTable Data = new DataTable();

        /// <summary>
        /// 初始化SQL类
        /// </summary>
        /// <param name="sqlname_">数据库文件地址</param>
        public SQLclass(string sqlname_)
        {
            sqlname = sqlname_;
        }

        /// <summary>
        /// 获得DataTable列名
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> GetColumnsNameByDataTable(DataTable dt)
        {
            List<string> strColumns = new List<string> { };

            if (dt.Columns.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strColumns.Add(dt.Columns[i].ColumnName);
                }
            }
            return strColumns;
        }

        /// <summary>
        /// 获得DataTable列属性
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> GetColumnsTypeByDataTable(DataTable dt)
        {
            List<string> strColumns = new List<string> { };

            if (dt.Columns.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    switch (dt.Columns[i].DataType.Name)
                    {
                        case nameof(String):
                            strColumns.Add("TEXT");
                            break;
                        case nameof(Int32):
                            strColumns.Add("INT");
                            break;
                        default:
                            strColumns.Add("TEXT");
                            break;
                    }
                    
                }
            }
            return strColumns;
        }

        /// <summary>
        /// 对比班级原表与完成人数
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetNowData(string user_name, string table_name)
        {
            SQLiteConnection conn = new SQLiteConnection("data source = " + sqlname);
            conn.Open();
            Dictionary<string, int> dic = new Dictionary<string, int> { };
            try
            {
                string query = "select count(" + user_name + ".名称) from " + user_name + ", " + table_name + " where " + user_name + ".名称 = " + table_name + ".姓名";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(Data);
                int row = int.Parse(Data.Rows[0][0].ToString());
                dic.Add("完成", row);

                string query_ = "select count(" + user_name + ".名称) from " + user_name;
                SQLiteCommand cmd_ = new SQLiteCommand(query_, conn);
                SQLiteDataAdapter da_ = new SQLiteDataAdapter(cmd_);
                Data.Clear();
                da_.Fill(Data);
                dic.Add("未完成", int.Parse(Data.Rows[0][0].ToString()) - row);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                Data.Clear();
            }
            return dic;
        }

        /// <summary>
        /// DataTable导入数据库
        /// </summary>
        /// <param name="table_name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Insert_table(string table_name ,DataTable data)
        {
            bool b = false;
            List<string> list_name = GetColumnsNameByDataTable(data);
            List<string> list_type = GetColumnsTypeByDataTable(data);
            b = Create_table(table_name, list_name, list_type);
            
            //特殊代码
            //主要是将datatable转为list<list>
            List<List<string>> list = data.AsEnumerable().Select(t => t.ItemArray.ToList().Select(c => c.ToString()).ToList()).ToList();


            b = Insert_table(table_name, list_name, list);
            return b;
        }

        /// <summary>
        /// 新建数据表
        /// </summary>
        /// <param name="table_name">数据表名称</param>
        /// <param name="list_name">数据列名称</param>
        /// <param name="list_type">数据列类型</param>
        /// <returns>bool</returns>
        public bool Create_table(string table_name, List<string> list_name, List<string> list_type)
        {
            SQLiteConnection conn = new SQLiteConnection("data source = " + sqlname);
            conn.Open();
            bool check = false;
            try
            {
                string queryString = "CREATE TABLE IF NOT EXISTS " + table_name + "( " + list_name[0] + " " + list_type[0];

                for (int i = 1; i < list_name.Count; i++)
                {
                    queryString += ", " + list_name[i] + " " + list_type[i];
                }
                queryString += "  ) ";
                SQLiteCommand dbCommand = conn.CreateCommand();
                dbCommand.CommandText = queryString;
                SQLiteDataReader dataReader = dbCommand.ExecuteReader();
                check = true;
            }
            catch (Exception ex)
            {
                check = false;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return check;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="list_name">数据列名称</param>
        /// <param name="list">添加的数据</param>
        /// <returns>bool</returns>
        public bool Insert_table(string table_name, List<string> list_name, List<List<string>> list)
        {
            SQLiteConnection conn = new SQLiteConnection("data source = " + sqlname);
            SQLiteCommand cmd = conn.CreateCommand();
            conn.Open();
            SQLiteTransaction tran = conn.BeginTransaction();
            bool check = false;
            string listname = "";
            foreach (string item in list_name)
            {
                listname += item + ",";
            }
            listname = listname.Substring(0, listname.Length - 1);
            try
            {
                foreach (List<string> item in list)
                {
                    string query = "insert into " + table_name + " (" + listname + ") values (";
                    foreach (string data in item)
                    {
                        query += "'" + data + "',";
                    }
                    query = query.Substring(0, query.Length - 1);
                    query += ")";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
                check = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                check = false;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return check;
        }

        /// <summary>
        /// 查询整个表
        /// </summary>
        /// <param name="table_name">表名称</param>
        /// <returns>bool</returns>
        public bool Query_Alltable(string table_name)
        {
            SQLiteConnection conn = new SQLiteConnection("data source = " + sqlname);
            conn.Open();
            bool check = false;
            try
            {
                string query = "select * from " + table_name;
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(Data);

                check = true;
            }
            catch (Exception ex)
            {
                check = false;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return check;
        }

        /// <summary>
        /// 指定查询语句
        /// </summary>
        /// <param name="commend">查询命令</param>
        /// <returns></returns>
        public bool Query_Command(string commend)
        {
            SQLiteConnection conn = new SQLiteConnection("data source = " + sqlname);
            conn.Open();
            bool check = false;
            try
            {
                ;
                SQLiteCommand cmd = new SQLiteCommand(commend, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(Data);

                check = true;
            }
            catch (Exception ex)
            {
                check = false;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return check;
        }

        public bool ExistTable(string table_name)
        {
            //if (System.IO.File.Exists(path) == false)
            //    SQLiteConnection.CreateFile(path);

            using (SQLiteConnection con = new SQLiteConnection(string.Format("Data Source={0};", sqlname)))
            {
                con.Open();
                string count = "0";
                //开启事务
                using (SQLiteTransaction tr = con.BeginTransaction())
                {
                    using (SQLiteCommand cmd = con.CreateCommand())
                    {
                        string existSql = String.Format("select count(*)  from sqlite_master where type='table' and name = '{0}'", table_name);

                        cmd.Transaction = tr;
                        cmd.CommandText = existSql;
                        //使用result = cmd.ExecuteNonQuery();这句判断返回值的方法不正确，不论是否存在返回值都为-1

                        SQLiteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            count = reader[0].ToString();
                        }
                    }
                    //提交事务
                    tr.Commit();
                }
                con.Close();
                return count != "0";
            }
        }

        public void ExitTable(string table_name)
        {
            SQLiteConnection conn = new SQLiteConnection("data source = " + sqlname);
            conn.Open();
            try
            {
                string query = "DROP TABLE " + table_name;
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool Count_Columns(string table_name, string column_name)
        {
            SQLiteConnection conn = new SQLiteConnection("data source = " + sqlname);
            conn.Open();
            bool check = false;
            try
            {
                string query = "SELECT " + column_name + ",COUNT(*) FROM " + table_name + " GROUP BY " + column_name;
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(Data);

                check = true;
            }
            catch (Exception ex)
            {
                check = false;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return check;
        }

        /// <summary>
        /// 根据行输出
        /// </summary>
        /// <returns>完整字符串</returns>
        public string DataRowsToString()
        {
            string a = "";
            foreach (DataRow items in Data.Rows)
            {
                for (int i = 0; i < Data.Columns.Count; i++)
                {
                    a += items[i].ToString();
                }
            }
            return a;
        }

        /// <summary>
        /// 根据行输出，在行尾增加分隔符
        /// </summary>
        /// <param name="row_insert">分隔符</param>
        /// <returns>完整字符串</returns>
        public string DataRowsToString(string row_insert)
        {
            string a = "";
            foreach (DataRow items in Data.Rows)
            {
                for (int i = 0; i < Data.Columns.Count; i++)
                {
                    a += items[i].ToString();
                }
                a += row_insert;
            }
            return a;
        }

        /// <summary>
        /// 根据行输出，每列增加分隔符，每行增加分隔符
        /// </summary>
        /// <param name="row_insert">行分隔符</param>
        /// <param name="columns_insert">列分隔符</param>
        /// <returns>完整字符串</returns>
        public string DataRowsToString(string row_insert, string columns_insert)
        {
            string a = "";
            foreach (DataRow items in Data.Rows)
            {
                for (int i = 0; i < Data.Columns.Count; i++)
                {
                    a += items[i].ToString() + columns_insert;
                }
                a += row_insert;
            }
            return a;
        }

        /// <summary>
        /// 根据行输出，输出字典
        /// </summary>
        /// <returns>字典</returns>
        public Dictionary<string, int> DataToDict()
        {
            Dictionary<string, int> a = new Dictionary<string, int> { };
            if (Data.Columns.Count > 2)
            {
                a.Add("Error", 0);
                return a;
            }
            else
            {
                foreach (DataRow items in Data.Rows)
                {
                    a.Add(items[0].ToString(), int.Parse(items[1].ToString()));
                }
                return a;
            }
        }
    }
}
