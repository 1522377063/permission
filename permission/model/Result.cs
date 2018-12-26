/*************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    tzz
** date：    2018/5/22
** desc：    结果信息实体类
** Ver.:     V1.0.0
**************************************/

using System.Collections.Generic;

namespace permission.model
{
    #region 结果实体类
    /// <summary>
    /// 结果实体类
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string status { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// 结果
        /// </summary>
        public object result { set; get; }
    }
    #endregion

    #region layui table实体类
    /// <summary>
    /// layui table实体类
    /// </summary>
    public class LayTableResult<T>
    {
        /// <summary>
        /// 状态值
        /// </summary>
        public int code { set; get; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { set; get; }

        /// <summary>
        /// 数量总数
        /// </summary>
        public int count { set; get; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> data { set; get; }
    }
    #endregion
}