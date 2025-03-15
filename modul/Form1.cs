using System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms;

namespace modul
{
    public partial class Form1 : Form
    {
        private ListBox todoListBox, inProgressListBox, doneListBox;
        private Button addButton, editButton, moveToInProgressButton, moveToDoneButton, deleteButton;
        private TextBox taskTextBox;
        private MenuStrip menuStrip;
        private ToolStripMenuItem saveMenuItem, loadMenuItem;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
           
                this.Text = "Task Manager";
                this.Size = new System.Drawing.Size(700, 400);

                menuStrip = new MenuStrip();
                saveMenuItem = new ToolStripMenuItem("Save");
                loadMenuItem = new ToolStripMenuItem("Load");
                saveMenuItem.Click += SaveTasks;
                loadMenuItem.Click += LoadTasks;
                menuStrip.Items.Add(saveMenuItem);
                menuStrip.Items.Add(loadMenuItem);
                this.MainMenuStrip = menuStrip;
                this.Controls.Add(menuStrip);

                // Зміна шрифтів для міток
                Font labelFont = new Font("Arial", 12, FontStyle.Bold);

                Label todoLabel = new Label
                {
                    Text = "To Do",
                    Left = 20,
                    Top = 35,
                    Font = labelFont  // Новий шрифт для мітки
                };
                todoListBox = new ListBox { Left = 20, Top = 60, Width = 150, Height = 200 };

                Label inProgressLabel = new Label
                {
                    Text = "In Progress",
                    Left = 250,
                    Top = 35,
                    Font = labelFont  // Новий шрифт для мітки
                };
                inProgressListBox = new ListBox { Left = 250, Top = 60, Width = 150, Height = 200 };

                Label doneLabel = new Label
                {
                    Text = "Done",
                    Left = 480,
                    Top = 35,
                    Font = labelFont  // Новий шрифт для мітки
                };
                doneListBox = new ListBox { Left = 480, Top = 60, Width = 150, Height = 200 };

                taskTextBox = new TextBox { Left = 20, Top = 280, Width = 150 };

                // Колір кнопок та шрифт
                addButton = new Button
                {
                    Text = "Add",
                    Left = 180,
                    Top = 280,
                    BackColor = System.Drawing.Color.Gray,  // Сірий колір для кнопки
                    ForeColor = System.Drawing.Color.White  // Білий текст кнопки
                };

                editButton = new Button
                {
                    Text = "Edit",
                    Left = 260,
                    Top = 280,
                    BackColor = System.Drawing.Color.DarkGray,  // Трохи темніший сірий
                    ForeColor = System.Drawing.Color.White
                };

                deleteButton = new Button
                {
                    Text = "Delete",
                    Left = 340,
                    Top = 280,
                    BackColor = System.Drawing.Color.LightGray,  // Світліший сірий
                    ForeColor = System.Drawing.Color.White
                };

                moveToInProgressButton = new Button
                {
                    Text = "→",
                    Left = 170,
                    Top = 100,
                    BackColor = System.Drawing.Color.Gray,
                    ForeColor = System.Drawing.Color.White
                };

                moveToDoneButton = new Button
                {
                    Text = "→",
                    Left = 400,
                    Top = 100,
                    BackColor = System.Drawing.Color.Gray,
                    ForeColor = System.Drawing.Color.White
                };

                addButton.Click += AddTask;
                editButton.Click += EditTask;
                deleteButton.Click += DeleteTask;
                moveToInProgressButton.Click += MoveToInProgress;
                moveToDoneButton.Click += MoveToDone;

                this.Controls.Add(todoLabel);
                this.Controls.Add(todoListBox);
                this.Controls.Add(inProgressLabel);
                this.Controls.Add(inProgressListBox);
                this.Controls.Add(doneLabel);
                this.Controls.Add(doneListBox);
                this.Controls.Add(taskTextBox);
                this.Controls.Add(addButton);
                this.Controls.Add(editButton);
                this.Controls.Add(deleteButton);
                this.Controls.Add(moveToInProgressButton);
                this.Controls.Add(moveToDoneButton);
            
        }

        private void AddTask(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(taskTextBox.Text))
            {
                todoListBox.Items.Add(taskTextBox.Text);
                taskTextBox.Clear();
            }
        }

        private void EditTask(object sender, EventArgs e)
        {
            if (todoListBox.SelectedItem != null)
            {
                taskTextBox.Text = todoListBox.SelectedItem.ToString();
                todoListBox.Items.Remove(todoListBox.SelectedItem);
            }
        }

        private void DeleteTask(object sender, EventArgs e)
        {
            if (todoListBox.SelectedItem != null) todoListBox.Items.Remove(todoListBox.SelectedItem);
            else if (inProgressListBox.SelectedItem != null) inProgressListBox.Items.Remove(inProgressListBox.SelectedItem);
            else if (doneListBox.SelectedItem != null) doneListBox.Items.Remove(doneListBox.SelectedItem);
        }

        private void MoveToInProgress(object sender, EventArgs e)
        {
            if (todoListBox.SelectedItem != null)
            {
                inProgressListBox.Items.Add(todoListBox.SelectedItem);
                todoListBox.Items.Remove(todoListBox.SelectedItem);
            }
        }

        private void MoveToDone(object sender, EventArgs e)
        {
            if (inProgressListBox.SelectedItem != null)
            {
                doneListBox.Items.Add(inProgressListBox.SelectedItem);
                inProgressListBox.Items.Remove(inProgressListBox.SelectedItem);
            }
        }

        private void SaveTasks(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(saveFileDialog.FileName, GetTasks());
                }
            }
        }

        private void LoadTasks(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadTasksFromFile(openFileDialog.FileName);
                }
            }
        }

        private string[] GetTasks()
        {
            List<string> tasks = new List<string>();
            tasks.AddRange(todoListBox.Items.Cast<string>());
            tasks.AddRange(inProgressListBox.Items.Cast<string>());
            tasks.AddRange(doneListBox.Items.Cast<string>());
            return tasks.ToArray();
        }

        private void LoadTasksFromFile(string fileName)
        {
            string[] tasks = File.ReadAllLines(fileName);
            todoListBox.Items.Clear();
            inProgressListBox.Items.Clear();
            doneListBox.Items.Clear();
            foreach (string task in tasks)
            {
                todoListBox.Items.Add(task);
            }
        }
    

   
    private void Form1_Load(object sender, EventArgs e)
        {

        }
    }


   
}

