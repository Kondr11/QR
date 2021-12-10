using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.Common;
using ZXing;
using ZXing.QrCode;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using ZXing.Client;

namespace QR_mb
{
    public partial class Form1 : Form
    {
        IWebDriver web;
        public Form1()
        {
            InitializeComponent();
        }

        QrCodeEncodingOptions options = new QrCodeEncodingOptions();
        Bitmap bmp1;


        private void Form1_Load(object sender, EventArgs e)
        {
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 250,
                Height = 250,
            };
            var writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            comboBox1.Items.AddRange(Enum.GetNames(typeof(KnownColor)));
            comboBox1.Text = KnownColor.White.ToString();
            comboBox2.Items.AddRange(Enum.GetNames(typeof(KnownColor)));
            comboBox2.Text = KnownColor.Black.ToString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrEmpty(textBox1.Text))
            {
                pictureBox1.Image = null;
                MessageBox.Show("Text not found", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var qr = new ZXing.BarcodeWriter();
                qr.Options = options;
                qr.Format = ZXing.BarcodeFormat.QR_CODE;
                var result = new Bitmap(qr.Write(textBox1.Text.Trim()));
                pictureBox1.Image = result;
                textBox1.Clear();
                Clipboard.SetImage(pictureBox1.Image);
                bmp1 = new Bitmap(pictureBox1.Image);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitmap = new Bitmap(pictureBox1.Image);
                BarcodeReader reader = new BarcodeReader { AutoRotate = true, TryInverted = true };
                Result result = reader.Decode(bitmap);
                string decoded = result.ToString().Trim();
                textBox1.Text = decoded;
            }
            catch (Exception)
            {
                MessageBox.Show("Image not found", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var qr = new ZXing.BarcodeWriter();
                qr.Options = options;
                qr.Format = ZXing.BarcodeFormat.QR_CODE;
                pictureBox1.ImageLocation = open.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Image not found", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SaveFileDialog save = new SaveFileDialog();
                save.CreatePrompt = true;
                save.OverwritePrompt = true;
                save.FileName = "QR";
                save.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|GIF|*.gif";
                if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pictureBox1.Image.Save(save.FileName);
                    save.InitialDirectory = Environment.GetFolderPath
                                (Environment.SpecialFolder.Desktop);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string LOGIN = LOGINBOX.Text;
            string PASS = PASSBOX.Text;
            web = new ChromeDriver();
            web.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(40);
            try
            {
                try
                {
                    web.Navigate().GoToUrl("https://vk.com");
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось перейти по URL" + Environment.NewLine);
                }

                try
                {
                    web.FindElement(By.XPath("//input[@id='index_email']")).SendKeys(LOGIN);
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось ввести логин" + Environment.NewLine);
                }

                try
                {
                    web.FindElement(By.XPath("//input[@id='index_pass']")).SendKeys(PASS);
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось ввести пароль" + Environment.NewLine);
                }
                try
                {
                    web.FindElement(By.XPath("//button[@id='index_login_button']")).Click();
                    web.FindElement(By.XPath("//li[@id='l_msg']")).Click();
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось нажать кнопку ВОЙТИ" + Environment.NewLine);
                }

                try 
                {
                    web.FindElement(By.XPath("//li[@id='l_msg']/a[@class='left_row']/span[@class='left_fixer']/span[@class='left_label inl_bl']")).Click();
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось перейти в сообщения" + Environment.NewLine);
                }

                try
                {
                    web.FindElement(By.XPath("//input[@id='im_dialogs_search']")).SendKeys(RECIPIENT.Text); ;
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось ввести получателя" + Environment.NewLine);
                }

                try
                {
                    web.FindElement(By.XPath("//div[@class='nim-dialog--cw']")).Click(); ;
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось перейти в сообщения с получателем" + Environment.NewLine);
                }

                try
                {
                    web.FindElement(By.XPath("//div[@id='im_editable0']")).SendKeys(OpenQA.Selenium.Keys.Control + "v");
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось вставить картинку" + Environment.NewLine);
                }

                try
                {
                    web.FindElement(By.XPath("//button[@class='im-send-btn im-chat-input--send _im_send im-send-btn_send']")).Click(); ;
                }
                catch (Exception)
                {
                    LOG.AppendText("Ошибка. Не удалось отправить сообщение" + Environment.NewLine);
                }
            }
            catch (Exception)
            {
            }

            web.Dispose();
            web.Quit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!PASSBOX.UseSystemPasswordChar)
                PASSBOX.UseSystemPasswordChar = true;
            else
                PASSBOX.UseSystemPasswordChar = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            for (int i = 0; i < options.Height; i++)
            {
                for (int j = 0; j < options.Width; j++)
                {
                    if (bmp1.GetPixel(i, j).ToArgb() == Color.White.ToArgb())
                    {
                        bmp.SetPixel(i, j, Color.FromName(comboBox1.SelectedItem.ToString()));
                    }
                    else
                    {
                        if (bmp1.GetPixel(i, j).ToArgb() == Color.Black.ToArgb())
                            bmp.SetPixel(i, j, Color.FromName(comboBox2.SelectedItem.ToString()));
                    }
                }
            }
            pictureBox1.Image = bmp;
            Clipboard.SetImage(pictureBox1.Image);
        }
    }
}
