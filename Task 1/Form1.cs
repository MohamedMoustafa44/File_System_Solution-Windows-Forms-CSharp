using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Task_1
{
    public partial class Form1 : Form
    {
        string List1Direction;
        string List2Direction;
        string LastSelection;
        List<string> Drives;
        List<string> Directories1;
        List<string> Directories2;
        public Form1()
        {
            InitializeComponent();
            Drives = new List<string>();
            Directories1 = new List<string>();
            Directories2 = new List<string>();
            foreach(DriveInfo drive in DriveInfo.GetDrives())
            {
                Drives.Add(drive.Name);
                listBox1.Items.Add(drive.Name);
                listBox2.Items.Add(drive.Name);
            }
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List1Direction = listBox1.SelectedItem.ToString();
            LastSelection = listBox1.SelectedItem.ToString();

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            List2Direction = listBox2.SelectedItem.ToString();
            LastSelection = listBox2.SelectedItem.ToString();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RefreshList1();
            Directory.SetCurrentDirectory(List1Direction);
            MessageBox.Show(Environment.CurrentDirectory);
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RefreshList2();
            Directory.SetCurrentDirectory(List2Direction);
            MessageBox.Show(Environment.CurrentDirectory);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(LastSelection == List1Direction)
            {
                listBox1.Items.Clear();
                if (LastSelection ==  Drives.Find(drive => drive == LastSelection))
                {
                    listBox1.Items.AddRange(Drives.ToArray());
                }
                else
                {
                    List1Direction = Directory.GetParent(List1Direction).ToString();
                    LastSelection = List1Direction;
                    Directory.SetCurrentDirectory(List1Direction);
                    RefreshList1();
                }
            }
            else if (LastSelection == List2Direction)
            {
                listBox2.Items.Clear();
                if (LastSelection == Drives.Find(drive => drive == LastSelection))
                {
                    listBox2.Items.AddRange(Drives.ToArray());
                }
                else
                {
                    List2Direction = Directory.GetParent(List2Direction).ToString();
                    LastSelection = List2Direction;
                    Directory.SetCurrentDirectory(List2Direction);
                    RefreshList2();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Directory.Move(List1Direction,List2Direction + "\\" + Path.GetFileName(List1Direction));
            List1Direction = Directory.GetParent(List1Direction).ToString();
            Directory.SetCurrentDirectory(List2Direction);
            RefreshList1();
            RefreshList2();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileInfo info = new FileInfo(LastSelection);
            if(info.Attributes == FileAttributes.Directory)
            {
                Directory.Delete($@"{LastSelection}", true);
            }
            else
            {
                File.Delete($@"{LastSelection}");

            }
            if (LastSelection == List1Direction)
            {
                List1Direction = Directory.GetParent(List1Direction).ToString();
                RefreshList1();
            }
            else if(LastSelection == List2Direction)
            {
                List2Direction = Directory.GetParent(List2Direction).ToString();
                RefreshList2();
            }
            LastSelection = Directory.GetParent(LastSelection).ToString();

        }
        private void RefreshList1()
        {

            Directories1 = Directory.GetFileSystemEntries($@"{List1Direction}").ToList();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Directories1.ToArray());
            textBox1.Text = List1Direction;
        }
        private void RefreshList2()
        {
            Directories2 = Directory.GetFileSystemEntries($@"{List2Direction}").ToList();
            listBox2.Items.Clear();
            listBox2.Items.AddRange(Directories2.ToArray());
            textBox2.Text = List2Direction;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Directory.Move(List2Direction, List1Direction + "\\" + Path.GetFileName(List2Direction));
            List2Direction = Directory.GetParent(List2Direction).ToString();
            Directory.SetCurrentDirectory(List1Direction);
            RefreshList1();
            RefreshList2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileInfo info = new FileInfo(LastSelection);
            if (LastSelection == List1Direction)
            {
                if(info.Attributes == FileAttributes.Directory)
                {
                    CopyAllInDirectory(LastSelection, List2Direction);
                }
                else
                {
                    File.Copy(LastSelection, List2Direction + "\\" + Path.GetFileName(LastSelection));
                }
                RefreshList2();
            }
            else if(LastSelection == List2Direction)
            {
                if (info.Attributes == FileAttributes.Directory)
                {
                    CopyAllInDirectory(LastSelection, List1Direction);
                }
                else
                {
                    File.Copy(LastSelection, List1Direction + "\\" + Path.GetFileName(LastSelection));
                }
                RefreshList1();
            }
        }
        private void CopyAllInDirectory(string sourcePath, string targetPath)
        {
            Directory.CreateDirectory(targetPath + "\\" + Path.GetFileName(sourcePath));
            targetPath = targetPath + "\\" + Path.GetFileName(sourcePath);
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

    }
}
