using System;
using Application.Core;

namespace Application.Activities
{
    // Represents filtering and pagination parameters for querying activities.
    // Supports flags for showing activities the user is attending or hosting,
    // and allows filtering from a specific start date onward.
    // Inherits pagination properties from PagingParams.

    public class ActivityParams : PagingParams
    {
        public bool IsGoing { get; set; }
        public bool IsHost { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}
