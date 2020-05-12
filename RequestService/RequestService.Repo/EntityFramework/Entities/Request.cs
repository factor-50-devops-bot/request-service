﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RequestService.Repo.EntityFramework.Entities
{
    public class Request
    {
        public Request()
        {
            SupportActivities = new HashSet<SupportActivities>();
        }

        public int Id { get; set; }
        public string PostCode { get; set; }
        public DateTime DateRequested { get; set; }
        public bool IsFulfillable { get; set; }
        public bool CommunicationSent { get; set; }

        public byte? FulfillableStatus { get; set; }

        public string SpecialCommunicationNeeds { get; set; }
        public string OtherDetails { get; set; }
        public bool? ReadPrivacyNotice { get; set; }
        public bool? AcceptedTerms { get; set; }
        public bool? ForRequestor { get; set; }
        public int? PersonIdRequester { get; set; }
        public int? PersonIdRecipient { get; set; }

        public virtual Person PersonIdRecipientNavigation { get; set; }
        public virtual Person PersonIdRequesterNavigation { get; set; }

        public virtual PersonalDetails PersonalDetails { get; set; }
        public virtual ICollection<SupportActivities> SupportActivities { get; set; }
        public virtual ICollection<Job> Job { get; set; }
    }
}
