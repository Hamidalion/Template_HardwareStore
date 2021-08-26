using Microsoft.Extensions.DependencyInjection;
using Template_HardwareStore.DAL.Repository;
using Template_HardwareStore.DAL.Repository.Interface;

namespace Template_HardwareStore.DAL.DependencyInjection
{
    public class IocDal
    {
        public static void Setup(IServiceCollection services)
        {
            AddRepositories(services);
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IApplicationTypeRepository, ApplicationTypeRepository>(); // сервис активный в течении одного запроса
            services.AddScoped<IInquiryDetailRepository, InquiryDetailRepository>();
            services.AddScoped<IInquiryHeaderRepository, InquiryHeaderRepository>();
        }
    }
}
