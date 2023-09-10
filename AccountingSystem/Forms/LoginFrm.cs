using AccountingSystem.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AccountingSystem
{
    public partial class LoginFrm : Form
    {
        accauntingDBEntities context = new accauntingDBEntities();

        public int checkUser(string username , string password)
        {
            var checkRole = (from a in context.userTbls
                            where a.userUsername == username && a.userPassword == password
                            select a).FirstOrDefault();
            if (checkRole == null)
            {
                return -1;
            }
            else
            {
                if (checkRole.userRoleIdFK == 1)
                {
                    return 1;
                }
                else if (checkRole.userRoleIdFK == 2)
                {
                    return 2;
                }
                else
                {
                    return -1;
                }
            }
        }

        public LoginFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var user = (from a in context.userTbls
                             where a.userUsername == textBox1.Text && a.userPassword == textBox2.Text
                             select a).FirstOrDefault();
            

            if (checkUser(textBox1.Text, textBox2.Text) == 1)
            {
                this.Hide();
                adminFrm a = new adminFrm(user);
                a.ShowDialog();
            }

            else if(checkUser(textBox1.Text, textBox2.Text) == 2)
            {
                this.Hide();
                operatorFrm o = new operatorFrm(user);
                o.ShowDialog();
            }

            else
            {
                MessageBox.Show("کاربر یافت نشد ");
                textBox1.Clear();
                textBox2.Clear();
            }

        }

        private void LoginFrm_Load(object sender, EventArgs e)
        {
            var role = from a in context.RoleTbls
                       select a;
            var user = from a in context.userTbls
                       select a;

            if(role.Any() && user.Any())
            {

            }
            else
            {
                RoleTbl r = new RoleTbl();
                RoleTbl r2 = new RoleTbl();
                userTbl u = new userTbl();
                userTbl u2 = new userTbl();

                r.roleName = "admin";
                context.RoleTbls.Add(r);

                r2.roleName = "user";
                context.RoleTbls.Add(r2);

                u.userUsername = "admin";
                u.userPassword = "admin";
                u.userRoleIdFK = 1;
                context.userTbls.Add(u);

                u2.userUsername = "user";
                u2.userPassword = "user";
                u2.userRoleIdFK = 2;
                context.userTbls.Add(u2);
                context.SaveChanges();
            }
        }
    }
}