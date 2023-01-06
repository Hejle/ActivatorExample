using System;
using System.Linq;
using System.Reflection;

namespace ActivatorExample
{
    public class CreateServiceFactory : ICreateServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateService<T>(params object[] constructorInput) where T : class
        {

            var constructors = typeof(T)
                .GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length)
                .ToArray();
            ConstructorInfo? matchedConstructor = null;
            object[]? matchedConstructorParameters = null;

            for (int i = 0; i < constructors.Length; i++)
            {
                matchedConstructor = constructors[i];
                matchedConstructorParameters = GetMatchingConstructor(matchedConstructor, constructorInput);
                if (matchedConstructorParameters == null)
                {
                    continue;
                }
                break;
            }
            if(matchedConstructorParameters is null || matchedConstructor is null)
            {
                throw new InvalidOperationException($"Could not create service {typeof(T).FullName}");
            }
            return (T)matchedConstructor.Invoke(matchedConstructorParameters);
        }

        private object[]? GetMatchingConstructor(ConstructorInfo item, object[] constructorInput)
        {
            var parameters = item.GetParameters();
            var serviceProviderCount = 0;

            if(constructorInput.Length > parameters.Length)
            {
                return null;
            }

            var resultArray = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                var constructInput = constructorInput.ElementAtOrDefault(i-serviceProviderCount);
                if (param.ParameterType == constructInput?.GetType())
                {
                    resultArray[i] = constructInput;
                    continue;
                }
                var service = _serviceProvider.GetService(param.ParameterType);
                if(service is not null)
                {
                    serviceProviderCount++;
                    resultArray[i] = service;
                    continue;
                }
                return null;
            }
            if (constructorInput.Length + serviceProviderCount != parameters.Length)
            {
                return null;
            }

            return resultArray;
        }
    }
}
