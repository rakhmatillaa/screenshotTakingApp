using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace screenshotTakingApp
{
    public partial class Form1 : Form
    {
        private string selectedPath = "";

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern void ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedPath = folderDialog.SelectedPath;

                    if (selectedPath.Length > 30)
                    {
                        btnSelectPath.Text = "..." + selectedPath.Substring(selectedPath.Length - 30);
                    }
                    else
                    {
                        btnSelectPath.Text = selectedPath;
                    }
                }
            }
        }

        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedPath))
            {
                MessageBox.Show("Please select a folder first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IntPtr hWnd = GetForegroundWindow();

            ShowWindow(hWnd, SW_HIDE);

            System.Threading.Thread.Sleep(200);

            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;

            Bitmap screenshot = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
            }

            ShowWindow(hWnd, SW_SHOW);

            string fileName = "screenshot_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
            string filePath = System.IO.Path.Combine(selectedPath, fileName);

            screenshot.Save(filePath, ImageFormat.Png);
            screenshot.Dispose();
            MessageBox.Show("Screenshot saved at:\n" + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}