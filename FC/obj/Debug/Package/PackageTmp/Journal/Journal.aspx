﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Journal.aspx.cs" Inherits="FC.Journal.Journal" %>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8"/>
  <title><%=FC.cjstudio.getConfigValue("MainTitle")%></title>
  <meta name="author" content="CJ_Studio"/>

  <!-- Le styles -->
  <link href="../JqueryUi/assets/css/bootstrap.min.css" rel="stylesheet">
  <link type="text/css" href="../JqueryUi/css/custom-theme/jquery-ui-1.10.0.custom.css" 
        rel="stylesheet" />
  <link type="text/css" href="../JqueryUi/assets/css/font-awesome.min.css" rel="stylesheet" />

  <link href="../JqueryUi/assets/css/docs.css" rel="stylesheet">
  <link href="../JqueryUi/assets/js/google-code-prettify/prettify.css" rel="stylesheet">

</head>

<body  style="padding-top:0px;min-height:800px;">
                    
  <div class="container">

        <div class="container-fluid">
            <% if (isIdenUser && (user.type&1)!=0 )
               { %>
	        <div class="row-fluid">
		        <div class="span12">
                    
		        </div>
	        </div>
	        <div class="row-fluid">
		        <div class="span12">
				        <div class="span2">
					        <div class="row-fluid">
							        <div class="accordion" id="accordion-000">
								        <div class="accordion-group">
									        <div class="accordion-heading">
										         <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion-000" href="#accordion-element-001">
                                                 个人中心</a>
									        </div>
									        <div id="accordion-element-001" class="accordion-body collapse in">
										        <div class="accordion-inner" onclick=";">
                                                <a class="accordion-toggle" data-toggle="collapse" onclick="chageIframeContent('main_content','../Account/BasicInformation.aspx');">
                                                 &nbsp;&nbsp;基本信息</a>
										        </div>
                                                <div class="accordion-inner">
                                                <a class="accordion-toggle" data-toggle="collapse"  onclick="chageIframeContent('main_content','MoreConfig.aspx');">
                                                 &nbsp;&nbsp;会员认证</a>
											        <a id="A102" ></a>
										        </div>
                                                <div class="accordion-inner">
                                                <a class="accordion-toggle" data-toggle="collapse" onclick="chageIframeContent('main_content','../Account/Repassword.aspx');">
                                                 &nbsp;&nbsp;重置密码</a>
										        </div>
									        </div>
								        </div>

								        <div class="accordion-group">
									        <div class="accordion-heading">
										         <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion-000" href="#accordion-element-002">
                                                 志愿者活动</a>
									        </div>
									        <div id="accordion-element-002" class="accordion-body collapse">
										        <div class="accordion-inner">
											        <a class="accordion-toggle" data-toggle="collapse" >
                                                 &nbsp;&nbsp;待开发</a>
                                                    <script type="text/javascript">
                                                        function test() { alert("fuck"); }
                                                    </script>
										        </div>
									        </div>
								        </div>
							        </div>
						        </div>
				        </div>


                        
				        <div class="span8">
                            <div  id="main_content">
                             '<iframe src="../Account/BasicInformation.aspx"  scrolling="no" frameborder="0" height="100%" id="content_iframe" width="100%" onload="IFrameReSize('content_iframe');IFrameReSizeWidth('content_iframe');"></iframe>
                            </div>
                        </div>
		        </div>
	        </div>
            <% }
               else if(isIdenUser)
               { %>
							<div class="alert alert-error">
								 <button type="button" class="close" data-dismiss="alert">×</button>
								<h4>
									提示!
								</h4> <strong>警告!</strong> 您的账号没有查看此页的权限.
							</div>
                <% } else { %>
							<div class="alert alert-error">
								 <button type="button" class="close" data-dismiss="alert">×</button>
								<h4>
									提示!
								</h4> <strong>警告!</strong> 访问此页需要先登录.
							</div>
                        <% } %>
						</div>
        </div>

  <!-- Placed at the end of the document so the pages load faster
  <script src="http://cdn.bootcss.com/jquery/1.11.1/jquery.min.js"></script> -->
  <script src="../Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
  <script src="../Scripts/md5.js" type="text/javascript"></script>
  <script src="../Scripts/cjstudio.js" type="text/javascript"></script>
  <script src="../JqueryUi/assets/js/bootstrap.min.js" type="text/javascript"></script>
  <script src="../JqueryUi/assets/js/jquery-ui-1.10.0.custom.min.js" type="text/javascript"></script>
  <script src="../JqueryUi/assets/js/google-code-prettify/prettify.js" type="text/javascript"></script>
  <script src="../JqueryUi/assets/js/docs.js" type="text/javascript"></script>
  <script src="../JqueryUi/assets/js/demo.js" type="text/javascript"></script>
</body>
</html>
