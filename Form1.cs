using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF_Lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            var ent = new CompanyEntities();
            var dept = from d in ent.Departments select d;
            foreach (var d in dept)
            {
                comboBox1.Items.Add(d.dept_Id.ToString());
            }
        }

        // Department -> comboBox1
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            var selectedDeptID = int.Parse(comboBox1.Text);
            var ent = new CompanyEntities();

            var deptFound = (from d in ent.Departments 
                where d.dept_Id == selectedDeptID 
                select d).FirstOrDefault();
            if (deptFound != null)
            {
                textBox1.Text = deptFound.dept_Id.ToString();
                textBox2.Text = deptFound.dept_name;
                var emp = from em in ent.Employees
                    where em.dept_Id == selectedDeptID
                    select em;
                foreach (var em in emp)
                {
                    comboBox2.Items.Add(em.id.ToString());
                }
            }
            else
            {
                MessageBox.Show("Not Found!");
            }

        }

        // Add New Department
        private void button1_Click(object sender, EventArgs e)
        {
            var dept = new Department();
            dept.dept_Id = int.Parse(textBox1.Text);
            dept.dept_name = textBox2.Text;
            var ent = new CompanyEntities();
            try
            {
                ent.Departments.Add(dept);
                ent.SaveChanges();
                MessageBox.Show("Added Successfully!");
            }
            catch
            {
                MessageBox.Show("Already Exist!");
            }
            finally
            {
                textBox1.Text = textBox2.Text = "";
                refreshComboBox();
            }
        }

        // Update Department
        private void button2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                var selectedID = int.Parse(textBox1.Text);
                var ent = new CompanyEntities();
                var deptFound = (from d in ent.Departments
                    where d.dept_Id == selectedID
                    select d).FirstOrDefault();
                if (deptFound != null && !String.IsNullOrEmpty(textBox2.Text))
                {
                    deptFound.dept_name = textBox2.Text;
                    ent.SaveChanges();
                    MessageBox.Show("Updated Successfully!");
                }
                else if (String.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Name is empty!");
                }
                else
                {
                    MessageBox.Show("Not Found!");
                }
            }
            else
            {
                MessageBox.Show("ID is empty!");
            }
            
            textBox1.Text = textBox2.Text = "";
            refreshComboBox();
        }

        // Delete Department
        private void button3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                var selectedID = int.Parse(textBox1.Text);
                var ent = new CompanyEntities();
                var deptFound = (from d in ent.Departments
                    where d.dept_Id == selectedID
                    select d).FirstOrDefault();
                int count = 0;
                if (deptFound != null)
                {
                    var emp = from em in ent.Employees
                        where em.dept_Id == selectedID
                        select em;
                    foreach (var em in emp)
                    {
                        ent.Employees.Remove(em);
                        count++;
                    }
                    ent.Departments.Remove(deptFound);
                    ent.SaveChanges();
                    count++;
                    MessageBox.Show($"{count} rows affected!");
                }
                else
                {
                    MessageBox.Show("Not Found!");
                }
            }
            else
            {
                MessageBox.Show("ID is empty!");
            }
            textBox1.Text = textBox2.Text = "";
            refreshComboBox();
        }

        // Add New Employee
        private void button4_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                var selectedID = int.Parse(textBox1.Text);
                var ent = new CompanyEntities();
                var deptFound = (from d in ent.Departments
                    where d.dept_Id == selectedID
                    select d).FirstOrDefault();
                if (deptFound != null)
                {
                    var emp = new Employee();
                    emp.id = int.Parse(textBox3.Text);
                    emp.name = textBox4.Text;
                    emp.dept_Id = selectedID;
                    try
                    {
                        ent.Employees.Add(emp);
                        ent.SaveChanges();
                        MessageBox.Show("Added Successfully!");
                    }
                    catch
                    {
                        MessageBox.Show("Already Exist!");
                    }
                    finally
                    {
                        textBox3.Text = textBox4.Text = "";
                    }

                }
                else
                {
                    MessageBox.Show("Department Not Found!");
                }
            }
            else
            {
                MessageBox.Show("No Department Selected!");
            }

            refreshComboBox();
        }

        // Update Employee
        private void button5_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                var selectedID = int.Parse(textBox3.Text);
                var ent = new CompanyEntities();
                var empFound = (from em in ent.Employees
                    where em.id == selectedID
                    select em).FirstOrDefault();
                if (empFound != null && !String.IsNullOrEmpty(textBox4.Text))
                {
                    empFound.name = textBox4.Text;
                    ent.SaveChanges();
                    MessageBox.Show("Updated Successfully!");
                }
                else if (String.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Name is empty!");
                }
                else
                {
                    MessageBox.Show("Not Found!");
                }
            }
            else
            {
                MessageBox.Show("ID is empty!");
            }
            textBox3.Text = textBox4.Text = "";
            refreshComboBox();

        }

        // Delete Employee
        private void button6_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox4.Text))
            {
                var str = textBox4.Text;
                var ent = new CompanyEntities();
                var empFound = from em in ent.Employees
                    where em.name == str
                    select em;
                int count = 0;
                foreach (var emp in empFound)
                {
                    ent.Employees.Remove(emp);
                    count++;
                }

                if (count > 0)
                {
                    ent.SaveChanges();
                    MessageBox.Show($"{count} rows affected!");
                }
                else
                {
                    MessageBox.Show("Not Found!");
                }
            }
            else
            {
                MessageBox.Show("Name is empty!");
            }
            
            textBox3.Text = textBox4.Text = "";
            refreshComboBox();
        }

        // Employee -> comboBox2
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedEmpID = int.Parse(comboBox2.Text);
            var ent = new CompanyEntities();

            var empFound = (from em in ent.Employees
                where em.id == selectedEmpID
                select em).FirstOrDefault();
            if (empFound != null)
            {
                textBox3.Text = empFound.id.ToString();
                textBox4.Text = empFound.name;
            }
            else
            {
                MessageBox.Show("Not Found!");
            }
        }

        private void refreshComboBox()
        {
            comboBox1.Items.Clear();
            var ent = new CompanyEntities();
            var dept = from d in ent.Departments select d;
            foreach (var d in dept)
            {
                comboBox1.Items.Add(d.dept_Id.ToString());
            }
        }
    }
}
