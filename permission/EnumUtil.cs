/*************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    xy
** date：    2018/7/13
** desc：    枚举转换类
** Ver.:     V1.0.0
**************************************/

namespace permission.utils
{
    public class EnumUtil
    {
        #region 转换描述
        /// <summary>
        /// 转换描述
        /// </summary>
        /// <param name="flag">标识</param>
        /// <returns>string</returns>
        public static string getMessageStr(int flag)
        {
            //声明结果字符
            string result = "";
            //判断获取转换
            switch (flag)
            {
                case 1:
                    result = "查询成功";
                    break;
                case 2:
                    result = "新增成功";
                    break;
                case 3:
                    result = "修改成功";
                    break;
                case 4:
                    result = "删除成功";
                    break;
                case 5:
                    result = "查询失败";
                    break;
                case 6:
                    result = "新增失败";
                    break;
                case 7:
                    result = "修改失败";
                    break;
                default:
                    result = "删除失败";
                    break;
            }

            return result;
        }
        #endregion
    }
}