﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="志愿者脚步.aspx.cs" Inherits="FC.Others.Home.志愿者脚步" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<link href="../../JqueryUi/assets/css/bootstrap.min.css" rel="stylesheet">
  <link type="text/css" href="../../JqueryUi/css/custom-theme/jquery-ui-1.10.0.custom.css" 
        rel="stylesheet" />
  <link type="text/css" href="../../JqueryUi/assets/css/font-awesome.min.css" rel="stylesheet" />

  <link href="../../JqueryUi/assets/css/docs.css" rel="stylesheet">
  <link href="../../JqueryUi/assets/js/google-code-prettify/prettify.css" rel="stylesheet">

<title>志愿者风采</title>
</head>
<style>
body,a
{
font-family:"Microsoft Yahei";
}
a{
text-decoration:none;
font-family:"Microsoft Yahei";
}
div {
display: block;
}
.news {
width: 970px;
float: left;
display: inline;
padding-bottom: 20px;
}
.news .news_1 {
float: left;
display: inline;
width: 466px;
height: 187px;
margin-top: 38px;
}
.news .news_2 {
float: left;
display: inline;
width: 466px;
height: 182px;
margin: 35px 0 0 35px;
}

.news .news_tu {
float: left;
display: inline;
width: 128px;
height: 77px;
_height: 77px;
_overflow: hidden;
border: #d9d9d9 1px solid;
padding:3px;
margin-top:9px;
 
}
.news .news_list
{
float: left;
display: inline;
margin-left: 12px;
_margin-left: 10px;
width: 318px;
}
h1
{
	border-top: 1px solid #e0e0e0;
	height: 32px;
	line-height: 32px;
	font-size: 14px;
	font-weight: bold;
	background-image: linear-gradient(to bottom,#D66123, orange);
}
.news .news_ul {
width: 318px;
float: left;
display: inline;
border-bottom: #d9d9d9 1px dashed;
padding-bottom: 18px;
}
ul, li 
{
	list-style: none;
}
.news .news_ul li {
width: 298px;
padding-left: 18px;
margin: 14px 0 0 3px;
color: #333237;
}
.news .news_ul li a {
color: #333237;
text-decoration: none;
}
.box {
width: 970px;
margin: 0 auto;
overflow: hidden;
}
h1 a
{
	float:right;
}
</style>
<body>
	<div class="box">
	<div class="news">
		<div class="news_1">
        	<div class="news_tu">
                <img style="width:135px; height:85px;" src="../<%=FC.cjstudio.getConfigValue("志愿者脚步Pic4")%>" />
            </div>
            <div class="news_list">
            	<h1>&nbsp;优秀志愿者风采<a href="">更多...&nbsp;&nbsp;&nbsp;&nbsp;</a></h1>
                <div class="news_ul">
                	<%
                        List<FC.cjstudio.Article> article =
                            FC.cjstudio.getArticleByTypeName("优秀志愿者风采");
                        for (int i = 0; i < article.Count && i < 8; i++)
                      {
                          try
                          {
                              string tmp = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(article[i].title)); ;
                              tmp = tmp.Length > 12 ? tmp.Substring(0, 10) + "..." : tmp;

                              Response.Write("<ul><a href=\"../../Others/ShowArticlePage.aspx?article=" +
                                  article[i].id + "\">" + tmp + "</a>&nbsp;" +
                                  article[i].createDT.ToShortDateString() + "</ul>");
                          }
                          catch (Exception)
                          {
                              throw;
                          }
                      }                      
                       %>
             	</div>
            </div>
        </div>
        <div class="news_2">
        	<div class="news_tu">
                <img style="width:135px; height:85px;" src="../<%=FC.cjstudio.getConfigValue("志愿者脚步Pic3")%>" />
            </div>
            <div class="news_list">
            	<h1>&nbsp;志愿者认证<a href="">更多...&nbsp;&nbsp;&nbsp;&nbsp;</a></h1>
                <div class="news_ul">
                	<%
                        article = FC.cjstudio.getArticleByTypeName("志愿者认证");
                        for (int i = 0; i < article.Count && i < 8; i++)
                      {
                          try
                          {
                              string tmp = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(article[i].title)); ;
                              tmp = tmp.Length > 12 ? tmp.Substring(0, 10) + "..." : tmp;

                              Response.Write("<ul><a href=\"../../Others/ShowArticlePage.aspx?article=" +
                                  article[i].id + "\">" + tmp + "</a>&nbsp;" +
                                  article[i].createDT.ToShortDateString() + "</ul>");
                          }
                          catch (Exception)
                          {
                              throw;
                          }
                      }                      
                       %>
             	</div>
            </div>
        </div>
	</div>
	</div>
</body>
</html>

