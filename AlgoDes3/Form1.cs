using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoDes3
{
    public partial class Form1 : Form
    {
        private readonly AVLTree AvlTree = new AVLTree();

        public Form1()
        {
            InitializeComponent();
        }

        private void TextBoxAdd(int width, int height, int positionX, int positionY, string tag)
        {
            TextBox textBox = new TextBox
            {
                Tag = tag,
                Location = new Point(positionX, positionY),
                Size = new Size(width, height)
            };
            Controls.Add(textBox);
        }

        private void LabelAdd(int width, int height, int positionX, int positionY, string tag, string text,
            ContentAlignment textAlign)
        {
            Label label = new Label
            {
                Tag = tag,
                Location = new Point(positionX, positionY),
                Size = new Size(width, height),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Text = text,
                TextAlign = textAlign,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            Controls.Add(label);
        }

        private void ButtonAdd(int width, int height, int positionX, int positionY, string tag, string text)
        {
            Button button = new Button
            {
                Tag = tag,
                Location = new Point(positionX, positionY),
                Size = new Size(width, height),
                BackColor = Color.Bisque,
                ForeColor = Color.Black,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            Controls.Add(button);
        }

        private void RemoveControl(params string[] tags)
        {
            foreach (string tag in tags)
            {
                Control control = FindControl(tag);
                Controls.Remove(control);
            }
        }

        private Control FindControl(string tag)
        {
            foreach (Control control in Controls)
            {
                if ((string)control.Tag == tag)
                    return control;
            }

            return null;
        }

        //Додати
        private void AddButton_Click(object sender, EventArgs e)
        {
            RemoveControl("Message", "TextBoxKey", "LabelKey", "TextBoxData", "LabelData", "AddDataButton",
                "SearchDataButton", "EditDataButton", "DeleteDataButton", "LabelBoxData");
            TextBoxAdd(60, 30, 270, 75, "TextBoxKey");
            TextBoxAdd(150, 30, 340, 75, "TextBoxData");
            LabelAdd(60, 20, 270, 50, "LabelKey", "Ключ", ContentAlignment.MiddleCenter);
            LabelAdd(150, 20, 340, 50, "LabelData", "Дані", ContentAlignment.MiddleCenter);
            ButtonAdd(100, 48, 390, 155, "AddDataButton", "Додати");
            Button buttonAdd = FindControl("AddDataButton") as Button;
            buttonAdd.Click += ButtonAdd_Click;
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            Label message = FindControl("Message") as Label;
            Controls.Remove(message);

            TextBox textBoxKey = FindControl("TextBoxKey") as TextBox;
            TextBox textBoxData = FindControl("TextBoxData") as TextBox;

            if (textBoxKey.Text == "" || textBoxData.Text == "")
            {
                LabelAdd(170, 20, 320, 120, "Message", "Введіть ключ та дані", ContentAlignment.MiddleCenter);
                return;
            }

            int key;
            try
            {
                key = int.Parse(textBoxKey.Text);
            }
            catch (FormatException)
            {
                LabelAdd(170, 20, 320, 120, "Message", "Ключ - ціле число", ContentAlignment.MiddleCenter);
                return;
            }

            string data = textBoxData.Text;
            AVLNode record = new AVLNode(key, data, null);
            AvlTree.AddRecord(record);
            LabelAdd(170, 20, 320, 120, "Message", "Додано", ContentAlignment.MiddleCenter);
        }

        //Знайти
        private void SearchButton_Click(object sender, EventArgs e)
        {
            RemoveControl("Message", "TextBoxKey", "LabelKey", "TextBoxData", "LabelData", "AddDataButton",
                "SearchDataButton", "EditDataButton", "DeleteDataButton", "LabelBoxData");
            TextBoxAdd(60, 30, 270, 75, "TextBoxKey");
            LabelAdd(60, 20, 270, 50, "LabelKey", "Ключ", ContentAlignment.MiddleCenter);
            ButtonAdd(100, 48, 390, 155, "SearchDataButton", "Знайти");
            Button buttonSearch = FindControl("SearchDataButton") as Button;
            buttonSearch.Click += ButtonSearch_Click;
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            Label message = FindControl("Message") as Label;
            Label dataLabel = FindControl("LabelBoxData") as Label;
            Controls.Remove(message);
            Controls.Remove(dataLabel);

            TextBox textBoxKey = FindControl("TextBoxKey") as TextBox;
            if (textBoxKey.Text == "")
            {
                LabelAdd(170, 20, 320, 120, "Message", "Введіть ключ", ContentAlignment.MiddleCenter);
                return;
            }

            int key;
            try
            {
                key = int.Parse(textBoxKey.Text);
            }
            catch (FormatException)
            {
                LabelAdd(170, 20, 320, 120, "Message", "Ключ - ціле число", ContentAlignment.MiddleCenter);
                return;
            }


            AVLNode record = AvlTree.SearchRecord(key, AvlTree.Root);
            if (record != null)
            {
                string data = record.Data;
                LabelAdd(150, 20, 340, 50, "LabelData", "Дані", ContentAlignment.MiddleCenter);
                LabelAdd(150, 20 * (data.Length / 17 + 1), 340, 75, "LabelBoxData", data, ContentAlignment.TopLeft);
            }
            else
            {
                LabelAdd(200, 40, 320, 110, "Message", "Даних за ключем не знайдено", ContentAlignment.MiddleCenter);
            }

            Console.WriteLine(AvlTree.iterations);
            AvlTree.iterations = 0;
        }

        //Редагувати
        private void EditButton_Click(object sender, EventArgs e)
        {
            RemoveControl("Message", "TextBoxKey", "LabelKey", "TextBoxData", "LabelData", "AddDataButton",
                "SearchDataButton", "EditDataButton", "DeleteDataButton", "LabelBoxData");
            TextBoxAdd(60, 30, 270, 75, "TextBoxKey");
            TextBoxAdd(150, 30, 340, 75, "TextBoxData");
            LabelAdd(60, 20, 270, 50, "LabelKey", "Ключ", ContentAlignment.MiddleCenter);
            LabelAdd(150, 20, 340, 50, "LabelData", "Дані", ContentAlignment.MiddleCenter);
            ButtonAdd(100, 48, 390, 155, "EditDataButton", "Редагувати");
            Button buttonEdit = FindControl("EditDataButton") as Button;
            buttonEdit.Click += ButtonEdit_Click;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            Label message = FindControl("Message") as Label;
            Controls.Remove(message);

            TextBox textBoxKey = FindControl("TextBoxKey") as TextBox;
            TextBox textBoxData = FindControl("TextBoxData") as TextBox;

            if (textBoxKey.Text == "" || textBoxData.Text == "")
            {
                LabelAdd(170, 20, 320, 120, "Message", "Введіть ключ та дані", ContentAlignment.MiddleCenter);
                return;
            }

            int key = 0;
            try
            {
                key = int.Parse(textBoxKey.Text);
            }
            catch (FormatException)
            {
                LabelAdd(170, 20, 320, 120, "Message", "Ключ - ціле число", ContentAlignment.MiddleCenter);
            }

            string data = textBoxData.Text;
            LabelAdd(200, 40, 320, 110, "Message",
                AvlTree.EditRecord(key, data) ? "Дані редаговано" : "Даних за ключем не знайдено",
                ContentAlignment.MiddleCenter);
        }

        //Видалити
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            RemoveControl("Message", "TextBoxKey", "LabelKey", "TextBoxData", "LabelData", "AddDataButton",
                "SearchDataButton", "EditDataButton", "DeleteDataButton", "LabelBoxData");
            TextBoxAdd(60, 30, 270, 75, "TextBoxKey");
            LabelAdd(60, 20, 270, 50, "LabelKey", "Ключ", ContentAlignment.MiddleCenter);
            ButtonAdd(100, 48, 390, 155, "DeleteDataButton", "Видалити");
            Button buttonDelete = FindControl("DeleteDataButton") as Button;
            buttonDelete.Click += ButtonDelete_Click;
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            Label message = FindControl("Message") as Label;
            Controls.Remove(message);

            TextBox textBoxKey = FindControl("TextBoxKey") as TextBox;
            if (textBoxKey.Text == "")
            {
                LabelAdd(170, 20, 320, 120, "Message", "Введіть ключ", ContentAlignment.MiddleCenter);
                return;
            }

            int key;
            try
            {
                key = int.Parse(textBoxKey.Text);
            }
            catch (FormatException)
            {
                LabelAdd(170, 20, 320, 120, "Message", "Ключ - ціле число", ContentAlignment.MiddleCenter);
                return;
            }

            LabelAdd(200, 40, 320, 110, "Message",
                AvlTree.DeleteRecord(key) ? "Видалено" : "Даних за ключем не знайдено", ContentAlignment.MiddleCenter);
        }
    }
}