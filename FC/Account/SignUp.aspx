﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="FC.Account.SignUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<head runat="server">
    <title></title>
    <link href="../JqueryUi/assets/css/bootstrap.min.css" rel="stylesheet">
    <link type="text/css" href="../JqueryUi/css/custom-theme/jquery-ui-1.10.0.custom.css" 
        rel="stylesheet" />
    <link type="text/css" href="../JqueryUi/assets/css/font-awesome.min.css" rel="stylesheet" />

    <link href="../JqueryUi/assets/css/docs.css" rel="stylesheet">
    <link href="../JqueryUi/assets/js/google-code-prettify/prettify.css" rel="stylesheet">
    
</head>
<body>
    <div class="container-fluid">
	    <div class="row-fluid">
		    <div class="span12">
			    <div class="row-fluid">
				    <div class="span12">
				    </div>
			    </div>
			    <div class="row-fluid">
				    <div class="span12">
					    <div class="row-fluid">
						    <div class="span3">

						    </div>
						    <div class="span9">
							    <div class="row-fluid">
								    <div class="span12">
									    <form>
                                            <fieldset>
                                                <legend id="signUp_success" >新用户注册</legend> 
                                                    <div id="signUp_status"></div>
                                                    <p>
                                                        <label>选择注册用户类型</label>
                                                        <select id="utype">
                                                            <option value ="1">志愿者</option>
                                                            <option value ="2">社区管理员</option>
                                                            <option value ="4">老人</option>
                                                        </select>
                                                    </p>
                                                    <p>
                                                        <label>用户名</label>
                                                        <input id="uname" type="text" placeholder="请输入您的真实姓名"/>
                                                    </p>
                                                    <p>
                                                        <label>邮箱</label>
                                                        <input id="uemail" type="text" placeholder="请输入您的常用邮箱"/>
                                                    </p>
                                                    <p>
                                                        <label>密码</label>
                                                        <input id="upasswd1" type="password" placeholder="请输入密码" />
                                                    </p>
                                                    <p>
                                                        <label>密码</label>
                                                        <input id="upasswd2" type="password" placeholder="请重复密码" />
                                                    </p>
                                                   
                                                    <iframe src="../Others/provision.html" scrolling="yes" width="600px" title="用户条款">
                                                    </iframe>
                                                    <label class="checkbox" >
                                                    <input type="checkbox" id="agree_provision" /> 勾选同意以上条款</label>
                                                    <input  class="btn " type="button" onclick="doCommit();"  value="提交"/> 
                                                    
										    </fieldset>
									    </form>
								    </div>
							    </div>
						    </div>
					    </div>
				    </div>
			    </div>
		    </div>
	    </div>
    </div>

    <script type="text/javascript">
        var is_lawful = false;
        var is_commit = false;
        var is_signup_success = false;
        function doCommit() {
            var utype = document.getElementById("utype").value.toString();
            var uname = document.getElementById("uname").value.toString();
            var uemail = document.getElementById("uemail").value.toString();
            var passwd1 = document.getElementById("upasswd1").value.toString();
            var passwd2 = document.getElementById("upasswd2").value.toString();
            var agree = document.getElementById("agree_provision").checked.toString();
            var oDiv = document.getElementById("signUp_status");
            if (is_commit == true) {return;}
            if (uname == '') {
                doErrorNotice(oDiv, '错误', '用户名不能为空');
                is_lawful = false;
                return;
            }
            if (!isLawfulInput(uname)) {
                doErrorNotice(oDiv, '错误', '用户名中含有非法字符');
                is_lawful = false;
                return;
            }
            if (uemail == '') {
                doErrorNotice(oDiv, '错误', '用户邮箱不能为空');
                is_lawful = false;
                return;
            }
            if (!isLawfulInput(uemail)) {
                doErrorNotice(oDiv, '错误', '用户邮箱中含有非法字符');
                is_lawful = false;
                return;
            }
            if (passwd1 == '' || passwd2 == '') {
                doErrorNotice(oDiv, '错误', '密码不能为空');
                is_lawful = false;
                return;
            }
            if (!isLawfulInput(passwd1) || !isLawfulInput(passwd2)) {
                doErrorNotice(oDiv, '错误', '密码中含有非法字符');
                is_lawful = false;
                return;
            }
            if (passwd1 != passwd2) {
                doErrorNotice(oDiv, '错误', '两次密码不一致');
                is_lawful = false;
                return;
            }
            if (agree != 'true') {
                doErrorNotice(oDiv, '提示', '请认真阅读服务条款，并确认遵守');
                is_lawful = false;
                return;
            }
            is_lawful = true;

            var password = passwd1;
            if (password.length != 32) {
                password = hex_md5(passwd1).toLocaleLowerCase();
            }
            htmlobj = $.ajax({ url: "./SignUpScript.aspx",
                async: false,
                data: { "username": uname, "password": password, "email": uemail,"type":utype }
            });
            var dataRes = $.parseJSON(htmlobj.responseText);
            if (dataRes.status == "Success") {
                SignUpSuccess(oDiv, dataRes);
            } else if (dataRes.status == "Error") {
                oDiv.innerHTML =
                '<div class="alert alert-danger" role="alert">' + dataRes.msg + '</div>';
            } else {
                oDiv.innerHTML =
                 '<div class="alert alert-warning" role="alert">' + dataRes.msg + '</div>';
            }
        }
        function SignUpSuccess(oDiv, dataRes) {
            alert("注册成功" + dataRes.id + dataRes.name + dataRes.email);
            var div2 = document.getElementById("signUp_success");
            div2.innerHTML = '<div align="center" >'+
                '<br><br><font size="+4" color="#00CC00">注册成功</font><br><br><br>'+
                '<font size="+4" color="#0000CC" face="Times New Roman, Times, serif">' + dataRes.name + ',' +
                '</font><font size="+1">您的账号为：</font><font size="+4" color="#FF0000">' + dataRes.id + '</font><br><br><br>' +
                '<font size="+1">您的登陆邮箱为：</font><font size="+2" color="#0000CC">' + dataRes.email + '</font><br><br>' +
                '<font size="+1">点击<a href="'+getPathByType(dataRes.type)+'">此处</a>完善个人资料</font></div>';
        }
        function getPathByType(type) {
            if (type == '4') {
                return '../Elderly/Elderly.aspx';
            }
            else if (type == '2') {
                return '../Community/Community.aspx';
            }
            else {
                return '../Journal/Journal.aspx';
            }

        }
        function isLawfulInput(str) {
            var bases = new Array();
            bases[0]=' ';
            bases[1]='\'';
            bases[2]='"';
            bases[3]='<';
            bases[4]='>';
            for (var i = 0; i < bases.length; i++) {
                if (str.indexOf(bases[i]) >= 0) {
                    return false;
                }
            }
            return true;
        }
        function doErrorNotice(oDiv, title, msg) {
            oDiv.innerHTML = '<div class="alert alert-danger" role="alert"><strong>'+title+': </strong>'+msg+'</div>'; ;
        }
    </script>
    <script src="http://cdn.bootcss.com/jquery/1.11.1/jquery.min.js"></script>
  
    <script src="../Scripts/md5.js" type="text/javascript"></script>
    <script src="../JqueryUi/assets/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../JqueryUi/assets/js/jquery-ui-1.10.0.custom.min.js" type="text/javascript"></script>
    <script src="../JqueryUi/assets/js/google-code-prettify/prettify.js" type="text/javascript"></script>
    <script src="../JqueryUi/assets/js/docs.js" type="text/javascript"></script>
    <script src="../JqueryUi/assets/js/demo.js" type="text/javascript"></script>
</body>
</html>
