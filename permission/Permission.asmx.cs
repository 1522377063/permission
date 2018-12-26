using permission.service;
using permission.service.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace permission
{
    /// <summary>
    /// Permission 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Permission : System.Web.Services.WebService
    {
        IPermissionService permissionService = new PermissionService();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(Description = @"<h3>method: GetUserPermissionList</h3>
        <p>方法描述：<strong>查询用户权限列表</strong></p> 
        <p>参数：{<strong>page<=>当前页|</strong><strong>rows<=>分页每页大小|</strong>}</p>
        <p style='color:green'>成功结果：<strong>{'total':2,'rows':[{'username':'张三','cunzhuang':1,'cesuo':0,'minsu':0,'jingqu':1,'addtuceng':1,'shouhuiditu':0,'addimage':0,'ninghaimaoyucun':0,'ninghai':1,'addmodel':0,'addpanorama':1}]}</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void GetUserPermissionList(int page, int rows)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(permissionService.GetUserPermissionList(page, rows));
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: EditUserPermission</h3>
        <p>方法描述：<strong>修改用户权限列表</strong></p> 
        <p>参数：{<strong>json<=>要编辑的数据以json格式传送</strong>}</p>
        <p style='color:green'>成功结果：<strong>{status: '200', message: '修改成功', result: null}</strong></p>
        < p>输出格式：<strong>json</strong></p></br>")]
        public void EditUserPermission(string json)
        {
            //HttpRequest request = HttpContext.Current.Request;
            //Stream stream = request.InputStream;
            //StreamReader streamReader = new StreamReader(stream);
            //string json = string.Empty;
            //json = streamReader.ReadToEnd();
            ////{"username":"张三","cunzhuang":0,"cesuo":0,"minsu":1,"jingqu":1,"addtuceng":1,"shouhuiditu":0,"addimage":1,"ninghaimaoyucun":0,"ninghai":1,"addmodel":1,"addpanorama":1}
            //json = HttpUtility.UrlDecode(json);

            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(permissionService.EditUserPermission(json));
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: DeleteUserPermission</h3>
        <p>方法描述：<strong>删除用户权限列表</strong></p> 
        <p>参数：{<strong>username<=>用户名</strong>}</p>
        <p style='color:green'>成功结果：<strong>{status: '200', message: '删除成功', result: null}</strong></p>
        < p>输出格式：<strong>json</strong></p></br>")]
        public void DeleteUserPermission(string username)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(permissionService.DeleteUserPermission(username));
            Context.Response.End();
        }
    }
}
