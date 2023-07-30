using System;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace AuthorizationSystem
{
    public partial class Form4 : Form
    {

        // Создаём таблицу
        DataTable table = new DataTable();

        public Form4()
        {
            InitializeComponent();

            // Сохраняем / Открываем файлы только в txt
            saveFileDialog1.Filter = "Text File (*.txt) | *.txt";
            openFileDialog1.Filter = "Text File (*.txt) | *.txt";

            // Добавляем 2 столбца в таблицу
            table.Columns.Add("Наименование снаряжения", typeof(string));
            table.Columns.Add("Пояснение", typeof(string));

            // Записываем нашу таблицу в DataGridView
            dataGridView1.DataSource = table;
        }

        Equipment equipment = new Equipment();

        public class Equipment
        {


            // Добавляет снаряжение.
            public void AddEquipment(string Equipment, string Comment, DataGridView dataGridView1, DataTable table)
            {
              
                // Проверка на совпадения снаряжений.
                bool check = true;

                // Создаём массив с названиями снаряжений.
                int n = dataGridView1.RowCount;
                string[] Equipments = new string[n];

                // Делаем проверку на пустые пола.
                if (Equipment != String.Empty && Comment != String.Empty)
                {


                    // Проверка на совпадение названий снаряжений. 

                    for (int i = 0; i < n; i++)
                    {
                        Equipments[i] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        if (Equipment == Equipments[i])
                        {
                            MessageBox.Show("Такое снаряжение уже существует!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            check = false;
                        }
                    }


                    // Если совпадений нет, заносим данные в таблицу.
                    if (check)
                    {
                        table.Rows.Add(Equipment, Comment);
                    }

                }

                // Если все поля пустые.
                else
                {
                    MessageBox.Show("Заполните все поля!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            // Открытие файла
            public void OpenEquipment(OpenFileDialog openFileDialog1, DataTable table)
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

                    sr.Close();
                }
            }


            // Сохранение снаряжения
            public void SaveEquipment(DataGridView dataGridView1, SaveFileDialog saveFileDialog1)
            {
                if (dataGridView1.RowCount == 0)
                {
                    MessageBox.Show("Нельзя сохранить пустую таблицу!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                        // Открываем поток и записываем данные таблицы в файл, разделяя каждое значение пустым пробелом.
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


            // Удаляем снаряжение.
            public void DeleteEquipment(DataGridView dataGridView1, string index)
            {

                // Если пустое поле, то выходит ошибка.
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
                            // Удаление ячейки по индексу.
                            dataGridView1.Rows.RemoveAt(i);
                        }

                    }

                }
            }
        }



        // Кнопка "Добавить"
        private void bunifuAddButton1_Click(object sender, EventArgs e)
        {
            equipment.AddEquipment(richTextBox1.Text, richTextBox2.Text, dataGridView1, table);
        }

        // Кнопка "Удалить"
        private void bunifuDeleteButton2_Click(object sender, EventArgs e)
        {
            equipment.DeleteEquipment(dataGridView1, richTextBox7.Text);
        }

        // Кнопка "Сохранить"
        private void bunifuSaveButton3_Click(object sender, EventArgs e)
        {
            equipment.SaveEquipment(dataGridView1, saveFileDialog1);
        }

        // Кнопка "Открыть"
        private void bunifuOpenButton1_Click(object sender, EventArgs e)
        {
            equipment.OpenEquipment(openFileDialog1, table);
        }
    }
}
