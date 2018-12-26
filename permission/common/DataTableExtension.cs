/*************************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    xy
** date：    2018/7/13
** desc：    DataTable、DataRow映射实体类集合/json
** Ver.:     V1.0.0
**************************************************/

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace permission.common
{
    /// <summary>
    /// DataTable扩展
    /// </summary>
    public static class DataTableExtension
    {
        #region DataTable转List
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <param name="obj">扩展对象</param>
        public static List<T> ToList<T>(this DataTable tb)
        {
            if (tb == null || tb.Rows.Count == 0) return null;
            //获取对象所有属性
            Type t = typeof(T);
            List<T> ListModel = new List<T>();

            PropertyInfo[] propertyInfo = t.GetProperties();
            if (propertyInfo.Length > 0)
            {
                #region 全部是属性
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    //创建泛型对象
                    T model = Activator.CreateInstance<T>();
                    for (int j = 0; j < tb.Columns.Count; j++)
                    {
                        foreach (PropertyInfo info in propertyInfo)
                        {
                            //属性名称和列名相同时赋值
                            if (tb.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                            {
                                if (tb.Rows[i][j] != DBNull.Value)
                                {
                                    if (tb.Columns[j].DataType == typeof(System.Guid))
                                    {
                                        info.SetValue(model, Convert.ToString(tb.Rows[i][j]), null);
                                    }
                                    else if (tb.Columns[j].DataType == typeof(System.Byte))
                                    {
                                        info.SetValue(model, Convert.ToInt32(tb.Rows[i][j]), null);
                                    }
                                    else if (tb.Columns[j].DataType == typeof(System.Double) || tb.Columns[j].DataType == typeof(System.Single))
                                    {
                                        info.SetValue(model, Convert.ToDecimal(tb.Rows[i][j]), null);
                                    }
                                    else
                                    {
                                        info.SetValue(model, tb.Rows[i][j], null);
                                    }
                                }
                                else
                                {
                                    info.SetValue(model, null, null);
                                }
                                break;
                            }
                        }
                    }
                    ListModel.Add(model);
                }
                #endregion
            }
            else
            {
                #region 全部是字段
                FieldInfo[] fieldinfo = t.GetFields();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    //创建泛型对象
                    T model = Activator.CreateInstance<T>();
                    for (int j = 0; j < tb.Columns.Count; j++)
                    {
                        foreach (FieldInfo info in fieldinfo)
                        {
                            //属性名称和列名相同时赋值
                            if (tb.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                            {
                                if (tb.Rows[i][j] != DBNull.Value)
                                {
                                    if (tb.Columns[j].DataType == typeof(System.Guid))
                                    {
                                        info.SetValue(model, Convert.ToString(tb.Rows[i][j]));
                                    }
                                    else if (tb.Columns[j].DataType == typeof(System.Byte))
                                    {
                                        info.SetValue(model, Convert.ToInt32(tb.Rows[i][j]));
                                    }
                                    else if (tb.Columns[j].DataType == typeof(System.Double) || tb.Columns[j].DataType == typeof(System.Single))
                                    {
                                        info.SetValue(model, Convert.ToDecimal(tb.Rows[i][j]));
                                    }
                                    else
                                    {
                                        info.SetValue(model, tb.Rows[i][j]);
                                    }
                                }
                                else
                                {
                                    info.SetValue(model, null);
                                }
                                break;
                            }
                        }
                    }
                    ListModel.Add(model);
                }
                #endregion
            }
            return ListModel;
        }
        #endregion

        #region 转换DataRow为Model
        public static T ToModel<T>(this DataRow row)
        {
            //获取对象所有属性
            DataTable tb = row.Table;
            Type t = typeof(T);
            T model = Activator.CreateInstance<T>();
            PropertyInfo[] propertyInfo = t.GetProperties();
            if (propertyInfo.Length > 0)
            {
                for (int j = 0; j < tb.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值
                        if (tb.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (row[j] != DBNull.Value)
                            {
                                if (tb.Columns[j].DataType == typeof(System.Guid))
                                {
                                    info.SetValue(model, Convert.ToString(row[j]), null);
                                }
                                else if (tb.Columns[j].DataType == typeof(System.Byte))
                                {
                                    info.SetValue(model, Convert.ToInt32(row[j]), null);
                                }
                                else
                                {
                                    info.SetValue(model, row[j], null);
                                }
                            }
                            else
                            {
                                info.SetValue(model, null, null);
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                FieldInfo[] fieldinfo = t.GetFields();
                for (int j = 0; j < tb.Columns.Count; j++)
                {
                    foreach (FieldInfo info in fieldinfo)
                    {
                        //属性名称和列名相同时赋值
                        if (tb.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (row[j] != DBNull.Value)
                            {
                                if (tb.Columns[j].DataType == typeof(System.Guid))
                                {
                                    info.SetValue(model, Convert.ToString(row[j]));
                                }
                                else if (tb.Columns[j].DataType == typeof(System.Byte))
                                {
                                    info.SetValue(model, Convert.ToInt32(row[j]));
                                }
                                else
                                {
                                    info.SetValue(model, row[j]);
                                }
                            }
                            else
                            {
                                info.SetValue(model, null);
                            }
                            break;
                        }
                    }
                }
            }
            return model;
        }
        #endregion

        #region DataTable转Json
        public static string ToJson(this DataTable tb)
        {
            if (tb.Rows.Count == 0) return "";
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long DatetimeMinTimeTicks = time.Ticks;
            string s = "[";
            foreach (DataRow row in tb.Rows)
            {
                string a = "{";
                foreach (DataColumn col in tb.Columns)
                {
                    if (col.ExtendedProperties.Contains("NoJson") == true) continue;
                    if (row.IsNull(col.ColumnName) == true)
                    {
                        a += "\"" + col.ColumnName + "\":null,";
                        continue;
                    }
                    if (col.DataType == typeof(string) || col.DataType == typeof(System.Guid))
                    {
                        string vvv = row[col.ColumnName].ToString();
                        vvv = vvv.Replace("\r\n", "\\r\\n").Replace("\n", "\\n");
                        a += "\"" + col.ColumnName + "\":\"" + vvv + "\",";
                        continue;
                    }
                    if (col.DataType == typeof(DateTime))
                    {
                        DateTime datetime = Convert.ToDateTime(row[col.ColumnName]);
                        a += "\"" + col.ColumnName + "\":\"\\/Date(" + (long)((datetime.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 0x2710) + ")\\/\",";
                        continue;
                    }
                    if (col.DataType == typeof(bool))
                    {
                        a += "\"" + col.ColumnName + "\":" + row[col.ColumnName].ToString().ToLower() + ",";
                        continue;
                    }
                    if (col.DataType == typeof(Int64))
                    {
                        a += "\"" + col.ColumnName + "\":\"" + row[col.ColumnName] + "\",";
                        continue;
                    }
                    if (col.DataType == typeof(decimal))
                    {

                        a += "\"" + col.ColumnName + "\":" + Convert.ToDecimal(row[col.ColumnName]).ToString("0.#########") + ",";
                        continue;
                    }
                    if (col.DataType == typeof(Int32) || col.DataType == typeof(Int16) || col.DataType == typeof(float) || col.DataType == typeof(short))
                    {
                        a += "\"" + col.ColumnName + "\":" + row[col.ColumnName] + ",";
                        continue;
                    }
                }
                a = a.Remove(a.Length - 1);
                a += "},";
                s += a;
            }
            s = s.Remove(s.Length - 1);
            s += "]";
            return s;
        }
        #endregion

        #region 转换DataRow为Json
        public static string ToJson(this DataRow row)
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long DatetimeMinTimeTicks = time.Ticks;
            string a = "{";
            foreach (DataColumn col in row.Table.Columns)
            {
                if (col.ExtendedProperties.Contains("NoJson") == true) continue;
                if (row.IsNull(col.ColumnName) == true)
                {
                    a += "\"" + col.ColumnName + "\":null,";
                    continue;
                }
                if (col.DataType == typeof(string) || col.DataType == typeof(System.Guid))
                {
                    string vvv = row[col.ColumnName].ToString();
                    vvv = vvv.Replace("\r\n", "\\r\\n").Replace("\n", "\\n");
                    a += "\"" + col.ColumnName + "\":\"" + vvv + "\",";
                    continue;
                }
                if (col.DataType == typeof(DateTime))
                {
                    DateTime datetime = Convert.ToDateTime(row[col.ColumnName]);
                    a += "\"" + col.ColumnName + "\":\"\\/Date(" + (long)((datetime.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 0x2710) + ")\\/\",";
                    continue;
                }
                if (col.DataType == typeof(bool))
                {
                    a += "\"" + col.ColumnName + "\":" + row[col.ColumnName].ToString().ToLower() + ",";
                    continue;
                }
                if (col.DataType == typeof(Int64))
                {
                    a += "\"" + col.ColumnName + "\":\"" + row[col.ColumnName] + "\",";
                    continue;
                }
                if (col.DataType == typeof(decimal))
                {
                    a += "\"" + col.ColumnName + "\":" + Convert.ToDecimal(row[col.ColumnName]).ToString("0.#########") + ",";
                    continue;
                }
                if (col.DataType == typeof(Int32) || col.DataType == typeof(Int16) || col.DataType == typeof(float) || col.DataType == typeof(short))
                {
                    a += "\"" + col.ColumnName + "\":" + row[col.ColumnName] + ",";
                    continue;
                }
            }
            a = a.Remove(a.Length - 1);
            a += "}";
            return a;
        }
        #endregion

        #region DataTable转JArray
        public static JArray ToJArray(DataTable dt)
        {
            //声明数组
            JArray jarr = new JArray();
            //遍历行
            foreach (DataRow dr in dt.Rows)
            {
                //声明对象
                JObject jobj = new JObject();
                //遍历列
                foreach (DataColumn dc in dt.Columns)
                {
                    //对象添加键值
                    jobj.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
                //数组添加元素
                jarr.Add(jobj);
            }

            return jarr;
        }
        #endregion
    }
}