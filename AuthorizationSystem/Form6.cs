using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AuthorizationSystem
{
    public partial class Form6 : Form
    {

        // Выделяем таблицу под память.
        DataTable table = new DataTable();

        public Form6()
        {
            InitializeComponent();
            
            // Сохраняем / Открываем файлы только в txt
            saveFileDialog1.Filter = "Text File (*.txt) | *.txt";
            openFileDialog1.Filter = "Text File (*.txt) | *.txt";

            // Добавляем в таблицу 2 столбца.
            table.Columns.Add("Инструктаж", typeof(string));
            table.Columns.Add("Пояснение", typeof(string));

            dataGridView1.DataSource = table;

        }

        Briefing briefing = new Briefing();

        public class Briefing
        {


            // Добавляем информацию об инструктаже.
            public void AddBriefing(string Briefing, string Comment, DataGridView dataGridView1, DataTable table)
            {

                // Проверка на совпадения информации об инструктаже.
                bool check = true;

                // Создаём массив для выявления совпадений в информации об инструктаже.
                int n = dataGridView1.RowCount;
                string[] Briefings = new string[n];


                // Проверка на введённые значения.
                if (Briefing != String.Empty && Comment != String.Empty)
                {

                    // Проверка на повторение информации об инструктаже.
                    for (int i = 0; i < n; i++)
                    {
                        Briefings[i] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        if (Briefing == Briefings[i])
                        {
                            MessageBox.Show("Такое снаряжение уже существует!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            check = false;
                        }
                    }

                    // Если нет совпадений, то добавляем информацию в таблицу.
                    if (check)
                    {
                        table.Rows.Add(Briefing, Comment);
                    }

                }
                else
                {
                    MessageBox.Show("Заполните все поля!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Открываем файл с инструктажём.
            public void OpenBriefing(OpenFileDialog openFileDialog1, DataTable table)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    // Открываем поток и читаем файл.
                    string nameFile = openFileDialog1.FileName;
                    FileStream fs = new FileStream(nameFile, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs);

                    // Записываем всё содержимое в массив.
                    string[] lines = File.ReadAllLines(nameFile);

                    // Создаём массив, в котором будут находиться обрезанные значения.
                    string[] values;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        values = lines[i].ToString().Split(' ');

                        string[] row = new string[values.Length];

                        for (int j = 0; j < values.Length; j++)
                        {
                            row[j] = values[j].Trim();

                        }

                        try
                        {
                            table.Rows.Add(row);
                        }

                        catch
                        {

                        }

                    }
                    
                    // Закрываем поток
                    sr.Close();
                }
            }


            // Сохраняем файл с инструктажём.
            public void SaveBriefing(DataGridView dataGridView1, SaveFileDialog saveFileDialog1)
            {
                
                // Если таблица пустая, то на экран выводится сообщение с ошибкой.
                if (dataGridView1.RowCount == 0)
                {
                    MessageBox.Show("Нельзя сохранить пустую таблицу!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                        // Открываем поток на сохранение информации об инструктаже.
                        string s;
                        string nameFile = saveFileDialog1.FileName;
                        FileStream fs = new FileStream(nameFile, FileMode.Append, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);


                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            for (int j = 0; j < dataGridView1.ColumnCount; j++)
                            {
                                if (j + 1 == dataGridView1.ColumnCount)
                                {
                                    s = dataGridView1.Rows[i].Cells[j].Value.ToString() + "";
                                }
                                else
                                {
                                    s = dataGridView1.Rows[i].Cells[j].Value.ToString() + " ";
                                }


                                sw.Write(s);
                            }
                            sw.WriteLine();
                        }
                        sw.Close();

                    }
                }
            }


            // Удаляем инструктаж.
            public void DeleteBriefing(DataGridView dataGridView1, string index)
            {

                // Если строка индекса пустая, то выведется на экран ошибка.
                if (index.Length == 0)
                {
                    MessageBox.Show("Введите индекс!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {

                    int number = Convert.ToInt32(index);


                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (Convert.ToInt32(dataGridView1[0, i].Value) == number)
                        {

                            // Удаление строки по индексу.
                            dataGridView1.Rows.RemoveAt(i);
                        }

                    }

                }
            }
        }



        // Кнопка добавления инструктажа.
        private void bunifuAddButton1_Click(object sender, EventArgs e)
        {
            briefing.AddBriefing(richTextBox1.Text, richTextBox2.Text, dataGridView1, table);
        }

        // Кнопка удаления инструктажа.
        private void bunifuDeleteButton2_Click(object sender, EventArgs e)
        {
            briefing.DeleteBriefing(dataGridView1, richTextBox7.Text);
        }

        // Кнопка сохранения инструктажа.
        private void bunifuSaveButton3_Click(object sender, EventArgs e)
        {
            briefing.SaveBriefing(dataGridView1, saveFileDialog1);
        }

        // Кнопка открытия файла с инструктажем.
        private void bunifuOpenButton1_Click(object sender, EventArgs e)
        {
            briefing.OpenBriefing(openFileDialog1, table);
        }
    }
}
