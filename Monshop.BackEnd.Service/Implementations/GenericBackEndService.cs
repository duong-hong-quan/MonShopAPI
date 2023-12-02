using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Implementations
{
    public class GenericBackendService
    {
        private IServiceProvider _serviceProvider;

        public GenericBackendService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Resolve<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }
    }
}
