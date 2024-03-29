﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace FC.Account
{
    public partial class AjaxServer : System.Web.UI.Page
    {
        public bool isIdenUser = false;
        public FC.cjstudio.User user;
        public string code;

        protected void Page_Load(object sender, EventArgs e)
        {
            initPage();
            switch(code){
                case "":checkError("Json 数据类型获取失败");
                    break;
                case "user_home_page_path":
                    if (isIdenUser && user.type != 0) { returnValue(FC.cjstudio.getTypePath(user.type)); }
                    else { checkError("用户验证失败或权限不够"); }
                    break;
                case "just_user_pic":
                    uploaderPic();
                    break;
                case "get_user_pic_src":
                    getUserPicSrc();
                    break;
                case "just_user_information":
                    updateUserInformation();
                    break;
                case "repassword":
                    repassword();
                    break;
                case "get_content_type":
                    getContentType();
                    break;
                case "commit_update_article":
                    updateArticle();
                    break;
                case "commit_article":
                    commitArticleByAdmin();
                    break;
                case "commit_delete_article":
                    deleteArticle();
                    break;
                case "get_citys_by_province":
                    getCitysByProvince();
                    break;
                case "get_cityzones_by_city":
                    getCityzonesByCity();
                    break;
                case "home_roll_pic_file":
                    chageHomeRollPic();
                    break;
                case "chage_home_roll_pic":
                    chageHomeRollContent();
                    break;
                default:
                    checkError("没有处理相关数据的方法");
                    break;
            }
        }
        public void chageHomeRollContent()
        {
            string picSrcPath = Server.MapPath("~/Others/tmp/HomeRollPic/");
            string picDstPath = Server.MapPath("~/img/");
            try
            {
                string title1 = HttpContext.Current.Request["title1"].ToString();
                string content1 = HttpContext.Current.Request["content1"].ToString();

                string title2 = HttpContext.Current.Request["title2"].ToString();
                string content2 = HttpContext.Current.Request["content2"].ToString();

                string title3 = HttpContext.Current.Request["title3"].ToString();
                string content3 = HttpContext.Current.Request["content3"].ToString();

                cjstudio.setConfitKeyValue("HomeMainRollPic1_Title", title1);
                cjstudio.setConfitKeyValue("HomeMainRollPic1_Content", content1);
                cjstudio.setConfitKeyValue("HomeMainRollPic2_Title", title2);
                cjstudio.setConfitKeyValue("HomeMainRollPic2_Content", content2);
                cjstudio.setConfitKeyValue("HomeMainRollPic3_Title", title3);
                cjstudio.setConfitKeyValue("HomeMainRollPic3_Content", content3);

                cjstudio.setConfitKeyValue("HomeMainRollPic1_Pic", cjstudio.getConfigValue("HomeMainRollPic1_Pic_tmp"));
                cjstudio.setConfitKeyValue("HomeMainRollPic2_Pic", cjstudio.getConfigValue("HomeMainRollPic2_Pic_tmp"));
                cjstudio.setConfitKeyValue("HomeMainRollPic3_Pic", cjstudio.getConfigValue("HomeMainRollPic3_Pic_tmp"));
                try
                {
                    File.Move(picSrcPath + cjstudio.getConfigValue("HomeMainRollPic1_Pic_tmp"),
                        picDstPath + cjstudio.getConfigValue("HomeMainRollPic1_Pic_tmp"));
                }
                catch (Exception)
                {
                    ;
                } 
                try
                {
                    File.Move(picSrcPath + cjstudio.getConfigValue("HomeMainRollPic2_Pic_tmp"),
                        picDstPath + cjstudio.getConfigValue("HomeMainRollPic2_Pic_tmp"));
                }
                catch (Exception)
                {
                    ;
                } 
                try
                {
                    File.Move(picSrcPath + cjstudio.getConfigValue("HomeMainRollPic3_Pic_tmp"),
                        picDstPath + cjstudio.getConfigValue("HomeMainRollPic3_Pic_tmp"));
                }
                catch (Exception)
                {
                    ;
                }
                returnValue("修改成功");
            }
            catch (Exception)
            {
                ;
            }
        }
        public void chageHomeRollPic()
        {
            string picPath = Server.MapPath("~/Others/tmp/HomeRollPic/");
            if (Request.Files.Count == 1)
            {
                try
                {
                    string picNum = HttpContext.Current.Request["picNum"].ToString();
                    string picType = Path.GetExtension(Request.Files[0].FileName);
                    
                    Request.Files[0].SaveAs(picPath + "homepagecontent"+picNum + picType);
                    cjstudio.setConfitKeyValue("HomeMainRollPic" + picNum +"_Pic_tmp", "homepagecontent" + picNum + picType);
                    returnValue("homepagecontent" + picNum + picType);
                }
                catch (Exception)
                {
                    Response.Write("{\"error\" :\"error\",\"status\" :\"error\", msg:\"文件上传出现异常\"}"); ;
                }
            }
            else
            {
                Response.Write("{\"error\":\"=========\", \"filetype\":\"Error\"}");
            }
            Response.End();
        }
        public void deleteArticle()
        {
            string articleId = HttpContext.Current.Request["article_id"].ToString();
            if (cjstudio.deleteArticle(articleId))
            {
                returnValue("删除成功");
            }
            else
            {
                checkError("删除时出现异常");
            }
        }
        public void updateArticle() 
        {
            cjstudio.Article article = new cjstudio.Article();
            try
            {
                article.id = HttpContext.Current.Request["article_id"].ToString();
                article.title = HttpContext.Current.Request["title"].ToString();
                article.content = HttpContext.Current.Request["content"].ToString();
                article.contentType = HttpContext.Current.Request["content_type"].ToString();
                string showAtHome = HttpContext.Current.Request["show_at_home"].ToString();
                if (showAtHome == "true")
                {
                    article.status = 15;
                }
                else
                {
                    article.status = 7;
                }
                article.contentMd5 = cjstudio.EncryptMd5(article.content);
                if (cjstudio.updateArticle(article))
                {
                    returnValue("文章已修改成功");
                }
                else
                {
                    checkError("文章修改加失败");
                }
            }
            catch (Exception)
            {
                ;
            }
        }
        public void getCityzonesByCity()
        {
            try
            {
                string cityid = HttpContext.Current.Request["cityid"].ToString();
                List<cjstudio.Address> citys = cjstudio.getCityzones(cityid);
                string res = "";
                foreach (cjstudio.Address addr in citys)
                {
                    res += "<option value=\"" + addr.id + "\">" + addr.name + "</option>";
                }
                returnValue(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(res)));
            }
            catch (Exception)
            {

                throw;
            };
        }
        public void getCitysByProvince()
        {
            try
            {
                string provinceId = HttpContext.Current.Request["provinceid"].ToString();
                List<cjstudio.Address> citys = cjstudio.getCitys(provinceId);
                string res = "";
                foreach (cjstudio.Address  addr in citys)
                {
                   res += "<option value=\"" + addr.id + "\">" + addr.name + "</option>";
                }
                returnValue(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(res)));
            }
            catch (Exception)
            {
                
                throw;
            };
        }
        public void commitArticleByAdmin()
        {
            cjstudio.Article article = new cjstudio.Article();
            try
            {
                article.title = HttpContext.Current.Request["title"].ToString();
                article.content = HttpContext.Current.Request["content"].ToString();
                article.contentType = HttpContext.Current.Request["content_type"].ToString();
                string showAtHome = HttpContext.Current.Request["show_at_home"].ToString();
                if (showAtHome == "true")
                {
                    article.status = 15;
                }
                else
                {
                    article.status = 7;
                }
                article.contentMd5 = cjstudio.EncryptMd5(article.content);
                if (cjstudio.addArticle(user.id, article))
                {
                    returnValue("文章已添加成功");
                }
                else {
                    checkError("文章添加失败");
                }
            }
            catch (Exception)
            {
                ;
            }
        }
        public void getContentType()
        {
            try
            {
                string superType = HttpContext.Current.Request["super_content_typeId"].ToString();
                Dictionary<int, string> contentType =
                    new Dictionary<int, string>();
                contentType = cjstudio.getContentType(int.Parse(superType));
                if (contentType != null)
                {
                    string value = "";
                    foreach (KeyValuePair<int, string> pair in contentType)
                    {
                        value += "<option value=\"" + pair.Key + "\">" +pair.Value + "</option>";
                    }
                    returnValue(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value)));
                }
            }
            catch (Exception)
            {

            }
            Response.End();
        }
        public void repassword()
        {
            try
            {
                string oldpassword=HttpContext.Current.Request["oldpassword"].ToString();
                string newpassword1=HttpContext.Current.Request["newpassword1"].ToString();
                if (cjstudio.isLoginSuccess(user.id, oldpassword)) {
                    string passwd = newpassword1.Length == 32 ? newpassword1.ToLower() : cjstudio.EncryptMd5(newpassword1).ToLower();
                    if (cjstudio.rePassword(user.id, passwd))
                    {
                        returnValue("密码修改成功！");
                        return;
                    }
                    else {
                        checkError("密码重置失败...");
                        return;
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
        }
        public void updateUserInformation()
        {
            try 
	        {
                user.name = cjstudio.checkUserInput(HttpContext.Current.Request["name"].ToString()) ?
                    HttpContext.Current.Request["name"].ToString() : user.name;

                user.sex = HttpContext.Current.Request["sex"].ToString();
                int year = int.Parse(HttpContext.Current.Request["year"].ToString());
                int month = int.Parse(HttpContext.Current.Request["month"].ToString());
                int day = int.Parse(HttpContext.Current.Request["day"].ToString());

                user.phone = cjstudio.checkUserInput(HttpContext.Current.Request["phone"].ToString()) ?
                    HttpContext.Current.Request["phone"].ToString() : user.phone;
                user.homeAddress = cjstudio.checkUserInput(HttpContext.Current.Request["homeaddress"].ToString()) ?
                    HttpContext.Current.Request["homeaddress"].ToString() : user.homeAddress;
                user.livingAddress = cjstudio.checkUserInput(HttpContext.Current.Request["livingaddress"].ToString()) ?
                    HttpContext.Current.Request["livingaddress"].ToString() : user.livingAddress;
                DateTime tmpdt = new DateTime(year, month, day);
                user.birDT = tmpdt;
                user.birthday = year + "-" + month + "-" + day;
                user.homeAddressId = HttpContext.Current.Request["homeaddressid"].ToString();
                user.livingAddressId = HttpContext.Current.Request["livingaddressid"].ToString();
                if (cjstudio.updateUserInformation(user))
                {
                    returnValue("用户信息修改成功");
                }
                else {
                    checkError("向数据库提交时出现异常");
                }
	        }
	        catch (Exception)
            {
                //checkError("提交数据有误");
	        }
        }
        public void getUserPicSrc()
        {
            string picPath = Server.MapPath("~/Account/userPic/");
            string picName = FC.cjstudio.getUserPicPath(user.id);
            if (picName.Length <5)
            {
                Response.Write("{\"error\" :\"\", msg:\"Default.jpg\"}");
            }
            else if (File.Exists(picPath + picName))
            {
                Response.Write("{\"error\" :\"\", msg:\"" + picName + "\"}");
            }
            else
            {
                Response.Write("{\"error\" :\"\", msg:\"Default.jpg\"}");
            }
            Response.End();
        }
        public void uploaderPic()
        {
            string picPath = Server.MapPath("~/Account/userPic/");
            if (Request.Files.Count == 1)
            {
                string picType=Path.GetExtension(Request.Files[0].FileName);
                Request.Files[0].SaveAs(picPath + user.id + picType);
                if (FC.cjstudio.updateUserPicPath(user.id, picType))
                {
                    Response.Write("{\"error\" :\"\", msg:\"" + user.id + picType + "\"}");
                }
                else {
                    Response.Write("{\"error\" :\"error\", msg:\"向数据库提交过程中出现异常\"}");
                }
            }
            else
            {
                Response.Write("{\"error\":\"=========\", \"filetype\":\"Error\"}");
            }
            Response.End();
        }
        public void checkError(string msg)
        {
            Response.Write("{\"status\":\"error\",\"msg\":\""+msg+"\"}");
            try
            {
                Response.End();
            }
            catch (Exception)
            {
                Response.Flush();
                throw;
            }
        }
        public void returnValue(string value)
        {
            try
            {
                Response.Write("{\"status\":\"success\",\"msg\":\"" + value + "\"}");
                Response.End();
            }
            catch (Exception)
            {
                Response.Flush();
            }
        }
        public void initPage()
        {
            try
            {
                user.id = Session["uid"].ToString();
                user.password32 = Session["upasswd"].ToString();
                user.name = Session["uname"].ToString();
                user.type = int.Parse(Session["utype"].ToString());
                isIdenUser = true;
            }
            catch (Exception)
            {
                isIdenUser = false;
            }

            try
            {
                code = HttpContext.Current.Request["code"].ToString();
            }
            catch (Exception)
            {
                code = "";
            }
        }
    }
}