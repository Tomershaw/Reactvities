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
        // Indicates whether to filter activities the user is attending.
        public bool IsGoing { get; set; }

        // Indicates whether to filter activities the user is hosting.
        public bool IsHost { get; set; }

        // Specifies the start date for filtering activities.
        // Defaults to the current UTC date and time.
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}
