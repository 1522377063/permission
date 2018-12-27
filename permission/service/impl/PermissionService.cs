using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private string strSql;
        private MySqlParameter[] mySqlParameters;
        public string GetUserPermissionList(int page, int rows)
        {
            int total = int.Parse(ResultUtil.getResultString("SELECT count(1) FROM `user`"));
            //获取每一个用户
            strSql = "SELECT * FROM `user` LIMIT @begin,@rows";
            mySqlParameters = new MySqlParameter[]
            {
                new MySqlParameter("@begin",MySqlDbType.Int32) {Value=((page-1)*Convert.ToInt32(rows)) },
                new MySqlParameter("@rows",MySqlDbType.Int32) {Value=Convert.ToInt32(rows) }
            };
            List<User> userList = ResultUtil.getResultList<User>(strSql, mySqlParameters);
            JObject jo1 = new JObject();
            jo1.Add("total", total);
            if (userList != null)
            {
                JArray ja = new JArray();

                foreach (User user in userList)
                {

                    strSql = @"SELECT u.hy_username as username,p.p_name as pname,up.p_value as pvalue FROM user_permission up RIGHT JOIN (SELECT * FROM `user` WHERE `user`.hy_userid=@hy_userid) u ON up.uid=u.hy_userid RIGHT JOIN permission p ON up.pid=p.p_id";
                    mySqlParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@hy_userid",MySqlDbType.VarChar,50) {Value = user.hy_userid},
                    };
                    //获取每一个用户对应的权限
                    List<PartPermission> partPermissionList = ResultUtil.getResultList<PartPermission>(strSql, mySqlParameters);
                    if (partPermissionList != null)
                    {
                        JObject jo = new JObject();
                        jo.Add("username", user.hy_username);
                        foreach (PartPermission partPermission in partPermissionList)
                        {
                            jo.Add(partPermission.pname, partPermission.pvalue);
                        }
                        ja.Add(jo);
                    }

                }

                jo1.Add("rows", ja);

            }

            return jo1.ToString();
        }

        public string EditUserPermission(string json)
        {
            JObject jo = JsonConvert.DeserializeObject(json) as JObject;
            string username = jo["username"].ToString();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("cunzhuang", (int)jo["cunzhuang"]);
            dic.Add("cesuo", (int)jo["cesuo"]);
            dic.Add("minsu", (int)jo["minsu"]);
            dic.Add("jingqu", (int)jo["jingqu"]);
            dic.Add("addtuceng", (int)jo["addtuceng"]);
            dic.Add("shouhuiditu", (int)jo["shouhuiditu"]);
            dic.Add("addimage", (int)jo["addimage"]);
            dic.Add("ninghaimaoyucun", (int)jo["ninghaimaoyucun"]);
            dic.Add("ninghai", (int)jo["ninghai"]);
            dic.Add("addmodel", (int)jo["addmodel"]);
            dic.Add("addpanorama", (int)jo["addpanorama"]);
            strSql = @"SELECT u.hy_username as username,p.p_name as pname,up.p_value as pvalue FROM user_permission up RIGHT JOIN (SELECT * FROM `user` WHERE `user`.hy_username=@hy_username) u ON up.uid=u.hy_userid RIGHT JOIN permission p ON up.pid=p.p_id";
            mySqlParameters = new MySqlParameter[]
            {
                new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value = username},
            };
            //获取每一个用户对应的权限
            List<PartPermission> partPermissionList = ResultUtil.getResultList<PartPermission>(strSql, mySqlParameters);
            foreach (PartPermission pp in partPermissionList)
            {
                if (pp.pvalue == null && pp.username == null)
                {
                    strSql =
                        @"SELECT COUNT(1) FROM user_permission up WHERE up.uid=(SELECT u.hy_userid FROM `user` u WHERE u.hy_username=@hy_username) AND up.pid=(SELECT p.p_id FROM permission p WHERE p.p_name=@p_name)";
                    mySqlParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value=username },
                        new MySqlParameter("@p_name",MySqlDbType.VarChar,50) {Value=pp.pname }
                    };
                    int num = int.Parse(ResultUtil.getResultString(strSql, mySqlParameters));
                    if (num > 0)
                    {
                        continue;
                    }
                    strSql =
                        @"INSERT INTO user_permission (uid,pid,p_value) VALUES((SELECT u.hy_userid FROM `user` u WHERE u.hy_username=@hy_username),(SELECT p.p_id FROM permission p WHERE p.p_name=@p_name),@p_value)";
                    mySqlParameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value=username },
                        new MySqlParameter("@p_name",MySqlDbType.VarChar,50) {Value=pp.pname },
                        new MySqlParameter("@p_value",MySqlDbType.Int32) {Value=pp.pvalue }
                    };
                    ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);
                }
            }
            foreach (KeyValuePair<string, int> kv in dic)
            {
                strSql = "UPDATE user_permission up SET up.p_value=@p_value WHERE up.uid=(SELECT `user`.hy_userid FROM `user` WHERE `user`.hy_username=@hy_username) AND up.pid=(SELECT permission.p_id FROM permission WHERE permission.p_name=@p_name)";
                mySqlParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@p_value",MySqlDbType.Int32) {Value=kv.Value },
                    new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value=username },
                    new MySqlParameter("@p_name",MySqlDbType.VarChar,50) {Value=kv.Key }
                };
                int row = ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);

            }
            return ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Update), null);
        }

        public string DeleteUserPermission(string username)
        {
            strSql = "DELETE FROM user_permission WHERE user_permission.uid=(SELECT `user`.hy_userid FROM `user` WHERE `user`.hy_username=@hy_username)";
            mySqlParameters = new MySqlParameter[]
            {
                new MySqlParameter("@hy_username",MySqlDbType.VarChar,50) {Value=username }
            };
            ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);
            return ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Delete), null);
        }
    }
}