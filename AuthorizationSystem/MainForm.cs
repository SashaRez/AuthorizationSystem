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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        NuclearSystem nuclearSystem = new NuclearSystem();

       
        public class NuclearSystem
        {

            // Открываем конкретные формы и помещает их на панель.  
            public void openForm(Form form, Panel panel)             
            {
                // Окно нижнего уровня (Неосновная форма приложения)
                form.TopLevel = false;   
                
                // Убираем границы у формы нижнего уровня.
                form.FormBorderStyle = FormBorderStyle.None;  
                
                // Добавляем нашу форму на панель.
                panel.Controls.Add(form);

                // В зависимости от размеров окна форма менялась также вслед.
                form.Dock = DockStyle.Fill;

                //Помещает форму на передний план.
                form.BringToFront();

                //Отображает форму
                form.Show();
            }

            public void ExitTop()            //Выход из программы
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите выйти?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }

            public void ExitBottom()        //Выход из программы
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите завершить работу?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }



        private void bunifuImageButton1_Click(object sender, EventArgs e)   // Верхняя кнопка для выхода
        {
            nuclearSystem.ExitTop();
        }


        private void bunifuFlatButton1_Click(object sender, EventArgs e)    // Нижняя кнопка для выхода
        {
            nuclearSystem.ExitBottom();
        }



        private void button1_Click(object sender, EventArgs e)      // Открывает вкладку "Служащие"
        {
            nuclearSystem.openForm(new Form3(), panelChildForm);
        }


        private void button2_Click(object sender, EventArgs e)      // Открывает вкладку "Снаряжение"
        {
            nuclearSystem.openForm(new Form4(), panelChildForm);
        }

        private void button3_Click(object sender, EventArgs e)      // Открывает вкладку "Выдача"
        {
            nuclearSystem.openForm(new Form5(), panelChildForm);
        }

        private void button4_Click(object sender, EventArgs e)      // Открывает вкладку "Инструктаж"
        {
            nuclearSystem.openForm(new Form6(), panelChildForm);
        }

        private void button5_Click(object sender, EventArgs e)      // Открывает вкладку "Проведение инструктажа"
        {
            nuclearSystem.openForm(new Form7(), panelChildForm);
        }
    }
}
