using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Globalization;
using System.Data.SqlClient; 
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class PageSchool : System.Web.UI.Page
{
    private SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=School;Integrated Security=True");
    private SqlCommand com = new SqlCommand();
    private DateTime dt = DateTime.Now;
    private PersianCalendar pc = new PersianCalendar();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            con.Open();
            Session.Add("dresh", 0);
            com.CommandText = "select base,ns from tblschool where id=" + Session["id_s"].ToString();
            com.Connection = con;
            SqlDataReader dr = com.ExecuteReader();
            dr.Read();
            lbltitr.Text =  "آموزشگاه"+" "+dr["ns"].ToString();
            if (Session["m"].ToString() == "0")
            {
                Menu1.Items[0].Enabled = true;
            }
            else
            {
                Menu1.Items[1].Enabled = true;
                Menu1.Items[2].Enabled = true;
                Menu1.Items[3].Enabled = true;
                Menu1.Items[4].Enabled = true;
                Menu1.Items[6].Enabled = true;

            }
 
            if (int.Parse(dr["base"].ToString ()) == 3)
            {
                dryer.AutoPostBack = true; 
            }
        
        }
         
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dryer.SelectedValue != "اول دبیرستان" && dryer.AutoPostBack==true)
        {
            drresh.Visible = true;
            lblresh.Visible = true;
        }
        else
        {
            drresh.Visible = false;
            lblresh.Visible = false;

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        int flag1=0;
        int flag2=0;
        lbld2.Visible =false ;
        lbld1.Visible =false ;
        if (CheckBox1.Checked == true)
        {
            int yer = int.Parse(TextBox5.Text.Substring(0, 4));
            if (pc.GetYear(dt)==yer && int.Parse(TextBox5.Text.Substring(5, 2))<=12 && int.Parse(TextBox5.Text.Substring(8, 2))<=31 )
            {
                flag1=1;
            }
            else
            {
                lbld1.Visible =true ;
                flag2 =1;
            }
        }
        if (CheckBox2.Checked == true)
        {

            int yer1 = int.Parse(TextBox7.Text.Substring(0, 4));
            dt= pc.AddYears (dt,1);            
            
            if (yer1 ==pc.GetYear(dt) && int.Parse(TextBox7.Text.Substring(5, 2)) <= 12 && int.Parse(TextBox7.Text.Substring(8, 2)) <= 31)
            {
                flag1 += 2;
            }
            else
            {
                lbld2.Visible = true;
                flag2 = 1;
            }
        }
        if (flag2 !=1)
        {
            sabtlesson(flag1);
        }
              
    }
    private void sabtlesson(int f)
    {
        
        if (drresh.Visible == false)
            if (dryer.SelectedValue == "اول دبیرستان")
            {
                Session["dresh"] = 1;
            }
            else
            {
                Session["dresh"] = 0;
            }
        else
            Session["dresh"] = drresh.SelectedValue;
        com.Connection = con;
        con.Open();
        int i = dryer.SelectedIndex;
        i = i + 1;
        com.CommandText = "select * from lesson where id_school=" + Session["id_s"].ToString() + " and code=" + TextBox1.Text;
        SqlDataReader dr = com.ExecuteReader();
        dr.Read();
        if (!dr.HasRows)
        {
            dr.Close();
            
            if (f == 1)
            {
                com.CommandText = "insert into Lesson (code,yer,zarib,resh,name,id_school,date1,date2) values(" + TextBox1.Text + "," + i + "," + DropDownList2.SelectedValue + "," + Session["dresh"].ToString() + ",'" + TextBox2.Text + "'," + Session["id_s"].ToString() + ",'"+TextBox5.Text+"','-')";
            }
            else if (f == 2)
            {
                com.CommandText = "insert into Lesson (code,yer,zarib,resh,name,id_school,date2,date1) values(" + TextBox1.Text + "," + i + "," + DropDownList2.SelectedValue + "," + Session["dresh"].ToString() + ",'" + TextBox2.Text + "'," + Session["id_s"].ToString() + ",'" + TextBox7.Text + "','-')";
            }
            else if (f == 3)
            {
                com.CommandText = "insert into Lesson (code,yer,zarib,resh,name,id_school,date1,date2) values(" + TextBox1.Text + "," + i + "," + DropDownList2.SelectedValue + "," + Session["dresh"].ToString() + ",'" + TextBox2.Text + "'," + Session["id_s"].ToString() + ",'" + TextBox5.Text + "','" + TextBox7.Text + "')";
            }
            
            com.ExecuteNonQuery();
            lblerror.Visible = false;
        }
        else
            lblerror.Visible = true;

        txtempty();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        com.Connection = con;
        con.Open();
        if (Session["m"].ToString() == "1")
        {
            com.CommandText = "select * from tbl_login_man where id_school=" + Session["id_s"].ToString() + " and password='" + TextBox3.Text + "'";
            SqlDataReader dr = com.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                dr.Close();
                com.CommandText = "update tbl_login_man set password='" + TextBox4.Text + "' where  id_school=" + Session["id_s"].ToString();
                com.ExecuteNonQuery();
                lblerr.Visible = false;
                changepassword.Visible = false;

            }
            else
                lblerr.Visible = true;
        }
        else
        {
            com.CommandText = "select * from tbl_login_tec where id_school=" + Session["id_s"].ToString() + " and password='" + TextBox3.Text + "' and username="+Session["id_t"].ToString () ;
            SqlDataReader dr = com.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                dr.Close();
                com.CommandText = "update tbl_login_tec set password='" + TextBox4.Text + "' where  id_school=" + Session["id_s"].ToString() + " and username=" + Session["id_t"].ToString();
                com.ExecuteNonQuery();
                lblerr.Visible = false;
                changepassword.Visible = false;

            }
            else
                lblerr.Visible = true;

        }
    }

