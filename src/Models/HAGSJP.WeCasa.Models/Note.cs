using System.Text.Json.Serialization;


namespace HAGSJP.WeCasa.Models
{
    [Serializable]
   public class Note
   {
       public string Message { get; set; }
       public int? NoteId { get; set; }
       public int GroupId { get; set; }
       public DateTime? DateEntered { get; set; }
       public DateTime? DateModified { get; set; }
       public string LastModifiedUser { get; set; }
       public Boolean? IsDeleted { get; set; }
       public DateTime? DateDeleted { get; set; }
       public string? PhotoFileName { get; set; }
       public string? Color { get; set; }
       public int? X { get; set; }
       public int? Y { get; set; }
       
     [JsonConstructor]
       public Note(){}

        public Note(int noteId,int groupId, DateTime dateEntered, string message, Boolean isDeleted, DateTime dateDeleted, string photoFileName)
       {
            NoteId = noteId;
            GroupId = groupId;
            DateEntered = dateEntered;
            Message = message;
            DateModified = DateModified;
            LastModifiedUser = LastModifiedUser;
            IsDeleted = isDeleted;
            DateDeleted = dateDeleted;
            PhotoFileName = photoFileName;
       }
    }
}
