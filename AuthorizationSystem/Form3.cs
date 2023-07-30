using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace AuthorizationSystem
{
    public partial class Form3 : Form
    {

        // Создание таблицы в памяти
        DataTable table = new DataTable();       

        public Form3()
        {
            InitializeComponent();

            // Сохраняем / Открываем файл только в txt.
            saveFileDialog1.Filter = "Text File (*.txt) | *.txt";
            openFileDialog1.Filter = "Text File (*.txt) | *.txt";

            // Добавляем столбцы в нашу таблицу.
            table.Columns.Add("Номер личного дела", typeof(int));
            table.Columns.Add("Имя", typeof(string));
            table.Columns.Add("Фамилия", typeof(string));
            table.Columns.Add("Отчество", typeof(string));
            table.Columns.Add("Подразделение", typeof(int));
            table.Columns.Add("Размер одежды", typeof(string));


            // Записываем нашу таблицу в dataGridView
            dataGridView1.DataSource = table;


        }

        Employee employee = new Employee();

        public class Employee                   // Класс "Сотрудник"
        {

            int num;                            // Номер личного дела
            string name;                        // Имя сотрудника
            string secondName;                  // Фамилия сотрудника
            string patronymic;                  // Отчество сотрудника
            int subdivision;                    // Номер подразделения
            string size;                        // Размер одежды
            bool check = true;                  // Проверка на совпадения в личных делах


            // Добавляет данные сотрудника в таблицу
            public void AddEmployee(string Num, string Name, string SecondName, string Patronymic, string Subdivision, string Size, DataGridView dataGridView1, DataTable table)
            {
                // Создаём массив с номерами личных дел.
                int n = dataGridView1.RowCount;
                int[] CaseNumbers = new int[n];

                // Делаем проверку на пустые поля
                if (Num != String.Empty && Name != String.Empty && secondName != String.Empty && patronymic != String.Empty && Subdivision != String.Empty && Size != String.Empty)
                {

                    try
                    {
                        num = Convert.ToInt32(Num);
                        name = Name;
                        secondName = SecondName;
                        patronymic = Patronymic;
                        subdivision = Convert.ToInt32(Subdivision);
                        size = Size;


                        // Проверка на совпадения номеров личных дел. 
                        for (int i = 0; i < n; i++)
                        {
                            CaseNumbers[i] = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                            if (num == CaseNumbers[i])
                            {
                                MessageBox.Show("Такое личное дело уже существует!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                check = false;
                            }
                        }


                        // Если совпадений нет, то заносим данные в таблицу
                        if (check)
                        {
                            table.Rows.Add(num, name, secondName, patronymic, subdivision, size);
                        }

                    }

                    // Если были введены нечисловые данные.
                    catch
                    {
                        MessageBox.Show("Несоответствие типов данных!\n\"Номер личного дела\" и \"Номер подразделения\" должны содержать числовое значение", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }

                // Если не все поля были заполнены
                else
                {
                    MessageBox.Show("Заполните все поля!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            // Открытие файла
            public void OpenEmployee(OpenFileDialog openFileDialog1, DataTable table)
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


            public void SaveEmployee(DataGridView dataGridView1, SaveFileDialog saveFileDialog1)
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


            public void DeleteEmployee(DataGridView dataGridView1, string index)
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
                            dataGridView1.Rows.RemoveAt(i);         // Удаление ячейки по индексу.
                        }

                    }

                }
            }
        }




        private void bunifuAddButton1_Click(object sender, EventArgs e)       // Кнопка "Добавить"
        {

            employee.AddEmployee(richTextBox1.Text, richTextBox2.Text, richTextBox3.Text, richTextBox4.Text, richTextBox5.Text, richTextBox6.Text, dataGridView1, table);
           
        }

        private void bunifuDeleteButton2_Click(object sender, EventArgs e)      // Кнопка "Удалить"
        {
            employee.DeleteEmployee(dataGridView1, richTextBox7.Text);
        }

        private void bunifuSaveButton3_Click(object sender, EventArgs e)        // Кнопка "Сохранить"
        {
            employee.SaveEmployee(dataGridView1, saveFileDialog1);
        }

        private void bunifuOpenButton1_Click(object sender, EventArgs e)        // Кнопка "Открыть"
        {
            employee.OpenEmployee(openFileDialog1, table);
        }
    }
}
