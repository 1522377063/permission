/*************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    xy
** date：    2018/7/13
** desc：    GuId生成工具封装类
** Ver.:     V1.0.0
**************************************/

using System;

namespace permission.common
{
    /// <summary>
    /// GuId生成工具类
    /// </summary>
    public class GuIdHelper
    {
        /// <summary>
        /// GUID生成
        /// </summary>
        /// <param name="format">格式 可填写N、D、B、P、X</param>
        /// <returns></returns>
        public static string GetNewGuId(string format = "")
        {
            if (string.IsNullOrEmpty(format))
                return Guid.NewGuid().ToString();
            else
                return Guid.NewGuid().ToString(format);
        }
    }
}