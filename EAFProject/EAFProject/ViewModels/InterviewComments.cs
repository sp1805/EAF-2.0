using System;
using System.ComponentModel;

namespace EAFProject.ViewModels
{
    public class InterviewComments
    {
        public string InterviewerName { get; set; }
        public string InterviewerId { get; set; }
        public string Comments { get; set; }
        [DisplayName("Date Time")]
        public DateTime Datetime { get; set; }
        public string Status { get; set; }
    }
}