/*************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    xy
** date：    2018/7/13
** desc：    数据库连接封装类
** Ver.:     V1.0.0
**************************************/

using MySql.Data.MySqlClient;
using System.Configuration;

namespace permission.common
{
    /// <summary>
    /// mysql连接
    /// </summary>
    public class MySqlConnect
    {
        #region 数据库连接
        /// <summary>
        /// 获取数据库连接信息
        /// </summary>
        /// <returns>数据库连接</returns>
        public string connection()
        {
            return ConfigurationManager.ConnectionStrings["MysqlConStr"].ConnectionString;
        }
        #endregion

        #region 一个有效的数据库连接对象
        /// <summary>
        /// 一个有效的数据库连接对象 
        /// </summary>
        /// <param name="conStr"></param>
        /// <returns>MySqlConnection</returns>
        public MySqlConnection getConnection(string conStr = null)
        {
            MySqlConnection Connection = new MySqlConnection(conStr == null ? new MySqlConnect().connection() : conStr);
            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
            return Connection;
        }
        #endregion
    }
}