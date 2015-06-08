using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;
using DotNetOpenAuth;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        OpenIdRelyingParty rp = new OpenIdRelyingParty(); //Provides the programmatic facilities to act as an OpenId consumer.
        var response = rp.GetResponse(); //checks for a response
        if (response != null) //if there is a response
        {
            switch (response.Status) //this checks the status of the response
            {
                case AuthenticationStatus.Authenticated: //if the status is "authenticated" then...
                    Session["GoogleIdentifier"] = response.ClaimedIdentifier.ToString(); //gets the current session object provided by ASP.Net which is "GoogleIdentifier" and sets the claimed identifier from the response to it
                    FetchResponse fetch = response.GetExtension<FetchResponse>(); //looks for openID extension responses
                    Session["first"] = fetch.GetAttributeValue(WellKnownAttributes.Name.First); //gets the users first and last name 
                    Session["last"] = fetch.GetAttributeValue(WellKnownAttributes.Name.Last);
                    welcomeLbl.Text = "Welcome, " + Session["first"].ToString() + " " +
                        Session["last"].ToString() + "!"; //changes the welcome to what it fetched
                    Session["email"] = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                    btnGoogleLogin.Visible = false; //takes away the button
                    break;
                case AuthenticationStatus.Canceled: //if the status is "canceled"
                    break; //it just breaks
                case AuthenticationStatus.Failed: //if the status is "failed"
                    Session["GoogleIdentifier"] = "Login Failed."; //it lets you know that it failed
                    break;
            }
        }

    }
    protected void insertRecordBtn_Click(object sender, EventArgs e)
    {
        if (Session["first"] != null)
        {

            if (FileUpload1.HasFile)
                try
                {
                   FileUpload1.SaveAs("C:\\Temp\\" + FileUpload1.FileName);
                   Label1.Text = "File name: " + FileUpload1.PostedFile.FileName;
                }
                catch (Exception temp)
                {
                    Label1.Text = "Error: " + temp.Message.ToString();
                }
            else
            {
                Label1.Text = "No file found";
            }
            //String filePath;
            String path;
            String pathURL;

            using (System.Drawing.Image tempImage = System.Drawing.Image.FromFile("C:\\Temp\\" + FileUpload1.FileName))
            {

                // Create string to draw.
                String drawString = Meme_Text_Box.Text;

                // Create font and brush.
                Font drawFont = new Font("Impact", 100);
                SolidBrush drawBrush = new SolidBrush(Color.Red);

                // Create point for upper-left corner of drawing.
                PointF drawPoint = new PointF(75.0F, 25.0F);

                // Draw string to screen.
                using (Graphics g = Graphics.FromImage(tempImage))
                {
                    g.DrawString(drawString, drawFont, drawBrush, drawPoint);

                }
                using (var m = new MemoryStream())
                {
                    String time_stamp = Stopwatch.GetTimestamp().ToString();
                    String uniqueName = alt_text_box.Text;
                    path = Server.MapPath(@"\\temp\\") + uniqueName + time_stamp + FileUpload1.FileName;
                    pathURL = "\\temp\\" + uniqueName + time_stamp + FileUpload1.FileName;


                    new Bitmap(tempImage, 300, 300).Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Image1.ImageUrl = pathURL;
                    
                }

            }

            String myconn = "uid=christensenkc;server=***.***.***.***;port=3306;database=it210b;password=*******;";

            MySqlConnection conn = new MySqlConnection(myconn);

            MySqlCommand command = conn.CreateCommand();

            conn.Open();

            String email = "'" + Session["email"].ToString() + "'";
            String query1 = "SELECT UserId FROM it210b.Users WHERE Email = " + email;

            MySqlCommand check = new MySqlCommand(query1, conn);
            String ID = check.ExecuteScalar().ToString();

            conn.Close();
            conn.Open();


            // Here goes the code needed to perform operations on the
            string query = "INSERT INTO  images values (@image_id,@image_path,@image_approved,@alt_text,@userID,@numLikes,@createdAt,@updatedAt)";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@image_id", null);
            cmd.Parameters.AddWithValue("@image_path", pathURL);
            cmd.Parameters.AddWithValue("@image_approved", 1);
            cmd.Parameters.AddWithValue("@alt_text", alt_text_box.Text);
            cmd.Parameters.AddWithValue("@userID", ID);
            cmd.Parameters.AddWithValue("@numLikes", "0");
            cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
            cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);

            cmd.ExecuteNonQuery();
            conn.Close();
        }
        else
        {
            Response.Write("<script>alert('Error: Please log in to upload.')</script>");
        }

    }
    protected void btnGoogleLogin_Click(object sender, CommandEventArgs e)
    {
        string discoveryUri = e.CommandArgument.ToString();
        OpenIdRelyingParty openid = new OpenIdRelyingParty();
        var URIbuilder = new UriBuilder(Request.Url) { Query = "" };
        var req = openid.CreateRequest(discoveryUri, URIbuilder.Uri, URIbuilder.Uri);
        var fetchRequest = new FetchRequest();
        fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
        fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
        fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
        req.AddExtension(fetchRequest);
        req.RedirectToProvider();
    }

    protected void alt_text_box_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Meme_Text_Box_TextChanged(object sender, EventArgs e)
    {

    }
}