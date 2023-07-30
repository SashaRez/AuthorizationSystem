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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        AuthorizationSystem authorization = new AuthorizationSystem();


        public class AuthorizationSystem 
        {
            public string login;        // Логин, получаемый из файла "Data.log"
            public string password;     // Пароль, получаемый из файла "Data.log"

            public AuthorizationSystem()
            {
                // Открываем поток для чтения файла "Data.log"
                FileStream myStream = new FileStream("Data.log", FileMode.OpenOrCreate, FileAccess.Read);   
                
                // Считываем данные из файла "Data.log"
                StreamReader sr = new StreamReader(myStream);

                login = sr.ReadLine();              // Записываем 1-ю строку из файла
                password = sr.ReadLine();           // Записываем 2-ю строку из файла

                login = login.Substring(7);             // Обрезаем текст "Login: " и записываем искомое значение в файл
                password = password.Substring(10);      // Обрезаем текст "Password: " и записываем искомое значение в файл

                byte[] bufferL = Convert.FromBase64String(login);       // Конвертируем login (base64) -> login (byte)
                byte[] bufferP = Convert.FromBase64String(password);    // Конвертируем password (base64) -> password (byte)

                login = Encoding.ASCII.GetString(bufferL);              // Конвертируем login (byte) в набор символов ASCII.
                password = Encoding.ASCII.GetString(bufferP);           // Конвертируем password (byte) в набор символов ASCII.

                sr.Close();                                             // Закрываем поток.
            }

            public void Join(string userLogin, string userPassword)         // Метод для входа в главную форму
            {

         
                if (login == userLogin && password == userPassword)         // Сравнение вводимых значений и значений, полученных из файла
                {

                    ActiveForm.Hide();                                       // Скрытие формы авторизации
                    MainForm mainForm = new MainForm();
                    mainForm.Show();                                        // Открытие главной формы
                }

                else                                                       // Отображение сообщения с ошибкой
                {
                    MessageBox.Show("Введен неверный логин или пароль", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public void Exit()                          // Выход из программы через диалоговое окно
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите выйти?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();                 // Закрытие программы
                }

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            authorization.Exit();       // Выход из программы
        }


        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            authorization.Join(bunifuMetroTextbox6.Text, bunifuMetroTextbox5.Text);     // Вызов метода авторизации
            
        }
    }
}
