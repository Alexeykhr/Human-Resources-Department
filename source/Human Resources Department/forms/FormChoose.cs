﻿using System;
using System.IO;
using System.Windows.Forms;

using Human_Resources_Department.classes;
using Human_Resources_Department.classes.DB;

namespace Human_Resources_Department
{
    public partial class FormChoose : Form
    {
        private bool is_open;

        Config cfg = new Config();

        public FormChoose()
        {
            InitializeComponent();
            this.Text = Config.PROJECT_NAME + " - Вибір фірми";
        }

        private void InitialForm_Load(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(cfg.projectFolder);
                GetFolders();
            }
            catch
            {
                MessageBox.Show("Неможливо створити папку для збережень", "Помилка");
                this.Close();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if ( String.IsNullOrWhiteSpace(textBox1.Text) )
            {
                MessageBox.Show("Введіть ім'я нової фірми", "Помилка");
                return;
            }

            CreateNewProject();
        }

        private void CreateNewProject()
        {
            DialogResult result = MessageBox.Show(
                "Створити нову фірму з назвою: " + textBox1.Text + "?",
                "Фірма: " + textBox1.Text,
                MessageBoxButtons.YesNo
            );

            if (result == DialogResult.Yes)
            {
                string folder = cfg.projectFolder + "\\" + textBox1.Text;

                try
                {
                    Directory.CreateDirectory(folder);
                    Database.CreateDatabase(folder, "users");
                    Directory.CreateDirectory(folder + "\\img");
                    
                    cfg.CurrentFolder = folder;
                }
                catch
                {
                    MessageBox.Show("Не вдалося створити нову фірму", "Помилка");
                    return;
                }

                GetFolders();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            GetFolders();
            label3.Visible = true;
            timer1.Enabled = true;
        }

        private void GetFolders()
        {
            listBox1.Items.Clear();
            
            DirectoryInfo dir = new DirectoryInfo(cfg.projectFolder);

            foreach ( var item in dir.GetDirectories() )
            {
                listBox1.Items.Add(item.Name);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            label3.Visible = false;
            timer1.Enabled = false;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                button3.Enabled = true;
                button4.Enabled = true;
                this.Text = Config.PROJECT_NAME + " - " + listBox1.SelectedItem;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Видалити фірму з назвою: " + listBox1.Text + "?",
                "Фірма: " + listBox1.Text,
                MessageBoxButtons.YesNo
            );

            if (result == DialogResult.Yes)
            {
                DirectoryInfo dir = new DirectoryInfo(cfg.projectFolder + "\\" + listBox1.SelectedItem);
                dir.Delete(true);
                GetFolders();

                button3.Enabled = false;
                button4.Enabled = false;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.is_open = true;
            this.Close();
        }

        public string GetURI()
        {
            return cfg.projectFolder + "\\" + this.listBox1.SelectedItem;
        }

        public string GetNameFolder()
        {
            return (listBox1.SelectedIndex > -1) ? listBox1.SelectedItem.ToString() : "";
        }

        public bool IsOpen()
        {
            return this.is_open;
        }
    }
}