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

namespace FC
{
    public static class cjstudio
    {
        public static string connStr = ConfigurationManager.ConnectionStrings["fc_db"].ConnectionString;
        public struct User
        {
            public string id, name, password32, email,idCard,sex,birthday,qq,homeAddress,livingAddress;
            public string description, picPath, politicalStatus, homeAddressId, livingAddressId,phone;
            public int type,auth,score;
            public DateTime birDT; 
            public Address fullHomeAddress, fullLivingAddress;
        }
        public struct Community
        {
            public string id, name,adminId,description;
            public Address address;
        }
        public struct Elderly
        {
            public string id, name, idCard, password32, sex, birthday, phoneNum, description,
                healthyType, livingAddress, guardianName, guardianPhone,community;
            public DateTime birDT, CraDT;
        }
        public struct Journal
        {
            public string id, description, dangyuanPic, politicalStatus,idCarcPic1,idCardPic2;
            public int auth, score;
        }
        public struct Article 
        {
            public int status;
            public string id, authorId, authorName, title, contentMd5, content,
                contentType, contentTypeName, contentTypeParent;
            public DateTime updateDT, createDT;
        }
        public struct Address 
        {
            public string name, id,parentId;
            public string province, city, cityzone;
            public string provinceId, cityId, cityzoneId;
            public string description;
        }
        

