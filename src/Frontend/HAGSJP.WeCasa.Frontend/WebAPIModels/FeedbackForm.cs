﻿using System.Text.Json.Serialization;
[Serializable]
public class FeedbackForm
{
    [JsonConstructor]
    public FeedbackForm() { }

    public DateTime? SubmissionDate { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String Email { get; set; }
    public Boolean FeedbackType { get; set; }
    public String FeedbackMessage { get; set; }
    public float FeedbackRating { get; set; }
    public Boolean ResolvedStatus { get; set; }
    public DateTime? ResolvedDate { get; set; }
}
