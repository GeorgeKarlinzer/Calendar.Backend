using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Calendar.Shared.Infrastructure.Api
{
    internal class InternalControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo AlgorithmPlatformInfo)
        {
            if (!AlgorithmPlatformInfo.IsClass)
            {
                return false;
            }

            if (AlgorithmPlatformInfo.IsAbstract)
            {
                return false;
            }

            if (AlgorithmPlatformInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (AlgorithmPlatformInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!AlgorithmPlatformInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
                !AlgorithmPlatformInfo.IsDefined(typeof(ControllerAttribute)))
            {
                return false;
            }

            return true;
        }
    }
}
