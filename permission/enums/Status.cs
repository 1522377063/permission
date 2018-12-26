/*************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    xy
** date：    2018/7/13
** desc：    枚举定义类
** Ver.:     V1.0.0
**************************************/

using System;
using System.ComponentModel;

namespace permission.enums
{
    #region 状态枚举
    /// <summary>
    /// 状态枚举
    /// </summary>
    [Flags]
    public enum Status
    {
        [Description("正常")]
        Normal = 200,

        [Description("异常")]
        Error = 0
    }
    #endregion

    #region 数据库操作枚举
    /// <summary>
    /// 数据库操作枚举
    /// </summary>
    [Flags]
    public enum Message
    {
        [Description("查询成功")]
        Query = 1,

        [Description("新增成功")]
        Insert = 2,

        [Description("修改成功")]
        Update = 3,

        [Description("删除成功")]
        Delete = 4,

        [Description("查询失败")]
        QueryFailure = 5,

        [Description("新增失败")]
        InsertFailure = 6,

        [Description("修改失败")]
        UpdateFailure = 7,

        [Description("删除失败")]
        DeleteFailure = 8
    }
  

    #endregion

    public enum CreateInfo
    {
        [Description("创建失败")]
        CreateFailure = 0,

        [Description("创建成功")]
        Createtrue = 1
    }

    public enum UpdateInfo
    {
        [Description("修改失败")]
        UpdateFailure = 0,

        [Description("修改成功")]
        Updatetrue = 1
    }

    public enum DeleteInfo
    {
        [Description("删除失败")]
        DeleteFailure = 0,

        [Description("删除成功")]
        Deletetrue = 1
    }
}