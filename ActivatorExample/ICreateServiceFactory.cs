namespace ActivatorExample
{
    public interface ICreateServiceFactory
    {
        T CreateService<T>(params object[] constructorInput) where T : class;
    }
}