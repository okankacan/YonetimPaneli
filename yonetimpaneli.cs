using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using MesajPaneli.Business;
using MesajPaneli.Models;
using MesajPaneli.Models.JsonPostModels;

namespace yonetimpaneli
{

    public class Degiskenler
    {
        public string hata { get; set; }
        public string title { get; set; }
        public string AltTitle { get; set; }
        public string Altkeywords { get; set; }
        public string Altdescription { get; set; }
        public string keywords { get; set; }
        public string description { get; set; }
        public string Phone { get; set; }
        public string txtgoogle { get; set; }
        public string txttwitter { get; set; }
        public string txtfacebook { get; set; }
        public string MansetAltiGetir { get; set; }
        public string Sidebar { get; set; }
        public string Veri { get; set; }
        public string logo { get; set; }
        public string nereden { get; set; }
        public string nereye { get; set; }
        public string originCode { get; set; }
        public string originName { get; set; }
        public string originAirportName { get; set; }
        public string ArriveName { get; set; }
        public string ArriveCode { get; set; }
        public string ArriveAirportName { get; set; }
        public string yurtici { get; set; }
        public string fiyat { get; set; }
        public string onresim { get; set; }
        public string anasayfaSecim { get; set; }
        public string slogangetir { get; set; }
        public string footercek { get; set; }
        public string kategori { get; set; }
        public string TopKategori { get; set; }
        public DataSet kullanicilarDataSet = new DataSet();
        public DataSet SiteBilgileriDataSet = new DataSet();
        public DataSet HavayollariDataset = new DataSet();
        public DataSet makaleDataset = new DataSet();
        public DataSet MakaleYurticiDataset = new DataSet();
        public DataSet MakaleYurtdisiDataset = new DataSet();
        public DataSet HaberDataset = new DataSet();
        public DataSet HaberKamHabDataset = new DataSet();
        public DataSet BenzerKonularDataSet = new DataSet();
        public DataSet SatisbilerileriDataset = new DataSet();
        public DataSet SitelerDataset = new DataSet();
        public DataSet GrafikDataset = new DataSet();
        public DataSet SmsListDataset = new DataSet();
        public DataSet AlarmDataSet = new DataSet();
        public static String YeniCalismaDB = ConfigurationManager.ConnectionStrings["YeniCalismaDB"].ConnectionString;
        public decimal ToplamKomisyon { get; set; }
        public decimal ToplamTutar { get; set; }
        public int Toplamislem { get; set; }
        public int ToplamRezervasyon { get; set; }
        public int DisHatIslem { get; set; }
        public int icHatIslem { get; set; }
        public decimal biletbankKalan { get; set; }
        public decimal kalanToplam { get; set; }

    }
    public class YonetimPaneli : Degiskenler
    {
        private static String BiletBankDB = "Server=ip no; Initial Catalog=dbname; User Id=username; Password=pwd; Trusted_Connection=False;Pooling=false;";

        SqlConnection baglanti = new SqlConnection(YeniCalismaDB);

        /// <summary>
        ///  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="gelen"></param>
        /// <returns></returns>
        public static string sqlkontrol(string gelen)
        {
            gelen = gelen.Replace("'", "");
            gelen = gelen.Replace("=", "");
            return gelen.ToString();
        }

        /// <summary>
        /// Yönetim paneline giris kısmını şifre ve admin kullanıcı adına bakara kontrol eder. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="_userName"></param>
        /// <param name="_password"></param>
        public void YonetimGiris(string _userName, string _password)
        {

            try
            {



                string sqlstr = "select * from Users where userName='" + sqlkontrol(_userName) + "' and password='" + sqlkontrol(_password) + "'";
                SqlCommand command = new SqlCommand(sqlstr, baglanti);
                baglanti.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    HttpContext.Current.Session.Add("adminadi", _userName);
                    HttpContext.Current.Response.Redirect("/yonetim/Default.aspx");
                }
                else
                {
                    hata = "Mail adresini ve şifrenizi doğru giriniz...";
                }


                oku.Close();
                baglanti.Close();
                baglanti.Dispose();


            }
            catch (Exception ex)
            {

                hata = ex.ToString();
            }



        }

