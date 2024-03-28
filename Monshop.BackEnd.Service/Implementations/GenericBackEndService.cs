namespace Monshop.BackEnd.Service.Implementations;

public class GenericBackendService
{
    private readonly IServiceProvider _serviceProvider;

    public GenericBackendService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T Resolve<T>()
    {
        return (T)_serviceProvider.GetService(typeof(T));
    }
}