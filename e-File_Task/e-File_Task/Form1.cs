using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace e_File_Task
{
    public partial class Form1 : Form
    {
        private string filePath = "E:";
        private bool isFile = false;
        private string currentlySelectedItemName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filePathTextBox.Text = filePath;
            loadFilesAndDirectories();
            NameBox.Text = "Name...";
            AddressBox.Text = "Address...";
            EmailBox.Text = "email@examle.com";
        }
        public void loadFilesAndDirectories()
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;
            try
            {

                if (isFile)
                {
                    tempFilePath = filePath + "/" + currentlySelectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    fileAttr = File.GetAttributes(tempFilePath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = new Bitmap(tempFilePath);
                    //Process.Start(tempFilePath);
                }
                else
                {
                    fileAttr = File.GetAttributes(filePath);

                }

                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] files = fileList.GetFiles(); // GET ALL THE FILES
                    DirectoryInfo[] dirs = fileList.GetDirectories(); // GET ALL THE DIRS
                    string fileExtension = "";
                    listView1.Items.Clear();

                    for (int i = 0; i < files.Length; i++)
                    {
                        fileExtension = files[i].Extension.ToUpper();
                        switch (fileExtension)
                        {
                            case ".PNG":
                            case ".JPG":
                            case ".JPEG":
                                listView1.Items.Add(files[i].Name, 1);
                                break;

                            default:
                                //listView1.Items.Add(files[i].Name, 2);
                                break;
                        }
                    }

                    for (int i = 0; i < dirs.Length; i++)
                    {
                        listView1.Items.Add(dirs[i].Name, 3);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        public void goBack()
        {
            try
            {
                removeBackSlash();
                string path = filePathTextBox.Text;
                path = path.Substring(0, path.LastIndexOf("/"));
                this.isFile = false;
                filePathTextBox.Text = path;
                removeBackSlash();
            }
            catch (Exception e)
            {

            }
        }
        public void loadButtonAction()
        {
            removeBackSlash();
            filePath = filePathTextBox.Text;
            loadFilesAndDirectories();
            isFile = false;
        }

        public void removeBackSlash()
        {
            string path = filePathTextBox.Text;
            if (path.LastIndexOf("/") == path.Length - 1)
            {
                filePathTextBox.Text = path.Substring(0, path.Length - 1);
            }
        }
        private void backButton_Click(object sender, EventArgs e)
        {
            goBack();
            loadButtonAction();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            loadButtonAction();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            currentlySelectedItemName = e.Item.Text;

            FileAttributes fileAttr = File.GetAttributes(filePath + "/" + currentlySelectedItemName);
            if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                isFile = false;
                filePathTextBox.Text = filePath + "/" + currentlySelectedItemName;
            }
            else
            {
                isFile = true;
            }

        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            loadButtonAction();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void NameBox_Enter(object sender, EventArgs e)
        {
            if (NameBox.Text == "Name...")
            {
                NameBox.Text = "";
                NameBox.ForeColor = Color.Black;
            }
        }

        private void NameBox_Leave(object sender, EventArgs e)
        {
            if (NameBox.Text.Trim() == "")
            {
                NameBox.Text = "Name...";
                NameBox.ForeColor = Color.LightGray;
            }
        }

        private void AddressBox_Enter(object sender, EventArgs e)
        {
            if (AddressBox.Text == "Address...")
            {
                AddressBox.Text = "";
                AddressBox.ForeColor = Color.Black;
            }
        }

        private void AddressBox_Leave(object sender, EventArgs e)
        {
            if (AddressBox.Text.Trim() == "")
            {
                AddressBox.Text = "Address...";
                AddressBox.ForeColor = Color.LightGray;
            }
        }

        private void EmailBox_Enter(object sender, EventArgs e)
        {
            if (EmailBox.Text == "email@examle.com")
            {
                EmailBox.Text = "";
                EmailBox.ForeColor = Color.Black;
            }
        }

        private void EmailBox_Leave(object sender, EventArgs e)
        {
            if (EmailBox.Text.Trim() == "")
            {
                EmailBox.Text = "email@examle.com";
                EmailBox.ForeColor = Color.LightGray;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=SAYED;Initial Catalog=Viewing_Images;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("Insert Into Image_Data (Name,Address,Email,Image)" +
                "Values('" + NameBox.Text + "','" + AddressBox.Text + "','" + EmailBox.Text + "',@pic)", con);
            MemoryStream stream = new MemoryStream();
            pictureBox1.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] pic = stream.ToArray();
            cmd.Parameters.AddWithValue("@pic", pic);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Image Saved Successfully");
        }
    }
}
