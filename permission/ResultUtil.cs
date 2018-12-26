/*************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    xy
** date：    2018/7/13
** desc：    数据库操作转换实现类
** Ver.:     V1.0.0
**************************************/




using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using permission.model;
using permission.common;

namespace permission.utils
{
    public class ResultUtil
    {
        #region 查询返回List<T>
        /// <summary>
        /// 查询返回List<T>
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="sqlStr">sql语句</param>
        /// <returns>List<T></returns>
        public static List<T> getResultList<T>(string sqlStr)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //执行数据库查询操作
            DataTable dt = MySqlHelper.ExecuteDataset(conn, sqlStr).Tables[0];
            //关闭释放连接资源
            conn.Close();

            //DataTable转List对应实体类
            return dt.ToList<T>();
        }
        #endregion

        #region 数据库参数式查询返回List<T>
        /// <summary>
        /// 数据库参数式查询返回List<T>
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="sqlStr">sql语句</param>
        /// <returns>List<T></returns>
        public static List<T> getResultList<T>(string sqlStr, MySqlParameter[] commandParameters)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //执行数据库查询操作
            DataTable dt = MySqlHelper.ExecuteDataset(conn, sqlStr, commandParameters).Tables[0];
            //关闭释放连接资源
            conn.Close();

            //DataTable转List对应实体类
            return dt.ToList<T>();
        }
        #endregion

        #region 查询返回字符集
        /// <summary>
        /// 查询返回字符集
        /// </summary>
        /// <param name="sqlStr">sql字符</param>
        /// <returns>string</returns>
        public static string getResultString(string sqlStr)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //执行数据库查询操作
            string result = MySqlHelper.ExecuteScalar(conn, sqlStr).ToString();
            //关闭释放连接资源
            conn.Close();

            //返回字符集
            return result;
        }
        #endregion

        #region 数据库参数式查询返回字符集
        /// <summary>
        /// 数据库参数式查询返回字符集
        /// </summary>
        /// <param name="sqlStr">sql字符</param>
        /// <returns>string</returns>
        public static string getResultString(string sqlStr, MySqlParameter[] commandParameters)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //执行数据库查询操作
            string result = MySqlHelper.ExecuteScalar(conn, sqlStr, commandParameters).ToString();
            //关闭释放连接资源
            conn.Close();

            //返回字符集
            return result;
        }
        #endregion

        #region 新增、修改、删除操作
        /// <summary>
        /// 新增、修改、删除操作
        /// </summary>
        /// <param name="sqlStr">sql字符</param>
        /// <returns>无</returns>
        public static void insOrUpdOrDel(string sqlStr)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //启动一个事务
            MySqlTransaction myTrans = conn.BeginTransaction();
            //事务命令
            MySqlCommand cmd = conn.CreateCommand();
            //事务命令
            cmd.Transaction = myTrans;
            try
            {
                //执行数据库新增、修改、删除操作
                MySqlHelper.ExecuteNonQuery(conn, sqlStr);
                //事务提交
                myTrans.Commit();
                //关闭释放连接资源
                conn.Close();
            }
            catch (Exception ex)
            {
                //异常回滚
                myTrans.Rollback();
                //关闭释放连接资源
                conn.Close();
                //异常日志
                LogHelper.WriteError("新增、修改、删除操作异常回滚：" + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 数据库参数式新增、修改、删除操作
        /// <summary>
        /// 数据库参数式新增、修改、删除操作
        /// </summary>
        /// <param name="sqlStr">sql字符</param>
        /// <returns>无</returns>
        public static int insOrUpdOrDel(string sqlStr, MySqlParameter[] commandParameters)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //启动一个事务
            MySqlTransaction myTrans = conn.BeginTransaction();
            //事务命令
            MySqlCommand cmd = conn.CreateCommand();
            //事务命令
            cmd.Transaction = myTrans;
            int result = 0;
            try
            {
                //执行数据库新增、修改、删除操作
                result = MySqlHelper.ExecuteNonQuery(conn, sqlStr, commandParameters);
                //事务提交
                myTrans.Commit();
                //关闭释放连接资源
                conn.Close();
                //
                return result;
            }
            catch (Exception ex)
            {
                //异常回滚
                myTrans.Rollback();
                //关闭释放连接资源
                conn.Close();
                //异常日志
                LogHelper.WriteError("新增、修改、删除操作异常回滚：" + ex.Message);
                return 0;
            }
        }
        #endregion

        #region 查询返回DataSet
        /// <summary>
        /// 查询返回DataSet
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <returns>getDataSet<T></returns>
        public static DataSet getDataSet(string sqlStr)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //执行数据库查询操作
            DataSet ds = MySqlHelper.ExecuteDataset(conn, sqlStr);
            //关闭释放连接资源
            conn.Close();

            //返回DataTable
            return ds;
        }
        #endregion

        #region 查询返回DataTable
        /// <summary>
        /// 查询返回DataTable
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <returns>DataTable<T></returns>
        public static DataTable getDataTable(string sqlStr)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //执行数据库查询操作
            DataTable dt = MySqlHelper.ExecuteDataset(conn, sqlStr).Tables[0];
            //关闭释放连接资源
            conn.Close();

            //返回DataTable
            return dt;
        }
        #endregion

        #region 数据库参数式查询返回DataTable
        /// <summary>
        /// 数据库参数式查询返回DataTable
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <returns>DataTable<T></returns>
        public static DataTable getDataTable(string sqlStr, MySqlParameter[] commandParameters)
        {
            //创建数据库连接
            MySqlConnection conn = new MySqlConnect().getConnection(new MySqlConnect().connection());
            //执行数据库查询操作
            DataTable dt = MySqlHelper.ExecuteDataset(conn, sqlStr, commandParameters).Tables[0];
            //关闭释放连接资源
            conn.Close();

            //返回DataTable
            return dt;
        }
        #endregion

        #region 返回规范结果集
        /// <summary>
        /// 返回规范结果集
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="message">描述</param>
        /// <param name="obj">结果</param>
        /// <returns>string</returns>
        public static string getStandardResult(int status, string message, object obj)
        {
            //new对象
            Result result = new Result();
            //状态
            result.status = status.ToString();
            //描述
            result.message = message;
            //结果
            result.result = obj;

            return JsonConvert.SerializeObject(result);
        }
        #endregion
    }
}