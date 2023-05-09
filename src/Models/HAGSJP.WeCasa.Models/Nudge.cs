using System;

namespace HAGSJP.WeCasa.Models
{
    public class Nudge
    {
        public int NudgeId { get; set; }
        public int GroupId { get; set; }
        public int ChoreId { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public string Message { get; set; }
        public DateTime? LastNudgeSent { get; set; }
        public Boolean IsComplete { get; set; }

        public Nudge()
        {
        }
    }
}
