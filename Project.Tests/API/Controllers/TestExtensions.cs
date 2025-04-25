    using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Project.Tests.Utils
{
    public static class ControllerTestExtensions
    {
        public static void SetMediator(this ControllerBase controller, IMediator mediator)
        {
            var field = controller.GetType()
                .BaseType
                .GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);

            field?.SetValue(controller, mediator);
        }
    }
}
