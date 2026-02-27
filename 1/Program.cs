using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class MainForm : Form
    {
        private TextBox textBoxEditor;
        private TextBox textBoxOutput;
        private MenuStrip menuStrip;
        private ToolStrip toolStrip;
        private SplitContainer splitContainer;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Текстовый редактор - Языковой процессор";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(500, 400);

            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = Orientation.Horizontal;
            splitContainer.SplitterDistance = this.ClientSize.Height * 2 / 3;
            splitContainer.SplitterWidth = 5;
            splitContainer.Panel1MinSize = 100;
            splitContainer.Panel2MinSize = 100;

            textBoxEditor = new TextBox();
            textBoxEditor.Multiline = true;
            textBoxEditor.ScrollBars = ScrollBars.Both;
            textBoxEditor.Dock = DockStyle.Fill;
            textBoxEditor.Font = new Font("Consolas", 10);
            textBoxEditor.AcceptsTab = true;
            textBoxEditor.WordWrap = false;

            textBoxOutput = new TextBox();
            textBoxOutput.Multiline = true;
            textBoxOutput.ScrollBars = ScrollBars.Both;
            textBoxOutput.Dock = DockStyle.Fill;
            textBoxOutput.ReadOnly = true;
            textBoxOutput.Font = new Font("Consolas", 10);
            textBoxOutput.BackColor = Color.Lavender;
            textBoxOutput.ForeColor = Color.DarkBlue;

            splitContainer.Panel1.Controls.Add(textBoxEditor);
            splitContainer.Panel2.Controls.Add(textBoxOutput);

            menuStrip = new MenuStrip();
            menuStrip.Dock = DockStyle.Top;

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.Add("Создать", null, OnFileNew);
            fileMenu.DropDownItems.Add("Открыть", null, OnFileOpen);
            fileMenu.DropDownItems.Add("Сохранить", null, OnFileSave);
            fileMenu.DropDownItems.Add("Сохранить как", null, OnFileSaveAs);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("Выход", null, OnFileExit);

            ToolStripMenuItem editMenu = new ToolStripMenuItem("Правка");
            editMenu.DropDownItems.Add("Отменить", null, OnEditUndo);
            editMenu.DropDownItems.Add(new ToolStripSeparator());
            editMenu.DropDownItems.Add("Вырезать", null, OnEditCut);
            editMenu.DropDownItems.Add("Копировать", null, OnEditCopy);
            editMenu.DropDownItems.Add("Вставить", null, OnEditPaste);
            editMenu.DropDownItems.Add("Удалить", null, OnEditDelete);
            editMenu.DropDownItems.Add(new ToolStripSeparator());
            editMenu.DropDownItems.Add("Выделить все", null, OnEditSelectAll);

            ToolStripMenuItem textMenu = new ToolStripMenuItem("Текст");
            ToolStripMenuItem startMenu = new ToolStripMenuItem("Пуск");

            ToolStripMenuItem helpMenu = new ToolStripMenuItem("Справка");
            helpMenu.DropDownItems.Add("Вызов справки", null, OnHelp);
            helpMenu.DropDownItems.Add("О программе", null, OnAbout);

            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu, textMenu, startMenu, helpMenu });

            toolStrip = new ToolStrip();
            toolStrip.Dock = DockStyle.Top;
            toolStrip.ImageScalingSize = new Size(20, 20);

            toolStrip.Items.Add(new ToolStripButton("📄", null, OnFileNew, "New"));
            toolStrip.Items.Add(new ToolStripButton("📂", null, OnFileOpen, "Open"));
            toolStrip.Items.Add(new ToolStripButton("💾", null, OnFileSave, "Save"));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("↶", null, OnEditUndo, "Undo"));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("✂", null, OnEditCut, "Cut"));
            toolStrip.Items.Add(new ToolStripButton("📋", null, OnEditCopy, "Copy"));
            toolStrip.Items.Add(new ToolStripButton("📝", null, OnEditPaste, "Paste"));
            toolStrip.Items.Add(new ToolStripButton("❌", null, OnEditDelete, "Delete"));
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(new ToolStripButton("🔍", null, OnHelp, "Help"));
            toolStrip.Items.Add(new ToolStripButton("ℹ", null, OnAbout, "About"));

            this.Controls.Add(splitContainer);
            this.Controls.Add(toolStrip);
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;

            this.Resize += MainForm_Resize;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                splitContainer.SplitterDistance = Math.Max(
                    100,
                    Math.Min(
                        splitContainer.Height - 100,
                        this.ClientSize.Height * 2 / 3
                    )
                );
            }
        }

        private void OnFileNew(object sender, EventArgs e)
        {
            if (ConfirmSaveChanges())
            {
                textBoxEditor.Clear();
                textBoxOutput.Clear();
                this.Text = "Текстовый редактор - Языковой процессор [Новый файл]";
            }
        }

        private void OnFileOpen(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (ConfirmSaveChanges())
                {
                    textBoxEditor.Text = System.IO.File.ReadAllText(dialog.FileName);
                    this.Text = $"Текстовый редактор - Языковой процессор [{dialog.FileName}]";
                }
            }
        }

        private void OnFileSave(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(dialog.FileName, textBoxEditor.Text);
                this.Text = $"Текстовый редактор - Языковой процессор [{dialog.FileName}]";
                MessageBox.Show("Файл сохранен успешно!", "Сохранение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnFileSaveAs(object sender, EventArgs e) => OnFileSave(sender, e);

        private void OnFileExit(object sender, EventArgs e)
        {
            if (ConfirmSaveChanges())
            {
                this.Close();
            }
        }

        private bool ConfirmSaveChanges()
        {
            if (!string.IsNullOrEmpty(textBoxEditor.Text))
            {
                DialogResult result = MessageBox.Show(
                    "Сохранить изменения в текущем файле?",
                    "Подтверждение",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    OnFileSave(this, EventArgs.Empty);
                    return true;
                }
                else if (result == DialogResult.No)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        private void OnEditUndo(object sender, EventArgs e)
        {
            if (textBoxEditor.CanUndo)
            {
                textBoxEditor.Undo();
                textBoxEditor.ClearUndo();
            }
        }

        private void OnEditCut(object sender, EventArgs e) => textBoxEditor.Cut();
        private void OnEditCopy(object sender, EventArgs e) => textBoxEditor.Copy();
        private void OnEditPaste(object sender, EventArgs e) => textBoxEditor.Paste();
        private void OnEditDelete(object sender, EventArgs e) => textBoxEditor.SelectedText = "";
        private void OnEditSelectAll(object sender, EventArgs e) => textBoxEditor.SelectAll();

        private void OnHelp(object sender, EventArgs e)
        {
            string helpText = @"Текстовый редактор - Языковой процессор

Реализованные функции:
• Файл: Создать, Открыть, Сохранить, Сохранить как, Выход
• Правка: Отменить, Вырезать, Копировать, Вставить, Удалить, Выделить всё
• Справка: Данное окно и информация о программе";

            MessageBox.Show(helpText, "Справка",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnAbout(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Текстовый редактор v1.0\n" +
                "Языковой процессор (будущая версия)\n" +
                "(c) 2025",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}