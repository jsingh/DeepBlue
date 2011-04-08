using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Investor {
    public class InvestorTest : Base {
        public DeepBlue.Models.Entity.Investor DefaultInvestor { get; set; }

        public Mock<IInvestorService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IInvestorService>();

            DefaultInvestor = new DeepBlue.Models.Entity.Investor(MockService.Object);
            MockService.Setup(x => x.SaveInvestor(It.IsAny<DeepBlue.Models.Entity.Investor>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.Investor investor, bool ifValid) {
            RequiredFieldDataMissing(investor, ifValid);
            StringLengthInvalidData(investor, ifValid);
            foreach (InvestorAddress address in investor.InvestorAddresses) {
                RequiredFieldDataMissingForAddress(address.Address, ifValid);
                StringLengthInvalidDataForAddress(address.Address, ifValid);
            }

            foreach (InvestorCommunication comm in investor.InvestorCommunications) {
                RequiredFieldDataMissingForCommunication(comm.Communication, ifValid);
                StringLengthInvalidDataForCommunication(comm.Communication, ifValid);
            }

            foreach (InvestorContact contact in investor.InvestorContacts) {
                RequiredFieldDataMissingForInvestorContact(contact.Contact, ifValid);
                StringLengthInvalidDataForInvestorContact(contact.Contact, ifValid);
                foreach (ContactAddress address in contact.Contact.ContactAddresses) {
                    RequiredFieldDataMissingForAddress(address.Address, ifValid);
                    StringLengthInvalidDataForAddress(address.Address, ifValid);
                }

                foreach (ContactCommunication comm in contact.Contact.ContactCommunications) {
                    RequiredFieldDataMissingForCommunication(comm.Communication, ifValid);
                    StringLengthInvalidDataForCommunication(comm.Communication, ifValid);
                }
            }
        }

        #region Investor
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.Investor investor, bool ifValidData) {
            int delta = 0;
            if (ifValidData) {
                delta = 1;
            }
            if (ifValidData) {
                investor.EntityID = 2;
            } else {
                investor.EntityID = 0;
            }

            investor.InvestorEntityTypeID = investor.CreatedBy = 0 + delta;
            investor.PrevInvestorID = investor.ResidencyState = 0 + delta;
            if (ifValidData) {
                investor.LastName = "";
                investor.CreatedDate = DateTime.Now;
            } else {
                investor.LastName = string.Empty;
                investor.CreatedDate = DateTime.MinValue;
            }
        }

        private void StringLengthInvalidData(DeepBlue.Models.Entity.Investor investor, bool ifValidData) {
            int delta = 0;
            if (!ifValidData) {
                delta = 1;
            }
            investor.InvestorName = GetString(100 + delta);
            investor.Alias = GetString(50 + delta);
            investor.ManagerName = GetString(40 + delta);
            investor.LastName = GetString(30 + delta);
            investor.FirstName = GetString(30 + delta);
            investor.MiddleName = GetString(30 + delta);
            investor.Notes = GetString(500 + delta);
        }
        #endregion

        #region InvestorContact
        private void RequiredFieldDataMissingForInvestorContact(DeepBlue.Models.Entity.Contact contact, bool ifValidData) {
            if (ifValidData) {
                contact.CreatedBy = 1;
                contact.EntityID = 2;
            } else {
                contact.CreatedBy = 0;
                contact.EntityID = 0;
            }

            if (ifValidData) {
                contact.LastName = "";
                contact.CreatedDate = DateTime.Now;
            } else {
                contact.LastName = string.Empty;
                contact.CreatedDate = DateTime.MinValue;
            }
        }

        private void StringLengthInvalidDataForInvestorContact(DeepBlue.Models.Entity.Contact contact, bool ifValidData) {
            int delta = 0;
            if (!ifValidData) {
                delta = 1;
            }
            contact.ContactName = GetString(10 + delta);
            contact.Designation = GetString(20 + delta);
            contact.LastName = GetString(30 + delta);
            contact.FirstName = GetString(30 + delta);
            contact.MiddleName = GetString(30 + delta);
        }
        #endregion

        #region Address
        private void RequiredFieldDataMissingForAddress(DeepBlue.Models.Entity.Address address, bool ifValidData) {
            int delta = 0;
            if (ifValidData) {
                delta = 1;
            }

            if (ifValidData) {
                address.EntityID = 2;
                address.CreatedDate = DateTime.Now;
            } else {
                address.EntityID = 0;
                address.CreatedDate = DateTime.MinValue;
            }
            address.AddressTypeID = address.CreatedBy = 0 + delta;
            address.State = 0 + delta;
            address.Country = 0 + delta;
        }

        private void StringLengthInvalidDataForAddress(DeepBlue.Models.Entity.Address address, bool ifValidData) {
            int delta = 0;
            if (!ifValidData) {
                delta = 1;
            }
            address.Address1 = address.Address2 = address.Address3 = GetString(40 + delta);
            address.City = GetString(30 + delta);
            address.StProvince = GetString(125 + delta);
            address.PostalCode = GetString(10 + delta);
            address.County = GetString(50 + delta);
        }
        #endregion

        #region Communication
        private void RequiredFieldDataMissingForCommunication(DeepBlue.Models.Entity.Communication communication, bool ifValidData) {
            int delta = 0;
            if (ifValidData) {
                delta = 1;
            }

            if (ifValidData) {
                communication.EntityID = 2;
                communication.CreatedDate = DateTime.Now;
            } else {
                communication.EntityID = 0;
                communication.CreatedDate = DateTime.MinValue;
            }

            communication.CommunicationTypeID = 0 + delta;
            communication.CreatedBy = 0 + delta;
        }

        private void StringLengthInvalidDataForCommunication(DeepBlue.Models.Entity.Communication communication, bool ifValidData) {
            int delta = 0;
            if (!ifValidData) {
                delta = 1;
            }
            communication.CommunicationValue = GetString(200 + delta);
            communication.LastFourPhone = GetString(4 + delta);
            communication.CommunicationComment = GetString(200 + delta);
        }
        #endregion
    }
}