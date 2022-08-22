using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Fresher_Welcome_Voting.Controllers;

[ApiController]
[Route("[controller]")]
public class VoterController : ControllerBase
{
    [HttpPost]
    public Object Post(Voter voter)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        try
        {
            sqlCommand.CommandText = "select * from Voter where rno = @rno";
            sqlCommand.Parameters.AddWithValue("@rno", voter.Rno);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
            {
                sqlDataReader.Close();
                sqlConnection.Close();
                return new Feedback(Response, 424, "Test", "Err");
            }
            sqlDataReader.Close();

            sqlCommand.CommandText = "insert into Voter (rno,psw) values (@rno,@psw)";
            sqlCommand.Parameters.AddWithValue("@psw", voter.Psw);
            sqlCommand.ExecuteNonQuery();

            return new Feedback(Response, 200, "OK", "Voter Added Successfully!");
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
    public Object Get([FromQuery] string? rno)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();

        try
        {
            if (rno?.Length >= 0)
            {
                sqlCommand.CommandText = "select * from voter where rno=@rno;";
                sqlCommand.Parameters.AddWithValue("@rno", rno);
            }
            else
                sqlCommand.CommandText = "select rno from Voter;";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            var result = new List<Object>();
            while (sqlDataReader.Read())
                result.Add(
                    (rno?.Length >= 0) ? new
                    {
                        rno = sqlDataReader["rno"],
                        king = sqlDataReader["king"],
                        queen = sqlDataReader["queen"],
                        popular = sqlDataReader["popular"],
                        attraction = sqlDataReader["attraction"],
                        innocent = sqlDataReader["innocent"],
                        smart = sqlDataReader["smart"],
                        bestcouple = sqlDataReader["bestcouple"],
                    } : sqlDataReader["rno"]
                );
            sqlDataReader.Close();
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

    [HttpDelete]
    public Feedback Delete([FromQuery] string rno)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();

        try
        {
            sqlCommand.CommandText = "delete from Voting where rno = @rno;";
            if (sqlCommand.ExecuteNonQuery() <= 0)
                return new Feedback(Response, 404, "Not Found", "Rollno not found!");
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
