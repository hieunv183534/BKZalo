using System;
using System.Collections.Generic;
using System.Text;

namespace BKZalo.Core.Entities
{
    public class Report : BaseEntity
    {
        public Report()
        {

        }

        public Report(Guid postId, Guid accountId, int subject, string details)
        {
            this.PostId = postId;
            this.AccountId = accountId;
            this.Subject = subject;
            this.Details = details;
        }

        public Guid ReportId { get; set; }

        public Guid PostId { get; set; }

        public Guid AccountId { get; set; }

        public int Subject { get; set; }

        public string Details { get; set; }
    }
}
