using AutoMapper;
using DwellingAPI.DAL.Entities;
using DwellingAPI.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Mappers
{
    public class EntitiesModelsMapper : Profile
    {
        public EntitiesModelsMapper()
        {
            CreateMap<Apartment, ApartmentModel>()
                .ForMember(x=>x.Id, x=>x.MapFrom(a=>a.Id.ToString()))
                .ForPath(x => x.CreationDate, x => x.MapFrom(x => x.Details.CreationDate))
                .ForPath(x => x.LastlyUpdatedDate, x => x.MapFrom(x => x.Details.LastlyUpdatedDate))
                .ForPath(x => x.RealtorName, x => x.MapFrom(x => x.Details.RealtorName))
                .ForPath(x => x.RealtorPhone, x => x.MapFrom(x => x.Details.RealtorPhone))
                .ForPath(x => x.Description, x => x.MapFrom(x => x.Details.Description));

            CreateMap<ApartmentModel, Apartment>()
                .ForMember(x => x.Id, x => x.MapFrom(a => string.IsNullOrEmpty(a.Id) ? Guid.NewGuid() : Guid.Parse(a.Id)))
                .ForPath(x=>x.Details.CreationDate, x=>x.MapFrom(x=>x.CreationDate))
                .ForPath(x => x.Details.LastlyUpdatedDate, x => x.MapFrom(x => x.LastlyUpdatedDate))
                .ForPath(x => x.Details.RealtorName, x => x.MapFrom(x => x.RealtorName))
                .ForPath(x => x.Details.RealtorPhone, x => x.MapFrom(x => x.RealtorPhone))
                .ForPath(x => x.Details.ApartmentId, x => x.MapFrom(a => string.IsNullOrEmpty(a.Id) ? Guid.NewGuid() : Guid.Parse(a.Id)))
                .ForPath(x => x.Details.Description, x => x.MapFrom(x => x.Description));

            CreateMap<Account, AccountModel>()
                .ForMember(x => x.Id, x=>x.MapFrom(m => m.Id))
                .ForMember(x => x.Username, x => x.MapFrom(m => m.UserName))
                .ForMember(x => x.MobilePhone, x => x.MapFrom(m => m.PhoneNumber))
                .ForMember(x => x.Email, x => x.MapFrom(m => m.Email));
            CreateMap<AccountModel, Account>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.UserName, x => x.MapFrom(m => m.Username))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(m => m.MobilePhone))
                .ForMember(x => x.Email, x => x.MapFrom(m => m.Email));

            CreateMap<ApartmentPhoto, ApartmentPhotoModel>()
                .ForMember(x => x.Id, x => x.MapFrom(a => a.Id.ToString()));
            CreateMap<ApartmentPhotoModel, ApartmentPhoto>()
                .ForMember(x => x.Id, x => x.MapFrom(a => string.IsNullOrEmpty(a.Id) ? Guid.NewGuid() : Guid.Parse(a.Id)));

            CreateMap<Call, CallModel>()
                .ForMember(x => x.Id, x => x.MapFrom(a => a.Id.ToString()));
            CreateMap<CallModel, Call>()
                .ForMember(x => x.Id, x => x.MapFrom(a => string.IsNullOrEmpty(a.Id) ? Guid.NewGuid() : Guid.Parse(a.Id)));

            CreateMap<RequestCallModel, Call>()
                .ForMember(x => x.Id, f=>f.MapFrom(x => Guid.NewGuid()))
                .ForMember(x => x.ToName, f => f.MapFrom(x => x.FirstName))
                .ForMember(x => x.ToPhone, f => f.MapFrom(x => x.MobilePhone));

            CreateMap<Contact, ContactModel>()
                .ForMember(x => x.Id, x => x.MapFrom(a => a.Id.ToString()));
            CreateMap<ContactModel, Contact>()
                .ForMember(x => x.Id, x => x.MapFrom(a => string.IsNullOrEmpty(a.Id) ? Guid.NewGuid() : Guid.Parse(a.Id)));

            CreateMap<Order, OrderModel>()
                .ForMember(x => x.Id, x => x.MapFrom(a => a.Id.ToString()));
            CreateMap<OrderModel, Contact>()
                .ForMember(x => x.Id, x => x.MapFrom(a => string.IsNullOrEmpty(a.Id) ? Guid.NewGuid() : Guid.Parse(a.Id)));

            CreateMap<Agreement, AgreementModel>()
                .ForMember(x => x.Id, x => x.MapFrom(a => a.Id.ToString()));
            CreateMap<AgreementModel, Agreement>()
                .ForMember(x => x.Id, x => x.MapFrom(a => string.IsNullOrEmpty(a.Id) ? Guid.NewGuid() : Guid.Parse(a.Id)));

            CreateMap<OrderApartment, ApartmentModel>()
                .ForPath(x => x.Id, x => x.MapFrom(x => x.Apartment.Id.ToString()))
                .ForPath(x => x.Number, x => x.MapFrom(x => x.Apartment.Number))
                .ForPath(x => x.Price, x => x.MapFrom(x => x.Apartment.Price))
                .ForPath(x => x.IsActive, x => x.MapFrom(x => x.Apartment.IsActive));

            CreateMap<ApartmentModel, OrderApartment>()
               .ForPath(x => x.Apartment.Id, x => x.MapFrom(x => Guid.Parse(x.Id)))
               .ForPath(x => x.Apartment.Number, x => x.MapFrom(x => x.Number))
               .ForPath(x => x.Apartment.Price, x => x.MapFrom(x => x.Price))
               .ForPath(x => x.Apartment.IsActive, x => x.MapFrom(x => x.IsActive));

            CreateMap<OrderApartment, OrderModel>()
                .ForPath(x => x.Id, x => x.MapFrom(x => x.Order.Id.ToString()))
                .ForPath(x => x.AccountId, x => x.MapFrom(x => x.Order.AccountId))
                .ForPath(x => x.CreationDate, x => x.MapFrom(x => x.Order.CreationDate))
                .ForPath(x => x.LastlyUpdatedDate, x => x.MapFrom(x => x.Order.LastlyUpdatedDate))
                .ForPath(x => x.EstimatedPriceLimit, x => x.MapFrom(x => x.Order.EstimatedPriceLimit))
                .ForPath(x => x.EstimatedRoomsQuantity, x => x.MapFrom(x => x.Order.EstimatedRoomsQuantity))
                .ForPath(x => x.OrderStatus, x => x.MapFrom(x => x.Order.OrderStatus));

            CreateMap<OrderModel, OrderApartment>()
               .ForPath(x => x.Order.Id, x => x.MapFrom(x => Guid.Parse(x.Id)))
               .ForPath(x => x.Order.AccountId, x => x.MapFrom(x => x.AccountId))
               .ForPath(x => x.Order.CreationDate, x => x.MapFrom(x => x.CreationDate))
               .ForPath(x => x.Order.LastlyUpdatedDate, x => x.MapFrom(x => x.LastlyUpdatedDate))
               .ForPath(x => x.Order.EstimatedPriceLimit, x => x.MapFrom(x => x.EstimatedPriceLimit))
               .ForPath(x => x.Order.EstimatedRoomsQuantity, x => x.MapFrom(x => x.EstimatedRoomsQuantity))
               .ForPath(x => x.Order.OrderStatus, x => x.MapFrom(x => x.OrderStatus));

            CreateMap<SignUpModel, Account>()
                .ForMember(x => x.FullName, x => x.MapFrom(x => x.FullName))
                .ForMember(x => x.Email, x => x.MapFrom(x => x.Email))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(x => x.MobilePhone))
                .ForMember(x => x.UserName, x => x.MapFrom(x => x.Username));

            CreateMap<Order, OrderModel>()
               .ForPath(x => x.Id, x => x.MapFrom(x => x.Id.ToString()))
               .ForPath(x => x.Apartments, x => x.MapFrom(x => new List<Apartment>()));

            CreateMap<OrderModel, Order>()
               .ForMember(x => x.Id, x => x.MapFrom(a => string.IsNullOrEmpty(a.Id) ? Guid.NewGuid() : Guid.Parse(a.Id)))
               .ForPath(x => x.OrderApartments, x => x.MapFrom(x => new List<OrderApartment>()));
        }
    }
}
