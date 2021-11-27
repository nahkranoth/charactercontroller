using System;
using System.Collections.Generic;

public class MessageQuestion
{
    public string question;
    public List<(string, Action)> answers = new List<(string, Action)>();
}
