﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace FC.Account
{
    public partial class Login : System.Web.UI.Page
    {
        string username = "", passwd = "", member = "false";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Request["code"].ToString() == "exit")
                {
                    Session.Clear();
                    return;
                }
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
           string[] unlawfulChars = { "'", "\"", "<", ">", "-", " " };
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
                    Response.Write("{ \"status\" : \"Error\" \"msg\" : \"用户名或密码有误\"}");
                    Response.End();
                    //Response.Write("{ \"status\" : \"Error\", \"msg\" : \"用户名有误"+passwd+sql+"\"}");
                    return false;
                }
                else
                {
                    string standpass=rs.Rows[0]["passwd_c"].ToString().ToLower().Trim();
                    if ((passwd.Length == 32 ? EncryptSHA256(passwd).ToLower() : EncryptSHA256(EncryptMd5(passwd).ToLower()).ToLower())
                        != standpass)
                    {
                        Response.Write("{ \"status\" : \"Error\" \"msg\" : \"用户名或密码有误\"}");
                        Response.End();
                        //Response.Write("{ \"status\" : \"Error\", \"msg\" : \"密码有误#" + rs.Rows[0]["passwd_c"].ToString()+"##"+passwd + "#\"}");
                        return false;
                    }
                    else 
                    {
                        Response.Write("{ \"status\" : \"Success\", " +
                            " \"username\" : \"" + rs.Rows[0]["name_c"] + " \" , " +
                            " \"userid\" : \"" + rs.Rows[0]["id_i"] + " \" , " +
                            " \"usertype\" : \"" + rs.Rows[0]["type_i"] + " \", " +
                            " \"picpath\" : \"" + rs.Rows[0]["picPath_c"] + " \", " +
                            " \"member\" : \"" + member + " \", " +
                            " \"usersex\" : \"" + rs.Rows[0]["sex_c"] + " \" }");
                        Session["uname"] = rs.Rows[0]["name_c"];
                        Session["utype"] = rs.Rows[0]["type_i"];
                        Session["uid"] = rs.Rows[0]["id_i"];
                        Session["upasswd"] = passwd;
                        Session["member"] = member;

                        HttpCookie cookie = new HttpCookie("fc_user");
                        cookie.Values.Add("id", rs.Rows[0]["id_i"].ToString());
                        cookie.Values.Add("name", rs.Rows[0]["name_c"].ToString());
                        cookie.Values.Add("password", passwd);
                        cookie.Values.Add("type", rs.Rows[0]["type_i"].ToString());
                        cookie.Values.Add("member", member );
                        if (member == "true")
                        {
                            cookie.Expires = DateTime.Now.AddDays(30);
                        }
                        else {
                        }
                        Response.AppendCookie(cookie);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("{ \"status\" : \"Other\", \"msg\" : \"系统连接数据库失败\"}");
                return false;
            }
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
        public string EncryptSHA256(string strPwd)
        {
            byte[] result = Encoding.Default.GetBytes(strPwd);
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] output = sha256.ComputeHash(result);
            string str = BitConverter.ToString(output).Replace("-", "");
            return str.ToUpper();
        }
        public string EncryptMd5(string strPwd)
        {
            byte[] result = Encoding.Default.GetBytes(strPwd);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string str = BitConverter.ToString(output).Replace("-", "");
            return str.ToUpper();
        }
    }
}