        public static List<Elderly> getElderlysByCommunityId(string communityId)
        {
            List<Elderly> elderly = new List<Elderly>();

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql;
                sql = "select tb_user.id_i as userId, tb_user.name_c as userName,tb_user.sex_c as userSex,"+
                    " tb_user.birthday_d as userBirth,tb_user.idCard_c as userIdCard,"+
                    " tb_user.livingPlace_c as userAddr,tb_user.phone_c as userPhone,"+
                    " tb_elderly.healthyType_i as userHealthy,tb_elderly.guardianName_c as userGuardianName,"+
                    " tb_elderly.guardianPhone_c as userGuardianPhone,tb_elderly."+
                    " description_c as userDes from tb_user,tb_elderly" +
                    " where tb_user.id_i = tb_elderly.id_i"+
                    " and tb_elderly.community_i = " + communityId;
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                for (int i = 0; i < rs.Rows.Count; i++)
                {
                    Elderly tmp = new Elderly();
                    tmp.id = rs.Rows[i]["userId"].ToString();
                    tmp.name = rs.Rows[i]["userName"].ToString();
                    tmp.sex = rs.Rows[i]["userSex"].ToString();
                    tmp.idCard = rs.Rows[i]["userIdCard"].ToString();
                    tmp.birthday = rs.Rows[i]["userBirth"].ToString();
                    tmp.guardianName = rs.Rows[i]["userGuardianName"].ToString();
                    tmp.guardianPhone = rs.Rows[i]["userGuardianPhone"].ToString();
                    tmp.healthyType = rs.Rows[i]["userHealthy"].ToString();
                    tmp.phoneNum = rs.Rows[i]["userPhone"].ToString();
                    tmp.description = rs.Rows[i]["userDes"].ToString();
                    elderly.Add(tmp);
                }
            }
            catch
            {
                ;
            }
            return elderly;
        }
        public static Community getCommunityByAdmin(string adminId)
        {
            Community community = new Community();
            community.adminId = adminId;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "select id_i,name_c,address_c from tb_community where adminId_i = " + adminId ;
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                if (rs.Rows.Count == 1)
                {
                    community.id = rs.Rows[0]["id_i"].ToString();
                    community.name = rs.Rows[0]["name_c"].ToString();
                    community.address.description = rs.Rows[0]["address_c"].ToString();
                }
            }
            catch (Exception)
            {
                ;
            }
            return community;
        }
        public static string addElderly(Elderly elderly)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adr ;
            string sql;
            int status;
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                sql = "insert tb_user(name_c,sex_c,birthday_d,idCard_c,livingPlace_c,phone_c) "+
                    "values('"+elderly.name+"','"+
                    elderly.sex+"','"+
                    elderly.birthday+"','"+
                    elderly.idCard +"','"+
                    elderly.livingAddress + "','" +
                    elderly.phoneNum + "')";
                cmd = new SqlCommand(sql, conn);
                status = cmd.ExecuteNonQuery();
                if (status < 1)
                {
                    return "创建用户 " + elderly.name + " 失败";
                }
                sql = "select id_i from tb_user where idCard_c = '" + elderly.idCard + "'";
                cmd.CommandText = sql;
                adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                if (rs.Rows.Count != 1)
                {
                    return "创建用户 " + elderly.name + " 出现异常";
                }
                elderly.id = rs.Rows[0][0].ToString();
                int tmpHealthy = getHealthyIdByName(elderly.healthyType);
                sql = "insert into tb_elderly(id_i,status_i,healthyType_i,guardianName_c,"+
                    "guardianPhone_c,description_c,community_i) " +
                    "values("+elderly.id+",1,"+tmpHealthy+",'"+elderly.guardianName+"','"+
                    elderly.guardianPhone+"','"+elderly.description+"',"+
                    elderly.community+")";
                cmd.CommandText = sql;
                status = cmd.ExecuteNonQuery();
                if (status !=0)
                {
                    return "创建用户 " + elderly.name + " 成功";
                }
            }
            catch (Exception)
            {
                return "创建用户 " + elderly.name + " 失败";
            }
            return "";
        }
        public static int getHealthyIdByName(string name)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "select id_i from tb_healthyType where name_c = '" + name+"'";
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                if (rs.Rows.Count == 1)
                {
                    return int.Parse(rs.Rows[0][0].ToString());
                }
            }
            catch (Exception)
            {
                ;
            }
            return 0;
        }
        public static Address getFullAddress(string cityzoneId)
        {
            Address addr = new Address();
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql;
                sql = "select tb_province.id_i as ProvinceId,tb_province.name_c as ProvinceName,"+
                    " tb_city.id_i as CityId, tb_city.name_c as CityName,"+
                    " tb_cityzone.id_i as CityzoneId,tb_cityzone.name_c as CityzoneName"+
                    " from tb_city,tb_cityzone,tb_province"+
                    " where tb_city.provinceId_i = tb_province.id_i"+
                    " and tb_city.id_i = tb_cityzone.cityId_i" +
                    " and tb_cityzone.id_i=" + cityzoneId;
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                if (rs.Rows.Count == 1)
                {
                    addr.province = rs.Rows[0]["ProvinceName"].ToString();
                    addr.provinceId = rs.Rows[0]["ProvinceId"].ToString();
                    addr.cityId = rs.Rows[0]["CityId"].ToString();
                    addr.city = rs.Rows[0]["CityName"].ToString();
                    addr.cityzone = rs.Rows[0]["CityzoneName"].ToString();
                    addr.cityzoneId = rs.Rows[0]["CityzoneId"].ToString();
                }
            }
            catch
            {
                ;
            }
            return addr;
        }
        public static List<Address> getCityzones(string cityId)
        {
            List<Address> cityzones = new List<Address>();

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql;
                sql = "select * from tb_cityzone where cityId_i = " + cityId;
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                for (int i = 0; i < rs.Rows.Count; i++)
                {
                    Address addr = new Address();
                    addr.name = rs.Rows[i]["name_c"].ToString();
                    addr.id = rs.Rows[i]["id_i"].ToString();
                    cityzones.Add(addr);
                }
            }
            catch
            {
                ;
            }
            return cityzones;
        }
        public static List<Address> getCitys(string provinceId)
        {
            List<Address> citys = new List<Address>();

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql;
                sql = "select * from tb_city where provinceId_i = " + provinceId;
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                for (int i = 0; i < rs.Rows.Count; i++)
                {
                    Address addr = new Address();
                    addr.name = rs.Rows[i]["name_c"].ToString();
                    addr.id = rs.Rows[i]["id_i"].ToString();
                    citys.Add(addr);
                }
            }
            catch
            {
                ;
            }
            return citys;
        }
        public static List<Address> getProvinces()
        {
            List<Address> provinces = new List<Address>();
            
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql;
                sql = "select * from tb_province";
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                for (int i = 0; i < rs.Rows.Count; i++) 
                {
                    Address addr = new Address();
                    addr.name = rs.Rows[i]["name_c"].ToString();
                    addr.id = rs.Rows[i]["id_i"].ToString();
                    provinces.Add(addr);
                }
            }
            catch {
                ;
            }
            return provinces;
        }
        public static List<Article> getArticleByTypeName(string typeName)
        {
            List<Article> articles = new List<Article>();
            Article article;

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql;
                sql = "select tb_article.id_i as articleID, tb_user.name_c as AutherName, " +
                     "tb_user.id_i as AuthorID, tb_article.title_c as Title, t1.parentType_i as ContentTypeParent, " +
                     "tb_article.contentMd5_c as Md5,tb_article.content_t as Content, tb_article.status_i as Status, " +
                     "tb_article.type_i as ContentTypeID, t1.name_c as ContentTypeName, " +
                     "tb_article.createTime_dt as CreateTime,tb_article.updateTime_dt as UpdateTime " +
                     "from tb_article, tb_user, tb_contentType as t1 " +
                     "where tb_article.authorId_i = tb_user.id_i " +
                     "and tb_article.type_i = t1.id_i " +
                     "and (t1.name_c = '"+typeName+
                     "' or tb_article.type_i in ( select t2.id_i from tb_contentType as t2,tb_contentType as t3 "+
                     "where t3.id_i =t2.parentType_i and t3.name_c = '" + typeName + "')) order by tb_article.createTime_dt desc ";
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                for (int i = 0; i < rs.Rows.Count; i++)
                {
                    article = new Article();
                    article.id = rs.Rows[i]["articleID"].ToString();
                    article.authorName = rs.Rows[i]["AutherName"].ToString();
                    article.authorId = rs.Rows[i]["AuthorID"].ToString();
                    article.title = rs.Rows[i]["Title"].ToString();
                    article.contentMd5 = rs.Rows[i]["Md5"].ToString();
                    article.content = rs.Rows[i]["Content"].ToString();
                    article.contentType = rs.Rows[i]["ContentTypeID"].ToString();
                    article.contentTypeName = rs.Rows[i]["ContentTypeName"].ToString();
                    article.contentTypeParent = rs.Rows[i]["ContentTypeParent"].ToString();
                    try
                    {
                        article.status = int.Parse(rs.Rows[i]["Status"].ToString());
                    }
                    catch (Exception)
                    {
                        article.status = 0;
                    } 
                    try
                    {
                        article.createDT = DateTime.Parse(rs.Rows[i]["CreateTime"].ToString());
                        article.updateDT = DateTime.Parse(rs.Rows[i]["UpdateTime"].ToString());
                    }
                    catch (Exception)
                    {
                        article.createDT = DateTime.Now;
                        article.updateDT = DateTime.Now;
                    } 

                    articles.Add(article);
                }
            }
            catch (Exception)
            {
                ;
            }
            return articles;
        }
        public static Article getArticleById(string articleId)
        {
            Article article = new Article();
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql;
                    sql = "select tb_article.id_i as articleID, tb_user.name_c as AutherName, " +
                         "tb_user.id_i as AuthorID, tb_article.title_c as Title,  t1.parentType_i as ContentTypeParent," +
                         "tb_article.contentMd5_c as Md5,tb_article.content_t as Content, tb_article.status_i as Status," +
                         "tb_article.type_i as ContentTypeID, t1.name_c as ContentTypeName, " +
                         "tb_article.createTime_dt as CreateTime,tb_article.updateTime_dt as UpdateTime " +
                         "from tb_article, tb_user, tb_contentType as t1 " +
                         "where tb_article.authorId_i = tb_user.id_i " +
                         "and tb_article.type_i = t1.id_i " +
                         "and tb_article.id_i = "+articleId;
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                if(rs.Rows.Count == 1)
                {
                    article = new Article();
                    article.id = articleId;
                    article.authorName = rs.Rows[0]["AutherName"].ToString();
                    article.authorId = rs.Rows[0]["AuthorID"].ToString();
                    article.title = rs.Rows[0]["Title"].ToString();
                    article.contentMd5 = rs.Rows[0]["Md5"].ToString();
                    article.content = rs.Rows[0]["Content"].ToString();
                    article.contentType = rs.Rows[0]["ContentTypeID"].ToString();
                    article.contentTypeName = rs.Rows[0]["ContentTypeName"].ToString();
                    article.contentTypeParent = rs.Rows[0]["ContentTypeParent"].ToString();

                    try
                    {
                        article.status = int.Parse(rs.Rows[0]["Status"].ToString());
                    }
                    catch (Exception)
                    {
                        article.status = 0;
                    } 
                    try
                    {
                        article.createDT = DateTime.Parse(rs.Rows[0]["CreateTime"].ToString());
                        article.updateDT = DateTime.Parse(rs.Rows[0]["UpdateTime"].ToString());
                    }
                    catch (Exception)
                    {
                        article.createDT = DateTime.Now;
                        article.updateDT = DateTime.Now;
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
            return article;
        }
        public static List<Article> getArticleByTypeId(int type) 
        {
            List<Article> articles = new List<Article>();
            Article article;

            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql;
                if (type != 0)
                {
                    sql = "select tb_article.id_i as articleID, tb_user.name_c as AutherName, " +
                         "tb_user.id_i as AuthorID, tb_article.title_c as Title, t1.parentType_i as ContentTypeParent, " +
                         "tb_article.contentMd5_c as Md5,tb_article.content_t as Content, tb_article.status_i as Status, " +
                         "tb_article.type_i as ContentTypeID, t1.name_c as ContentTypeName, " +
                         "tb_article.createTime_dt as CreateTime,tb_article.updateTime_dt as UpdateTime " +
                         "from tb_article, tb_user, tb_contentType as t1 " +
                         "where tb_article.authorId_i = tb_user.id_i " +
                         "and tb_article.type_i = t1.id_i " +
                         "and ( tb_article.type_i = " + type +
                         " or tb_article.type_i in (select id_i from tb_contentType as t2" +
                         "where t2.parentType_i = " + type + " )) order by tb_article.createTime_dt desc ";
                }
                else {
                    sql = "select tb_article.id_i as articleID, tb_user.name_c as AutherName, " +
                         "tb_user.id_i as AuthorID, tb_article.title_c as Title, t1.parentType_i as ContentTypeParent, " +
                         "tb_article.contentMd5_c as Md5,tb_article.content_t as Content,  tb_article.status_i as Status," +
                         "tb_article.type_i as ContentTypeID, t1.name_c as ContentTypeName, " +
                         "tb_article.createTime_dt as CreateTime,tb_article.updateTime_dt as UpdateTime " +
                         "from tb_article, tb_user, tb_contentType as t1 " +
                         "where tb_article.authorId_i = tb_user.id_i " +
                         "and tb_article.type_i = t1.id_i order by tb_article.createTime_dt desc ";
                }
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                for (int i = 0; i < rs.Rows.Count; i++) 
                {
                    article = new Article();
                    article.id = rs.Rows[i]["articleID"].ToString();
                    article.authorName = rs.Rows[i]["AutherName"].ToString();
                    article.authorId = rs.Rows[i]["AuthorID"].ToString();
                    article.title = rs.Rows[i]["Title"].ToString();
                    article.contentMd5 = rs.Rows[i]["Md5"].ToString();
                    article.content = rs.Rows[i]["Content"].ToString();
                    article.contentType = rs.Rows[i]["ContentTypeID"].ToString();
                    article.contentTypeName = rs.Rows[i]["ContentTypeName"].ToString();
                    article.contentTypeParent = rs.Rows[i]["ContentTypeParent"].ToString();
                    try
                    {
                        article.status = int.Parse(rs.Rows[i]["Status"].ToString());
                    }
                    catch (Exception)
                    {
                        article.status = 0;
                    } 
                    try
                    {
                        article.createDT = DateTime.Parse(rs.Rows[i]["CreateTime"].ToString());
                        article.updateDT = DateTime.Parse(rs.Rows[i]["UpdateTime"].ToString());
                    }
                    catch (Exception)
                    {
                        article.createDT = DateTime.Now;
                        article.updateDT = DateTime.Now;
                    } 
                    articles.Add(article);
                }
            }
            catch (Exception)
            {
                ;
            }
            return articles;
        }
        public static bool addArticle(string uid,Article article)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "select * from tb_article where contentMd5_c = '" + article.contentMd5 + "'";
                int status;
                cmd = new SqlCommand(sql, conn);

                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                status = rs.Rows.Count;
                if (status != 0)
                {
                    return false;
                }

                sql = "insert into tb_article(title_c,authorId_i,status_i,type_i,contentMd5_c) values('" +
                article.title + "'," + uid + "," + article.status.ToString() + "," + article.contentType + ",'" + article.contentMd5 +
                "')";
                cmd.CommandText = sql;
                status = cmd.ExecuteNonQuery();
                if (status > 0)
                {
                    
                    sql = "declare @p varbinary(16) ;"+
                        "select @p=textptr(content_t) from tb_article where contentMd5_c = '"+
                        article .contentMd5+"' ;"+
                        "writetext tb_article.content_t @p '"+article.content+"'";
                    cmd.CommandText = sql;
                    status = cmd.ExecuteNonQuery();
                    if (status == -1)
                    {
                        cmd.Dispose();
                        conn.Close();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
            return false;
        }
        public static bool updateArticle(Article article)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "update tb_article set title_c='"+article.title+
                    "', status_i |= "+article.status+
                    ", type_i |= "+article.contentType+
                    ", contentMd5_c='"+article.contentMd5+
                    "' where id_i = "+article.id;
                int status;
                cmd = new SqlCommand(sql, conn);

                status = cmd.ExecuteNonQuery();
                if (status > 0)
                {
                    sql = "declare @p varbinary(16) ;" +
                        "select @p=textptr(content_t) from tb_article where id_i = " + article.id +
                        " ;writetext tb_article.content_t @p '" + article.content + "'";
                    cmd.CommandText = sql;
                    status = cmd.ExecuteNonQuery();
                    if (status == -1)
                    {
                        cmd.Dispose();
                        conn.Close();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
            return false;
        }
        public static bool deleteArticle(string articleId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "delete tb_article where id_i = "+articleId;
                int status;
                cmd = new SqlCommand(sql, conn);
                status = cmd.ExecuteNonQuery();
                if (status > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                ;
            }
            return false;
        }
        public static Dictionary<int, string> getContentType(int parentTypeId)
        {
            Dictionary<int, string> contentType = 
                new Dictionary<int,string>();
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = 
                    "select id_i,name_c from tb_contentType where parentType_i = " + parentTypeId.ToString();
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                for (int i = 0; i < rs.Rows.Count;i++ ) 
                {
                    int typeId = int.Parse(rs.Rows[i]["id_i"].ToString());
                    string typeName = rs.Rows[i]["name_c"].ToString();
                    contentType.Add(typeId, typeName);
                }
                return contentType;
            }
            catch (Exception)
            {
                ;
            }
            return null;
        }
        public static bool checkUserInput(string str)
        {
            string[] unlawfulChars = { "'", "\"", "<", ">", "-", " " };
            foreach (string s in unlawfulChars)
            {
                if (str.IndexOf(s) >= 0)
                    return false;
            }
            return true;
        }
        public static string getConfigValue(string key)
        {
            try
            {
                Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                return config.AppSettings.Settings[key].Value;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static bool setConfitKeyValue(string key, string value)
        {
            try
            {   
                Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                if (config.AppSettings.Settings[key].Value != "") 
                {
                    config.AppSettings.Settings.Remove(key);
                }
                config.AppSettings.Settings.Add(key,value);
                config.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string EncryptSHA256(string strPwd)
        {
            byte[] result = Encoding.Default.GetBytes(strPwd);
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] output = sha256.ComputeHash(result);
            string str = BitConverter.ToString(output).Replace("-", "");
            return str.ToUpper();
        }
        public static string EncryptMd5(string strPwd)
        {
            byte[] result = Encoding.Default.GetBytes(strPwd);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string str = BitConverter.ToString(output).Replace("-", "");
            return str.ToUpper();
        }
        public static bool isLoginSuccess(string uid, string passwd)
        {
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
                sql += getUsernameSql(uid);
                cmd = new SqlCommand(sql, conn);
                adr = new SqlDataAdapter(cmd);
                adr.Fill(dataset);
                rs = dataset.Tables[0];
                if (rs.Rows.Count == 1 &&
                    (passwd.Length == 32 ? EncryptSHA256(passwd).ToLower() : EncryptSHA256(EncryptMd5(passwd).ToLower()).ToLower())
                    == rs.Rows[0]["passwd_c"].ToString().Trim())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                ;
            }
            return false;
        }
        public static string getUsernameSql(string namestr)
        {
            string str = "";
            try
            {
                Int32.Parse(namestr);
                str = " id_i = " + namestr + " ";
            }
            catch (Exception)
            {
                if (namestr.IndexOf("@") > 0)
                {
                    str = " email_c = '" + namestr + "' ";
                }
                else
                {
                    str = " name_c = '" + namestr + "' ";
                }
            }
            return str;
        }
        public static string getTypePath(string type)
        {
            try
            {
                int.Parse(type);
            }
            catch (Exception)
            {
                return "#" + type;
            }
            if ((int.Parse(type) & 1) != 0)
            {
                return "Journal/Journal.aspx";
            }
            else if ((int.Parse(type) & 2) != 0)
            {
                return "Community/Community.aspx";
            }
            else if ((int.Parse(type) & 4) != 0)
            {
                return "Elderly/Elderly.aspx";
            }
            else if ((int.Parse(type) & 8) != 0)
            {
                return "Admin/Default.aspx";
            }
            else
                return "#" + type;
        }
        public static string getTypePath(int type)
        {
            if ((type & 1) != 0)
            {
                return "Journal/Journal.aspx";
            }
            else if ((type & 2) != 0)
            {
                return "Community/Community.aspx";
            }
            else if ((type & 4) != 0)
            {
                return "Elderly/Elderly.aspx";
            }
            else if ((type & 8) != 0)
            {
                return "Admin/Default.aspx";
            }
            else
                return "#" + type;
        }
        public static bool updateUserPicPath(string uid, string picType)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "update tb_user set picPath_c = '"+uid+picType+"' where id_i = "+uid;
                cmd = new SqlCommand(sql, conn);
                int status =cmd.ExecuteNonQuery();
                if (status == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                ;
            }
            return false;
        }
        public static bool updateUserInformation(User user)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "update tb_user set name_c = '"+user.name+""+
                    "',phone_c = '"+user.phone +
                    "',birthday_d='"+ user.birthday +
                    "',sex_c='"+user.sex+
                    "',homeAddress_c='"+user.homeAddress+
                    "',livingPlace_c='" + user.livingAddress +
                    "',homeAddress_i= " + user.homeAddressId +
                    " ,livingAddress_i= " + user.livingAddressId +
                    " where id_i = " + user.id;
                cmd = new SqlCommand(sql, conn);
                int status = cmd.ExecuteNonQuery();
                if (status == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                ;
            }
            return false;
        }
        public static string getUserPicPath(string uid)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "select picPath_c from tb_user where id_i = " + uid;
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adr = new SqlDataAdapter(cmd);
                DataSet dataset = new DataSet();
                adr.Fill(dataset);
                DataTable rs = dataset.Tables[0];
                if (rs.Rows.Count == 1)
                {
                    return rs.Rows[0][0].ToString();
                }
            }
            catch (Exception)
            {
                ;
            }
            return "";
        }
        public static bool rePassword(string uid,string newpassword)
        {
            string password = EncryptSHA256(newpassword).ToLower();
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                String sql = "update tb_user set passwd_c = '"+password+"' where id_i = " + uid;
                cmd = new SqlCommand(sql, conn);
                int status = cmd.ExecuteNonQuery();
                if (status == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                ;
            }
            return false;
        }
    }
}