        /// <summary>
        /// Sitenizin seo ayalarını getirmek için kullanılır.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void SiteAyarlariGetir()
        {
            try
            {

                string sqlstr = "select * from SiteSettings where id=1";
                SqlCommand command = new SqlCommand(sqlstr, baglanti);
                baglanti.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    title = oku["SiteName"].ToString();
                    description = oku["SiteDescription"].ToString();
                    keywords = oku["SiteKeywords"].ToString();
                    Phone = oku["SitePhone"].ToString();
                    txtfacebook = oku["facebooklink"].ToString();
                    txttwitter = oku["twitterlink"].ToString();
                    txtgoogle = oku["googlepluslink"].ToString();
                    MansetAltiGetir = oku["DefeultMansetAlti"].ToString();
                    Sidebar = oku["sidebar"].ToString();
                    slogangetir = oku["slogan"].ToString();
                    footercek = oku["footer"].ToString();

                }
                else
                {
                    hata = "Site Bilgileriniz getirilirken bir hata oluştu...";
                }


                oku.Close();
                baglanti.Close();
                baglanti.Dispose();
            }
            catch (Exception)
            {

                hata = "Site Bilgileriniz getirilirken bir hata oluştu...";
            }



        }
        /// <summary>
        /// Seo ayarlarını değiştirmek için gereken parametlere değer verin .  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="keywords"></param>
        public void SiteAyalariDegistir(string title, string description, string keywords, string phone, string facebook, string twiiter, string google)
        {
            try
            {

                SqlCommand command = new SqlCommand("UPDATE SiteSettings SET SiteName=@title,SiteDescription=@description,SiteKeywords=@keywords,SitePhone=@Phone,facebooklink=@facebooklink,twitterlink=@twitterlink,googlepluslink=@googlepluslink WHERE id=1 ", baglanti);
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@keywords", keywords);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@facebooklink", facebook);
                command.Parameters.AddWithValue("@twitterlink", twiiter);
                command.Parameters.AddWithValue("@googlepluslink", google);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Başarıyle Güncellendi...";
            }
            catch (Exception)
            {
                hata = "Seo Bilgileri güncellenirken bir hata oluştu!";
            }

        }

        /// <summary>
        /// Gönderdiğiniz parametreyi footer yazar.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="footertxt"></param>
        public void footerAyarlariDegistir(string footertxt)
        {
            try
            {
                SqlCommand command = new SqlCommand("Update SiteSettings Set footer='" + footertxt + "' where id=1", baglanti);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Footer değiştirme işlemi gerçekleşti";
            }
            catch (Exception ex)
            { hata = ex.ToString() + " Footer değiştirme işlemi başarısız"; }


        }

        /// <summary>
        /// footer yazılan yazıları cekmek için kullanılır.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void footerAyariCek()
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                SqlCommand command = new SqlCommand("Select footer from SiteSettings where id=1", baglanti1);
                baglanti1.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    footercek = oku["footer"].ToString();
                }
                oku.Close();
                baglanti1.Close();
                baglanti1.Dispose();
            }
            catch (Exception ex)
            {

                hata = ex.ToString() + " başarısız bir işlem gerçekleşti";
            }
        }

        /// <summary>
        /// Kullanıcıların listesi buradan bulabilirsiniz. Fakat sadece Admin girişi yapılarak gösterilir.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void KullaniciGetir()
        {
            try
            {
                var kullaniciaditanimla = HttpContext.Current.Session["adminadi"].ToString();
                if (kullaniciaditanimla == "admin")
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Users", baglanti);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(kullanicilarDataSet);
                    baglanti.Close();
                    baglanti.Dispose();
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Kullanıcı adı ve yetkilendirme işlemini gerekli parametreleri vererek gerçekleştirebilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="kullaniciadi"></param>
        /// <param name="sifre"></param>
        /// <param name="yetki"></param>
        public void KullaniciEkle(string kullaniciadi, string sifre, string yetki)
        {
            try
            {
                var kullaniciaditanimla = HttpContext.Current.Session["adminadi"].ToString();
                if (kullaniciaditanimla == "admin")
                {



                    SqlCommand command = new SqlCommand("insert into Users VALUES (@username,@sifre,@yetki)", baglanti);
                    command.Parameters.AddWithValue("@username", kullaniciadi);
                    command.Parameters.AddWithValue("@sifre", sifre);
                    command.Parameters.AddWithValue("@yetki", yetki);
                    baglanti.Open();
                    command.ExecuteNonQuery();
                    baglanti.Close();
                    baglanti.Dispose();
                    hata = "Kullanıcı Yetkilendirmesi Tamamlandı";
                }
                else
                    hata = "Kullanıcı Yetkilendirmesi için admin olarak giriş yapın";

            }
            catch (Exception)
            {
                hata = "Kullanıcı tanımlaması yaparken bir hata oluştu";
            }


        }


        /// <summary>
        /// Kullanıcının şifresini değiştirmek için gereken parametreyi verin.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="password"></param>
        public void KullaniciSifreDegistir(string password)
        {
            try
            {
                var kullaniciaditanimla = HttpContext.Current.Session["adminadi"].ToString();
                SqlCommand command = new SqlCommand("Update Users Set password=@password where userName='" + kullaniciaditanimla + "'", baglanti);
                command.Parameters.AddWithValue("@password", password);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Password değiştirme işlemi başarılı.";
            }
            catch (Exception)
            { hata = "Password değiştirme işlemi yapılırken hata oluştu."; }

        }

        /// <summary>
        /// Kullanıcı silmek için silinecek üyenin adını gönderin.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="username"></param>
        public void Kullanicisil(string username)
        {
            try
            {
                SqlCommand command = new SqlCommand("Delete from Users where userName='" + username + "'", baglanti);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Kullanıcı silme işlemi başarıyla gerçekleşti";
            }
            catch (Exception)
            { hata = "Kullanıcı silme işlemi hatalı"; }

        }

        /// <summary>
        /// Site bilgileri çekerek SiteBilgileriDataSet atar bu şekilde tabloları doldurabilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void SiteSettingsBilgileri()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SiteSEttings", baglanti);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(SiteBilgileriDataSet);
            baglanti.Close();
            baglanti.Dispose();
        }

        /// <summary>
        /// Slider Slogan eklemek için kullanabilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="slogan1"></param>
        public void SloganKaydet(string slogan1)
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                SqlCommand command = new SqlCommand("UPDATE SiteSettings SET slogan=@slogan1 WHERE id=1 ", baglanti1);
                command.Parameters.AddWithValue("@slogan1", slogan1);
                baglanti1.Open();
                command.ExecuteNonQuery();
                baglanti1.Close();
                baglanti1.Dispose();
                hata = "Başarıyle Güncellendi...";


            }
            catch (Exception ex)
            {
                //    hata = ex.ToString();
                hata = "Slogan Kaydetilirken hata oluştu.";
            }


        }


        /// <summary>
        ///  Default sayfasındaki banner altındaki slogan kısmını buradan parametreye değer vererek değiştirebilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="MansetAltiHeaderSlogani"></param>
        public void DefaultMansetAltidefaKaydet(string MansetAltiHeaderSlogani)
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                SqlCommand komut = new SqlCommand("UPDATE SiteSettings Set DefeultMansetAlti=@DefeultMansetAlti where id=1", baglanti1);
                komut.Parameters.AddWithValue("@DefeultMansetAlti", MansetAltiHeaderSlogani);
                baglanti1.Open();
                komut.ExecuteNonQuery();
                baglanti1.Close();
                baglanti1.Dispose();
                hata = "İşlem Başarılı.";

            }
            catch (Exception ex)
            {
                hata = ex.ToString() + " Günceleme yapılamadı.";
            }
        }


        /// <summary>
        ///  Sidebar değişiklik yapmak için bu method kullanılır.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="Sidebartxt"></param>
        public void SidebarIcerikKaydet(string Sidebartxt)
        {
            try
            {
                SqlCommand command = new SqlCommand("Update SiteSettings Set sidebar=@sidebar where id=1", baglanti);
                command.Parameters.AddWithValue("@sidebar", Sidebartxt);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Sidebar güncellendi";
            }
            catch (Exception ex)
            {
                hata = "Sidebar güncellenirken hata gelişti.";
            }

        }


        /// <summary>
        ///  havayollarının bilgileri almak için kullanılır.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void HavayollariDataGetir()
        {



            SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
            baglanti1.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM airlinesData", baglanti1);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(HavayollariDataset);
            baglanti1.Close();
            baglanti1.Dispose();

        }



        /// <summary>
        /// havayolu şirketini eklemek için gereken parametrelere değerleri verin.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="txttitle"></param>
        /// <param name="txtkeywords"></param>
        /// <param name="txtdescription"></param>
        /// <param name="txtdata"></param>
        /// <param name="txtlogo"></param>
        public void HavayollariDataEkle(string txttitle, string txtkeywords, string txtdescription, string txtdata, string txtlogo)
        {
            try
            {
                SqlCommand command = new SqlCommand("insert into airlinesData Values(@txttitle,@txtkeywords,@txtdescription,@txtdata,@txtlogo)", baglanti);
                command.Parameters.AddWithValue("@txttitle", txttitle);
                command.Parameters.AddWithValue("@txtkeywords", txtkeywords);
                command.Parameters.AddWithValue("@txtdescription", txtdescription);
                command.Parameters.AddWithValue("@txtdata", txtdata);
                command.Parameters.AddWithValue("@txtlogo", txtlogo);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Havayolu şirketi eklendi";
            }
            catch (Exception ex)
            {
                hata = "Havayolu şirketi eklenirken hata gerçekleşti";

            }

        }

        /// <summary>
        /// verdiğiniz id göre havayollarinin bilgilerini alır (title,keywords,description,Veri,logo) atar.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id"></param>
        public void HavayollariDataidGetir(int id1)
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                SqlCommand command = new SqlCommand("select * from airlinesData where id='" + id1 + "'", baglanti1);
                baglanti1.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    title = oku["title"].ToString();
                    keywords = oku["keywords"].ToString();
                    description = oku["description"].ToString();
                    Veri = oku["data"].ToString();
                    logo = oku["logo"].ToString();
                }
                else
                {
                    hata = "Havayolu getirilirken bir hata oluştu...";
                }

                baglanti1.Close();
                baglanti1.Dispose();
            }
            catch (Exception)
            {

                hata = "Havayolu getirilirken bir hata oluştu...";
            }

        }

        /// <summary>
        ///  title göre havayolu şirketi getir .  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="title"></param>
        public void HavayollariDatatitleGetir(string title)
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                SqlCommand command = new SqlCommand("select * from airlinesData where title='" + title + "'", baglanti1);
                baglanti1.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    AltTitle = oku["title"].ToString();
                    Altkeywords = oku["keywords"].ToString();
                    Altdescription = oku["description"].ToString();
                    Veri = oku["data"].ToString();
                    logo = oku["logo"].ToString();
                }
                else
                {
                    hata = "Havayolu getirilirken bir hata oluştu...";
                }

                baglanti1.Close();
                baglanti1.Dispose();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        ///  HAvayolu firmanlarının bilgilerini id ye göre güncellemek için gerekli parametrelere değer verin.   Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="txttitle"></param>
        /// <param name="txtkeywords"></param>
        /// <param name="txtdescription"></param>
        /// <param name="txtdata"></param>
        /// <param name="txtlogo"></param>
        public void HavayollariDataGuncelle(int id1, string txttitle, string txtkeywords, string txtdescription, string txtdata, string txtlogo)
        {

            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                SqlCommand command1 = new SqlCommand("UPDATE airlinesData Set title=@txttitle,keywords=@txtkeywords,description=@txtdescription,data=@txtdata,logo=@txtlogo where id='" + id1 + "'", baglanti1);
                command1.Parameters.AddWithValue("@txttitle", txttitle);
                command1.Parameters.AddWithValue("@txtkeywords", txtkeywords);
                command1.Parameters.AddWithValue("@txtdescription", txtdescription);
                command1.Parameters.AddWithValue("@txtdata", txtdata);
                command1.Parameters.AddWithValue("@txtlogo", txtlogo);
                if (baglanti1.State == ConnectionState.Closed)
                {
                    baglanti1.Open();
                }
                command1.ExecuteNonQuery();
                baglanti1.Close();
                baglanti1.Dispose();
                hata = "Havayolu şirketi güncelledi";
            }
            catch (Exception ex)
            { hata = ex.ToString() + "Havayolu şirketi güncelenirken bir hata gelişti"; }

        }

        /// <summary>
        /// verilen iddeki havayolu şirketini silmek için parametreye değer girin.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        public void HavayollariDataSil(int id1)
        {
            try
            {
                SqlCommand command = new SqlCommand("Delete airlinesData where id='" + id1 + "'", baglanti);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Belirtilen havayolu silindi";
            }
            catch (Exception ex)
            {
                hata = ex.ToString() + " Belirtilen havayolu silinirken hata oluştu";
            }


        }

        /// <summary>
        /// Makalelerin hepsini bir makaleDataset atar .  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void MakaleDataGetir()
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                baglanti1.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Makale", baglanti1);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(makaleDataset);
                baglanti1.Close();
                baglanti1.Dispose();
            }
            catch (Exception ex)
            { }


        }


        /// <summary>
        /// Makaleleriri verdiğiniz limit kadar tersten makaleDataset atar .  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="VeriSayisi"></param>
        public void MakaleDataGetir(int VeriSayisi)
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                baglanti1.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP " + VeriSayisi + " * FROM Makale order by id desc", baglanti1);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(makaleDataset);
                baglanti1.Close();
                baglanti1.Dispose();
            }
            catch (Exception ex)
            { }


        }

        /// <summary>
        ///  Makale ekleek için gereken parametreleri girin. txtanasayfaSecim, txtArriveName, txtArriveCode, txtArriveAirportName evet diyerek anasayfa görünmesini sağlayabilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="txttitle"></param>
        /// <param name="txtkeywords"></param>
        /// <param name="txtdescription"></param>
        /// <param name="txtveri"></param>
        /// <param name="nereden"></param>
        /// <param name="nereye"></param>
        /// <param name="originCode"></param>
        /// <param name="originName"></param>
        /// <param name="originAirportName"></param>
        /// <param name="fiyat"></param>
        /// <param name="yurtici"></param>
        /// <param name="txtonresim"></param>
        /// <param name="txtanasayfaSecim"></param>
        /// <param name="txtArriveName"></param>
        /// <param name="txtArriveCode"></param>
        /// <param name="txtArriveAirportName"></param>
        public void MakaleDataEkle(string txttitle1, string txtkeywords1, string txtdescription1, string txtveri1, string nereden1, string nereye1, string originCode1, string originName1, string originAirportName1, string fiyat1, string yurtici1, string txtonresim1, string txtanasayfaSecim1, string txtArriveName1, string txtArriveCode1, string txtArriveAirportName1)
        {
            try
            {
                SqlCommand command = new SqlCommand("insert into Makale  Values(@title,@keywords,@description,@veri,@nereden,@nereye,@originCode,@originName,@originAirportName,@fiyat,@yurtici,@onresim,@anasayfaSecim,@ArriveName,@ArriveCode,@ArriveAirportName)", baglanti);
                command.Parameters.AddWithValue("@title", txttitle1);
                command.Parameters.AddWithValue("@keywords", txtkeywords1);
                command.Parameters.AddWithValue("@description", txtdescription1);
                command.Parameters.AddWithValue("@veri", txtveri1);
                command.Parameters.AddWithValue("@nereden", nereden1);
                command.Parameters.AddWithValue("@nereye", nereye1);
                command.Parameters.AddWithValue("@originCode", originCode1);
                command.Parameters.AddWithValue("@originName", originName1);
                command.Parameters.AddWithValue("@originAirportName", originAirportName1);
                command.Parameters.AddWithValue("@fiyat", fiyat1);
                command.Parameters.AddWithValue("@yurtici", yurtici1);
                command.Parameters.AddWithValue("@onresim", txtonresim1);
                command.Parameters.AddWithValue("@anasayfaSecim", txtanasayfaSecim1);
                command.Parameters.AddWithValue("@ArriveName", txtArriveName1);
                command.Parameters.AddWithValue("@ArriveCode", txtArriveCode1);
                command.Parameters.AddWithValue("@ArriveAirportName", txtArriveAirportName1);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Makale ekleme işlemi başarılı";
            }
            catch (Exception ex)
            { hata = ex.ToString() + "Makale ekleme işlemi başarısız"; }

        }


        /// <summary>
        /// Anasayfa seçimi evet olan makaleleri getirip makaleDataset atar.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void MakaleDataAnasayfaGetir()
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                baglanti1.Open();
                string deger = "Evet";
                SqlCommand cmd = new SqlCommand("SELECT * FROM Makale where anasayfaSecim='" + deger + "'", baglanti1);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(makaleDataset);
                baglanti1.Close();
                baglanti1.Dispose();
            }
            catch (Exception ex)
            { }


        }

        /// <summary>
        /// idsi belirtilen makalenin verileri alır title,keywords,description,Veri,nereden,nereye,originCode,originName,originAirportName,fiyat,onresim,anasayfaSecim değişkenlerine atar.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        public void MakaleidDataGetir(string id1)
        {
            try
            {
                SqlCommand command = new SqlCommand("select * from Makale where id=@id1", baglanti);
                command.Parameters.AddWithValue("@id1", id1);
                baglanti.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    AltTitle = oku["title"].ToString();
                    Altkeywords = oku["keywords"].ToString();
                    Altdescription = oku["description"].ToString();
                    Veri = oku["veri"].ToString();
                    nereden = oku["nereden"].ToString();
                    nereye = oku["nereye"].ToString();
                    originCode = oku["originCode"].ToString();
                    originName = oku["originName"].ToString();
                    originAirportName = oku["originAirportName"].ToString();
                    fiyat = oku["fiyat"].ToString();
                    yurtici = oku["yurtici"].ToString();
                    onresim = oku["onresim"].ToString();
                    anasayfaSecim = oku["anasayfaSecim"].ToString();
                    ArriveName = oku["ArriveName"].ToString();
                    ArriveCode = oku["ArriveCode"].ToString();
                    ArriveAirportName = oku["ArriveAirportName"].ToString();
                }
                oku.Close();
                baglanti.Close();
                baglanti.Dispose();

            }
            catch (Exception ex)
            {
                hata = ex.ToString() + " makale seçimi yapılırken bir hata oluştu";
            }

        }

        /// <summary>
        /// Makale düzeltme işlemi için gereken parametreleri girin.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="txttitle1"></param>
        /// <param name="txtkeywords1"></param>
        /// <param name="txtdescription1"></param>
        /// <param name="txtveri1"></param>
        /// <param name="nereden1"></param>
        /// <param name="nereye1"></param>
        /// <param name="originCode1"></param>
        /// <param name="originName1"></param>
        /// <param name="originAirportName1"></param>
        /// <param name="fiyat1"></param>
        /// <param name="yurtici1"></param>
        /// <param name="txtonresim1"></param>
        /// <param name="txtanasayfaSecim1"></param>
        /// <param name="txtArriveName1"></param>
        /// <param name="txtArriveCode1"></param>
        /// <param name="txtArriveAirportName1"></param>
        public void MAakleidDataDuzelt(string id1, string txttitle1, string txtkeywords1, string txtdescription1, string txtveri1, string nereden1, string nereye1, string originCode1, string originName1, string originAirportName1, string fiyat1, string yurtici1, string txtonresim1, string txtanasayfaSecim1, string txtArriveName1, string txtArriveCode1, string txtArriveAirportName1)
        {
            try
            {
                SqlCommand command = new SqlCommand("Update Makale Set title=@title,keywords=@keywords,description=@description,veri=@veri,nereden=@nereden,nereye=@nereye,originCode=@originCode,originName=@originName,originAirportName=@originAirportName,fiyat=@fiyat,yurtici=@yurtici,onresim=@onresim,anasayfaSecim=@anasayfaSecim,ArriveName=@ArriveName,ArriveCode=@ArriveCode,ArriveAirportName=@ArriveAirportName where id='" + id1 + "'", baglanti);
                command.Parameters.AddWithValue("@title", txttitle1);
                command.Parameters.AddWithValue("@keywords", txtkeywords1);
                command.Parameters.AddWithValue("@description", txtdescription1);
                command.Parameters.AddWithValue("@veri", txtveri1);
                command.Parameters.AddWithValue("@nereden", nereden1);
                command.Parameters.AddWithValue("@nereye", nereye1);
                command.Parameters.AddWithValue("@originCode", originCode1);
                command.Parameters.AddWithValue("@originName", originName1);
                command.Parameters.AddWithValue("@originAirportName", originAirportName1);
                command.Parameters.AddWithValue("@fiyat", fiyat1);
                command.Parameters.AddWithValue("@yurtici", yurtici1);
                command.Parameters.AddWithValue("@onresim", txtonresim1);
                command.Parameters.AddWithValue("@anasayfaSecim", txtanasayfaSecim1);
                command.Parameters.AddWithValue("@ArriveName", txtArriveName1);
                command.Parameters.AddWithValue("@ArriveCode", txtArriveCode1);
                command.Parameters.AddWithValue("@ArriveAirportName", txtArriveAirportName1);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Makale düzenleme işlemi başarılı";
            }
            catch (Exception ex)
            {

                { hata = ex.ToString() + "Makale düzeleme işlemi başarısız"; }
            }
        }

        /// <summary>
        ///  Belirtilen id deki makaleyi siler.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        public void MakaleDataSil(string id1)
        {
            try
            {
                SqlCommand command = new SqlCommand("Delete from Makale where id='" + id1 + "'", baglanti);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Silindi";
            }
            catch (Exception ex)
            {
                hata = ex.ToString() + " Silinirken bir hata gelişti.";
            }
        }

        /// <summary>
        /// Yurt içi olan makaleleri tersten sıralar.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void MakaleDataGetirYurtici()
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                baglanti1.Open();
                string deger = "Evet";
                SqlCommand cmd = new SqlCommand("SELECT * FROM Makale where yurtici='" + deger + "' ORDER BY id desc", baglanti1);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(MakaleYurticiDataset);
                baglanti1.Close();
                baglanti1.Dispose();

            }
            catch (Exception ex)
            { hata = ex.ToString() + " yurtiçi verileri çekilirken hata gelişti"; }


        }

        /// <summary>
        /// Yurt dışı makaleleri tersten sıralar.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void MakaleDataGetirYurtdisi()
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                baglanti1.Open();
                string deger = "Hayır";
                SqlCommand cmd = new SqlCommand("SELECT * FROM Makale where yurtici='" + deger + "' ORDER BY id desc", baglanti1);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(MakaleYurtdisiDataset);
                baglanti1.Close();
                baglanti1.Dispose();

            }
            catch (Exception ex)
            { hata = ex.ToString() + " yurtiçi verileri çekilirken hata gelişti"; }


        }

        /// <summary>
        /// News tablosundaki verileri HaberDataset atar.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void HaberDataGetir()
        {
            try
            {
                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                baglanti1.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM News", baglanti1);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(HaberDataset);
                baglanti1.Close();
                baglanti1.Dispose();
            }
            catch (Exception ex)
            { }


        }

        /// <summary>
        /// Topkategori ile kampanyaları ve haberleri getirebilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="topkagetori"></param>
        public void HaberDataGetir(string topkagetori)
        {
            try
            {
                topkagetori = topkagetori.Replace("/ucak-bileti/", "");
                topkagetori = topkagetori.Replace("/duyuru/", "");
                if ((topkagetori.Contains("Haberler") || (topkagetori.Contains("Kampanyalar"))))

                {
                    SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                    baglanti1.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM News where TopCategories='" + topkagetori + "'", baglanti1);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(HaberDataset);
                    baglanti1.Close();
                    baglanti1.Dispose();
                }
                else
                {
                    topkagetori = topkagetori.Replace("-", " ");
                    topkagetori = topkagetori.Replace("kampanyalari", "Kampanyaları");

                    SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                    baglanti1.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM News where NewsCategories='" + topkagetori.ToLower().ToString() + "'", baglanti1);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(HaberDataset);
                    baglanti1.Close();
                    baglanti1.Dispose();
                }
            }
            catch (Exception ex)
            { hata = ex.ToString(); }
        }

        /// <summary>
        ///  id verilen haberin detaylarını size verir.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id"></param>
        public void HaberDataGetir(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand("Select * from News where id='" + id + "'", baglanti);
                baglanti.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    AltTitle = oku["title"].ToString();
                    Altkeywords = oku["keywords"].ToString();
                    Altdescription = oku["description"].ToString();
                    onresim = oku["onresim"].ToString();
                    Veri = oku["veri"].ToString();
                    kategori = oku["TopCategories"].ToString();
                    TopKategori = oku["NewsCategories"].ToString();
                }
                else
                {
                    hata = "Veri Okunurken Hata oluştu";
                }
                oku.Close();
                baglanti.Close();
                baglanti.Dispose();
            }
            catch (Exception ex)
            {
                hata = ex.ToString() + " Veri Okunurken Hata Oluştu";
            }
        }

        /// <summary>
        /// Havayolu firmalarına ait Kampanyaları çekmek için bu methodu kullanabilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="topkagetori"></param>
        public void HavayoluKampanyaGetir(string topkagetori)
        {
            try
            {
                string kampanyacek = "";

                topkagetori = topkagetori.Replace("/ucak-bileti/", "");
                topkagetori = topkagetori.Replace("/duyuru/", "");
                topkagetori = topkagetori.Replace("-", " ");

                topkagetori = topkagetori.Replace("kampanyalari", "Kampanyaları");

                var deger = topkagetori.IndexOf("Kampanyaları");
                if (deger == -1)
                    kampanyacek = topkagetori + " Kampanyaları";

                SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                baglanti1.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM News where NewsCategories='" + kampanyacek.ToLower().ToString() + "'", baglanti1);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(HaberDataset);
                baglanti1.Close();
                baglanti1.Dispose();

            }
            catch (Exception ex)
            { hata = ex.ToString(); }
        }

        /// <summary>
        /// top kategorisi verilen kayıtların bilgilerden birer   Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="topkagetori"></param>
        public void HaberAltDataGetir(string topkagetori)
        {
            try
            {
                topkagetori = topkagetori.Replace("/ucak-bileti/", "");
                topkagetori = topkagetori.Replace("/duyuru/", "");
                if ((topkagetori.Contains("Haberler") || (topkagetori.Contains("Kampanyalar"))))

                {
                    string kampanya = "";
                    if (topkagetori.Contains("Kampanyalar"))
                    {
                        kampanya = "Kampanyalar";
                    }
                    else
                    {
                        kampanya = "Haberler";
                    }

                    SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                    baglanti1.Open();
                    SqlCommand cmd = new SqlCommand("SELECT distinct NewsCategories FROM News where TopCategories='" + kampanya + "'", baglanti1);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(HaberKamHabDataset);

                    baglanti1.Close();
                    baglanti1.Dispose();
                }
                else

                {
                    string kampanya = "";
                    if (topkagetori.Contains("kampanyalari"))
                    {
                        kampanya = "Kampanyalar";
                    }
                    else
                    {
                        kampanya = "Haberler";
                    }
                    SqlConnection baglanti1 = new SqlConnection(YeniCalismaDB);
                    baglanti1.Open();
                    SqlCommand cmd = new SqlCommand("SELECT distinct NewsCategories FROM News where TopCategories='" + kampanya + "'", baglanti1);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(HaberKamHabDataset);

                    baglanti1.Close();
                    baglanti1.Dispose();
                }


            }
            catch (Exception ex)
            { hata = ex.ToString(); }
        }
        /// <summary>
        ///  Gereken parametrelere değer vererek haber ekleme işlemi yapabilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="title1"></param>
        /// <param name="keywords1"></param>
        /// <param name="description1"></param>
        /// <param name="onresim1"></param>
        /// <param name="veri1"></param>
        /// <param name="NewsCategories1"></param>
        ///   /// <param name="NewsCategoriesTop"></param>
        ///   

        public void HaberInsert(string title1, string keywords1, string description1, string onresim1, string veri1, string NewsCategories1, string NewsCategoriesTop1)
        {
            try
            {
                SqlCommand command = new SqlCommand("insert into News Values(@title, @keywords, @description, @onresim, @veri,@NewsCategories,@NewsCategoriesTop)", baglanti);
                command.Parameters.AddWithValue("@title", title1);
                command.Parameters.AddWithValue("@keywords", keywords1);
                command.Parameters.AddWithValue("@description", description1);
                command.Parameters.AddWithValue("@onresim", onresim1);
                command.Parameters.AddWithValue("@veri", veri1);
                command.Parameters.AddWithValue("@NewsCategories", NewsCategories1);
                command.Parameters.AddWithValue("@NewsCategoriesTop", NewsCategoriesTop1);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "İşlem Başarılı";

            }
            catch (Exception ex)
            { hata = ex.ToString() + " işlem başarısız"; }
        }

        /// <summary>
        ///  id si verilen haberi güncellemek için parametrelere gereken değerleri girin.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="title1"></param>
        /// <param name="keywords1"></param>
        /// <param name="description1"></param>
        /// <param name="onresim1"></param>
        /// <param name="veri1"></param>
        /// <param name="NewsCategories1"></param>
        public void HaberUpdate(string id1, string title1, string keywords1, string description1, string onresim1, string veri1, string NewsCategories1, string NewsCategoriesTop)
        {
            try
            {
                SqlCommand command = new SqlCommand("Update News Set title=@title, keywords=@keywords, description=@description, onresim=@onresim, veri=@veri, NewsCategories=@NewsCategories, TopCategories=@NewsCategoriesTop where id='" + id1 + "'", baglanti);
                command.Parameters.AddWithValue("@title", title1);
                command.Parameters.AddWithValue("@keywords", keywords1);
                command.Parameters.AddWithValue("@description", description1);
                command.Parameters.AddWithValue("@onresim", onresim1);
                command.Parameters.AddWithValue("@veri", veri1);
                command.Parameters.AddWithValue("@NewsCategories", NewsCategories1);
                command.Parameters.AddWithValue("@NewsCategoriesTop", NewsCategoriesTop);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Güncelleme işlemi Başarılı";

            }
            catch (Exception ex)
            { hata = ex.ToString() + " Güncelleme işlemi başarısız"; }
        }

        /// <summary>
        /// News tablosunda id göre haber verisi getirir.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        public void HaberSelect(string id1, string TopCategories1)
        {
            try
            {

                SqlCommand command = new SqlCommand("Select * from News where id='" + id1 + "' and TopCategories='" + TopCategories1 + "'", baglanti);
                baglanti.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    AltTitle = oku["title"].ToString();
                    Altkeywords = oku["keywords"].ToString();
                    Altdescription = oku["description"].ToString();
                    onresim = oku["onresim"].ToString();
                    Veri = oku["veri"].ToString();
                    kategori = oku["NewsCategories"].ToString();
                    TopKategori = oku["TopCategories"].ToString();
                }
                oku.Close();
                baglanti.Close();
                baglanti.Dispose();
                hata = "işlem başarılı";
            }
            catch (Exception ex)
            { hata = ex.ToString(); }
        }

        /// <summary>
        /// Benzer konular tablosundaki verileri verir. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void BenzerKonularDataGetir()
        {
            try
            {

                baglanti.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM benzerKonular", baglanti);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(BenzerKonularDataSet);
                baglanti.Close();
                baglanti.Dispose();
            }
            catch (Exception ex)
            { }


        }

        /// <summary>
        /// id verilen makalenin içeriğini getirir. AltTitle,Altkeywords,Altdescription,Veri atar.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        public void BenzerKonularSelect(string id1)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(YeniCalismaDB);
                SqlCommand command = new SqlCommand("Select * from benzerKonular where id='" + id1 + "'", baglanti);
                baglanti.Open();
                SqlDataReader oku = command.ExecuteReader();
                if (oku.Read())
                {
                    AltTitle = oku["title"].ToString();
                    Altkeywords = oku["keywords"].ToString();
                    Altdescription = oku["description"].ToString();
                    Veri = oku["veri"].ToString();
                }
                oku.Close();
                baglanti.Close();
                baglanti.Dispose();
                hata = "işlem başarılı";
            }
            catch (Exception ex)
            { hata = ex.ToString() + " işlem başarısız"; }
        }

        /// <summary>
        /// BenzerKonularInsert methodu ile benzerKonular veri tabanına veri ekleyebilirsiniz.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="title1"></param>
        /// <param name="keywords1"></param>
        /// <param name="description1"></param>
        /// <param name="veri1"></param>
        public void BenzerKonularInsert(string title1, string keywords1, string description1, string veri1)
        {
            try
            {


                SqlCommand command = new SqlCommand("insert into benzerKonular Values(@title,@keywords, @description, @veri)", baglanti);
                command.Parameters.AddWithValue("@title", title1);
                command.Parameters.AddWithValue("@keywords", keywords1);
                command.Parameters.AddWithValue("@description", description1);
                command.Parameters.AddWithValue("@veri", veri1);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "İşlem Başarılı";
            }
            catch (Exception ex)
            {

                hata = ex.ToString() + " İşlem başarısız";
            }

        }

        /// <summary>
        /// Verilen iddeki içeriği verilen parametreler ile değiştirir.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="title1"></param>
        /// <param name="keywords1"></param>
        /// <param name="description1"></param>
        /// <param name="veri1"></param>
        public void BenzerKonularUpdate(string id1, string title1, string keywords1, string description1, string veri1)
        {
            try
            {


                SqlCommand command = new SqlCommand("Update BenzerKonular Set title=@title, keywords=@keywords, description=@description, veri=@veri where id='" + id1 + "'", baglanti);
                command.Parameters.AddWithValue("@title", title1);
                command.Parameters.AddWithValue("@keywords", keywords1);
                command.Parameters.AddWithValue("@description", description1);
                command.Parameters.AddWithValue("@veri", veri1);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Güncelleme işlemi Başarılı";

            }
            catch (Exception ex)
            { hata = ex.ToString() + " Güncelleme işlemi başarısız"; }
        }


        /// <summary>
        /// idsi verilen parametreyi benzerkonular tablosundan siler.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id1"></param>
        public void BenzerKonularSil(string id1)
        {
            try
            {
                SqlCommand command = new SqlCommand("Delete from benzerKonular where id='" + id1 + "'", baglanti);
                baglanti.Open();
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "başarılı";
            }
            catch (Exception ex)
            { hata = ex.ToString() + " hata gelişti"; }
        }

        /// <summary>
        /// Burada SatışBilgileri tablosundaki verileri alırsınız ve SatisbilerileriDataset atarsınız  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void SatisVerileriAl()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 600 * FROM SATISBILGILERI WHERE PNR<>'' order by sira desc ", baglanti);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(SatisbilerileriDataset, "SATISBILGILERI");
                baglanti.Close();
                baglanti.Dispose();
            }
            catch (Exception)
            { }
        }

        /// <summary>
        ///  arama yapılacak kelimeyi belirterek SatışBilgileri tablosundaki verileri alırsınız ve SatisbilerileriDataset atarsınız Tip(Satış için true ,Rezervasyon için false) olmalı  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="search"></param>
        public void SatisVerileriAl(string search, bool Tip)
        {
            try
            {
                if (Tip == true)
                {
                    SqlConnection baglanti = new SqlConnection(BiletBankDB);
                    baglanti.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM SATISBILGILERI where  (PNR LIKE '" + search + "%' OR AD LIKE '" + search + "%' OR SOYAD LIKE '" + search + "%') and PNR<>''", baglanti);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(SatisbilerileriDataset, "SATISBILGILERI");
                    baglanti.Close();
                    baglanti.Dispose();
                }
                else
                {
                    SqlConnection baglanti = new SqlConnection(BiletBankDB);
                    baglanti.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM SATISBILGILERI where (PNR LIKE '" + search + "%' OR AD LIKE '" + search + "%' OR SOYAD LIKE '" + search + "%') and PNR=''", baglanti);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(SatisbilerileriDataset, "SATISBILGILERI");
                    baglanti.Close();
                    baglanti.Dispose();
                }




            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Verilen tarihler arasındaki veriyi alır  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="BugunTarihi"></param>
        /// <param name="GelenTarih"></param>
        public void SatisVerileriAl(string BugunTarihi, string GelenTarih)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("select * from SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'') order by sira desc", baglanti);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(SatisbilerileriDataset, "SATISBILGILERI");
                baglanti.Close();
                baglanti.Dispose();
            }
            catch (Exception)
            {
            }


        }

        /// <summary>
        ///  Verilen tarihler arasındaki veriyi alır. Gelen Tip ("Hesapla" yada "Rezervasyon") olarak gelmesi gerekiyor.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="BugunTarihi"></param>
        /// <param name="GelenTarih"></param>
        /// <param name="Tip"></param>
        public void SatisVerileriAl(string BugunTarihi, string GelenTarih, string Tip)
        {
            try
            {
                if (Tip == "Hesapla")
                {
             

                    SqlConnection baglanti = new SqlConnection(BiletBankDB);
                    baglanti.Open();
                    SqlCommand cmd = new SqlCommand("SELECT SUM (HIZMETBEDELI) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'')", baglanti);
                    SqlCommand cmd1 = new SqlCommand("SELECT SUM (TOPLAMTUTAR) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'')", baglanti);
                    SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'')", baglanti);
                    SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR='')", baglanti);
                    SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and HIZMETBEDELI>49)", baglanti);
                    SqlCommand cmd5 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and HIZMETBEDELI<49)", baglanti);
                    SqlCommand cmd6 = new SqlCommand("select * from SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'') order by sira desc", baglanti);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd6;
                    da.Fill(SatisbilerileriDataset, "SATISBILGILERI");
                    var result = cmd.ExecuteScalar();
                    var result1 = cmd1.ExecuteScalar();
                    var result2 = cmd2.ExecuteScalar();
                    var result3 = cmd3.ExecuteScalar();
                    var result4 = cmd4.ExecuteScalar();
                    var result5 = cmd5.ExecuteScalar();
                    ToplamKomisyon = Convert.ToDecimal(result);
                    ToplamTutar = Convert.ToDecimal(result1);
                    Toplamislem = Convert.ToInt32(result2);
                    ToplamRezervasyon = Convert.ToInt32(result3);
                    DisHatIslem = Convert.ToInt32(result4);
                    icHatIslem = Convert.ToInt32(result5);
                    biletbankKalan = ((DisHatIslem * 5) + (icHatIslem * 2));
                    kalanToplam = ToplamKomisyon - biletbankKalan;
                    baglanti.Close();
                    baglanti.Dispose();
                }
                else if (Tip == "Rezervasyon")
                {
                    SqlConnection baglanti = new SqlConnection(BiletBankDB);
                    baglanti.Open();
                    SqlCommand cmd = new SqlCommand("select * from SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR='') order by sira desc", baglanti);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(SatisbilerileriDataset, "SATISBILGILERI");
                    baglanti.Close();
                    baglanti.Dispose();
                }
                else if (Tip != "Rezervasyon" || Tip != "Hesapla")
                {
                    if (Tip== "Lütfen Seçiniz")
                    {
                        SqlConnection baglanti = new SqlConnection(BiletBankDB);
                        baglanti.Open();
                        SqlCommand cmd = new SqlCommand("SELECT SUM (HIZMETBEDELI) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'')", baglanti);
                        SqlCommand cmd1 = new SqlCommand("SELECT SUM (TOPLAMTUTAR) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'')", baglanti);
                        SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'')", baglanti);
                        SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR='')", baglanti);
                        SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and HIZMETBEDELI>49)", baglanti);
                        SqlCommand cmd5 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and HIZMETBEDELI<49)", baglanti);
                        SqlCommand cmd6 = new SqlCommand("select * from SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'') order by sira desc", baglanti);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd6;
                        da.Fill(SatisbilerileriDataset, "SATISBILGILERI");

                        var result = cmd.ExecuteScalar();
                        var result1 = cmd1.ExecuteScalar();
                        var result2 = cmd2.ExecuteScalar();
                        var result3 = cmd3.ExecuteScalar();
                        var result4 = cmd4.ExecuteScalar();
                        var result5 = cmd5.ExecuteScalar();
                        ToplamKomisyon = Convert.ToDecimal(result);
                        ToplamTutar = Convert.ToDecimal(result1);
                        Toplamislem = Convert.ToInt32(result2);
                        ToplamRezervasyon = Convert.ToInt32(result3);
                        DisHatIslem = Convert.ToInt32(result4);
                        icHatIslem = Convert.ToInt32(result5);
                        biletbankKalan = ((DisHatIslem * 5) + (icHatIslem * 2));
                        kalanToplam = ToplamKomisyon - biletbankKalan;


                        baglanti.Close();
                        baglanti.Dispose();
                    }
                    else
                    {
                        SqlConnection baglanti = new SqlConnection(BiletBankDB);
                        baglanti.Open();
                        SqlCommand cmd = new SqlCommand("SELECT SUM (HIZMETBEDELI) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Tip + "' )", baglanti);
                        SqlCommand cmd1 = new SqlCommand("SELECT SUM (TOPLAMTUTAR) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Tip + "' )", baglanti);
                        SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Tip + "' )", baglanti);
                        SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR='' and ALTACENTELINK='" + Tip + "' )", baglanti);
                        SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Tip + "' and HIZMETBEDELI>49)", baglanti);
                        SqlCommand cmd5 = new SqlCommand("SELECT COUNT(*) FROM SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Tip + "' and HIZMETBEDELI<49)", baglanti);
                        SqlCommand cmd6 = new SqlCommand("select * from SATISBILGILERI WHERE (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Tip + "') order by sira desc", baglanti);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd6;
                        da.Fill(SatisbilerileriDataset, "SATISBILGILERI");

                        var result = cmd.ExecuteScalar();
                        var result1 = cmd1.ExecuteScalar();
                        var result2 = cmd2.ExecuteScalar();
                        var result3 = cmd3.ExecuteScalar();
                        var result4 = cmd4.ExecuteScalar();
                        var result5 = cmd5.ExecuteScalar();
                        ToplamKomisyon = Convert.ToDecimal(result);
                        ToplamTutar = Convert.ToDecimal(result1);
                        Toplamislem = Convert.ToInt32(result2);
                        ToplamRezervasyon = Convert.ToInt32(result3);
                        DisHatIslem = Convert.ToInt32(result4);
                        icHatIslem = Convert.ToInt32(result5);
                        biletbankKalan = ((DisHatIslem * 5) + (icHatIslem * 2));
                        kalanToplam = ToplamKomisyon - biletbankKalan;


                        baglanti.Close();
                        baglanti.Dispose();
                    }
                  
                }
            }
            catch (Exception)
            {
            }


        }
        /// <summary>
        ///  gelen tarihe göre satışbilgileri tablosunun grafik oluşturur. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="BugunTarihi"></param>
        /// <param name="GelenTarih"></param>
        /// <param name="Siteismi"></param>
        public void SatisGrafigi(string BugunTarihi, string GelenTarih, string Siteismi)
        {
            try
            {
                if (Siteismi == "Lütfen Seçiniz")
                {
                    SqlConnection baglanti = new SqlConnection(BiletBankDB);
                    baglanti.Open();
                    var sorgu = "select havayolu, count(CASE WHEN(GMT >='" + BugunTarihi + "' and GMT <= '" + GelenTarih + "' and PNR <> '') THEN 1 END) toplam, sum(CASE WHEN(GMT >='" + BugunTarihi + "' and GMT <= '" + GelenTarih + "' and PNR <> '' and HIZMETBEDELI < 49) THEN 1 END) ichat, sum(CASE WHEN(GMT >='" + BugunTarihi + "' and GMT <= '" + GelenTarih + "' and PNR <> '' and HIZMETBEDELI > 49)  THEN 1 END) dishat from SATISBILGILERI group by havayolu";
                    SqlCommand cmd2 = new SqlCommand(sorgu, baglanti);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd2;
                    da.Fill(GrafikDataset, "SATISBILGILERI");
                    baglanti.Close();
                    baglanti.Dispose();
                }
                else
                {
                    SqlConnection baglanti = new SqlConnection(BiletBankDB);
                    baglanti.Open();
                    var sorgu = "select havayolu, count(CASE WHEN(GMT >='" + BugunTarihi + "' and GMT <= '" + GelenTarih + "' and PNR <> '' and ALTACENTELINK='" + Siteismi + "') THEN 1 END) toplam, sum(CASE WHEN(GMT >='" + BugunTarihi + "' and GMT <= '" + GelenTarih + "' and PNR <> '' and HIZMETBEDELI < 49 and ALTACENTELINK='" + Siteismi + "') THEN 1 END) ichat, sum(CASE WHEN(GMT >='" + BugunTarihi + "' and GMT <= '" + GelenTarih + "' and PNR <> '' and HIZMETBEDELI > 49 and ALTACENTELINK='" + Siteismi + "')  THEN 1 END) dishat from SATISBILGILERI group by havayolu";
                    SqlCommand cmd2 = new SqlCommand(sorgu, baglanti);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd2;
                    da.Fill(GrafikDataset, "SATISBILGILERI");
                    baglanti.Close();
                    baglanti.Dispose();
                }


            }
            catch (Exception)
            { }

        }
        /// <summary>
        /// Satış yapılan sitelerin hepsini seçmek için kullanılır.   Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır. /// </summary>
        public void SiteleriCek()
        {
            try
            {
                DateTime simdikitarih = DateTime.Now;
               
                string bitissec = simdikitarih.ToString("yyyy-MM-dd") + " 23:59:59";

                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                 SqlCommand cmd3 = new SqlCommand("select count(*), ALTACENTELINK from SATISBILGILERI WHERE  (GMT>='2016-04-07 00:00:00' and GMT<='" + bitissec + "' and PNR<>'') Group By ALTACENTELINK", baglanti);
                 SqlDataAdapter da2 = new SqlDataAdapter();
                 da2.SelectCommand = cmd3;
                 da2.Fill(SitelerDataset, "SATISBILGILERI");
                baglanti.Close();
                baglanti.Dispose();
            }
            catch (Exception)
            { }

        }
        /// <summary>
        /// sadece id kısmını almak için kullandığımız bir parametre  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="metin"></param>
        /// <returns></returns>
        public string UrlSifirla(string metin)
        {
            string str = metin.ToLower();
            str = str.Replace("-", "");
            str = str.Replace(" ", "");
            str = str.Replace("A", "");
            str = str.Replace("B", "");
            str = str.Replace("C", "");
            str = str.Replace("Ç", "");
            str = str.Replace("ç", "");
            str = str.Replace("D", "");
            str = str.Replace("E", "");
            str = str.Replace("F", "");
            str = str.Replace("G", "");
            str = str.Replace("Ğ", "");
            str = str.Replace("ğ", "");
            str = str.Replace("H", "");
            str = str.Replace("I", "");
            str = str.Replace("ı", "");
            str = str.Replace("İ", "");
            str = str.Replace("J", "");
            str = str.Replace("K", "");
            str = str.Replace("L", "");
            str = str.Replace("M", "");
            str = str.Replace("N", "");
            str = str.Replace("O", "");
            str = str.Replace("Ö", "");
            str = str.Replace("ö", "");
            str = str.Replace("P", "");
            str = str.Replace("R", "");
            str = str.Replace("S", "");
            str = str.Replace("Ş", "");
            str = str.Replace("ş", "");
            str = str.Replace("T", "");
            str = str.Replace("U", "");
            str = str.Replace("Ü", "");
            str = str.Replace("ü", "");
            str = str.Replace("W", "");
            str = str.Replace("X", "");
            str = str.Replace("Y", "");
            str = str.Replace("Z", "");
            str = str.Replace("\"", "");
            str = str.Replace("/", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("%", "");
            str = str.Replace("&", "");
            str = str.Replace("=", "");
            str = str.Replace("_", "");
            str = str.Replace("+", "");
            str = str.Replace(".", "");
            str = str.Replace("?", "");
            str = str.Replace(",", "");
            str = str.Replace("'", "-");
            str = str.Replace("!", "");
            str = str.Replace("#34;", "");
            str = str.Replace("quot;", "");
            str = str.Replace(";", "");
            str = str.Replace(">", "");
            str = str.Replace("<", "");
            str = str.Replace("$", "dolar");
            str = str.Replace("€", "euro");
            str = str.Replace("--", "-");
            str = str.Replace("q", "");
            str = str.Replace("w", "");
            str = str.Replace("e", "");
            str = str.Replace("r", "");
            str = str.Replace("t", "");
            str = str.Replace("y", "");
            str = str.Replace("u", "");
            str = str.Replace("o", "");
            str = str.Replace("p", "");
            str = str.Replace("a", "");
            str = str.Replace("s", "");
            str = str.Replace("d", "");
            str = str.Replace("f", "");
            str = str.Replace("g", "");
            str = str.Replace("h", "");
            str = str.Replace("j", "");
            str = str.Replace("k", "");
            str = str.Replace("l", "");
            str = str.Replace("i", "");
            str = str.Replace("z", "");
            str = str.Replace("x", "");
            str = str.Replace("c", "");
            str = str.Replace("v", "");
            str = str.Replace("b", "");
            str = str.Replace("n", "");
            str = str.Replace("m", "");
            return str;
        }

        /// <summary>
        ///   Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="metin"></param>
        /// <returns></returns>
        public string UrlidAl(string metin)
        {
            try
            {
                string str = metin.ToLower();
                int deger = str.Length - 4;
                string deger2 = str.Substring(deger);
                str = UrlSifirla(deger2);
                return str.ToString();
            }
            catch (Exception ex)
            { return ""; }


        }

        /// <summary>
        ///  Havayolları isimlerini getirir.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="marketinAirline"></param>
        /// <returns></returns>
        public String HavayoluAdlariGetir(String marketinAirline)
        {

            bool isExist = AirlineCodeToAirline.ContainsKey(marketinAirline);
            if (!isExist)
                return marketinAirline;
            return AirlineCodeToAirline[marketinAirline].AirlineName;

 
        }

        public SortedDictionary<String, String> mrktToCmpny = new SortedDictionary<string, string>() {
            {"TK","Turkish Airlines"},
            {"XQ","Sun Express"},
            {"KK","AtlasGlobal"},
            {"8Q","Onur Air"},
            {"JU","Jet Airways"},
            {"JP","Adria Airways"},
            {"PC","Pegasus"},
            {"4U","Germanwings"},
            {"EW","Eurowings"},
            {"KL","KLM Royal Dutch Airlines"},
            {"RO","Tarom"},
            {"LH","Lufthansa"},
            {"CL","Lufthansa Cityline"},
            {"AF","Air France"},
            {"MS","Egyptair"},
            {"LX","Swiss"},
            {"I2","Munich Airlines"},
            {"RJ","Royal Jordanian"},
            {"TP","TAP Portugal"},
            {"SV","Saudi Arabian Airlines"},
            {"LO","LOT Polish Airlines"},
            {"AZ","Alitaliza"},
            {"SK","Scandinavian Airlines"},
            {"SU","Aeroflot"},
            {"EK","Emirates"},
            {"BA","British Airways"},
            {"CJ","BA Cityflyer"},
            {"MH","Malaysia Airlines"},
            {"AC","Air Canada"},
            {"CZ","China Southern Airlines"},
            {"PS","Ukraine Intl Airlines"},
            {"YB","Borajet"},
            {"J2","Azerbaijan Airlines"},
            {"EY","Etihad Airways"},
            {"GF","Gulf Air"},
            {"A3","Aegean Airlines"},
            {"AT","Royal Air Maroc"},
            {"DE","Condor"},
            {"DV","Jsc Aircompany Scat"},
            {"AB","Air Berlin"},
            {"XY","Nasair"},
            {"X3","TUIfly"},
            {"HY","Uzbekistan Airways"},
            {"QR","Qatar Airways"},
            {"KC","Air Astana"},
            {"AJ","Anadolu Jet"},
            {"Z4","Zagros Jet"},
            {"8U","Afriqiyah Airways"},
            {"9U","Air Moldova"},
            {"ET","Ethiopian Airlines"},
            {"FZ","Flydubai"},
            {"G9","Air Arabia"},
            {"SQ","Singapore Airlines"},
            {"XC","KD Air"},
            {"AH","Air Algerie"},
            {"Z6","Dnieproavia"},
            {"ME","Middle East Airlines"},
            {"IB","Iberia"},
            {"ST","Germania"},
            {"XG","Clickair"},
            {"SZ","SomonAir"},
            {"DL","Delta Air Lines"},
            {"TU","Tunisair"},
            {"AA","American Airlines"}

        };
        /// <summary>
        /// Bu method kalan sms kredisini getirir.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <returns></returns>
        public string SmsKalanLimit()
        {
            try
            {
                string kalansmsdegeri;
                smsData MesajPaneli = new smsData();

                MesajPaneli.user = new UserInfo("", "");

                ReturnValue ReturnData = MesajPaneli.DoPost("http://api.mesajpaneli.com/json_api/login/", true, true);

                if (ReturnData.status)
                {
                    kalansmsdegeri = "<div class=\"col-md-12\"><h4 class=\"col-md-4\"> Kalan Sms Kredisi = </h4>" + "<div class=\"col-md-8\" style=\"color: red\"><h4>" + ReturnData.userData.orjinli.ToString() + " Adet</h4></div></div>";


                }
                else
                {
                    kalansmsdegeri = "<div class=\"col-md-12\"><h4><div class=\"col-md-12\" style=\"color: red\"> Sms Sistemi Çalışmıyor </div><h4></div>";


                }
                return kalansmsdegeri;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        ///  Sms listesini oluşturmak için gereken parametreleri gönderin.  Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="BugunTarihi"></param>
        /// <param name="GelenTarih"></param>
        /// <param name="Siteismi"></param>
        public void SmsList(string BugunTarihi, string GelenTarih, string Siteismi,string YurtDisi)
        {
            try
            {
                if (Siteismi == "Lütfen Seçiniz")
                {
                    if(YurtDisi == "Lütfen Seçiniz")
                    {
                        SqlConnection baglanti = new SqlConnection(BiletBankDB);
                        baglanti.Open();
                        SqlCommand cmd3 = new SqlCommand("select count(*), AD, SOYAD, ALTACENTELINK, ceptel from SATISBILGILERI WHERE  (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'') Group By AD,SOYAD,ALTACENTELINK,ceptel", baglanti);
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        da2.SelectCommand = cmd3;
                        da2.Fill(SmsListDataset, "SATISBILGILERI");
                        baglanti.Close();
                        baglanti.Dispose();
                    }
                    else if (YurtDisi == "Yurt içi")
                    {
                        SqlConnection baglanti = new SqlConnection(BiletBankDB);
                        baglanti.Open();
                        SqlCommand cmd3 = new SqlCommand("select count(*), AD, SOYAD, ALTACENTELINK, ceptel from SATISBILGILERI WHERE  (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and HIZMETBEDELI<49) Group By AD,SOYAD,ALTACENTELINK,ceptel", baglanti);
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        da2.SelectCommand = cmd3;
                        da2.Fill(SmsListDataset, "SATISBILGILERI");
                        baglanti.Close();
                        baglanti.Dispose();
                    }
                    else if (YurtDisi == "Yurt Dışı")
                    {
                        SqlConnection baglanti = new SqlConnection(BiletBankDB);
                        baglanti.Open();
                        SqlCommand cmd3 = new SqlCommand("select count(*), AD, SOYAD, ALTACENTELINK, ceptel from SATISBILGILERI WHERE  (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and HIZMETBEDELI>49) Group By AD,SOYAD,ALTACENTELINK,ceptel", baglanti);
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        da2.SelectCommand = cmd3;
                        da2.Fill(SmsListDataset, "SATISBILGILERI");
                        baglanti.Close();
                        baglanti.Dispose();
                    }

                }
                else
                {
                    if (YurtDisi == "Lütfen Seçiniz")
                    {
                        SqlConnection baglanti = new SqlConnection(BiletBankDB);
                        baglanti.Open();
                        SqlCommand cmd3 = new SqlCommand("select count(*), AD, SOYAD, ALTACENTELINK, ceptel from SATISBILGILERI WHERE  (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Siteismi + "') Group By AD, SOYAD, ALTACENTELINK, ceptel", baglanti);
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        da2.SelectCommand = cmd3;
                        da2.Fill(SmsListDataset, "SATISBILGILERI");
                        baglanti.Close();
                        baglanti.Dispose();
                    }
                    else if (YurtDisi == "Yurt içi")
                    {
                        SqlConnection baglanti = new SqlConnection(BiletBankDB);
                        baglanti.Open();
                        SqlCommand cmd3 = new SqlCommand("select count(*), AD, SOYAD, ALTACENTELINK, ceptel from SATISBILGILERI WHERE  (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Siteismi + "' and HIZMETBEDELI<49) Group By AD, SOYAD, ALTACENTELINK, ceptel", baglanti);
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        da2.SelectCommand = cmd3;
                        da2.Fill(SmsListDataset, "SATISBILGILERI");
                        baglanti.Close();
                        baglanti.Dispose();
                    }
                    else if (YurtDisi == "Yurt Dışı")
                    {
                        SqlConnection baglanti = new SqlConnection(BiletBankDB);
                        baglanti.Open();
                        SqlCommand cmd3 = new SqlCommand("select count(*), AD, SOYAD, ALTACENTELINK, ceptel from SATISBILGILERI WHERE  (GMT>='" + BugunTarihi + "' and GMT<='" + GelenTarih + "' and PNR<>'' and ALTACENTELINK='" + Siteismi + "' and HIZMETBEDELI>49) Group By AD, SOYAD, ALTACENTELINK, ceptel", baglanti);
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        da2.SelectCommand = cmd3;
                        da2.Fill(SmsListDataset, "SATISBILGILERI");
                        baglanti.Close();
                        baglanti.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {

            
            }
        }
        /// <summary>
        ///  Bu methot ile gelen tarihler arasında verilere göre sms atabilirsiniz. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="BugunTarihi"></param>
        /// <param name="GelenTarih"></param>
        /// <param name="Siteismi"></param>
        /// <param name="smsmesaj"></param>
        /// <param name="smsBaslik"></param>
        public void TopluSmsGonder(string BugunTarihi, string GelenTarih, string Siteismi,string yurtdisi,string smsmesaj, string smsBaslik)
        {
 
            try
            {
                SmsList(BugunTarihi, GelenTarih, Siteismi, yurtdisi);
                if (SmsListDataset != null)
                {
                    foreach (DataTable table in SmsListDataset.Tables)
                    {
                        for (var sayi = 0; sayi <= table.Rows.Count - 1; sayi++)
                        { 
                            string ceptelal = SmsListDataset.Tables[0].Rows[sayi]["ceptel"].ToString();
                            ceptelal = ceptelal.Replace("-", "");
                            ceptelal = ceptelal.Replace(" ", "");
                            var donenveri= SmsSend(smsmesaj, smsBaslik, ceptelal);
                            if (donenveri)
                                hata = "<div style=\"color:Green\">Sms Başarılı olarak gönderildi</div>";
                            else
                                hata = "<div style=\"color:Red\">Sms Gönderimi Başarısınız</div>";
                        }

                    }
                    SitelerDataset = null;
                }
                else
                {
                    hata = "Sms göndericeğiniz siteyi seçin.";
                }

              
            }
            catch (Exception ex)
            {

                hata = ex.ToString();
            }
        }

        /// <summary>
        ///  Bu method ile sms gönderebilirsiniz. Geri true yada false döndürür. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="smsmesaj"></param>
        /// <param name="smsBaslik"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static Boolean SmsSend(String smsmesaj, String smsBaslik, String phoneNumber)
        {
            smsData MesajPaneli = new smsData();

            MesajPaneli.user = new UserInfo("", "");
            MesajPaneli.msgBaslik = smsBaslik;
            MesajPaneli.msgData.Add(new msgdata(phoneNumber, smsmesaj));   // Numaralar başında "0" olmadan yazılacaktır
            ///MesajPaneli.msgData.Add(new msgdata("Alıcı No 2", "Mesaj 2"));
            ReturnValue ReturnData = MesajPaneli.DoPost("http://api.mesajpaneli.com/json_api/", true, true);


            if (ReturnData.status)
            {

                return true;
            }
            else
            {
      
                return false;
            }

        }

        /// <summary>
        /// Verilen tarihler arasında sitelerden kaç adet satış yapıldığı gösterilir.Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="BugunTarihi"></param>
        /// <param name="GelenTarih"></param>
        public void SatisOranlari(string BugunTarihi, string GelenTarih)
        {
            try
            {
              
                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                SqlCommand cmd3 = new SqlCommand("select count(*), ALTACENTELINK from SATISBILGILERI WHERE  (GMT>='"+ BugunTarihi+"' and GMT<='" + GelenTarih + "' and PNR<>'') Group By ALTACENTELINK", baglanti);
                SqlDataAdapter da2 = new SqlDataAdapter();
                da2.SelectCommand = cmd3;
                da2.Fill(SitelerDataset, "SATISBILGILERI");
                baglanti.Close();
                baglanti.Dispose();
            }
            catch (Exception)
            { }

        }

        /// <summary>
        /// BiletAlarm tablosundaki verileri getirir. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        public void AlarmGetir()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                SqlCommand cmd3 = new SqlCommand("select * from BiletAlarm order by id desc", baglanti);
                SqlDataAdapter da2 = new SqlDataAdapter();
                da2.SelectCommand = cmd3;
                da2.Fill(AlarmDataSet, "BiletAlarm");
                baglanti.Close();
                baglanti.Dispose();
            }
            catch (Exception ex)
            {
                hata = ex.ToString();
            }
        }

        /// <summary>
        ///  Verilen bilgiler ile BiletAlarm tablosuna veri insert eder. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="urlBilgisi"></param>
        /// <param name="BilgiNotu"></param>
        /// <param name="Biletkontroltarihi"></param>
        /// <param name="flightTime"></param>
        /// <param name="flightPrice"></param>
        /// <param name="AirlineCode"></param>
        /// <param name="MailAdresi"></param>
        public void AlarmEkle(string urlBilgisi, string BilgiNotu, DateTime Biletkontroltarihi, string flightTime, decimal flightPrice, string AirlineCode, string MailAdresi,string Aktarma,string dishatDurum)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                SqlCommand command = new SqlCommand("insert into BiletAlarm Values(@urlBilgisi,@BilgiNotu,@BiletKontrolTarihi,@flighttime,@flightPrice,@airlineCode,@mailadresi,@aktarma,@DishatDurum)",baglanti);
                command.Parameters.AddWithValue("@urlBilgisi", urlBilgisi);
                command.Parameters.AddWithValue("@BilgiNotu", BilgiNotu);
                command.Parameters.AddWithValue("@BiletKontrolTarihi", Biletkontroltarihi);
                command.Parameters.AddWithValue("@flighttime", flightTime);
                command.Parameters.AddWithValue("@flightPrice", flightPrice);
                command.Parameters.AddWithValue("@airlineCode", AirlineCode);
                command.Parameters.AddWithValue("@mailadresi", MailAdresi);
                command.Parameters.AddWithValue("@aktarma", Aktarma);
                command.Parameters.AddWithValue("@DishatDurum", dishatDurum);
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Alarm Ekleme Tamamlandı";
            }
            catch (Exception ex)
            {
                hata = ex.ToString();
            }
        }

        /// <summary>
        ///  id parametresine göre BiletAlarm Verileri Günceller. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="urlBilgisi"></param>
        /// <param name="BilgiNotu"></param>
        /// <param name="Biletkontroltarihi"></param>
        /// <param name="flightTime"></param>
        /// <param name="flightPrice"></param>
        /// <param name="AirlineCode"></param>
        /// <param name="MailAdresi"></param>
        public void Alarmduzenle(int id,string urlBilgisi, string BilgiNotu, DateTime Biletkontroltarihi, string flightTime, decimal flightPrice, string AirlineCode, string MailAdresi,string aktarma,string dishatDurum)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                SqlCommand command = new SqlCommand("UPDATE BiletAlarm SET  urlBilgisi=@urlBilgisi, BilgiNotu=@BilgiNotu, BiletKontrolTarihi=@BiletKontrolTarihi, flighttime=@flighttime, flightPrice=@flightPrice, airlineCode=@airlineCode, mailadresi=@mailadresi, aktarma=@Aktarma, dishatDurum=@DishatDurum where id='" + id + "'"+baglanti);
                command.Parameters.AddWithValue("@urlBilgisi", urlBilgisi);
                command.Parameters.AddWithValue("@BilgiNotu", BilgiNotu);
                command.Parameters.AddWithValue("@BiletKontrolTarihi", Biletkontroltarihi);
                command.Parameters.AddWithValue("@flighttime", flightTime);
                command.Parameters.AddWithValue("@flightPrice", flightPrice);
                command.Parameters.AddWithValue("@airlineCode", AirlineCode);
                command.Parameters.AddWithValue("@mailadresi", MailAdresi);
                command.Parameters.AddWithValue("@Aktarma",aktarma);
                command.Parameters.AddWithValue("@DishatDurum", dishatDurum);
                command.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Alarm Ekleme Tamamlandı";
            }
            
            catch (Exception ex)
            {
                hata = ex.ToString();
            }
        }

        /// <summary>
        /// Verilen id BiletAlarm tablosundan silindi. Bu kod okan kacan ( Dizayn34.com ) tarafından hazırlanmıştır.
        /// </summary>
        /// <param name="id"></param>
        public void AlarmSil(int id)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("DELETE from BiletAlarm where (id='" + id + "')", baglanti);
                cmd.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Verilen id 'deki Alarm silindi";

            }
            catch (Exception ex)
            {
                hata = "Alarm Silinirken Hata Gelişti. " + ex.ToString();
            }
        }
       /// <summary>
       /// Verilen id ve ver tarih kullanarak veriyi siler.
       /// </summary>
       /// <param name="id"></param>
       /// <param name="date"></param>
        public void Alarmsil(int id , DateTime date)
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(BiletBankDB);
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("DELETE from BiletAlarm where (id='" + id + "' and BiletKontrolTarihi='" + date + "')", baglanti);
                cmd.ExecuteNonQuery();
                baglanti.Close();
                baglanti.Dispose();
                hata = "Alarm Silindi Bilgilerinize..";
            }
            catch (Exception ex)
            {

                hata ="hata "+ ex.ToString();
            }
         
        }
    }
}
