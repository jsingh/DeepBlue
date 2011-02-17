using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Member;

namespace DeepBlue.Controllers.Member {
    //public interface IMemberRepository {
    //    IQueryable<AddressType> GetAllAddressTypes();
    //    IQueryable<COUNTRY> GetAllCountries();
    //    IQueryable<STATE> GetAllStates();
    //    IQueryable<MemberEntityType> GetAllMemberEntityTypes();
    //    IQueryable<CommunicationType> GetAllCommunicationTypes();
    //    void Add(DeepBlue.Models.Entity.Member member);
    //    void Delete(DeepBlue.Models.Entity.Member member);
    //    void Save();
    //}

    public interface IMemberRepository {
        List<AddressType> GetAllAddressTypes();
        List<COUNTRY> GetAllCountries();
        List<STATE> GetAllStates();
        List<MemberEntityType> GetAllMemberEntityTypes();
        List<CommunicationType> GetAllCommunicationTypes();
        void Add(DeepBlue.Models.Entity.Member member);
        void Delete(DeepBlue.Models.Entity.Member member);
        void Save();
    }
}