protected void  menu(object sender, MenuEventArgs e)
{
    
    switch (int.Parse(Menu1.SelectedValue))
    {
        case 30:
            enterlesson.Visible = true;
            break;
        case 6:
            changepassword.Visible = true;
            break;
        case 31:
            Response.Redirect("editlesson.aspx");
            break;
        case 5:
            addteacher.Visible = true;
            break;
        case 13:
            editte.Visible = true;
            break;
        case 2:
            Response.Redirect("classb.aspx");  
            break;
        case 7:
            Response.Redirect("program.aspx");
            break;
        case 81:
            Session.Add("index", 1);
            Response.Redirect("editclass.aspx");            
            break;
        case 82:
            Session.Add("index", 2);
            Response.Redirect("editclass.aspx");            
            break;
        case 32:
            Response.Redirect("teachtolesson.aspx");
            break;
        case 10:
            Response.Redirect("entermark.aspx");
            break;
        case 11:
            Response.Redirect("editmark.aspx");
            break;
        case  40:
            Response.Redirect("search.aspx");
            break;
        case 41:
            Response.Redirect("editprosudent.aspx");
            break;
       

    }

    
}
    protected void txtempty()
    {
        TextBox1.Text = "";
        TextBox2.Text = "";
        txtaddt.Text = "";
        txtecd.Text = "";
        txtidt.Text = "";
        txtnamet.Text = "";
        txttelt.Text = "";
        TextBox6.Text = "";
        TextBox5.Text = "";
        TextBox7.Text ="";
        TextBox7.Enabled = false;
        CheckBox2.Checked = false; 
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        com.CommandText = "select * from tblteacher where id=" + txtidt.Text ;
        com.Connection =con;
        con.Open();
        SqlDataReader dr = com.ExecuteReader();
        dr.Read();
        if (!dr.HasRows)
        {
            dr.Close(); 
            com.CommandText = "insert into tblteacher values(" + txtidt.Text + ",'" + txtnamet.Text + "','" + txtecd.Text + "','" + txttelt.Text + "','" + txtaddt.Text + "'," + Session["id_s"].ToString() + ",'" + drm.SelectedValue + "')";
            com.ExecuteNonQuery();
            com.CommandText = "insert into tbl_login_tec values("+Session["id_s"].ToString () +",'"+ txtidt.Text +"','"+TextBox6.Text+"')";
            com.ExecuteNonQuery();  
            lblerrt.Visible = false;
            txtempty();

        }
        else
            lblerrt.Visible = true; 
    }

    protected void drresh_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drresh.SelectedValue == "ریاض فیزیک")
        {
            Session["dresh"] = 2;
        }
        else if (drresh.SelectedValue == "علوم تجربی")
        {
            Session["dresh"] = 3;
        }
        else if (drresh.SelectedValue == "علوم انسانی")
        {
            Session["dresh"] = 4;
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox1.Checked == false)
        {
            RegularExpressionValidator1.Enabled = false;
            TextBox5.Enabled = false;
            TextBox5.Text = "";

        }
        else
        {
            RegularExpressionValidator1.Enabled = true ;
            TextBox5.Enabled = true;

        }
    }
    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox2.Checked == false)
        {
            RegularExpressionValidator2.Enabled = false;
            TextBox7.Enabled = false;
            TextBox7.Text = "";

        }
        else
        {
            RegularExpressionValidator2.Enabled = true;
            TextBox7.Enabled = true;

        }

    }
}
