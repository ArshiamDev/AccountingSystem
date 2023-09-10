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
    public partial class adminFrm : Form
    {
        accauntingDBEntities context = new accauntingDBEntities();
        string userName;
        int userId;

        //////////// customer Side //////////////
        int customerId;
        public void editCustomer(int id)
        {
            var q = (from a in context.CustomerTbls
                    where a.customerID == id
                    select a).FirstOrDefault();

            q.customerName = textBox1.Text.ToString();
            q.customerFamily = textBox2.Text.ToString();
            q.customerFatherName = textBox3.Text.ToString();
            q.customerPhoneNumber = textBox4.Text.ToString();
            q.customerAddress = textBox1.Text.ToString();
            context.SaveChanges();
            showCustomer();
        }
        public void addCustomer()
        {
            CustomerTbl c = new CustomerTbl();
            c.customerName = textBox1.Text.ToString();
            c.customerFamily = textBox2.Text.ToString();
            c.customerFatherName = textBox3.Text.ToString();
            c.customerPhoneNumber = textBox4.Text.ToString();
            c.customerNationalCode = Int64.Parse(textBox5.Text);
            c.customerAddress = textBox6.Text.ToString();
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

        //////////// article Side //////////////

        public void addArticle()
        {
            ArticleTbl c = new ArticleTbl();
            c.articleCode = textBox7.Text.ToString();
            c.articleName = textBox8.Text.ToString();
            c.articleDesc = textBox9.Text.ToString();
            c.articlePrice = Int64.Parse(textBox10.Text);
            c.articleExDate = dateTimePicker1.Value;
            context.ArticleTbls.Add(c);
            context.SaveChanges();
        }

        //////////// operator Side //////////////

        public void addOperator()
        {
            userTbl c = new userTbl();
            c.userUsername = textBox13.Text.ToString();
            c.userPassword = textBox14.Text.ToString();
            c.userRoleIdFK = 2 ;
            context.userTbls.Add(c);
            context.SaveChanges();
        }

        //////////// show storage Side //////////////

        public void showStorage()
        {
            var storage = from a in context.mainStorageTbls
                          select new {a.articleCode,a.operatorName, a.setDate , a.articlePrice , a.articleCount };

            var articleCount = from a in context.mainStorageTbls
                               join b in context.ArticleTbls
                               on a.articleCode equals b.articleCode
                               select new { a.articleCount, b.articleName };

            
            dataGridView5.DataSource = articleCount.ToList();
            dataGridView2.DataSource = storage.ToList();
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
                    select new { a.articleName , a.articleCount , a.articlePrice , a.totalPrice};
            dataGridView3.DataSource = q.ToList();
        }

        public void buyArticle()
        {
            DateTime time = DateTime.Now;
            var articleDesc = (from a in context.ArticleTbls
                              where (comboBox1.SelectedIndex + 1 ) == a.articleID
                              select new { a.articlePrice, a.articleName , a.articleID , a.articleExDate , a.articleCode }).FirstOrDefault();

            var storage = from a in context.StorageTbls
                          select a;


            StorageTbl s = new StorageTbl();

            s.articleName = articleDesc.articleName.ToString();
            s.articleIdFK = articleDesc.articleID;
            s.articleCount = (Int32)numericUpDown1.Value;
            s.articlePrice = articleDesc.articlePrice;
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
                    select new {a.sellerPerson , b.customerName , b.customerAddress , b.customerPhoneNumber , c.articleName , a.articlePrice , a.articleCount , a.discount , a.sellPrice     };
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
            s.articlePrice = ((articleDesc.articlePrice *7)/100 )+articleDesc.articlePrice;
            s.articleCount = (Int32)numericUpDown2.Value;
            s.discount = Int64.Parse(textBox16.Text);
            s.articleIdFK = articleDesc.articleID;
            s.sellPrice = articleDesc.articlePrice * s.articleCount;
            context.sellFactors.Add(s);
            context.SaveChanges();
            showSellFactor();
        }

        ////////////             main Side            //////////////

        public adminFrm()
        {
            InitializeComponent();
        }
        public adminFrm( userTbl user)
        {
            InitializeComponent();
            userName = user.userUsername.ToString();
            userId = user.userID;
        }

        private void adminFrm_Load(object sender, EventArgs e)
        {
            label8.Text = userName +"   خوش آمدید";
            showStorage();
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            customerId = (Int32)dataGridView1.CurrentRow.Cells[0].Value;
            selectCustomer(customerId);
            textBox5.ReadOnly = true    ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && textBox3.Text.Length > 0 && textBox4.Text.Length > 0 && textBox5.Text.Length > 0 && textBox6.Text.Length > 0  ) {
                addCustomer();
                MessageBox.Show("کاربر با موفقیت افزوده شد");
            }
            else
            {
                MessageBox.Show("فیلد ها باید پر شوند");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            editCustomer(customerId);
            MessageBox.Show("کاربر با موفقیت به روزرسانی شد");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //if (textBox7.Text.Length > 0 && textBox8.Text.Length > 0 && textBox9.Text.Length > 0 && textBox10.Text.Length > 0 && dateTimePicker1.Value == null)
            //{
                addArticle();
                MessageBox.Show("کالا با موفقیت افزوده شد");
            //}
            //else
            //{
            //    MessageBox.Show("فیلد ها باید پر شوند");

            //}

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox13.Text.Length > 0 && textBox14.Text.Length > 0 )
            {
                addOperator();
                MessageBox.Show("اپراتور با موفقیت افزوده شد");
            }
            else
            {
                MessageBox.Show("فیلد ها باید پر شوند");

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //if (comboBox1.Items == null && numericUpDown1.Value == null)
            //{
                buyArticle();
                MessageBox.Show("کالا با موفقیت افزوده شد");
            //}
            //else
            //{
            //    MessageBox.Show("فیلد ها باید پر شوند");
            //}

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

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
            textBox15.Clear();
            textBox18.Clear();
            showStorageFacotr();
            showStorage();
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            showStorage();
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
            articleCombo();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            addToSell();
            MessageBox.Show(" کالا با موفقیت افزوده شد");
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

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {
            textBox5.ReadOnly = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
