using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculate_The_cutting_spread
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        
        int selectedRow;
        Single SawWidth=4.5f;
        int RodLength=6000;
        
        private void btnAddToList_Click(object sender, EventArgs e)
        {
            if ((txtLength.Text != "") &&  (txtQuantity.Text  != "")) 
            {
                dataGridView.Rows.Add(dataGridView.RowCount, txtLength.Text, txtQuantity.Text);
                //dataGridView.UpdateCellValue(0, 0) ;
                txtID.Text = Convert.ToString (dataGridView.RowCount-1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 12);
            listBox.HorizontalScrollbar = true;
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            selectedRow = e.RowIndex;
            if ((selectedRow > -1))
            {
                DataGridViewRow row = dataGridView.Rows[selectedRow];
                if ((row.Cells[0].Value != null))
                {
                    if (row.Cells[0].Value.ToString() != null)
                    {
                        txtID.Text = row.Cells[0].Value.ToString();
                        txtLength.Text = row.Cells[1].Value.ToString();
                        txtQuantity.Text = row.Cells[2].Value.ToString();
                    }
                }
            }  
         }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if ((selectedRow > -1) && (selectedRow != dataGridView.RowCount )) 
            { 
                if ((txtLength.Text != "") && (txtQuantity.Text != "")) 
                {
                    DataGridViewRow newDataRow = dataGridView.Rows[selectedRow];
                    newDataRow.Cells[0].Value = txtID.Text;
                    newDataRow.Cells[1].Value = txtLength.Text;
                    newDataRow.Cells[2].Value = txtQuantity.Text;
                }
                
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell != null)
            {
                selectedRow = dataGridView.CurrentCell.RowIndex;
                if ((selectedRow > -1) && (selectedRow != dataGridView.RowCount))
                {
                    // delete datagridview row selected row
                    dataGridView.Rows.RemoveAt(selectedRow);
                    txtID.Text = null;
                    txtLength.Text = null;
                    txtQuantity.Text = null;
                }
                for (int i = 0; i < dataGridView.RowCount ; i++)
                {
                    DataGridViewRow newDataRow = dataGridView.Rows[i];
                    newDataRow.Cells[0].Value = i;
                }
            }
        }
        string listData = null;
        private void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView.RowCount > 1)
                {
                    int count = 0;
                    listBox.Items.Clear();
                    string strData = null;
                    listData = null;
                    // declaring and initializing the size of array 
                    double[] arr = new double[1000];
                    for (int i = 0; i < dataGridView.RowCount; i++)
                    {
                        DataGridViewRow newDataRow = dataGridView.Rows[i];
                        for (int j = 0; j < Convert.ToInt32(newDataRow.Cells[2].Value); j++)
                        {
                            arr[count] = Convert.ToDouble(newDataRow.Cells[1].Value);
                            count += 1;
                        }
                    }
                    // Resize the array to a bigger size (five elements larger).
                    Array.Resize(ref arr, count);

                    // Sort array in ascending order. 
                    Array.Sort(arr);

                    // reverse array 
                    Array.Reverse(arr);

                    List<List<double>> res = new List<List<double>>();
                    //List<int> first = new List<int>();
                    res.Add(new List<double>());
                    int l = 0;
                    foreach (double k in arr)
                    {
                        l = 0;
                        while (l < res.Count)
                        {
                            if ((res[l].Sum() + SawWidth * (res[l].Count + 1) + k <= RodLength) || (res[l].Sum() + SawWidth * (res[l].Count) + k == RodLength))
                            {
                                res[l].Add(k);
                                break;
                            }
                            l += 1;
                        }
                        if (l == res.Count)
                        {
                            res.Add(new List<double>());
                            res[l].Add(k);
                        }
                    }
                    int posDat = 0;
                    int cnt = 0;
                    int numProfiles = 0;
                    string bData = null;
                    listData = "Cutting Spread" + "\n" + "\n";
                    // print all element of array 
                    foreach (List<double> iter in res)
                    {
                        numProfiles += 1;
                        strData = Convert.ToString(numProfiles) + "). ";
                        foreach (double i in iter)
                        {
                            strData += i;
                            strData += " , ";
                        }

                        strData += " ---->Sum = " + iter.Sum() + " -(" + Convert.ToString((iter.Count()) * SawWidth) + ")" + " -[" + Convert.ToString(Convert.ToInt32(RodLength - iter.Sum() - (iter.Count()) * SawWidth)) + "]";
                        strData += "\n";
                        cnt = strData.Length;
                        if (cnt < 125)
                        {
                            listData += strData;
                        }
                        else
                        {
                            posDat = strData.LastIndexOf(",", 125, 50) + 1;
                            bData = strData.Substring(0, posDat);
                            listData += bData;
                            bData = "\n" + "        " + "##" + strData.Substring(posDat, cnt - posDat);
                            listData += bData;
                        }

                        //listData += "\n"; 
                        listBox.Items.Add(strData);

                        strData = null;
                    }
                } 
            }

            catch
            {
                MessageBox.Show ("Number of Segments Can't be more then 1000","Error Calaculate");
            }
        }
       
        private void btnClearTable_Click(object sender, EventArgs e)
        {
            string message = "Do you want to Clear Table?";
            string title = "Clear Table Grid";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                dataGridView.Rows.Clear();
                txtID.Text = "";
                txtLength.Text = "";
                txtQuantity.Text = "";
                listBox.Items.Clear();
                txtProfileType.Text = "";
                txtDuplicate.Text = "";
            }
            else
            {
                // Do something  
            }  
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string message = "Do you want to close this window?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                // Do something  
            }  
        }

        private void txtLength_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtLength.Text, "[^0-9.]"))
            {
                MessageBox.Show("Please enter only numbers.","Length");
                txtLength.Text = txtLength.Text.Remove(txtLength.Text.Length - 1);
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtQuantity.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.","Quantity");
                txtQuantity.Text = txtQuantity.Text.Remove(txtQuantity.Text.Length - 1);
            }
        }
        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (listData != null)
            {
                DGVPrinter printer = new DGVPrinter();
                printer.Title = "List Of length Profiles Nedded - " + txtProfileType.Text;
                printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = false;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.Footer = listData;
                printer.FooterSpacing = 25;
                printer.FooterAlignment = StringAlignment.Near;
                printer.printDocument.DefaultPageSettings.Landscape = true; //print Lanscape =true
                printer.PrintDataGridView(dataGridView);
            }
            else
            {
                MessageBox.Show("Please, Calculate First","Instruction - Print");
            }
            
            
        }

        private void txtSawWidth_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtSawWidth.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.", "Saw Width");
                txtSawWidth.Text = txtSawWidth.Text.Remove(txtSawWidth.Text.Length - 1);
            }
            else
            {
                SawWidth = Convert.ToSingle(txtSawWidth.Text);
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                btnAddToList.PerformClick();
            } 
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnExit.PerformClick();
        }

        

        private void txtRodLength_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtRodLength.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.", "Saw Width");
                txtRodLength.Text = txtRodLength.Text.Remove(txtRodLength.Text.Length - 1);
            }
            else
            {
                RodLength = Convert.ToInt32(txtRodLength.Text);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnPrint.PerformClick();
        }

                
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 Form2 = new AboutBox1();
            Form2.ShowDialog();

        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if ((txtDuplicate.Text != "") && (dataGridView.RowCount>0))
            {
                if (Convert.ToInt32(txtDuplicate.Text )> 1)
                {
                    for (int i = 0; i < dataGridView.RowCount; i++)
                    {
                        DataGridViewRow row = dataGridView.Rows[i];
                        DataGridViewRow newDataRow = dataGridView.Rows[i];
                        newDataRow.Cells[2].Value = Convert.ToString(Convert.ToInt32(row.Cells[2].Value) * Convert.ToInt32(txtDuplicate.Text)) ;
                    }
                    txtDuplicate.Text = null;
                }
            }
        }

        private void txtDuplicate_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtDuplicate.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.", "Duplicate");
                txtDuplicate.Text = txtDuplicate.Text.Remove(txtDuplicate.Text.Length - 1);
            }
        }
    }
    }

