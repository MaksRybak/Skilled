using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;



namespace Skilled
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private TSklad MySklad;
        public static string GlStringParameter;


        private void Form1_Load(object sender, EventArgs e)
        {
            MySklad = new TSklad();
            DGSklad.DataSource = MySklad.SkladView;
            MySklad.CreateDovGrupa();
            MySklad.AddComboGrupa(DGSklad);

            MySklad.CreateDovVal();
            MySklad.AddComboVal(DGSklad);
            
            MySklad.CreateDovSklad();
            MySklad.AddComboSklad(DGSklad);

            foreach (DataRow r in MySklad.DovGrupa.Rows)   
            {
                string s = (string)r["Група"];
                CBGrupa.Items.Add(r["Група"]);
            }
            foreach (DataRow r in MySklad.DovVal.Rows)   //Для Валют
            {
                string s = (string)r["Валюта"];
                CBOdyn.Items.Add(r["Валюта"]); // Додаємо у "випадайку" контрола ComboBox ComboBox ComboBox ComboBox елементи із довідника
            }
            foreach (DataRow r in MySklad.DovSklad.Rows)   //Для складу
            {
                string s = (string)r["Склад"]; //
                CBSklad.Items.Add(r["Склад"]);// Додаємо у "випадайку" контрола ComboBox ComboBox ComboBox ComboBox елементи із довідника
            }

            CBShowSk.Items.Add("1"); //Заповнив ComboBox для відображення складів
            CBShowSk.Items.Add("2");
            CBShowSk.Items.Add("3");
            CBShowSk.Items.Add("4");
            CBShowSk.Items.Add("5");

        }

        private void BAddRowToTable_Click(object sender, EventArgs e)
        {
            Decimal pPcina = 0;
            Int32 pKilkist = 0;

            try
            {
                if (TBCina.Text != "")
                    pPcina = Convert.ToDecimal(TBCina.Text);
            }
            catch
            {
                MessageBox.Show("Введіть у поле ціни числове значення");
                return;
            }

            try
            {
                if (TBKilkist.Text != "")
                    pKilkist = Convert.ToInt32(TBKilkist.Text);
            }
            catch
            {
                MessageBox.Show("Введіть у поле кількості числове значення");
                return;
            }
            MySklad.TSkladAddRow(CBGrupa.Text, TBNazva.Text, CBVyrobnyk.Text, pKilkist, pPcina, CBOdyn.Text, TBPost.Text, CBSklad.Text);
            
            MySklad.SetSumy(DGSkladSum);
        }

        private void записатиВТаблицюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySklad.ZapTopFile();
            MessageBox.Show("Таблиця записана");
        }

        private void зчитатиТаблицюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySklad.ReadTabFile(DGSkladSum);

        }

        private void DGSklad_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int i, j; decimal vart, kilk, cin;
            i = e.RowIndex; 
            j = e.ColumnIndex; 
            if (i < 0) return;  
            if (j < 0) return;
            if ((DGSklad.Columns[j].Name == "Кількість") ^ (DGSklad.Columns[j].Name == "Ціна"))
           
            {
                try 
                {
                    cin = (decimal)DGSklad.Rows[i].Cells["Ціна"].Value;
                    kilk = Convert.ToDecimal((Int32)DGSklad.Rows[i].Cells["Кількість"].Value); 
                    vart = kilk * cin; 
                    DGSklad.Rows[i].Cells["Вартість"].Value = vart;
                }
                catch { }
            }
            MySklad.SetSumy(DGSkladSum);

        }

        private void встановитиФільтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form FiltrDialog = new FServ();
            FiltrDialog.Text = "Введіть критерій фільтрування - наприклад: Група = 'Книги' & Ціна < 70";
            GlStringParameter = MySklad.FiltrCriteria;
            FiltrDialog.ShowDialog();
            MySklad.TSkladValFiltr(GlStringParameter, DGSklad);
        }

        

        private void знятиФільтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlStringParameter = "";
            MySklad.TSkladValFiltr(GlStringParameter, DGSklad);
        }

        private void встановитиКритерійСортуванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form SortDialog = new FServ();
            SortDialog.Text = "Введіть критерій сортування - наприклад: Виробник, Ціна Desc";
            SortDialog.ShowDialog();
            MySklad.TSkladValSort(GlStringParameter, DGSklad, DGSkladSum);
        }

        private void сортуватиПоГрупіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlStringParameter = "Група, Назва";
            MySklad.TSkladValSort(GlStringParameter, DGSklad, DGSkladSum);
        }

        private void пошукПоНазвіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sNazva;
            Form SeekDialog = new FServ();
            SeekDialog.Text = "Введіть назву:";
            SeekDialog.ShowDialog();
            MySklad.SeekNazva(GlStringParameter, DGSklad);
        }



        private void DGSklad_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            decimal cin; Int32 kilk;
            if (DGSklad.Columns[e.ColumnIndex].Name == "Ціна") 
            {
                if (DGSklad.Rows[e.RowIndex].IsNewRow)
                {
                    return;
                }  
                if (!decimal.TryParse(e.FormattedValue.ToString(), out cin))
                {
                    MessageBox.Show("Введіть, будь ласка, числове значення у поле ціни .");
                }
            }
            if (DGSklad.Columns[e.ColumnIndex].Name == "Кількість")
            {
                if (DGSklad.Rows[e.RowIndex].IsNewRow)
                { return; } 
                            
                if (!Int32.TryParse(e.FormattedValue.ToString(), out kilk))
                {
                    MessageBox.Show("Введіть,будь ласка, ціле числове значення у поле кількості."); 
                    e.Cancel = true; 
                }
            }
            


        }

        private void CBGrupa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void DGSkladSum_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CBOdyn_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CBSklad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MySklad.SortSklad(CBSklad.Text, DGSklad);
        }

        private void CBShowSk_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySklad.TSkladValFiltr("Склад = " + CBShowSk.Text, DGSklad);
        }

        private void зчитатиТаблицюЗБазиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            SqlConnection SqlConnection1 = new SqlConnection(); // Оголосили з'єднання SqlConnection1

            SqlCommand cmd = new SqlCommand();  // Оголосили команду
            cmd.Connection = SqlConnection1;
            cmd.CommandType = CommandType.StoredProcedure;// Назначили команді з'єднання 
            cmd.CommandText = "spSkladTabRead"; // Назначили команді сторед-процедуру 
            SqlConnection1.ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Sklad;Trusted_Connection=True;"; //Заповнив власний ConnectionString

            SqlConnection1.Open();  // Відкриємо з'єднання з сервером SqlDataReader	
            SqlDataReader SqlIn = cmd.ExecuteReader();

            TSklad.TabSklad.Rows.Clear();
            while (SqlIn.Read())    // Запустили датарідер
            {
                DataRow rowSklad = TSklad.TabSklad.NewRow();   // Створюємо новий рядок таблиці TabSklad класу Sklad
                int nn; decimal d1, d2, d3; nn = SqlIn.GetInt32(0);
                rowSklad["N_пп"] = SqlIn.GetInt32(0);   // присвоюємо значенням полів значення, отримані з таблиці бази даних
                rowSklad["Група"] = SqlIn.GetString(1);
                rowSklad["Назва"] = SqlIn.GetString(2);
                rowSklad["Виробник"] = SqlIn.GetString(3);
                rowSklad["Ціна"] = SqlIn.GetDecimal(4);
                rowSklad["Кількість"] = SqlIn.GetInt32(5);
                rowSklad["Постачальник"] = SqlIn.GetString(6); //Добавив постачальника
                rowSklad["Валюта"] = SqlIn.GetString(7); //Добавив валюту
                rowSklad["Склад"] = SqlIn.GetString(8); //Добавив склад

                d1 = (decimal)rowSklad["Ціна"];
                d2 = (int)rowSklad["Кількість"]; d3 = d1 * d2;
                rowSklad["Вартість"] = (decimal)rowSklad["Ціна"] * (int)rowSklad["Кількість"];
                TSklad.TabSklad.Rows.Add(rowSklad); // Додаємо сформований рядок до таблиці
                MySklad.SetSumy(DGSkladSum);
            }
            SqlConnection1.Close();// Закриємо з’єднання з сервером, щоб не тримати ресурс

        }

        private void записатиТаблицюВБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlConnection SqlConnection1 = new SqlConnection();// Оголосили з'єднання SqlConnection1 
            SqlCommand cmd = new SqlCommand();	// Оголосили команду
            cmd.Connection = SqlConnection1;    // Назначили команді з'єднання 
            cmd.CommandType = CommandType.StoredProcedure;
            SqlTransaction tranPlSave;
            SqlConnection1.ConnectionString = "Server=localhost\\SQLEXPRESS;Database=Sklad;Trusted_Connection=True;"; //Заповнив ConnectionString
            SqlConnection1.Open();  // Відкрили з'єднання

            tranPlSave = SqlConnection1.BeginTransaction("tranPlSave"); 
            cmd.Transaction = tranPlSave;
            cmd.Parameters.Clear();
            try
            {
                cmd.CommandText = "spClearSklad";// Назначили команді сторед-процедуру для очищення таблиці склад у базі
                cmd.ExecuteNonQuery();  // Очистили таблицю склад у базі
                cmd.CommandText = "spZapSklad"; // Назначили команді сторед-процедуру для записування у таблицю склад у базі
                foreach (DataRow rr in TSklad.TabSklad.Rows) //Для кожного рядка rr із таблиці TabSklad
                {
                    cmd.Parameters.Clear();
                    SqlParameter par1 = new SqlParameter("@N_pp", SqlDbType.Int); par1.Value = rr["N_пп"];
                    SqlParameter par2 = new SqlParameter("@Grupa", SqlDbType.NVarChar, 255); par2.Value = rr["Група"];
                    SqlParameter par3 = new SqlParameter("@Nazva", SqlDbType.NVarChar, 255); par3.Value = rr["Назва"];
                    SqlParameter par4 = new SqlParameter("@Vyrobnyk", SqlDbType.NVarChar, 255); par4.Value = rr["Виробник"];
                    SqlParameter par5 = new SqlParameter("@Cina", SqlDbType.Decimal, 12); par5.Value = rr["Ціна"];
                    SqlParameter par6 = new SqlParameter("@Kilkist", SqlDbType.Int); par6.Value = rr["Кількість"];
                    SqlParameter par7 = new SqlParameter("@Postach", SqlDbType.NVarChar, 255); par7.Value = rr["Постачальник"]; //Добавив можливітсть запису поля постачальника
                    SqlParameter par8 = new SqlParameter("@Valuta", SqlDbType.NVarChar , 255); par8.Value = rr["Валюта"]; // добавив можливість запису для валюти
                    SqlParameter par9 = new SqlParameter("@Sklad", SqlDbType.NVarChar , 255); par9.Value = rr["Склад"]; // також для складу
                    cmd.Parameters.Add(par1);
                    cmd.Parameters.Add(par2);
                    cmd.Parameters.Add(par3);
                    cmd.Parameters.Add(par4); 
                    cmd.Parameters.Add(par5);
                    cmd.Parameters.Add(par6);
                    cmd.Parameters.Add(par7); // Добавив цих три поля, відподвіно до номера
                    cmd.Parameters.Add(par8); 
                    cmd.Parameters.Add(par9); 
                    cmd.ExecuteNonQuery();
                }
                tranPlSave.Commit(); // Підтвердити всі зміни у базі
            }
            catch
            {
                tranPlSave.Rollback();  // Виконати "відкат" у випадку невдалого записування
            }
            SqlConnection1.Close(); // Закрити з'єднання з сервером, звільнити ресурс 
            MessageBox.Show("Таблиця записана у базу даних");

        }

        private void запитиТаблицюВEntinyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var db = new SkladEntities1()) //Використовуєм простір імен, і робимо об'єкт нашої бази зі всіма таблицями та процедурами
            {
                db.spClearSklad();//Очищаєм таблицю за допомогою функції
                foreach (DataRow rr in TSklad.TabSklad.Rows)  //Через foreach проходимо по всіх рядках і стовпцях та записуємо у базу за допомогою процедури
                {
                    db.spZapSklad(Convert.ToInt32(rr["N_пп"]), Convert.ToString(rr["Група"]),Convert.ToString(rr["Назва"]),
                        Convert.ToString(rr["Виробник"]),Convert.ToInt32(rr["Ціна"]) ,Convert.ToInt32(rr["Кількість"]),
                        Convert.ToString(rr["Постачальник"]) ,Convert.ToString(rr["Валюта"]) ,Convert.ToString(rr["Склад"]));
                }
                db.SaveChanges(); //Зберігаєм зміни
            }
        }

        private void зчитатиТаблицюВEntinyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TSklad.TabSklad.Rows.Clear(); //Очищаєм таблицю
            using (var db = new SkladEntities1()) //Також використовуємо простір імен і об'єкт
            {
                foreach(var value in db.spSkladTabRead()) //Зчитуєм всі дані з бази та проходимось по ньому за допомогою циклу
                {
                    DataRow rowSklad = TSklad.TabSklad.NewRow();   // Створюємо новий рядок таблиці TabSklad класу Sklad
                    int nn; decimal d1, d2, d3;
                    rowSklad["N_пп"] = value.N_nn;  // присвоюємо значенням полів значення, отримані з таблиці бази даних
                    rowSklad["Група"] = value.Група;
                    rowSklad["Назва"] = value.Назва;
                    rowSklad["Виробник"] = value.Виробник;
                    rowSklad["Ціна"] = value.Ціна;
                    rowSklad["Кількість"] = value.Кількість;
                    rowSklad["Постачальник"] = value.Постачальник; //Добавив постачальника
                    rowSklad["Валюта"] = value.Валюта; //Добавив валюту
                    rowSklad["Склад"] = value.Склад; //Добавив склад

                    d1 = (decimal)rowSklad["Ціна"];
                    d2 = (int)rowSklad["Кількість"]; d3 = d1 * d2;
                    rowSklad["Вартість"] = (decimal)rowSklad["Ціна"] * (int)rowSklad["Кількість"];
                    TSklad.TabSklad.Rows.Add(rowSklad); // Додаємо сформований рядок до таблиці
                    MySklad.SetSumy(DGSkladSum);
                }

                db.SaveChanges();
            }
        }
    }
}
