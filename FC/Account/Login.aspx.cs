﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace FC.Account
{
    public partial class Login : System.Web.UI.Page
    {
        string username = "", passwd = "", member = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                username = HttpContext.Current.Request["username"].ToString();
                passwd = HttpContext.Current.Request["password"];
                member = HttpContext.Current.Request["member"];
                if (!isLawfulInput(username) || !isLawfulInput(passwd))
                {
                    Response.Write("{ \"status\" : \"Error\", \"msg\" : \"用户名或密码中存在敏感字符\"}");
                    Response.End();
                } 
                if (username=="" || passwd=="")
                {
                    Response.Write("{ \"status\" : \"Error\", \"msg\" : \"用户名或密码不能为空\"}");
                    Response.End();
                }
                Response.Clear();
            }
            catch (Exception)
            {
                Response.Write("{ \"status\" : \"Other\", \"msg\" : \"Json 数据传输错误\"}"); 
                Response.End();
                return;
            }
            if (identificaUser())
            {
                ;
            } 
            Response.End();
        }

        public bool isLawfulInput(string str)
       {
           string[] unlawfulChars = { "\'", "\"", "<", ">", "-", " " };
           foreach (string s in unlawfulChars )
           {
               if (str.IndexOf(s)>=0)
                   return false;
           }
           return true;
       }
        public bool identificaUser()
        {
            string connStr = ConfigurationManager.ConnectionStrings["fc_db"].ConnectionString;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adr = new SqlDataAdapter();
            DataSet dataset = new DataSet();
            DataTable rs = new DataTable();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "select id_i,name_c,passwd_c,type_i,sex_c,picPath_c from tb_user where ";
                sql += getUsernameSql();
                cmd = new SqlCommand(sql, conn);
                adr = new SqlDataAdapter(cmd);
                adr.Fill(dataset);
                rs = dataset.Tables[0];
                if (rs.Rows.Count != 1)
                {
                    //Response.Write("{ \"status\" : \"Error\" \"msg\" : \"用户名或密码有误\"}");
                    Response.Write("{ \"status\" : \"Error\", \"msg\" : \"用户名有误"+passwd+sql+"\"}");
                    return false;
                }
                else
                {
                    if (passwd != rs.Rows[0]["passwd_c"].ToString().Trim())
                    {
                        //Response.Write("{ \"status\" : \"Error\" \"msg\" : \"用户名或密码有误\"}");
                        Response.Write("{ \"status\" : \"Error\", \"msg\" : \"密码有误#" + rs.Rows[0]["passwd_c"].ToString()+"##"+passwd + "#\"}");
                        return false;
                    }
                    else 
                    {
                        Response.Write("{ \"status\" : \"Success\", " +
                            " \"username\" : \"" + rs.Rows[0]["name_c"] + " \" , " +
                            " \"userid\" : \"" + rs.Rows[0]["id_i"] + " \" , " +
                            " \"usertype\" : \"" + rs.Rows[0]["type_i"] + " \", " +
                            " \"picpath\" : \"" + rs.Rows[0]["picPath_c"] + " \", " +
                            " \"usersex\" : \"" + rs.Rows[0]["sex_c"] + " \" }");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("{ \"status\" : \"Other\", \"msg\" : \"系统连接数据库失败\"}"); 
            }
            return true;
        }
        public string getUsernameSql()
        {
            string str="";
            try
            {
                Int32.Parse(username);
                str = " id_i = " + username + " ";
            }
            catch (Exception)
            {
                if (username.IndexOf("@") > 0)
                {
                    str = " email_c = '" + username + "' ";
                }
                else
                {
                    str = " name_c = '" + username + "' ";
                }
            }
            return str;
        }
        
    }
}
