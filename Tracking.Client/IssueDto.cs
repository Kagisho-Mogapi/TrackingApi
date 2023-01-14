using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracking.Client
{
    internal class IssueDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public IssueType IssueType { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Completed { get; set; }

    }

    public enum Priority
    {
        Low, Medium, High
    }

    public enum IssueType
    {
        Feature, Bug, Documentation
    }
}
