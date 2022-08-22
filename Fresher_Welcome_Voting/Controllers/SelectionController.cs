using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Fresher_Welcome_Voting.Controllers;

[ApiController]
[Route("[controller]")]
public class SelectionController : ControllerBase
{
    [HttpPost]
    public Object Post(Selection selection)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        try
        {
            sqlCommand.CommandText = "select * from Selection where gender = @gender";
            sqlCommand.Parameters.AddWithValue("@gender", selection.Gender);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int cno = 1;
            while (sqlDataReader.Read())
            {
                cno++;
            }
            sqlDataReader.Close();

            sqlCommand.CommandText = "select * from Selection where rno = @rno";
            sqlCommand.Parameters.AddWithValue("@rno", selection.Rno);
            sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
            {
                sqlDataReader.Close();
                sqlConnection.Close();
                return new Feedback(Response, 424, "Test", "Err");
            }
            sqlDataReader.Close();

            sqlCommand.CommandText = "insert into Selection values (@rno,@cno,@section,@name,@gender,@age,@avatar)";
            sqlCommand.Parameters.AddWithValue("@cno", cno);
            sqlCommand.Parameters.AddWithValue("@section", selection.Section);
            sqlCommand.Parameters.AddWithValue("@name", selection.Name);
            sqlCommand.Parameters.AddWithValue("@age", selection.Age);
            sqlCommand.Parameters.AddWithValue("@avatar", selection.Avatar);
            sqlCommand.ExecuteNonQuery();


            sqlCommand.CommandText = "select * from Voting where cno=@cno";
            sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.Read())
            {
                sqlDataReader.Close();
                sqlCommand.CommandText = "insert into Voting (cno) values (@cno);";
                sqlCommand.ExecuteNonQuery();
            }
            else
            {
                sqlDataReader.Close();
            }
            return new
            {
                rno = selection.Rno,
                cno,
                section = selection.Section,
                name = selection.Name,
                gender = selection.Gender,
                age = selection.Age,
                avatar = selection.Avatar
            };
        }
        catch (Exception e)
        {
            return new Feedback(Response, 500, "Internal Sever Error", e.Message);
        }
        finally
        {
            sqlConnection.Close();
        }

    }

    [HttpGet]
    public Object Get([FromQuery] string? rno, string? cno, string? gender)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        try
        {
            string filter = "";
            if (rno?.Length > 0)
            {
                filter = " where rno = @rno";
                sqlCommand.Parameters.AddWithValue("@rno", rno);
            }
            else if (cno?.Length > 0 && gender?.Length > 0)
            {
                filter = " where cno = @cno and gender = @gender";
                sqlCommand.Parameters.AddWithValue("@cno", cno);
                sqlCommand.Parameters.AddWithValue("@gender", gender);
            }
            else if (cno?.Length > 0 || gender?.Length > 0)
            {
                return new Feedback(Response, 424, "Bad Req", "Provided info is not enough!");
            }
            sqlCommand.CommandText = "select * from Selection" + filter;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            var result = new List<Object>();
            while (sqlDataReader.Read())
            {
                result.Add(new
                {
                    rno = sqlDataReader["rno"],
                    cno = sqlDataReader["cno"],
                    section = sqlDataReader["section"],
                    name = sqlDataReader["name"],
                    gender = sqlDataReader["gender"],
                    age = sqlDataReader["age"],
                    avatar = sqlDataReader["avatar"]
                });
            }
            sqlDataReader.Close();

            if (result.Count() <= 0)
                return new Feedback(Response, 404, "Not Found", "Contestant is not found!");
            return result;
        }
        catch (Exception e)
        {
            return new Feedback(Response, 500, "Internal Sever Error", e.Message);
        }
        finally
        {
            sqlConnection.Close();
        }
    }

    [HttpPut]
    public Object Put(Selection selection)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        try
        {
            sqlCommand.CommandText = "select * from Selection where rno = @rno";
            sqlCommand.Parameters.AddWithValue("@rno", selection.Rno);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.Read())
            {
                sqlDataReader.Close();
                return new Feedback(Response, 404, "Not Found", "Rollno not found!");
            }

            int cno = (int)sqlDataReader["cno"];
            string changes = "";

            if (selection.Section?.Length > 0 && selection.Section != sqlDataReader["section"].ToString())
            {
                if (changes != "") changes += ", ";
                changes += "section=@section";
                sqlCommand.Parameters.AddWithValue("@section", selection.Section);
            }
            else
            {
                selection.Section = (string)sqlDataReader["section"];
            }

            if (selection.Name?.Length > 0 && selection.Name != sqlDataReader["name"].ToString())
            {
                if (changes != "") changes += ", ";
                changes += "name=@name";
                sqlCommand.Parameters.AddWithValue("@name", selection.Name);
            }
            else
            {
                selection.Name = (string)sqlDataReader["name"];
            }

            if (selection.Age > 0 && selection.Age != (int)sqlDataReader["age"])
            {
                if (changes != "") changes += ", ";
                changes += "age=@age";
                sqlCommand.Parameters.AddWithValue("@age", selection.Age);
            }
            else
            {
                selection.Age = (int)sqlDataReader["age"];
            }

            if (selection.Avatar?.Length > 0 && selection.Avatar != sqlDataReader["avatar"].ToString())
            {
                if (changes != "") changes += ", ";
                changes += "avatar=@avatar";
                sqlCommand.Parameters.AddWithValue("@avatar", selection.Avatar);
            }
            else
            {
                selection.Avatar = (string)sqlDataReader["avatar"];
            }

            if (selection.Gender?.Length > 0 && selection.Gender != sqlDataReader["gender"].ToString())
            {
                if (changes != "") changes += ", ";

                sqlDataReader.Close();
                sqlCommand.CommandText = "select * from Selection where gender = @gender";
                sqlCommand.Parameters.AddWithValue("@gender", selection.Gender);
                sqlDataReader = sqlCommand.ExecuteReader();
                cno = 1;
                while (sqlDataReader.Read())
                {
                    cno++;
                }
                sqlCommand.Parameters.AddWithValue("@cno", cno);
                changes += "gender = @gender, cno = @cno";
            }
            else
            {
                selection.Gender = (string)sqlDataReader["gender"];
            }

            sqlDataReader.Close();

            if (changes!="")
            {
                sqlCommand.CommandText = "update Selection set " + changes + " where rno = @rno";
                sqlCommand.ExecuteNonQuery();
            }

            return new
            {
                rno = selection.Rno,
                cno,
                section = selection.Section,
                name = selection.Name,
                gender = selection.Gender,
                age = selection.Age,
                avatar = selection.Avatar
            };
        }
        catch (Exception e)
        {
            return new Feedback(Response, 500, "Internal Sever Error", e.Message);
        }
        finally
        {
            sqlConnection.Close();
        }
    }

    [HttpDelete]
    public Feedback Delete([FromQuery] string rno)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();

        try
        {
            sqlCommand.CommandText = "select cno,gender from Selection where rno = @rno;";
            sqlCommand.Parameters.AddWithValue("@rno", rno);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.Read())
            {
                sqlDataReader.Close();
                return new Feedback(Response, 404, "Not Found", "Rollno not found!");
            }

            sqlCommand.CommandText = "select * from Selection where cno = @cno and gender!=@gender;";
            sqlCommand.Parameters.AddWithValue("@cno", sqlDataReader["cno"]);
            sqlCommand.Parameters.AddWithValue("@gender", sqlDataReader["gender"]);
            sqlDataReader.Close();

            sqlDataReader = sqlCommand.ExecuteReader();
            sqlCommand.CommandText = "delete from Selection where rno = @rno;";
            if (!sqlDataReader.Read())
                sqlCommand.CommandText += "delete from Voting where cno = @cno;";
            sqlDataReader.Close();

            if(sqlCommand.ExecuteNonQuery()<=0)
                return new Feedback(Response, 404, "Not Found", "Contestant not found!");

            return new Feedback(Response, 200, "OK", "Deleted");
        }
        catch (Exception e)
        {
            return new Feedback(Response, 500, "Internal Sever Error", e.Message);
        }
        finally
        {
            sqlConnection.Close();
        }
    }
}
