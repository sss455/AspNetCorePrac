namespace CoreEntity.Models;

public class EmailAddress
{
    public EmailAddress(string mail)
    {
        var mails = mail.Split("@", 2);
        Local = mails[0];
        Domain = mails[1];
    }

    public string Local { get; init; }
    public string Domain { get; init; }

    public override string ToString()
    {
        return $"{Local}@{Domain}";
    }
}