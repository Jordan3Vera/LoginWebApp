using System;
using System.Linq;
using WebApiRest.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;

namespace WebApiRest.Model.DTOs
{
    public class AutoMapperConfiguration
    {
        #region Explicación sobre su uso
        /**
         * DTO "Data Transfer Object"
         * Es es objeto que se va a devolver desde nustra API hacia otros servicios. Unicamente debe contener datos, nada de lógica de negocio ya que este 
           objeto debe ser serializable.

           Cuando se crea un DTO referenciado al modelo, cada DTO tendrá la misa estructura (propiedades) que el modelo que representa. Sin embargo, los elementos ICollection 
           se convierten en List y si cada clase utiliza objetos de otro modelo, se reemplaza por su respectivo DTO. 

           Una vez ingresado los datos datos en el DTO. A continuación se crea o hace una clase llamada AutoMapperConfiguration (la cual mapeará los modelos a los DTOs;
           además vamos a ignorar las propedades de navegación a fin de que los JSON generados por los controladores no tengan referencia circulares) en la misma carpeta
           DTOs.

           Luego en la clase Statup.cs hay que incluir la configuración de AutoMapper, la eliminación de referencias circulares por parte de la serealización
           Json y la inyección de dependencias que mensacionamos para el contexto.

           El funcionamineto se lleva bien a cabo a partir de la versión 7
         **/
        #endregion

        public static void Configure()
        {
            #region mapper
            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Customer, CustomerDTO>()
            //       .ForMember(x => x.CustomerOrder, o => o.Ignore())
            //       .ReverseMap();

            //    cfg.CreateMap<CustomerOrder, CustomerOrderDTO>()
            //       .ForMember(x => x.OrderDetail, o => o.Ignore())
            //       .ReverseMap();

            //    cfg.CreateMap<Employee, EmployeeDTO>()
            //       .ReverseMap();

            //    cfg.CreateMap<OrderDetail, OrderDetailDTO>()
            //       .ReverseMap();

            //    cfg.CreateMap<OrderStatus, OrderStatusDTO>()
            //       .ForMember(x => x.CustomerOrder, o => o.Ignore())
            //       .ReverseMap();

            //    cfg.CreateMap<Product, ProductDTO>()
            //       .ForMember(x => x.OrderDetail, o => o.Ignore())
            //       .ReverseMap();
            //});
            #endregion

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDTO>()
                   .ForMember(x => x.CustomerOrder, o => o.Ignore())
                   .ReverseMap();

                cfg.CreateMap<CustomerOrder, CustomerOrderDTO>()
                   .ForMember(x => x.OrderDetail, o => o.Ignore())
                   .ReverseMap();

                cfg.CreateMap<Employee, EmployeeDTO>()
                   .ReverseMap();

                cfg.CreateMap<OrderDetail, OrderDetailDTO>()
                   .ReverseMap();

                cfg.CreateMap<OrderStatus, OrderStatusDTO>()
                   .ForMember(x => x.CustomerOrder, o => o.Ignore())
                   .ReverseMap();

                cfg.CreateMap<Product, ProductDTO>()
                   .ForMember(x => x.OrderDetail, o => o.Ignore())
                   .ReverseMap();
            });
        }
    }
}
