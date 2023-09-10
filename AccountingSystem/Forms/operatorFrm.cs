using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingSystem.Forms
{
    public partial class operatorFrm : Form
    {
        accauntingDBEntities context = new accauntingDBEntities();
        string userName;
        int userId;

        //////////// customer Side //////////////
        public void addCustomer()
        {
            CustomerTbl c = new CustomerTbl();
            c.customerName = textBox1.Text.ToString();
            c.customerFamily = textBox2.Text.ToString();
            c.customerFatherName = textBox3.Text.ToString();
            c.customerPhoneNumber = textBox4.Text.ToString();
            c.customerNationalCode = Int64.Parse(textBox5.Text);
            c.customerAddress = textBox1.Text.ToString();
            context.CustomerTbls.Add(c);
            context.SaveChanges();
            showCustomer();
        }
        public void selectCustomer(int id)
        {
            var query = (from a in context.CustomerTbls
                         where a.customerID == id
                         select a).FirstOrDefault();
            textBox1.Text = query.customerName;
            textBox2.Text = query.customerFamily;
            textBox3.Text = query.customerFatherName;
            textBox4.Text = query.customerPhoneNumber.ToString();
            textBox5.Text = query.customerNationalCode.ToString();
            textBox6.Text = query.customerAddress;
        }
        public void showCustomer()
        {
            var customer = from a in context.CustomerTbls
                           select a;
            dataGridView1.DataSource = customer.ToList();
        }

        //////////// buy Side //////////////

        public void articleCombo()
        {
            var q = (from a in context.ArticleTbls
                     select a).ToList();

            foreach (var item in q)
            {
                comboBox1.Items.Add(item.articleName);
                comboBox2.Items.Add(item.articleName);
            }
        }

        public void showStorageFacotr()
        {
            var q = from a in context.StorageTbls
                    select new { a.articleName, a.articleCount, a.articlePrice, a.totalPrice };
            dataGridView3.DataSource = q.ToList();
        }

        public void buyArticle()
        {
            DateTime time = DateTime.Now;
            var articleDesc = (from a in context.ArticleTbls
                               where (comboBox1.SelectedIndex + 1) == a.articleID
                               select new { a.articlePrice, a.articleName, a.articleID, a.articleExDate, a.articleCode }).FirstOrDefault();

            var storage = from a in context.StorageTbls
                          select a;


            StorageTbl s = new StorageTbl();

            s.articleName = articleDesc.articleName.ToString();
            s.articleIdFK = articleDesc.articleID;
            s.articleCount = (Int32)numericUpDown1.Value;
            s.articlePrice = ((articleDesc.articlePrice * 7) / 100) + articleDesc.articlePrice;
            s.totalPrice = ((Int32)numericUpDown1.Value * articleDesc.articlePrice);
            s.operatorIdFK = userId;
            s.articleExDate = articleDesc.articleExDate;
            s.articleCode = articleDesc.articleCode;
            context.StorageTbls.Add(s);
            context.SaveChanges();
            showStorageFacotr();
            textBox18.Text += (articleDesc.articlePrice * 5) / 100;
            textBox15.Text += s.totalPrice + (articleDesc.articlePrice * 5) / 100;

        }
        //////////// buy Side  ::: Add to main storage //////////////

        public void addStorage()
        {
            DateTime time = DateTime.Now;

            mainStorageTbl ms = new mainStorageTbl();

            var factor = (from a in context.StorageTbls
                          select a).ToList();

            foreach (var item in factor)
            {
                ms.articleCode = item.articleCode.ToString();
                ms.operatorName = userName;
                ms.setDate = time;
                ms.articlePrice = item.articlePrice;
                ms.articleCount = item.articleCount;
                ms.articleExDate = item.articleExDate;
                context.mainStorageTbls.Add(ms);
                context.SaveChanges();
            }
            showStorageFacotr();
        }

        //////////// Sell Side //////////////
        public void showSellFactor()
        {
            var q = from a in context.sellFactors
                    join b in context.CustomerTbls
                    on a.customerIdFK equals b.customerID
                    join c in context.ArticleTbls
                    on a.articleIdFK equals c.articleID
                    select new { a.sellerPerson, b.customerName, b.customerAddress, b.customerPhoneNumber, c.articleName, a.articlePrice, a.articleCount, a.discount, a.sellPrice };
            dataGridView4.DataSource = q.ToList();
        }
        public void addToSell()
        {
            var q = (from a in context.CustomerTbls
                     where textBox12.Text.ToString() == a.customerName
                     select a).FirstOrDefault();

            var articleDesc = (from a in context.ArticleTbls
                               where (comboBox2.SelectedIndex + 1) == a.articleID
                               select new { a.articlePrice, a.articleName, a.articleID, a.articleExDate, a.articleCode }).FirstOrDefault();


            sellFactor s = new sellFactor();
            s.sellerPerson = textBox11.Text.ToString();
            s.customerIdFK = q.customerID;
            s.customerAddress = q.customerAddress;
            s.customerPhoneNumber = q.customerPhoneNumber;
            s.articleName = articleDesc.articleName.ToString();
            s.articlePrice = articleDesc.articlePrice;
            s.articleCount = (Int32)numericUpDown2.Value;
            s.discount = Int64.Parse(textBox16.Text);
            s.articleIdFK = articleDesc.articleID;
            s.sellPrice = articleDesc.articlePrice * s.articleCount;
            context.sellFactors.Add(s);
            context.SaveChanges();
            showSellFactor();
        }



        public operatorFrm()
        {
            InitializeComponent();
        }
        public operatorFrm(userTbl user)
        {
            InitializeComponent();
            userName = user.userUsername;
            userId = user.userID;
        }
        private void operatorFrm_Load(object sender, EventArgs e)
        {
            label8.Text = userName +"  خوش آمدید ";
            showCustomer();
            articleCombo();
            showStorageFacotr();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginFrm l = new LoginFrm();
            l.ShowDialog();

        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            showCustomer();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addCustomer();
            MessageBox.Show("مشتری با موفقیت افزوده شد !");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            buyArticle();
            MessageBox.Show("کالا با موفقیت افزوده شد");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            addStorage();
            MessageBox.Show("خرید کالا با موفقیت انجام شد");
            StorageTbl s = new StorageTbl();
            var q = (from a in context.StorageTbls
                     select a).ToList();
            foreach (var item in q)
            {
                context.StorageTbls.Remove(item);
            }
            context.SaveChanges();
            showStorageFacotr();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" کالا با موفقیت افزوده شد");
            var q = (from a in context.sellFactors
                     select a).ToList();
            foreach (var item in q)
            {
                context.sellFactors.Remove(item);
            }
            context.SaveChanges();
            addToSell();
            showSellFactor();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" فاکتور با موفقیت افزوده شد");
            var q = (from a in context.sellFactors
                     select a).ToList();
            foreach (var item in q)
            {
                context.sellFactors.Remove(item);
            }
            context.SaveChanges();
            showSellFactor();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
