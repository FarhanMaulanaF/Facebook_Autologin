using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace Facebook_Autologin_Selenium
{
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        ChromeDriver drv; Thread th;
        private void TextBoxChanged(object sender, EventArgs e)
        {
            if (tbUsername.Text != "" && tbPassword.Text != "")
            {
                btnLogin.ForeColor = Color.Lime;
                btnLogin.Cursor = Cursors.Hand;
            }
            else
            {
                btnLogin.ForeColor = Color.Red;
                btnLogin.Cursor = Cursors.No;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (btnLogin.Cursor == Cursors.Hand)
            {
                th = new Thread(Result); th.Start();
            }
        }

        private void Result()
        {
            btnLogin.ForeColor = Color.Gold;
            btnLogin.UseWaitCursor = true;
            btnLogin.Text = "Testing...";
            OpenSelenium();
            Thread.Sleep(3000);
            Login(tbUsername.Text, tbPassword.Text);
            if (TestAccount())
                MessageBox.Show("Login Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Username or Password is wrong!","Warning!",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
            CloseSelenium();
            btnLogin.ForeColor = Color.Lime;
            btnLogin.UseWaitCursor = false;
            btnLogin.Text = "Login";
        }

        private void Login(string username, string password)
        {
            try
            {
                drv.FindElements(By.XPath("//input[@class='inputtext _55r1 inputtext _1kbt inputtext _1kbt']"))[0].SendKeys(username); Thread.Sleep(3000);
                drv.FindElements(By.XPath("//input[@class='inputtext _55r1 inputtext _9npi inputtext _9npi']"))[0].SendKeys(password); Thread.Sleep(3000);
                drv.FindElement(By.XPath("//button[@class='_42ft _4jy0 _52e0 _4jy6 _4jy1 selected _51sy']")).Click(); Thread.Sleep(1000);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private bool TestAccount()
        {
            if (drv.Url == "https://www.facebook.com/login")
                return true;
            else
                return false;
        }

        private void OpenSelenium()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            drv = new ChromeDriver(service);
            drv.Navigate().GoToUrl("https://www.facebook.com/login");
        }

        private void Main_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseSelenium();
        }
        private void CloseSelenium()
        {
            drv.Quit();
        }
    }
}
