using API.Controllers;
using MediatR;
using System.Reflection;

namespace Project.Tests.API.Controllers
{
    public class TestableActivitiesController : ActivitiesController
    {
        public void SetMediator(IMediator mediator)
        {
            typeof(ActivitiesController)
                .BaseType
                .GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(this, mediator);
        }
    }
}