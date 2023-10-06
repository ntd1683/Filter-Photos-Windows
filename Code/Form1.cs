using System;
using NTD;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace filter_photo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FileFolderDialog dialog = new FileFolderDialog();
            dialog.Dialog.Title = "Chọn thư mục";

            string folder = @"C:\";

            if (!string.IsNullOrEmpty(txtFolder.Text))
            {
                folder = txtFolder.Text;
            }

            dialog.Dialog.InitialDirectory = folder;

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;

                txtFolder.Text = selectedPath;
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtNameFile.Paste();
            return;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtNameFile.Text) || string.IsNullOrEmpty(txtFolder.Text))
            {
                MessageBox.Show(@"Vui lòng nhập đủ thông tin");
                return;
            }

            string folderPathFilter, folderPath;
            FileAttributes attr = File.GetAttributes(txtFolder.Text);

            if (attr.HasFlag(FileAttributes.Directory))
                folderPath = txtFolder.Text;
            else
                folderPath = Path.GetDirectoryName(txtFolder.Text);

            folderPathFilter = folderPath + "\\filter";

            bool isFolderExists = Directory.Exists(folderPathFilter);

            if (!isFolderExists)
            {
                Directory.CreateDirectory(folderPathFilter);
            }

            string nameFile = txtNameFile.Text.Replace("\n", " ");
            string[] arrNameFile = nameFile.Split(' ');

            List<string> errors = new List<string>();

            DirectoryInfo folderInfo = new DirectoryInfo(folderPath);
            foreach (FileInfo file in folderInfo.GetFiles())
            {
                foreach (string name in arrNameFile)
                {
                    if (file.Name.Contains(name))
                    {
                        try
                        {
                            if (rbtnCopy.Checked)
                                file.CopyTo(folderPathFilter + '\\' + file.Name);
                            if (rbtnCut.Checked)
                                file.MoveTo(folderPathFilter + '\\' + file.Name);
                        } catch(System.IO.IOException ex)
                        {
                            errors.Add(ex.Message);
                        }
                    }
                }
            }

            MessageBox.Show("Đã Lọc Ảnh Xong");

            if (errors.Count > 0)
            {
                string errorMessage = "Có lỗi xảy ra khi copy hoặc cut các tệp:\n";
                foreach (string error in errors)
                {
                    errorMessage += error + "\n";
                }

                MessageBox.Show(errorMessage);
            }

            Process.Start(folderPathFilter);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string websiteUrl = "https://github.com/ntd1683/Filter_Photos";
            Process.Start(websiteUrl);
        }
    }
}
