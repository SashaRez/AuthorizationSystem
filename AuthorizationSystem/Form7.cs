using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace AuthorizationSystem
{
    public partial class Form7 : Form
    {

        // Выделяем таблицу в памяти.
        DataTable table = new DataTable();

        public Form7()
        {
            InitializeComponent();

            // Сохарняем / Открываем файл только в txt
            saveFileDialog1.Filter = "Text File (*.txt) | *.txt";
            openFileDialog1.Filter = "Text File (*.txt) | *.txt";

            // Добавляем 5 столбцов в таблицу.
            table.Columns.Add("Номер личного дела", typeof(int));
            table.Columns.Add("Дата проведения", typeof(string));
            table.Columns.Add("Дата следующего проведения", typeof(string));
            table.Columns.Add("Инструктаж", typeof(string));
            table.Columns.Add("Комментарий", typeof(string));

            dataGridView1.DataSource = table;
        }

        Conducting сonducting = new Conducting();

        public class Conducting
        {
            
            // Добавляет информацию о проведении инструктажа.
            public void AddConducting(string Num, DateTimePicker dateTimePicker1, DateTimePicker dateTimePicker2, string Equipment, string Comment, DataGridView dataGridView1, DataTable table)
            {
                int num;                        // Номер личного дела
                DateTime dataExtradition;       // Дата проведения инструктажа
                DateTime dataReplacement;       // Дата следующего проведения инструктажа

                // Проверка на совпадение номеров личных дел.
                bool check = true;

                // Создаём массив, чтобы вычислить повторяющиеся номера личных дел.
                int n = dataGridView1.RowCount;
                int[] CaseNumbers = new int[n];

                // Проверка на пустые строки.
                if (Num != String.Empty && dateTimePicker1.Value != null && dateTimePicker2.Value != null && Equipment != String.Empty && Comment != String.Empty)
                {

                    num = Convert.ToInt32(Num);
                    dataExtradition = dateTimePicker1.Value;
                    dataReplacement = dateTimePicker2.Value;



                    // Если есть совпадения, то выводится сообщение с ошибкой.
                    for (int i = 0; i < n; i++)
                    {
                        CaseNumbers[i] = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                        if (num == CaseNumbers[i])
                        {
                            MessageBox.Show("Такой номер личного дела уже существует!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            check = false;
                        }
                    }


                    // Если совпадений нет, то заносим данные в таблицу.
                    if (check)
                    {
                        table.Rows.Add(num, dataExtradition.Date.ToShortDateString(), dataReplacement.Date.ToShortDateString(), Equipment, Comment);
                    }

                }
                else
                {
                    MessageBox.Show("Заполните все поля!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            // Открывает файл с информацией о проведении инструктажа.
            public void OpenConducting(OpenFileDialog openFileDialog1, DataTable table)
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
                        values = lines[i].ToString().Split('|');

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

            // Сохраняет файл с информацией о проведении инструктажа.
            public void SaveConducting(SaveFileDialog saveFileDialog1, DataGridView dataGridView1)
            {

                // Если таблица пуста, то выйдет сообщение об ошибке.
                if (dataGridView1.RowCount == 0)
                {
                    MessageBox.Show("Нельзя сохранить пустую таблицу!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                        // Открываем поток и записываем данные таблицы в файл, разделяя каждое значение пустым пробелом с палкой.
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
                                    s = dataGridView1.Rows[i].Cells[j].Value.ToString() + " | ";
                                }


                                sw.Write(s);
                            }
                            sw.WriteLine();
                        }
                        sw.Close();

                    }
                }
            }

            // Удаляет информацию о проведении инструктажа.
            public void DeleteConducting(string index, DataGridView dataGridView1)
            {


                // Если индекс не был введён, то выйдет ошибка.
                if (index.Length == 0)
                {
                    MessageBox.Show("Введите номер личного дела, который вы бы хотели удалить!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {

                    int num = Convert.ToInt32(index);


                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (Convert.ToInt32(dataGridView1[0, i].Value) == num)
                        {
                            // Удаление строки по индексу. 
                            dataGridView1.Rows.RemoveAt(i);
                        }

                    }

                }
            }
        }


        // Кнопка "Добавить"
        private void bunifuAddButton1_Click(object sender, EventArgs e)
        {
            сonducting.AddConducting(richTextBox1.Text, dateTimePicker1, dateTimePicker2, richTextBox3.Text, richTextBox5.Text, dataGridView1, table);
        }

        // Кнопка "Удалить"
        private void bunifuDeleteButton2_Click(object sender, EventArgs e)
        {
            сonducting.DeleteConducting(richTextBox7.Text, dataGridView1);
        }

        // Кнопка "Сохранить"
        private void bunifuSaveButton3_Click(object sender, EventArgs e)
        {
            сonducting.SaveConducting(saveFileDialog1, dataGridView1);
        }

        // Кнопка "Открыть"
        private void bunifuOpenButton1_Click(object sender, EventArgs e)
        {
            сonducting.OpenConducting(openFileDialog1, table);
        }
    }
}

