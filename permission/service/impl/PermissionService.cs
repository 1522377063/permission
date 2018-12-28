using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using permission.common;
using permission.enums;
using permission.model;
using permission.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace permission.service.impl
{
    public class PermissionService:IPermissionService
    {
        //sql语句
        private string strSql;
        //sql语句对应的参数的参数值
        private MySqlParameter[] mySqlParameters;
        #region 获取所有的用户权限
        /// <summary>
        /// 获取所有的用户权限
        /// </summary>
        /// <param name="page">分页请求页</param>
        /// <param name="rows">每页大小</param>
        /// <returns>{"total":2,"rows":[{"username":"电子合同测试","cunzhuang":null,"cesuo":null,"minsu":null,"jingqu":null,"addtuceng":null,"shouhuiditu":null,"addimage":null,"ninghaimaoyucun":null,"ninghai":null,"addmodel":null,"addpanorama":null},{"username":"测试账号","cunzhuang":0,"cesuo":0,"minsu":0,"jingqu":0,"addtuceng":0,"shouhuiditu":0,"addimage":1,"ninghaimaoyucun":1,"ninghai":1,"addmodel":1,"addpanorama":1}]}</returns>
        public string GetUserPermissionList(int page, int rows)
        {
            try
            {
                //获取用户表除用户名为空的总数
                int total = int.Parse(ResultUtil.getResultString("SELECT count(1) FROM `user` WHERE hy_username!=''"));
               
                strSql = "SELECT * FROM `user` WHERE hy_username!='' LIMIT @begin,@rows";
                mySqlParameters = new MySqlParameter[]
                {
                    //开始位置
                    new MySqlParameter("@begin",MySqlDbType.Int32) {Value=((page-1)*Convert.ToInt32(rows)) },
                    //请求行数
                    new MySqlParameter("@rows",MySqlDbType.Int32) {Value=Convert.ToInt32(rows) }
                };
                //获取每一个用户,除username为null的用户
                List<User> userList = ResultUtil.getResultList<User>(strSql, mySqlParameters);
                //创建一个jsonObject
                JObject jo1 = new JObject();
                //添加total总数数值
                jo1.Add("total", total);
                //如果用户表集合不为空
                if (userList != null)
                {
                    //创建一个jsonArray
                    JArray ja = new JArray();
                    //遍历用户集合
                    foreach (User user in userList)
                    {
                        //sql语句，查询对应用户名所对应的用户名，权限名，权限值
                        strSql = @"SELECT u.hy_username as username,p.p_name as pname,up.p_value as pvalue FROM user_permission up RIGHT JOIN (SELECT * FROM `user` WHERE `user`.hy_userid=@hy_userid) u ON up.uid=u.hy_userid RIGHT JOIN permission p ON up.pid=p.p_id";
                        //sql语句对应的参数
                        mySqlParameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@hy_userid",MySqlDbType.VarChar,50) {Value = user.hy_userid},
                        };
                        //获取每一个用户对应的权限
                        List<PartPermission> partPermissionList = ResultUtil.getResultList<PartPermission>(strSql, mySqlParameters);
                        //判断PartPermission集合是否为空
                        if (partPermissionList != null)
                        {
                            //创建一个jsonObject
                            JObject jo = new JObject();
                            //添加用户名
                            jo.Add("username", user.hy_username);
                            foreach (PartPermission partPermission in partPermissionList)
                            {
                                //添加每一个用户对应的权限名的权限值
                                jo.Add(partPermission.pname, partPermission.pvalue);
                            }
                            ja.Add(jo);
                        }

                    }
                    //添加rows属性
                    jo1.Add("rows", ja);

                }
                //返回json 
                return jo1.ToString();
            }
            catch(Exception ex)
            {
                LogHelper.WriteError("查询失败：" + ex.Message);
                return "{ \"total\":0,\"rows\":[]}";
            }

        }
        #endregion

        #region 修改对应用户权限
        /// <summary>
        /// 修改对应用户权限
        /// </summary>
        /// <param name="json">要修改的用户权限比如：{"username":"张三","cunzhuang":0,"cesuo":0,"minsu":1,"jingqu":1,"addtuceng":1,"shouhuiditu":0,"addimage":1,"ninghaimaoyucun":0,"ninghai":1,"addmodel":1,"addpanorama":1}</param>
        /// <returns></returns>
        public string EditUserPermission(string json)
        {
            try
            {
                //解析json数据为jsonObject
                JObject jo = JsonConvert.DeserializeObject(json) as JObject;
                //获取用户名
                string username = jo["username"].ToString();
                //创建一个dictionary
                Dictionary<string, int?> dic = new Dictionary<string, int?>();
                //添加一些列值
                dic.Add("cunzhuang", (int?)jo["cunzhuang"]);
                dic.Add("cesuo", (int?)jo["cesuo"]);
                dic.Add("minsu", (int?)jo["minsu"]);
                dic.Add("jingqu", (int?)jo["jingqu"]);
                dic.Add("addtuceng", (int?)jo["addtuceng"]);
                dic.Add("shouhuiditu", (int?)jo["shouhuiditu"]);
                dic.Add("addimage", (int?)jo["addimage"]);
                dic.Add("ninghaimaoyucun", (int?)jo["ninghaimaoyucun"]);
                dic.Add("ninghai", (int?)jo["ninghai"]);
                dic.Add("addmodel", (int?)jo["addmodel"]);
                dic.Add("addpanorama", (int?)jo["addpanorama"]);
                strSql = @"SELECT u.hy_username as username,p.p_name as pname,up.p_value as pvalue FROM user_permission up RIGHT JOIN (SELECT * FROM `user` WHERE `user`.hy_username=@hy_username) u ON up.uid=u.hy_userid RIGHT JOIN permission p ON up.pid=p.p_id";
                mySqlParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value = username},
                };
                //获取每一个用户对应的权限
                List<PartPermission> partPermissionList = ResultUtil.getResultList<PartPermission>(strSql, mySqlParameters);
                //遍历partPermissionList
                foreach (PartPermission pp in partPermissionList)
                {
                    //如果权限值是空的则表示user_permission里面还没有对应的权限
                    if (pp.pvalue == null)
                    {
                        //添加用户权限值是null的权限值为0
                        strSql =
                            @"INSERT INTO user_permission (uid,pid,p_value) VALUES((SELECT u.hy_userid FROM `user` u WHERE u.hy_username=@hy_username),(SELECT p.p_id FROM permission p WHERE p.p_name=@p_name),@p_value)";
                        mySqlParameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value=username },
                            new MySqlParameter("@p_name",MySqlDbType.VarChar,50) {Value=pp.pname },
                            new MySqlParameter("@p_value",MySqlDbType.Int32) {Value=(pp.pvalue==null?0:pp.pvalue) }
                        };
                        //执行插入对应用户名的对应权限
                        ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);
                    }
                }
                //遍历dictionary
                foreach (KeyValuePair<string, int?> kv in dic)
                {
                    //修改用户权限值
                    strSql = "UPDATE user_permission up SET up.p_value=@p_value WHERE up.uid=(SELECT `user`.hy_userid FROM `user` WHERE `user`.hy_username=@hy_username) AND up.pid=(SELECT permission.p_id FROM permission WHERE permission.p_name=@p_name)";
                    mySqlParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@p_value",MySqlDbType.Int32) {Value=(kv.Value==null?0:kv.Value) },
                        new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value=username },
                        new MySqlParameter("@p_name",MySqlDbType.VarChar,50) {Value=kv.Key }
                    };
                    //修改对应用户名的对应权限
                    int row = ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);

                }
                //返回修改成功对应json
                return ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Update), null);
            }
            catch(Exception ex)
            {
                //往日志中写入错误
                LogHelper.WriteError("新增、修改、删除操作异常回滚：" + ex.Message);
                //返回修改失败对应的json
                return ResultUtil.getStandardResult((int)Status.Error, EnumUtil.getMessageStr((int)Message.UpdateFailure), null);
            }
            
        }
        #endregion

        #region 删除用户对应权限
        /// <summary>
        /// 删除用户对应权限
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>删除成功或失败的标准json</returns>
        public string DeleteUserPermission(string username)
        {
            try
            {
                strSql = "DELETE FROM user_permission WHERE user_permission.uid=(SELECT `user`.hy_userid FROM `user` WHERE `user`.hy_username=@hy_username)";
                mySqlParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value=username }
                };
                //根据用户名删除对应的所有的权限
                ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);
                //返回删除成功的json
                return ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Delete), null);
            }
            catch(Exception ex)
            {
                //往日志中写入错误
                LogHelper.WriteError("新增、修改、删除操作异常回滚：" + ex.Message);
                //返回修改失败对应的json
                return ResultUtil.getStandardResult((int)Status.Error, EnumUtil.getMessageStr((int)Message.DeleteFailure), null);
            }
            
        }
        #endregion
    }
}