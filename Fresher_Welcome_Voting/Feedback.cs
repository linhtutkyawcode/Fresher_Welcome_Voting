namespace Fresher_Welcome_Voting;

public class Feedback
{
    public Feedback(HttpResponse res, int statusCode, string statusText, string msg)
    {
        res.StatusCode = statusCode;
        this.statusCode = statusCode;
        this.statusText = statusText;
        this.msg = msg;
    }
    public int statusCode { get; private set; }
    public string statusText { get; private set; }
    public string msg { get; private set; }
}
