using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MercedesAraçFormu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ShowList();


            if (File.Exists(temp))
            {
                string jsondata = File.ReadAllText(temp);
                arabalar = JsonSerializer.Deserialize<List<Cars>>(jsondata);
            }
            ShowList();
        }

        public List<Cars> arabalar = new List<Cars>()
        {
            new Cars()
            {
              ArabaPlakasi= "34 EF 8048",
              ArabaModeli= "GLC Coupe",
              Yakit= "Dizel",
              Renk= "Kırmızı",
              Vites="Otomatik",
              KasaTipi = "Sedan Kasa Tipi"
            },

             new Cars()
             {
              ArabaPlakasi="34 GFR 3765",
              ArabaModeli= "Eqe",
              Yakit= "Elektrik",
              Renk= "Gri",
              Vites="Otomatik",
              KasaTipi = "Hatchback Kasa Tipi"
             },

             new Cars()
             {
              ArabaPlakasi="34 JKD 4561",
              ArabaModeli= "Eqs",
              Yakit= "Benzin",
              Renk= "Beyaz",
              Vites="Manuel",
              KasaTipi = "Hatchback Kasa Tipi"
             },

             new Cars()
             {
              ArabaPlakasi="34 GZS 9061",
              ArabaModeli= "G Serisi",
              Yakit= "Dizel",
              Renk= "Siyah",
              Vites="Yarı Otomatik",
              KasaTipi = "SUV Kasa Tipi"
             },

              new Cars()
              {
              ArabaPlakasi="34 KEA 3825",
              ArabaModeli= "SL Roadster",
              Yakit= "Süper Plus",
              Renk= "Siyah",
              Vites="Otomatik",
              KasaTipi = "SUV Kasa Tipi"
              }
        };

        public void ShowList()
        {
            listView1.Items.Clear();
            foreach (Cars cars in arabalar)
            {
                AddCarsToListView(cars);
            }
        }

        public void AddCarsToListView(Cars cars)
        {
            ListViewItem item = new ListViewItem(new string[]
            {
                cars.ArabaPlakasi,
                cars.ArabaModeli,
                cars.Yakit,
                cars.Renk,
                cars.Vites,
                cars.KasaTipi,
            });

            item.Tag = cars;
            listView1.Items.Add(item);

        }

        void EditCarsOnListView(ListViewItem cItem, Cars cars)
        {
            cItem.SubItems[0].Text = cars.ArabaPlakasi;
            cItem.SubItems[1].Text = cars.ArabaModeli;
            cItem.SubItems[2].Text = cars.Yakit;
            cItem.SubItems[3].Text = cars.Renk;
            cItem.SubItems[4].Text = cars.Vites;
            cItem.SubItems[5].Text = cars.KasaTipi;
           
            cItem.Tag = cars;
        }


        private void AracEkle(object sender, EventArgs e)
        {
            FrmArac frm = new FrmArac()
            { 
                Text = "Araç Ekle",
                StartPosition = FormStartPosition.CenterParent,
                cars = new Cars()
            };

            if(frm.ShowDialog()== DialogResult.OK)
            {
                arabalar.Add(frm.cars);
                AddCarsToListView(frm.cars);
            }
        }

        private void Duzenle(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem cItem = listView1.SelectedItems[0];
            Cars secili = cItem.Tag as Cars;

            FrmArac frm = new FrmArac()
            {
                Text = "Araç Düzenle",
                StartPosition = FormStartPosition.CenterParent,
                cars = Clone(secili),
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                secili = frm.cars;
                EditCarsOnListView(cItem, secili);
            }
        }

        Cars Clone(Cars cars)
        {
            return new Cars()
            {
                ArabaPlakasi = cars.ArabaPlakasi,
                ArabaModeli = cars.ArabaModeli,
                Yakit = cars.Yakit,
                Renk = cars.Renk,
                Vites = cars.Vites,
                KasaTipi = cars.KasaTipi
            };
        }

        private void DeleteCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem cItem = listView1.SelectedItems[0];
            Cars secili = cItem.Tag as Cars;

            var sonuc = MessageBox.Show($"Seçili araç silinsin mi?\n\n{secili.ArabaPlakasi} {secili.ArabaModeli}",
                 "Silmeyi Onayla",
                 MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                arabalar.Remove(secili);
                listView1.Items.Remove(cItem);
            }

        }

        private void SaveCommand(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Formatı|*.json|Xml Formatı|*.xml",
            };


            if (sf.ShowDialog() == DialogResult.OK)
            {
                if (sf.FileName.EndsWith("json"))
                {
                    string data = JsonSerializer.Serialize(arabalar);
                    File.WriteAllText(sf.FileName, data);
                }

                else if (sf.FileName.EndsWith("xml"))
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Cars>));
                    serializer.Serialize(sw, arabalar);
                    sw.Close();
                }
            }
        }

        private void LoadCommand(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog()
            {
                Filter = "Json, Xml Formatları|*.json;*.xml",
            };

            if (of.ShowDialog() == DialogResult.OK)
            {
                if (of.FileName.ToLower().EndsWith("json"))
                {
                    string jsondata = File.ReadAllText(of.FileName);
                    arabalar = JsonSerializer.Deserialize<List<Cars>>(jsondata);
                }
                else if (of.FileName.ToLower().EndsWith("xml"))
                {
                    StreamReader sr = new StreamReader(of.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Cars>));
                    arabalar = (List<Cars>)serializer.Deserialize(sr);
                    sr.Close();
                }

                ShowList();
            }
        }

        string temp = Path.Combine(Application.CommonAppDataPath, "data");

        protected override void OnClosing(CancelEventArgs e)
        {
            string data = JsonSerializer.Serialize(arabalar);
            File.WriteAllText(temp, data);

            base.OnClosing(e);
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
    }

    [Serializable]
        public class Cars
        {
            [Category("Araç Bilgisi"), DisplayName("Plaka")]
            public string ArabaPlakasi { get; set; }

            [Category("Araç Bilgisi"), DisplayName("Model")]
            public string ArabaModeli { get; set; }

            [Category("Araç Özelliği"), DisplayName("Yakıt")]
            public string Yakit { get; set; }

            [Category("Araç Özelliği"), DisplayName("Renk")]
            public string Renk { get; set; }

            [Category("Araç Özelliği"), DisplayName("Vites")]
            public string Vites { get; set; }

            [Category("Araç Özelliği"), DisplayName("Kasa Tipi")]
            public string KasaTipi { get; set; }
        }
}
