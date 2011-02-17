using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models;
using DeepBlue.Models.Entity;


namespace DeepBlue.Controllers.Member {
	public class MemberRepository : IMemberRepository {

		DeepBlueEntities DeepBlueDb = new DeepBlueEntities();

        #region IModelRepository Members

        public List<AddressType> GetAllAddressTypes() {
            return (from addressType in DeepBlueDb.AddressTypes
                    orderby addressType.AddressTypeName
                    select addressType).ToList();
        }


        public List<COUNTRY> GetAllCountries() {
            return (from country in DeepBlueDb.COUNTRies
                   orderby country.CountryName ascending
                    select country).ToList();
        }

        public List<STATE> GetAllStates() {
            return (from state in DeepBlueDb.STATEs
                   orderby state.Name ascending
                    select state).ToList();
        }

        public List<CommunicationType> GetAllCommunicationTypes() {
            return (from communicationType in DeepBlueDb.CommunicationTypes
                   orderby communicationType.CommunicationTypeName
                    select communicationType).ToList();
        }

        public List<MemberEntityType> GetAllMemberEntityTypes() {
            return (from memberEntityType in DeepBlueDb.MemberEntityTypes
                   orderby memberEntityType.MemberEntityTypeName
                    select memberEntityType).ToList();
        }

        public void Add(DeepBlue.Models.Entity.Member member) {
            DeepBlueDb.Members.AddObject(member);
        }

        public void Delete(DeepBlue.Models.Entity.Member member) {
            DeepBlueDb.Members.DeleteObject(member);
        }

        public void Save() {
            DeepBlueDb.SaveChanges();
        }

        #endregion
 
	}
}