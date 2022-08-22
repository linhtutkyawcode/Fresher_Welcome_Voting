using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Fresher_Welcome_Voting.Controllers;

[ApiController]
[Route("[controller]")]
public class VotingController : ControllerBase
{
    [HttpPost]
    public Object Post(Voting voting)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        try
        {
            sqlCommand.CommandText = "select cno from Voting where cno=@cno";
            sqlCommand.Parameters.AddWithValue("@cno", voting.Cno);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.Read())
            {
                sqlDataReader.Close();
                return new Feedback(Response, 404, "Not Found", "Contestant not found!");
            }
            sqlDataReader.Close();


            sqlCommand.CommandText = $"select {voting.Field} from Voter where rno=@rno";
            sqlCommand.Parameters.AddWithValue("@rno", voting.Voter?.Rno);
            sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.Read())
            {
                sqlDataReader.Close();
                return new Feedback(Response, 404, "Not Found", "Voter not found!");
            }
            if ((int)sqlDataReader[voting.Field] == 0){
                sqlDataReader.Close();
                return new Feedback(Response, 400, "Bad Request", "Vote already used!");
            }
            sqlDataReader.Close();

            sqlCommand.CommandText = $"update Voter set {voting.Field} = 0 where rno = @rno and psw = @psw";
            sqlCommand.Parameters.AddWithValue("@psw", voting.Voter?.Psw);
            if (sqlCommand.ExecuteNonQuery() <= 0)
            {
                return new Feedback(Response, 404, "Not Found", "Invalid voter credentials!");
            }

            sqlCommand.CommandText = $"update Voting set {voting.Field} = {voting.Field}+1 where cno = @cno;";
            sqlCommand.ExecuteNonQuery();

            return new Feedback(Response, 200, "OK", "Vote Successful!");
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
    public Object Get([FromQuery] string? cno, [FromQuery] string? gender)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();

        try
        {
            string fields = "";
            if (cno?.Length > 0)
            {
                if (gender == "Male")
                {
                    fields = "King, Popular, Smart";
                }
                else if (gender == "Female")
                {
                    fields = "Queen, Attraction, Innocent";
                }
                else
                {
                    fields = "BestCouple";
                }
                sqlCommand.CommandText = $"select {fields} from Voting where cno=@cno";
                sqlCommand.Parameters.AddWithValue("@cno", cno);
            }
            else
            {
                sqlCommand.CommandText = $"select * from Voting";
            }
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            var result = new List<Object>();
            while (sqlDataReader.Read())
            {
                switch (fields)
                {
                    case "King, Popular, Smart":
                        result.Add(new
                        {
                            cno,
                            king = sqlDataReader["king"],
                            popular = sqlDataReader["popular"],
                            smart = sqlDataReader["smart"],
                        }); break;
                    case "Queen, Attraction, Innocent":
                        result.Add(new
                        {
                            cno,
                            queen = sqlDataReader["queen"],
                            attraction = sqlDataReader["attraction"],
                            innocent = sqlDataReader["innocent"],
                        }); break;
                    case "BestCouple":
                        result.Add(new
                        {
                            cno,
                            bestcouple = sqlDataReader["bestcouple"],
                        }); break;

                    default:
                        result.Add(new
                        {
                            cno = sqlDataReader["cno"],
                            king = sqlDataReader["king"],
                            queen = sqlDataReader["queen"],
                            popular = sqlDataReader["popular"],
                            attraction = sqlDataReader["attraction"],
                            innocent = sqlDataReader["innocent"],
                            smart = sqlDataReader["smart"],
                            bestcouple = sqlDataReader["bestcouple"],
                        }); break;
                }
            }
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

    [HttpPut]
    public Object Put(Voting voting)
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Workspace\ASP.NET\Fresher_Welcome_Voting\FresherWelcome.mdf;Integrated Security=True;Connect Timeout=30");
        sqlConnection.Open();
        SqlCommand sqlCommand = sqlConnection.CreateCommand();
        try
        {
            sqlCommand.CommandText = "select cno from Voting where cno=@cno";
            sqlCommand.Parameters.AddWithValue("@cno", voting.Cno);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.Read())
            {
                sqlDataReader.Close();
                return new Feedback(Response, 404, "Not Found", "Contestant not found!");
            }
            sqlDataReader.Close();


            sqlCommand.CommandText = $"select {voting.Field} from Voter where rno=@rno";
            sqlCommand.Parameters.AddWithValue("@rno", voting.Voter?.Rno);
            sqlDataReader = sqlCommand.ExecuteReader();
            if (!sqlDataReader.Read())
            {
                sqlDataReader.Close();
                return new Feedback(Response, 404, "Not Found", "Voter not found!");
            }
            if ((int)sqlDataReader[voting.Field] != 0)
            {
                sqlDataReader.Close();
                return new Feedback(Response, 400, "Bad Request", "Vote not used!");
            }
            sqlDataReader.Close();

            sqlCommand.CommandText = $"update Voter set {voting.Field} = 1 where rno = @rno and psw = @psw";
            sqlCommand.Parameters.AddWithValue("@psw", voting.Voter?.Psw);
            if (sqlCommand.ExecuteNonQuery() <= 0)
            {
                return new Feedback(Response, 404, "Not Found", "Invalid voter credentials!");
            }

            sqlCommand.CommandText = $"update Voting set {voting.Field} = {voting.Field}-1 where cno = @cno;";
            sqlCommand.ExecuteNonQuery();

            return new Feedback(Response, 200, "OK", "Unvote Successful!");
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
