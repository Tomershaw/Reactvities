using Domain;
using FluentValidation;

namespace Application.Activities
{
    // FluentValidation validator for the Activity domain model.
    // Ensures that required fields are not empty before processing commands
    // such as Create or Edit.

    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            // Validates that the Title field is not empty.
            RuleFor(x => x.Title).NotEmpty();

            // Validates that the Description field is not empty.
            RuleFor(x => x.Description).NotEmpty();

            // Validates that the Date field is not empty.
            RuleFor(x => x.Date).NotEmpty();

            // Validates that the Category field is not empty.
            RuleFor(x => x.Category).NotEmpty();

            // Validates that the City field is not empty.
            RuleFor(x => x.City).NotEmpty();

            // Validates that the Venue field is not empty.
            RuleFor(x => x.Venue).NotEmpty();
        }
    }
}
