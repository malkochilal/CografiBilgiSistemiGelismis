﻿using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CografiBilgiSistemiProjesi1
{
    public partial class Form1 : Form
    {
        GMapOverlay katman1;
        List<Arac> list;
        SqlConnection baglanti = new SqlConnection(@"Data Source=LAPTOP-HT6PUQ4B\SQLEXPRESS;Initial Catalog=projelerVT;Integrated Security=True");


        public Form1()
        {
            InitializeComponent();
            initializeMap();
            aracListesiniOlustur();
        }
        private void araclariHaritadaGoster()
        {
            foreach (Arac arac in list)
            {
                GMarkerGoogle markerTmp = new GMarkerGoogle(arac.Konum, GMarkerGoogleType.green_dot);
                markerTmp.Tag = arac.Plaka;
                markerTmp.ToolTipText = arac.ToString();
                katman1.Markers.Add(markerTmp);
                markerTmp.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                Console.WriteLine(arac.ToString());

            }
        }
        private void aracListesiniOlustur()
        {
            list = new List<Arac>();
            //veritabanından ADO.NET İLE BİLGİLERİN ÇEKİLMESİ

            try
            {
                baglanti.Open();
                string sqlCumlesi = "SELECT Plaka,AracTipi,Nereden,Nereye,Enlem,Boylam FROM Araclar";
                SqlDataAdapter da = new SqlDataAdapter(sqlCumlesi, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }
                for (int i = 0; i < dt.Rows.Count; i++)

                {

                    list.Add(new Arac(dt.Rows[i][0].ToString(),
                        dt.Rows[i][1].ToString(),
                        dt.Rows[i][2].ToString(),
                        dt.Rows[i][3].ToString(),
                        new PointLatLng(Convert.ToDouble(dt.Rows[i][4].ToString()),
                                        Convert.ToDouble(dt.Rows[i][5].ToString()))));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısı sırasında bir hata oluştu,Hata Kodu:101\n" +
                    ex.Message);
            }
            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }
            araclariHaritadaGoster();
        }

        private void initializeMap()
        {
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            map.Position=new GMap.NET.PointLatLng(36.0,42.0);
            map.Zoom = 4;
            map.MinZoom = 3;
            map.MaxZoom=25;

             katman1 = new GMapOverlay();

            //Bir overlay oluşturmamız lazım
            //Harita üzerinde görüntülenecek tüm componentleri bu overlay'a eklememiz gerekiyor.
            //
            //GMapOverlay katman1= new GMapOverlay();
            //ilk olarak da yeni oluşturduğumuz katmanı harita nesnemize eklemeliyiz.
            map.Overlays.Add(katman1);
        }

      

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            map.Dispose();
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

       

        private void map_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            //int markerId = (int)item.Tag;
            //Console.WriteLine("id:"+markerId+" olan Markera tıklandı...");
            string secilenAracinPlakasi = (string)item.Tag;
            foreach(Arac arac in list)
            {
                if (secilenAracinPlakasi.Equals(arac.Plaka))
                {
                    textBox3.Text = secilenAracinPlakasi;
                    textBox4.Text = arac.Tipi;
                    textBox5.Text = arac.From;
                    textBox6.Text=arac.To;
                    break;
                }
            }
        
        }

      

       

        private void button3_Click(object sender, EventArgs e)
        {
                araclariHaritadaGoster();
        }
    }
}
