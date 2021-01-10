using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Skilled
{

    class TSklad
    {
        static public DataTable TabSklad = new DataTable();
        public DataView SkladView = new DataView(TabSklad);
        public string FiltrCriteria;
        public string SortCriteria;
        public DataGridViewComboBoxColumn cGrupaCB;
        public DataTable DovGrupa = new DataTable();

        public DataGridViewComboBoxColumn сValCB;
        public DataTable DovVal = new DataTable();
        
        public DataGridViewComboBoxColumn cSkladCB;
        public DataTable DovSklad = new DataTable();


        public TSklad()
        {
            DataColumn cNpp = new DataColumn("N_пп");
            DataColumn cNameGroup = new DataColumn("Група");
            DataColumn cNameProduct = new DataColumn("Назва");
            DataColumn cProduser = new DataColumn("Виробник");
            DataColumn cCount = new DataColumn("Кількість");
            DataColumn cPrise = new DataColumn("Ціна");
            DataColumn cVatrist = new DataColumn("Вартість");
            DataColumn cVal = new DataColumn("Валюта");
            DataColumn cPost = new DataColumn("Постачальник");
            DataColumn cSklad = new DataColumn("Склад");

            cNpp.DataType = System.Type.GetType("System.Int32");
            cNameGroup.DataType = System.Type.GetType("System.String");
            cProduser.DataType = System.Type.GetType("System.String");
            cCount.DataType = System.Type.GetType("System.Int32");
            cPrise.DataType = System.Type.GetType("System.Decimal");
            cVatrist.DataType = System.Type.GetType("System.Decimal");
            cVal.DataType = System.Type.GetType("System.String");
            cPost.DataType = System.Type.GetType("System.String");
            cSklad.DataType = System.Type.GetType("System.String");

            TabSklad.Columns.Add(cNpp);
            TabSklad.Columns.Add(cNameGroup);
            TabSklad.Columns.Add(cNameProduct);
            TabSklad.Columns.Add(cProduser);
            TabSklad.Columns.Add(cPrise);
            TabSklad.Columns.Add(cCount);
            TabSklad.Columns.Add(cVatrist);
            TabSklad.Columns.Add(cVal);
            TabSklad.Columns.Add(cPost);
            TabSklad.Columns.Add(cSklad);
        }

        public void TSkladAddRow(string pNameGroup, string pNameProduct, string pProduser, int pCount, decimal pPrise, string pVal, string pPost,string pSklad)
        {
            int nn;
            nn = TabSklad.Rows.Count;
            DataRow rowSklad = TabSklad.NewRow();

            rowSklad["N_пп"] = nn++;
            rowSklad["Група"] = pNameGroup;
            rowSklad["Назва"] = pNameProduct;
            rowSklad["Виробник"] = pProduser;
            rowSklad["Ціна"] = pPrise;
            rowSklad["Кількість"] = pCount;
            rowSklad["Вартість"] = pPrise * pCount;
            rowSklad["Валюта"] = pVal;
            rowSklad["Постачальник"] = pPost;
            rowSklad["Склад"] = pSklad;
            TabSklad.Rows.Add(rowSklad);
        }

        public void ColumnPropSet(DataGridView DGV)
        {
            DGV.Columns["N_пп"].HeaderText = "№ п/п";
            DGV.Columns["Група"].HeaderText = "Група";
            DGV.Columns["Назва"].HeaderText = "Назва";
            DGV.Columns["Виробник"].HeaderText = "Виробник";
            DGV.Columns["Ціна"].HeaderText = "Ціна";
            DGV.Columns["Кількість"].HeaderText = "Кількість";
            DGV.Columns["Вартість"].HeaderText = "Вартість";
            DGV.Columns["Валюта"].HeaderText = "Валюта";
            DGV.Columns["Склад"].HeaderText = "Склад";
            DGV.Columns["Постачальник"].HeaderText = "Постачальник";

            DGV.Columns["N_пп"].ReadOnly = true;
            DGV.Columns["Вартість"].ReadOnly = true;
            DGV.Columns["N_пп"].Width = 40;
            DGV.Columns["Група"].Width = 100;
            DGV.Columns["Назва"].Width = 160;
            DGV.Columns["Виробник"].Width = 160;
            DGV.Columns["Ціна"].Width = 70;
            DGV.Columns["Кількість"].Width = 70;
            DGV.Columns["Вартість"].Width = 70;
            DGV.Columns["Валюта"].Width = 70;
            DGV.Columns["Склад"].Width = 70;
            DGV.Columns[0].DefaultCellStyle.BackColor = Color.Green;

        }

        public void ZapTopFile()
        {
            string sNameFile, textRow;
            string sdir = Directory.GetCurrentDirectory();
            sNameFile = sdir + @"\FTabSklad.txt";
            try
            {
                if (File.Exists(sNameFile))
                    File.Delete(sNameFile);

                using (StreamWriter sw = new StreamWriter(sNameFile))
                {
                    foreach(DataRow rr in TabSklad.Rows)
                    {
                        textRow = rr["Група"] + ";" + rr["Назва"] + ";" + rr["Виробник"] + ";" +
                            Convert.ToString(rr["Кількість"]) + ";" + Convert.ToString(rr["Ціна"]);
                        sw.WriteLine(textRow);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Таблиця на записана");
            }
        }

        public void ReadTabFile(DataGridView DGS)
        {
            string sNameFile, textRow;
            string pGrupa, pNazva, pVyrobnyk, sKilksit, sCina, pVal, pPost, pSklad;
            int pKilkist;
            decimal PCina;
            int i, ip;

            TabSklad.Rows.Clear();
            string sdir = Directory.GetCurrentDirectory();
            sNameFile = sdir + @"\FTabSklad.txt";
            using (StreamReader sr = new StreamReader(sNameFile))
            {
                while (sr.Peek() >= 0)
                {
                    pGrupa = "";
                    pNazva = "";
                    pVyrobnyk = "";
                    sKilksit = "";
                    sCina = "";
                    pVal = "";
                    pPost = "";
                    pSklad = "";
                    textRow = sr.ReadLine();
                    i = textRow.IndexOf(';') - 1;
                    for (int j = 0; j <= i; j++)
                    {
                        pGrupa += textRow[j];
                    }
                    ip = i + 2;
                    i = textRow.IndexOf(';', ip) - 1;
                    for (int j = ip; j <= i; j++)
                    {
                        pNazva += textRow[j];
                    }
                    ip = i + 2;
                    i = textRow.IndexOf(';', ip) - 1;
                    for (int j = ip; j <= i; j++)
                    {
                        pVyrobnyk += textRow[j];
                    }
                    ip = i + 2;
                    i = textRow.IndexOf(';', ip) - 1;
                    for (int j = ip; j <= i; j++)
                    {
                        sKilksit += textRow[j];
                    }
                    ip = i + 2;
                    for (int j = ip; j <= i; j++)
                    {
                        sCina += textRow[j];
                    }
                    for (int j = ip; j <= i; j++)
                    {
                        pVal += textRow[j];
                    }
                    for (int j = ip; j <= i; j++)
                    {
                        pPost += textRow[j];
                    }
                    for(int j = ip; j <= textRow.Length; j++)
                    {
                        pSklad += textRow[j];
                    }
                    pKilkist = Convert.ToInt32(sKilksit);
                    PCina = Convert.ToDecimal(sCina);
                    TSkladAddRow(pGrupa, pNazva, pVyrobnyk, pKilkist, PCina, pVal, pPost, pSklad);
                }
            }
            SetSumy(DGS);
        }

        public void TSkladValFiltr(String PFilter, DataGridView DGV)
        {
            try
            {
                SkladView.RowFilter = PFilter;
                FiltrCriteria = PFilter;
                DGV.DataSource = SkladView;
            }
            catch
            {
                MessageBox.Show("Введений фільтр не правильний");
                return;
            }
        }

        public void TSkladValSort(String PSort, DataGridView DGV, DataGridView DGVSum)
        {
            try
            {
                SkladView.Sort = PSort;
                SortCriteria = PSort;
                DGV.DataSource = SkladView;
                DGV.Refresh();
            }
            catch
            {
                MessageBox.Show("Введений критерій сортування не правильний");
                return;
            }
        }

        public void SeekNazva(String sNazva, DataGridView DGV)
        {
            int nn;
            nn = -5;
            for (int i = 0; i < DGV.Rows.Count; i++)
            {
                if ((string)DGV.Rows[i].Cells["Назва"].Value == sNazva)
                {
                    nn = i;
                    break;
                }
            }
            if (nn >= 0)
            {
                DGV.FirstDisplayedCell = DGV.Rows[nn].Cells["Назва"];
                DGV.Rows[nn].Selected = true;
                DGV.CurrentCell = DGV.Rows[nn].Cells["Назва"];
            }
            else
            {
                MessageBox.Show("Значення не знайдено");
            }
        }

        public void SetSumy(DataGridView DGV)
        // Створюємо таблицю для підсумків, запишемо у неї підсумки і призначимо цю таблицю джерелом даних для DGV
        {
            string sGrupa, ssort, sVal; decimal DSuma;
            int i;
            DataTable TabSkladSum = new DataTable();    // Оголошуємо public-змінну TabSkladSum типу DataTable
                                                        // Таблиця підсумків буде складатись із 2 стовпців - група та вартість.
            DataColumn cNameGroupS = new DataColumn("Група");
            DataColumn cVartistS = new DataColumn("Вартість");
            DataColumn cVal = new DataColumn("Валюта");
            // Оголошуємо типи даних, що будуть зберігатись у стовпцях 
            cNameGroupS.DataType = System.Type.GetType("System.String");
            cVartistS.DataType = System.Type.GetType("System.Decimal");
            cVal.DataType = System.Type.GetType("System.String");
            // Додаєм стовпці до таблиці 
            TabSkladSum.Columns.Add(cNameGroupS);
            TabSkladSum.Columns.Add(cVartistS);
            TabSkladSum.Columns.Add(cVal);
            ssort = SkladView.Sort; // Запам’ятаємо можливо заданий користувачем критерій сортування 
            SkladView.Sort = "Група";	// Встановимо сортування по групах товару. SkladView.Count – кількість рядків 
            i = 0;
            while (i < SkladView.Count) // Цикл для всіх рядків із таблиці TabSklad, що впорядкована по групах
            {
                sGrupa = (string)SkladView[i]["Група"]; // Обираємо чергову групу товару
                sVal = (string)SkladView[i]["Валюта"];
                DSuma = 0.0M;   // Обнулюємо значення суми вартостей для кожної групи
                while ((i < SkladView.Count) & (sGrupa == (string)SkladView[i]["Група"]))
                {
                    try // Можливо у якомусь рядку не записана вартість, тому скористаємось засобами try - catch
                    {
                        DSuma = DSuma + (decimal)SkladView[i]["Вартість"];  // Накопичуємо суму вартостей по групі
                    }
                    catch
                    {
                        SkladView[i]["Вартість"] = 0M;
                    }
                    i = i + 1;
                    if (i == SkladView.Count) { break; }
                }
                DataRow rowSkladSum = TabSkladSum.NewRow(); // Створюємо новий рядок у таблиці підсумків 
                rowSkladSum["Група"] = sGrupa;	
                rowSkladSum["Вартість"] = DSuma;
                rowSkladSum["Валюта"] = sVal;
                TabSkladSum.Rows.Add(rowSkladSum);	// Додаємо сформований рядок до таблиці підсумків
            }
            DGV.DataSource = TabSkladSum;   // Призначаємо TabSkladSum як джерело даних для гріда
            SkladView.Sort = SortCriteria; // Відновимо критерій сортування, тому що для сум було встановлено сортування по групі
        }

        public void CreateDovGrupa()
        {
           
            DataColumn cNameGroup = new DataColumn("Група");
            cNameGroup.DataType = System.Type.GetType("System.String"); DovGrupa.Columns.Add(cNameGroup);   // Додаємо стовпець до таблиці DataRow rowSklad0 = DovGrupa.NewRow();
            DataRow rowSklad0 = DovGrupa.NewRow();
            rowSklad0[cNameGroup] = "Книги";
            DovGrupa.Rows.Add(rowSklad0);   
            DataRow rowSklad1 = DovGrupa.NewRow(); rowSklad1[cNameGroup] = "CD";
            DovGrupa.Rows.Add(rowSklad1); 
            DataRow rowSklad2 = DovGrupa.NewRow(); rowSklad2[cNameGroup] = "DVD";
            DovGrupa.Rows.Add(rowSklad2); 
            DataRow rowSklad3 = DovGrupa.NewRow(); rowSklad3[cNameGroup] = "Мобілки";
            DovGrupa.Rows.Add(rowSklad3);  
            DataRow rowSklad4 = DovGrupa.NewRow(); rowSklad4[cNameGroup] = "Плеєри";
            DovGrupa.Rows.Add(rowSklad4);  
            DataRow rowSklad5 = DovGrupa.NewRow(); rowSklad5[cNameGroup] = "Аксессуари";
            DovGrupa.Rows.Add(rowSklad5); 
            DataRow rowSklad6 = DovGrupa.NewRow(); rowSklad6[cNameGroup] = "Дисплеї";
            DovGrupa.Rows.Add(rowSklad6);   
            DataRow rowSklad7 = DovGrupa.NewRow(); rowSklad7[cNameGroup] = "Корпуси";
            DovGrupa.Rows.Add(rowSklad7);  
            DataRow rowSklad8 = DovGrupa.NewRow(); rowSklad8[cNameGroup] = "Блоки живлення";
            DovGrupa.Rows.Add(rowSklad8);  
            DataRow rowSklad9 = DovGrupa.NewRow(); rowSklad9[cNameGroup] = "Клавіатури";
            DovGrupa.Rows.Add(rowSklad9);  
            int nn = DovGrupa.Rows.Count;

            
        }

        public void CreateDovVal()
        {
            DataColumn cNameVal = new DataColumn("Валюта");
            cNameVal.DataType = System.Type.GetType("System.String"); DovVal.Columns.Add(cNameVal);   
            DataRow rowSklad10 = DovVal.NewRow();
            rowSklad10[cNameVal] = "₴";
            DovVal.Rows.Add(rowSklad10);
            DataRow rowSklad11 = DovVal.NewRow(); rowSklad11[cNameVal] = "$";
            DovVal.Rows.Add(rowSklad11);
            DataRow rowSklad12 = DovVal.NewRow(); rowSklad12[cNameVal] = "€";
            DovVal.Rows.Add(rowSklad12);
            DataRow rowSklad13 = DovVal.NewRow(); rowSklad13[cNameVal] = "₽";
            DovVal.Rows.Add(rowSklad13);
            DataRow rowSklad14 = DovVal.NewRow(); rowSklad14[cNameVal] = "zł";
            DovVal.Rows.Add(rowSklad14);
            int nn = DovVal.Rows.Count;
        }

        public void CreateDovSklad()
        {
            DataColumn cNameVal = new DataColumn("Склад");
            cNameVal.DataType = System.Type.GetType("System.String"); DovSklad.Columns.Add(cNameVal);
            DataRow rowSklad100 = DovSklad.NewRow();
            rowSklad100[cNameVal] = "1";
            DovSklad.Rows.Add(rowSklad100);
            DataRow rowSklad110 = DovSklad.NewRow(); rowSklad110[cNameVal] = "2";
            DovSklad.Rows.Add(rowSklad110);
            DataRow rowSklad120 = DovSklad.NewRow(); rowSklad120[cNameVal] = "3";
            DovSklad.Rows.Add(rowSklad120);
            DataRow rowSklad130 = DovSklad.NewRow(); rowSklad130[cNameVal] = "4";
            DovSklad.Rows.Add(rowSklad130);
            DataRow rowSklad140 = DovSklad.NewRow(); rowSklad140[cNameVal] = "5";
            DovSklad.Rows.Add(rowSklad140);
            int nn = DovSklad.Rows.Count;
        }

        public void AddComboGrupa(DataGridView DGV)
        {
            DataGridViewComboBoxColumn cGrupaCB = new DataGridViewComboBoxColumn();
            cGrupaCB.DataPropertyName = "Група";
            cGrupaCB.Name = "cNameGroupComb";   
            cGrupaCB.HeaderText = "Група"; 
            cGrupaCB.DropDownWidth = 200;   
            cGrupaCB.Width = 120;  
            cGrupaCB.MaxDropDownItems = 7; 
            cGrupaCB.FlatStyle = FlatStyle.Flat;
            cGrupaCB.ValueType = System.Type.GetType("System.string"); 
            String s; Int32 n;
            n = DovGrupa.Rows.Count;
 
            foreach (DataRow r in DovGrupa.Rows)    
            {
                s = (string)r["Група"];
                cGrupaCB.Items.AddRange(r["Група"]);  
            }
            DGV.Columns.Add(cGrupaCB);
            String ss;
            foreach (DataGridViewRow rrr in DGV.Rows)
            {
                ss = (string)rrr.Cells["Група"].Value;
                                                       
                rrr.Cells["Група"].Value = rrr.Cells["Група"].Value;
            }
            DGV.Columns.Remove("Група");
        }

        public void AddComboVal(DataGridView DGV)
        {
            DataGridViewComboBoxColumn cValCB = new DataGridViewComboBoxColumn();
            cValCB.DataPropertyName = "Валюта";
            cValCB.Name = "cNameValComb"; // Назва нового стовпця
            cValCB.HeaderText = "Валюта"; // Заголовок на гріді нового стовпця
            cValCB.DropDownWidth = 200; // Ширина "випадайки"
            cValCB.Width = 120;
            cValCB.MaxDropDownItems = 7; // Кількість рядків випадайки, які одночасно будуть видимі
            cValCB.FlatStyle = FlatStyle.Flat;
            cValCB.ValueType = System.Type.GetType("System.string");
            String s; Int32 n;
            n = DovVal.Rows.Count;

            foreach (DataRow r in DovVal.Rows) // Для кожного рядка r із таблиці DovGrupa DovGrupa DovGrupa
            {
                s = (string)r["Валюта"];
                cValCB.Items.AddRange(r["Валюта"]);
            }
            DGV.Columns.Add(cValCB);
            String ss;
            foreach (DataGridViewRow rrr in DGV.Rows)
            {
                ss = (string)rrr.Cells["Валюта"].Value;
                // Перезаписуєм значення комірки старого стовпця у комірку нового стовпця
                rrr.Cells["Валюта"].Value = rrr.Cells["Валюта"].Value;
            }
            DGV.Columns.Remove("Валюта");
        }

        public void AddComboSklad(DataGridView DGV)
        {
            DataGridViewComboBoxColumn cValCB = new DataGridViewComboBoxColumn();
            cValCB.DataPropertyName = "Склад";
            cValCB.Name = "cNameValComb"; // Назва нового стовпця
            cValCB.HeaderText = "Склад";
            cValCB.DropDownWidth = 200;
            cValCB.Width = 120;
            cValCB.MaxDropDownItems = 7;
            cValCB.FlatStyle = FlatStyle.Flat;
            cValCB.ValueType = System.Type.GetType("System.string");
            String s; Int32 n;
            n = DovSklad.Rows.Count;

            foreach (DataRow r in DovSklad.Rows)
            {
                s = (string)r["Склад"];
                cValCB.Items.AddRange(r["Склад"]);
            }
            DGV.Columns.Add(cValCB);
            String ss;
            foreach (DataGridViewRow rrr in DGV.Rows)
            {
                ss = (string)rrr.Cells["Склад"].Value;

                rrr.Cells["Склад"].Value = rrr.Cells["Склад"].Value;
            }
            DGV.Columns.Remove("Склад");
        }

        public void SortSklad(String PFilter, DataGridView DGV)
        {
            SkladView.RowFilter = PFilter;
            FiltrCriteria = PFilter;
            DGV.DataSource = SkladView;
        }

    }
    }
