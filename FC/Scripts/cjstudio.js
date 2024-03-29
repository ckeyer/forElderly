﻿
function checkUserInput(str) {
    var bases = new Array();
    bases[0] = ' ';
    bases[1] = '\'';
    bases[2] = '"';
    bases[3] = '<';
    bases[4] = '>';
    bases[4] = '-';
    for (var i = 0; i < bases.length; i++) {
        if (str.indexOf(bases[i]) >= 0) {
            return false;
        }
    }
    return true;
}
function checkUploderFileType(fileName) {
    var fileType = /\.[^\.]+/.exec(fileName);
    var allowType = new Array();
    allowType[0] = '.jpg';
    allowType[1] = '.gif';
    allowType[2] = '.png';
    allowType[3] = '.bmp';
    allowType[4] = '.ico';
    for (var i = 0; i < allowType.length; i++) {
        if (fileType == allowType[i]) return true;
    }
    return false;
}
function chageIframeContent(IframeDivId,IframeSrc) {
    var mainContentDiv = document.getElementById(IframeDivId);
    var divWidth = document.all.main_content.offsetWidth;

    mainContentDiv.innerHTML = '<iframe src="' + IframeSrc + '"  scrolling="no" frameborder="0" height="100%" id="' + IframeDivId + '" width="100%" onload="IFrameReSize(\'' + IframeDivId + '\');IFrameReSizeWidth(\'' + IframeDivId + '\');"></iframe>';
    var divIframe = document.getElementById("content_iframe");
    divIframe.offsetWidth;
}

function showErrorMsg(DivId, msg) {
    document.getElementById(DivId).innerHTML = '<div class="alert alert-error">' +
								 '<button type="button" class="close" data-dismiss="alert">×</button>' +
								 '<h4>' +
								 '提示!' +
								 '</h4> <strong>警告!</strong> ' +msg+
								 '</div>';
}

function showSuccessMsg(DivId, msg) {
    document.getElementById(DivId).innerHTML = '<div class="alert alert-success">' +
								 '<button type="button" class="close" data-dismiss="alert">×</button>' +
								 '<h4>' +
								 '提示!' +
								 '</h4> <strong>成功!</strong> ' + msg +
								 '</div>';
}

function IFrameReSize(iframename) {
    var pTar = document.getElementById(iframename);
    if (pTar) { //ff
        if (pTar.contentDocument && pTar.contentDocument.body.offsetHeight) {
            pTar.height = pTar.contentDocument.body.offsetHeight;
        } //ie
        else if (pTar.Document && pTar.Document.body.scrollHeight) {
            pTar.height = pTar.Document.body.scrollHeight;
        }
    }
}

//iframe宽度自适应

function IFrameReSizeWidth(iframename) {
    var pTar = document.getElementById(iframename);
    if (pTar) { //ff
        if (pTar.contentDocument && pTar.contentDocument.body.offsetWidth) {
            pTar.width = pTar.contentDocument.body.offsetWidth;
        } //ie
        else if (pTar.Document && pTar.Document.body.scrollWidth) {
            pTar.width = pTar.Document.body.scrollWidth;
        }
    }
}

function autoIframeSize(iframename) {
    IFrameReSizeWidth(iframename);
    IFrameReSize(iframename);
}



var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

function encode64(input) {
    input = utf16to8(input);
    var output = "";
    var chr1, chr2, chr3 = "";
    var enc1, enc2, enc3, enc4 = "";
    var i = 0;
    do {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);
        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
        if (isNaN(chr2)) {
            enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
            enc4 = 64;
        }
        output = output + keyStr.charAt(enc1) + keyStr.charAt(enc2)
+ keyStr.charAt(enc3) + keyStr.charAt(enc4);
        chr1 = chr2 = chr3 = "";
        enc1 = enc2 = enc3 = enc4 = "";
    } while (i < input.length);
    return output;
}
//将Base64编码字符串转换成Ansi编码的字符串
function decode64(input) {
    var output = "";
    var chr1, chr2, chr3 = "";
    var enc1, enc2, enc3, enc4 = "";
    var i = 0;
    if (input.length % 4 != 0) {
        return "";
    }
    var base64test = /[^A-Za-z0-9\+\/\=]/g;
    if (base64test.exec(input)) {
        return "";
    }
    do {
        enc1 = keyStr.indexOf(input.charAt(i++));
        enc2 = keyStr.indexOf(input.charAt(i++));
        enc3 = keyStr.indexOf(input.charAt(i++));
        enc4 = keyStr.indexOf(input.charAt(i++));
        chr1 = (enc1 << 2) | (enc2 >> 4);
        chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
        chr3 = ((enc3 & 3) << 6) | enc4;
        output = output + String.fromCharCode(chr1);
        if (enc3 != 64) {
            output += String.fromCharCode(chr2);
        }
        if (enc4 != 64) {
            output += String.fromCharCode(chr3);
        }
        chr1 = chr2 = chr3 = "";
        enc1 = enc2 = enc3 = enc4 = "";
    } while (i < input.length);
    return utf8to16(output);
}
function utf16to8(str) {
    var out, i, len, c;
    out = "";
    len = str.length;
    for (i = 0; i < len; i++) {
        c = str.charCodeAt(i);
        if ((c >= 0x0001) && (c <= 0x007F)) {
            out += str.charAt(i);
        } else if (c > 0x07FF) {
            out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
            out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        } else {
            out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        }
    }
    return out;
}
function utf8to16(str) {
    var out, i, len, c;
    var char2, char3;
    out = "";
    len = str.length;
    i = 0;
    while (i < len) {
        c = str.charCodeAt(i++);
        switch (c >> 4) {
            case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
                // 0xxxxxxx
                out += str.charAt(i - 1);
                break;
            case 12: case 13:
                // 110x xxxx 10xx xxxx
                char2 = str.charCodeAt(i++);
                out += String.fromCharCode(((c & 0x1F) << 6) | (char2 & 0x3F));
                break;
            case 14:
                // 1110 xxxx 10xx xxxx 10xx xxxx
                char2 = str.charCodeAt(i++);
                char3 = str.charCodeAt(i++);
                out += String.fromCharCode(((c & 0x0F) << 12) |
((char2 & 0x3F) << 6) |
((char3 & 0x3F) << 0));
                break;
        }
    }
    return out;
}

function cjstudioTest() {
    alert("Test OK");
}

// 获取URL参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}