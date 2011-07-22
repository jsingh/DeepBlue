using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Controllers.Investor;
using DeepBlue.Models.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MbUnit.Framework;
using DeepBlue.Controllers.Admin;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Controllers.Investor {
    public class InvestorBase : Base {
        public InvestorController DefaultController { get; set; }

        public Mock<IInvestorRepository> MockRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockRepository = new Mock<IInvestorRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

            // Spin up the controller with the mock http context, and the mock repository
            DefaultController = new InvestorController(MockRepository.Object, MockAdminRepository.Object);
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockAdminRepository.Setup(x => x.GetAllCountries()).Returns(GetMockCountries());
			MockAdminRepository.Setup(x => x.GetAllStates()).Returns(GetMockStates());
			MockAdminRepository.Setup(x => x.GetAllAddressTypes()).Returns(GetAddressTypes());
			MockAdminRepository.Setup(x => x.GetAllInvestorEntityTypes()).Returns(new List<InvestorEntityType>());
			MockAdminRepository.Setup(x => x.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor)).Returns(new List<CustomFieldDetail>());
        }

        private List<DeepBlue.Models.Entity.STATE> GetMockStates() {
            List<STATE> states = new List<STATE>();
            states.Add(new STATE() { StateID = 1, Abbr = "AL", Name = "Alabama" });
            states.Add(new STATE() { StateID = 2, Abbr = "AK", Name = "Alaska" });
            states.Add(new STATE() { StateID = 3, Abbr = "AZ", Name = "Arizona" });
            states.Add(new STATE() { StateID = 4, Abbr = "AR", Name = "Arkansas" });
            states.Add(new STATE() { StateID = 5, Abbr = "CA", Name = "California" });
            states.Add(new STATE() { StateID = 6, Abbr = "CO", Name = "Colorado" });
            states.Add(new STATE() { StateID = 7, Abbr = "CT", Name = "Connecticut" });
            states.Add(new STATE() { StateID = 8, Abbr = "DE", Name = "Delaware" });
            states.Add(new STATE() { StateID = 9, Abbr = "DC", Name = "District of Columbia" });
            states.Add(new STATE() { StateID = 10, Abbr = "FL", Name = "Florida" });
            states.Add(new STATE() { StateID = 11, Abbr = "GA", Name = "Georgia" });
            states.Add(new STATE() { StateID = 12, Abbr = "HI", Name = "Hawaii" });
            states.Add(new STATE() { StateID = 13, Abbr = "ID", Name = "Idaho" });
            states.Add(new STATE() { StateID = 14, Abbr = "IL", Name = "Illinois" });
            states.Add(new STATE() { StateID = 15, Abbr = "IN", Name = "Indiana" });
            states.Add(new STATE() { StateID = 16, Abbr = "IA", Name = "Iowa" });
            states.Add(new STATE() { StateID = 17, Abbr = "KS", Name = "Kansas" });
            states.Add(new STATE() { StateID = 18, Abbr = "KY", Name = "Kentucky" });
            states.Add(new STATE() { StateID = 19, Abbr = "LA", Name = "Louisiana" });
            states.Add(new STATE() { StateID = 20, Abbr = "ME", Name = "Maine" });
            states.Add(new STATE() { StateID = 21, Abbr = "MD", Name = "Maryland" });
            states.Add(new STATE() { StateID = 22, Abbr = "MA", Name = "Massachusetts" });
            states.Add(new STATE() { StateID = 23, Abbr = "MI", Name = "Michigan" });
            states.Add(new STATE() { StateID = 24, Abbr = "MN", Name = "Minnesota" });
            states.Add(new STATE() { StateID = 25, Abbr = "MS", Name = "Mississippi" });
            states.Add(new STATE() { StateID = 26, Abbr = "MO", Name = "Missouri" });
            states.Add(new STATE() { StateID = 27, Abbr = "MT", Name = "Montana" });
            states.Add(new STATE() { StateID = 28, Abbr = "NE", Name = "Nebraska" });
            states.Add(new STATE() { StateID = 29, Abbr = "NV", Name = "Nevada" });
            states.Add(new STATE() { StateID = 30, Abbr = "NH", Name = "New Hampshire" });
            states.Add(new STATE() { StateID = 31, Abbr = "NJ", Name = "New Jersey" });
            states.Add(new STATE() { StateID = 32, Abbr = "NM", Name = "New Mexico" });
            states.Add(new STATE() { StateID = 33, Abbr = "NY", Name = "New York" });
            states.Add(new STATE() { StateID = 34, Abbr = "NC", Name = "North Carolina" });
            states.Add(new STATE() { StateID = 35, Abbr = "ND", Name = "North Dakota" });
            states.Add(new STATE() { StateID = 36, Abbr = "OH", Name = "Ohio" });
            states.Add(new STATE() { StateID = 37, Abbr = "OK", Name = "Oklahoma" });
            states.Add(new STATE() { StateID = 38, Abbr = "OR", Name = "Oregon" });
            states.Add(new STATE() { StateID = 39, Abbr = "PA", Name = "Pennsylvania" });
            states.Add(new STATE() { StateID = 40, Abbr = "RI", Name = "Rhode Island" });
            states.Add(new STATE() { StateID = 41, Abbr = "SC", Name = "South Carolina" });
            states.Add(new STATE() { StateID = 42, Abbr = "SD", Name = "South Dakota" });
            states.Add(new STATE() { StateID = 43, Abbr = "TN", Name = "Tennessee" });
            states.Add(new STATE() { StateID = 44, Abbr = "TX", Name = "Texas" });
            states.Add(new STATE() { StateID = 45, Abbr = "UT", Name = "Utah" });
            states.Add(new STATE() { StateID = 46, Abbr = "VT", Name = "Vermont" });
            states.Add(new STATE() { StateID = 47, Abbr = "VA", Name = "Virginia" });
            states.Add(new STATE() { StateID = 48, Abbr = "WA", Name = "Washington" });
            states.Add(new STATE() { StateID = 49, Abbr = "WV", Name = "West Virginia" });
            states.Add(new STATE() { StateID = 50, Abbr = "WI", Name = "Wisconsin" });
            states.Add(new STATE() { StateID = 51, Abbr = "WY", Name = "Wyoming" });
            return states;
        }

        private List<DeepBlue.Models.Entity.COUNTRY> GetMockCountries() {
            List<COUNTRY> countries = new List<COUNTRY>();
            countries.Add(new COUNTRY() { CountryID = 1, CountryCode = "AL", CountryName = "Albania" });
            countries.Add(new COUNTRY() { CountryID = 2, CountryCode = "DZ", CountryName = "Algeria" });
            countries.Add(new COUNTRY() { CountryID = 3, CountryCode = "AS", CountryName = "American Samoa" });
            countries.Add(new COUNTRY() { CountryID = 4, CountryCode = "AD", CountryName = "Andorra" });
            countries.Add(new COUNTRY() { CountryID = 5, CountryCode = "AO", CountryName = "Angola" });
            countries.Add(new COUNTRY() { CountryID = 6, CountryCode = "AI", CountryName = "Anguilla" });
            countries.Add(new COUNTRY() { CountryID = 7, CountryCode = "AQ", CountryName = "Antarctica" });
            countries.Add(new COUNTRY() { CountryID = 8, CountryCode = "AG", CountryName = "Antigua and Barbuda" });
            countries.Add(new COUNTRY() { CountryID = 9, CountryCode = "AR", CountryName = "Argentina" });
            countries.Add(new COUNTRY() { CountryID = 10, CountryCode = "AM", CountryName = "Armenia" });
            countries.Add(new COUNTRY() { CountryID = 11, CountryCode = "AW", CountryName = "Aruba" });
            countries.Add(new COUNTRY() { CountryID = 12, CountryCode = "AU", CountryName = "Australia" });
            countries.Add(new COUNTRY() { CountryID = 13, CountryCode = "AT", CountryName = "Austria" });
            countries.Add(new COUNTRY() { CountryID = 14, CountryCode = "AZ", CountryName = "Azerbaijan" });
            countries.Add(new COUNTRY() { CountryID = 15, CountryCode = "BS", CountryName = "Bahama" });
            countries.Add(new COUNTRY() { CountryID = 16, CountryCode = "BH", CountryName = "Bahrain" });
            countries.Add(new COUNTRY() { CountryID = 17, CountryCode = "BD", CountryName = "Bangladesh" });
            countries.Add(new COUNTRY() { CountryID = 18, CountryCode = "BB", CountryName = "Barbados" });
            countries.Add(new COUNTRY() { CountryID = 19, CountryCode = "BY", CountryName = "Belarus" });
            countries.Add(new COUNTRY() { CountryID = 20, CountryCode = "BE", CountryName = "Belgium" });
            countries.Add(new COUNTRY() { CountryID = 21, CountryCode = "BZ", CountryName = "Belize" });
            countries.Add(new COUNTRY() { CountryID = 22, CountryCode = "BJ", CountryName = "Benin" });
            countries.Add(new COUNTRY() { CountryID = 23, CountryCode = "BM", CountryName = "Bermuda" });
            countries.Add(new COUNTRY() { CountryID = 24, CountryCode = "BT", CountryName = "Bhutan" });
            countries.Add(new COUNTRY() { CountryID = 25, CountryCode = "BO", CountryName = "Bolivia" });
            countries.Add(new COUNTRY() { CountryID = 26, CountryCode = "BA", CountryName = "Bosnia and Herzegovina" });
            countries.Add(new COUNTRY() { CountryID = 27, CountryCode = "BW", CountryName = "Botswana" });
            countries.Add(new COUNTRY() { CountryID = 28, CountryCode = "BV", CountryName = "Bouvet Island" });
            countries.Add(new COUNTRY() { CountryID = 29, CountryCode = "BR", CountryName = "Brazil" });
            countries.Add(new COUNTRY() { CountryID = 30, CountryCode = "IO", CountryName = "British Indian Ocean Territory" });
            countries.Add(new COUNTRY() { CountryID = 31, CountryCode = "BN", CountryName = "Brunei Darussalam" });
            countries.Add(new COUNTRY() { CountryID = 32, CountryCode = "BG", CountryName = "Bulgaria" });
            countries.Add(new COUNTRY() { CountryID = 33, CountryCode = "BF", CountryName = "Burkina Faso" });
            countries.Add(new COUNTRY() { CountryID = 34, CountryCode = "BI", CountryName = "Burundi" });
            countries.Add(new COUNTRY() { CountryID = 35, CountryCode = "KH", CountryName = "Cambodia" });
            countries.Add(new COUNTRY() { CountryID = 36, CountryCode = "CM", CountryName = "Cameroon" });
            countries.Add(new COUNTRY() { CountryID = 37, CountryCode = "CA", CountryName = "Canada" });
            countries.Add(new COUNTRY() { CountryID = 38, CountryCode = "CV", CountryName = "Cape verde" });
            countries.Add(new COUNTRY() { CountryID = 39, CountryCode = "KY", CountryName = "Cayman Islands" });
            countries.Add(new COUNTRY() { CountryID = 40, CountryCode = "CF", CountryName = "Central African Republic" });
            countries.Add(new COUNTRY() { CountryID = 41, CountryCode = "TD", CountryName = "Chad" });
            countries.Add(new COUNTRY() { CountryID = 42, CountryCode = "CL", CountryName = "Chile" });
            countries.Add(new COUNTRY() { CountryID = 43, CountryCode = "CN", CountryName = "China" });
            countries.Add(new COUNTRY() { CountryID = 44, CountryCode = "CX", CountryName = "Christmas Island" });
            countries.Add(new COUNTRY() { CountryID = 45, CountryCode = "CC", CountryName = "Cocos (Keeling) Islands" });
            countries.Add(new COUNTRY() { CountryID = 46, CountryCode = "CO", CountryName = "Colombia" });
            countries.Add(new COUNTRY() { CountryID = 47, CountryCode = "KM", CountryName = "Comoros" });
            countries.Add(new COUNTRY() { CountryID = 48, CountryCode = "CG", CountryName = "Congo" });
            countries.Add(new COUNTRY() { CountryID = 49, CountryCode = "CD", CountryName = "Congo, The Democratic Republic of the" });
            countries.Add(new COUNTRY() { CountryID = 50, CountryCode = "CK", CountryName = "Cook Islands" });
            countries.Add(new COUNTRY() { CountryID = 51, CountryCode = "CR", CountryName = "Costa Rica" });
            countries.Add(new COUNTRY() { CountryID = 52, CountryCode = "CI", CountryName = "Côte D'ivoire" });
            countries.Add(new COUNTRY() { CountryID = 53, CountryCode = "HR", CountryName = "Croatia" });
            countries.Add(new COUNTRY() { CountryID = 54, CountryCode = "CU", CountryName = "Cuba" });
            countries.Add(new COUNTRY() { CountryID = 55, CountryCode = "CY", CountryName = "Cyprus" });
            countries.Add(new COUNTRY() { CountryID = 56, CountryCode = "CZ", CountryName = "Czech Republic" });
            countries.Add(new COUNTRY() { CountryID = 57, CountryCode = "DK", CountryName = "Denmark" });
            countries.Add(new COUNTRY() { CountryID = 58, CountryCode = "DJ", CountryName = "Djibouti" });
            countries.Add(new COUNTRY() { CountryID = 59, CountryCode = "DM", CountryName = "Dominica" });
            countries.Add(new COUNTRY() { CountryID = 60, CountryCode = "DO", CountryName = "Dominican Republic" });
            countries.Add(new COUNTRY() { CountryID = 61, CountryCode = "EC", CountryName = "Ecuador" });
            countries.Add(new COUNTRY() { CountryID = 62, CountryCode = "EG", CountryName = "Egypt" });
            countries.Add(new COUNTRY() { CountryID = 63, CountryCode = "SV", CountryName = "El Salvador" });
            countries.Add(new COUNTRY() { CountryID = 64, CountryCode = "GQ", CountryName = "Equatorial Guinea" });
            countries.Add(new COUNTRY() { CountryID = 65, CountryCode = "ER", CountryName = "Eritrea" });
            countries.Add(new COUNTRY() { CountryID = 66, CountryCode = "EE", CountryName = "Estonia" });
            countries.Add(new COUNTRY() { CountryID = 67, CountryCode = "ET", CountryName = "Ethiopia" });
            countries.Add(new COUNTRY() { CountryID = 68, CountryCode = "FK", CountryName = "Falkland Islands (Malvinas)" });
            countries.Add(new COUNTRY() { CountryID = 69, CountryCode = "FO", CountryName = "Faroe Islands" });
            countries.Add(new COUNTRY() { CountryID = 70, CountryCode = "FJ", CountryName = "Fiji" });
            countries.Add(new COUNTRY() { CountryID = 71, CountryCode = "FI", CountryName = "Finland" });
            countries.Add(new COUNTRY() { CountryID = 72, CountryCode = "FR", CountryName = "France" });
            countries.Add(new COUNTRY() { CountryID = 73, CountryCode = "GF", CountryName = "French Guiana" });
            countries.Add(new COUNTRY() { CountryID = 74, CountryCode = "PF", CountryName = "French Polynesia" });
            countries.Add(new COUNTRY() { CountryID = 75, CountryCode = "TF", CountryName = "French Southern Territories" });
            countries.Add(new COUNTRY() { CountryID = 76, CountryCode = "GA", CountryName = "Gabon" });
            countries.Add(new COUNTRY() { CountryID = 77, CountryCode = "GM", CountryName = "Gambia" });
            countries.Add(new COUNTRY() { CountryID = 78, CountryCode = "GE", CountryName = "Georgia" });
            countries.Add(new COUNTRY() { CountryID = 79, CountryCode = "DE", CountryName = "Germany" });
            countries.Add(new COUNTRY() { CountryID = 80, CountryCode = "GH", CountryName = "Ghana" });
            countries.Add(new COUNTRY() { CountryID = 81, CountryCode = "GI", CountryName = "Gibraltar" });
            countries.Add(new COUNTRY() { CountryID = 82, CountryCode = "GR", CountryName = "Greece" });
            countries.Add(new COUNTRY() { CountryID = 83, CountryCode = "GL", CountryName = "Greenland" });
            countries.Add(new COUNTRY() { CountryID = 84, CountryCode = "GD", CountryName = "Grenada" });
            countries.Add(new COUNTRY() { CountryID = 85, CountryCode = "GP", CountryName = "Guadeloupe" });
            countries.Add(new COUNTRY() { CountryID = 86, CountryCode = "GU", CountryName = "Guam" });
            countries.Add(new COUNTRY() { CountryID = 87, CountryCode = "GT", CountryName = "Guatemala" });
            countries.Add(new COUNTRY() { CountryID = 88, CountryCode = "GN", CountryName = "Guinea" });
            countries.Add(new COUNTRY() { CountryID = 89, CountryCode = "GW", CountryName = "Guinea-Bissau" });
            countries.Add(new COUNTRY() { CountryID = 90, CountryCode = "GY", CountryName = "Guyana" });
            countries.Add(new COUNTRY() { CountryID = 91, CountryCode = "HT", CountryName = "Haiti" });
            countries.Add(new COUNTRY() { CountryID = 92, CountryCode = "HM", CountryName = "Heard Island and Mcdonald Islands" });
            countries.Add(new COUNTRY() { CountryID = 93, CountryCode = "VA", CountryName = "Holy See (Vatican City State)" });
            countries.Add(new COUNTRY() { CountryID = 94, CountryCode = "HN", CountryName = "Honduras" });
            countries.Add(new COUNTRY() { CountryID = 95, CountryCode = "HK", CountryName = "Hong Kong" });
            countries.Add(new COUNTRY() { CountryID = 96, CountryCode = "HU", CountryName = "Hungary" });
            countries.Add(new COUNTRY() { CountryID = 97, CountryCode = "IS", CountryName = "Iceland" });
            countries.Add(new COUNTRY() { CountryID = 98, CountryCode = "IN", CountryName = "India" });
            countries.Add(new COUNTRY() { CountryID = 99, CountryCode = "ID", CountryName = "Indonesia" });
            countries.Add(new COUNTRY() { CountryID = 100, CountryCode = "IR", CountryName = "Iran, Islamic Republic of" });
            countries.Add(new COUNTRY() { CountryID = 101, CountryCode = "IQ", CountryName = "Iraq" });
            countries.Add(new COUNTRY() { CountryID = 102, CountryCode = "IE", CountryName = "Ireland" });
            countries.Add(new COUNTRY() { CountryID = 103, CountryCode = "IL", CountryName = "Israel" });
            countries.Add(new COUNTRY() { CountryID = 104, CountryCode = "IT", CountryName = "Italy" });
            countries.Add(new COUNTRY() { CountryID = 105, CountryCode = "JM", CountryName = "Jamaica" });
            countries.Add(new COUNTRY() { CountryID = 106, CountryCode = "JP", CountryName = "Japan" });
            countries.Add(new COUNTRY() { CountryID = 107, CountryCode = "JO", CountryName = "Jordan" });
            countries.Add(new COUNTRY() { CountryID = 108, CountryCode = "KZ", CountryName = "Kazakhstan" });
            countries.Add(new COUNTRY() { CountryID = 109, CountryCode = "KE", CountryName = "Kenya" });
            countries.Add(new COUNTRY() { CountryID = 110, CountryCode = "KI", CountryName = "Kiribati" });
            countries.Add(new COUNTRY() { CountryID = 111, CountryCode = "KP", CountryName = "Korea, Democratic People's Republic of" });
            countries.Add(new COUNTRY() { CountryID = 112, CountryCode = "KR", CountryName = "Korea, Republic of" });
            countries.Add(new COUNTRY() { CountryID = 113, CountryCode = "KW", CountryName = "Kuwait" });
            countries.Add(new COUNTRY() { CountryID = 114, CountryCode = "KG", CountryName = "Kyrgyzstan" });
            countries.Add(new COUNTRY() { CountryID = 115, CountryCode = "LA", CountryName = "Lao People's Democratic Republic" });
            countries.Add(new COUNTRY() { CountryID = 116, CountryCode = "LV", CountryName = "Latvia" });
            countries.Add(new COUNTRY() { CountryID = 117, CountryCode = "LB", CountryName = "Lebanon" });
            countries.Add(new COUNTRY() { CountryID = 118, CountryCode = "LS", CountryName = "Lesotho" });
            countries.Add(new COUNTRY() { CountryID = 119, CountryCode = "LR", CountryName = "Liberia" });
            countries.Add(new COUNTRY() { CountryID = 120, CountryCode = "LY", CountryName = "Libyan Arab Jamahiriya" });
            countries.Add(new COUNTRY() { CountryID = 121, CountryCode = "LI", CountryName = "Liechtenstein" });
            countries.Add(new COUNTRY() { CountryID = 122, CountryCode = "LT", CountryName = "Lithuania" });
            countries.Add(new COUNTRY() { CountryID = 123, CountryCode = "LU", CountryName = "Luxembourg" });
            countries.Add(new COUNTRY() { CountryID = 124, CountryCode = "MO", CountryName = "Macao" });
            countries.Add(new COUNTRY() { CountryID = 125, CountryCode = "MK", CountryName = "Macedonia, The Former Yugoslav Republic of" });
            countries.Add(new COUNTRY() { CountryID = 126, CountryCode = "MG", CountryName = "Madagascar" });
            countries.Add(new COUNTRY() { CountryID = 127, CountryCode = "MW", CountryName = "Malawi" });
            countries.Add(new COUNTRY() { CountryID = 128, CountryCode = "MY", CountryName = "Malaysia" });
            countries.Add(new COUNTRY() { CountryID = 129, CountryCode = "MV", CountryName = "Maldives" });
            countries.Add(new COUNTRY() { CountryID = 130, CountryCode = "ML", CountryName = "Mali" });
            countries.Add(new COUNTRY() { CountryID = 131, CountryCode = "MT", CountryName = "Malta" });
            countries.Add(new COUNTRY() { CountryID = 132, CountryCode = "MH", CountryName = "Marshall islands" });
            countries.Add(new COUNTRY() { CountryID = 133, CountryCode = "MQ", CountryName = "Martinique" });
            countries.Add(new COUNTRY() { CountryID = 134, CountryCode = "MR", CountryName = "Mauritania" });
            countries.Add(new COUNTRY() { CountryID = 135, CountryCode = "MU", CountryName = "Mauritius" });
            countries.Add(new COUNTRY() { CountryID = 136, CountryCode = "YT", CountryName = "Mayotte" });
            countries.Add(new COUNTRY() { CountryID = 137, CountryCode = "MX", CountryName = "Mexico" });
            countries.Add(new COUNTRY() { CountryID = 138, CountryCode = "FM", CountryName = "Micronesia, Federated States of" });
            countries.Add(new COUNTRY() { CountryID = 139, CountryCode = "MD", CountryName = "Moldova, Republic of" });
            countries.Add(new COUNTRY() { CountryID = 140, CountryCode = "MC", CountryName = "Monaco" });
            countries.Add(new COUNTRY() { CountryID = 141, CountryCode = "MN", CountryName = "Mongolia" });
            countries.Add(new COUNTRY() { CountryID = 142, CountryCode = "MS", CountryName = "Montserrat" });
            countries.Add(new COUNTRY() { CountryID = 143, CountryCode = "MA", CountryName = "Morocco" });
            countries.Add(new COUNTRY() { CountryID = 144, CountryCode = "MZ", CountryName = "Mozambique" });
            countries.Add(new COUNTRY() { CountryID = 145, CountryCode = "MM", CountryName = "Myanmar" });
            countries.Add(new COUNTRY() { CountryID = 146, CountryCode = "NA", CountryName = "Namibia" });
            countries.Add(new COUNTRY() { CountryID = 147, CountryCode = "NR", CountryName = "Nauru" });
            countries.Add(new COUNTRY() { CountryID = 148, CountryCode = "NP", CountryName = "Nepal" });
            countries.Add(new COUNTRY() { CountryID = 149, CountryCode = "NL", CountryName = "Netherlands" });
            countries.Add(new COUNTRY() { CountryID = 150, CountryCode = "AN", CountryName = "Netherlands Antilles" });
            countries.Add(new COUNTRY() { CountryID = 151, CountryCode = "NC", CountryName = "New Caledonia" });
            countries.Add(new COUNTRY() { CountryID = 152, CountryCode = "NZ", CountryName = "New Zealand" });
            countries.Add(new COUNTRY() { CountryID = 153, CountryCode = "NI", CountryName = "Nicaragua" });
            countries.Add(new COUNTRY() { CountryID = 154, CountryCode = "NE", CountryName = "Niger" });
            countries.Add(new COUNTRY() { CountryID = 155, CountryCode = "NG", CountryName = "Nigeria" });
            countries.Add(new COUNTRY() { CountryID = 156, CountryCode = "NU", CountryName = "Niue" });
            countries.Add(new COUNTRY() { CountryID = 157, CountryName = "NF", CountryCode = "Norfolk Island" });
            countries.Add(new COUNTRY() { CountryID = 158, CountryName = "MP", CountryCode = "Northern Mariana Islands" });
            countries.Add(new COUNTRY() { CountryID = 159, CountryName = "NO", CountryCode = "Norway" });
            countries.Add(new COUNTRY() { CountryID = 160, CountryName = "OM", CountryCode = "Oman" });
            countries.Add(new COUNTRY() { CountryID = 161, CountryName = "PK", CountryCode = "Pakistan" });
            countries.Add(new COUNTRY() { CountryID = 162, CountryCode = "PW", CountryName = "Palau" });
            countries.Add(new COUNTRY() { CountryID = 163, CountryCode = "PS", CountryName = "Palestinian Territory, Occupied" });
            countries.Add(new COUNTRY() { CountryID = 164, CountryCode = "PA", CountryName = "Panama" });
            countries.Add(new COUNTRY() { CountryID = 165, CountryCode = "PG", CountryName = "Papua New Guinea" });
            countries.Add(new COUNTRY() { CountryID = 166, CountryCode = "PY", CountryName = "Paraguay" });
            countries.Add(new COUNTRY() { CountryID = 167, CountryCode = "PE", CountryName = "Peru" });
            countries.Add(new COUNTRY() { CountryID = 168, CountryCode = "PH", CountryName = "Philippines" });
            countries.Add(new COUNTRY() { CountryID = 169, CountryCode = "PN", CountryName = "Pitcairn" });
            countries.Add(new COUNTRY() { CountryID = 170, CountryCode = "PL", CountryName = "Poland" });
            countries.Add(new COUNTRY() { CountryID = 171, CountryCode = "PT", CountryName = "Portugal" });
            countries.Add(new COUNTRY() { CountryID = 172, CountryCode = "PR", CountryName = "Puerto Rico" });
            countries.Add(new COUNTRY() { CountryID = 173, CountryCode = "QA", CountryName = "Qatar" });
            countries.Add(new COUNTRY() { CountryID = 174, CountryCode = "RE", CountryName = "Réunion" });
            countries.Add(new COUNTRY() { CountryID = 175, CountryCode = "RO", CountryName = "Romania" });
            countries.Add(new COUNTRY() { CountryID = 176, CountryCode = "RU", CountryName = "Russian Federation" });
            countries.Add(new COUNTRY() { CountryID = 177, CountryCode = "RW", CountryName = "Rwanda" });
            countries.Add(new COUNTRY() { CountryID = 178, CountryCode = "SH", CountryName = "Saint Helena" });
            countries.Add(new COUNTRY() { CountryID = 179, CountryCode = "KN", CountryName = "Saint Kitts and Nevis" });
            countries.Add(new COUNTRY() { CountryID = 180, CountryCode = "LC", CountryName = "Saint Lucia" });
            countries.Add(new COUNTRY() { CountryID = 181, CountryCode = "PM", CountryName = "Saint Pierre and Miquelon" });
            countries.Add(new COUNTRY() { CountryID = 182, CountryCode = "VC", CountryName = "Saint Vincent and the Grenadines" });
            countries.Add(new COUNTRY() { CountryID = 183, CountryCode = "WS", CountryName = "Samoa" });
            countries.Add(new COUNTRY() { CountryID = 184, CountryCode = "SM", CountryName = "San Marino" });
            countries.Add(new COUNTRY() { CountryID = 185, CountryCode = "ST", CountryName = "Sao Tome and Principe" });
            countries.Add(new COUNTRY() { CountryID = 186, CountryCode = "SA", CountryName = "Saudi Arabia" });
            countries.Add(new COUNTRY() { CountryID = 187, CountryCode = "SN", CountryName = "Senegal" });
            countries.Add(new COUNTRY() { CountryID = 188, CountryCode = "CS", CountryName = "Serbia and Montenegro" });
            countries.Add(new COUNTRY() { CountryID = 189, CountryCode = "SC", CountryName = "Seychelles" });
            countries.Add(new COUNTRY() { CountryID = 190, CountryCode = "SL", CountryName = "Sierra Leone" });
            countries.Add(new COUNTRY() { CountryID = 191, CountryCode = "SG", CountryName = "Singapore" });
            countries.Add(new COUNTRY() { CountryID = 192, CountryCode = "SK", CountryName = "Slovakia" });
            countries.Add(new COUNTRY() { CountryID = 193, CountryCode = "SI", CountryName = "Slovenia" });
            countries.Add(new COUNTRY() { CountryID = 194, CountryCode = "SB", CountryName = "Solomon Islands" });
            countries.Add(new COUNTRY() { CountryID = 195, CountryCode = "SO", CountryName = "Somalia" });
            countries.Add(new COUNTRY() { CountryID = 196, CountryCode = "ZA", CountryName = "South Africa" });
            countries.Add(new COUNTRY() { CountryID = 197, CountryCode = "GS", CountryName = "South Georgia and the South Sandwich Islands" });
            countries.Add(new COUNTRY() { CountryID = 198, CountryCode = "ES", CountryName = "Spain" });
            countries.Add(new COUNTRY() { CountryID = 199, CountryCode = "LK", CountryName = "Sri Lanka" });
            countries.Add(new COUNTRY() { CountryID = 200, CountryCode = "SD", CountryName = "Sudan" });
            countries.Add(new COUNTRY() { CountryID = 201, CountryCode = "SR", CountryName = "Suriname" });
            countries.Add(new COUNTRY() { CountryID = 202, CountryCode = "SJ", CountryName = "Svalbard and Jan Mayen" });
            countries.Add(new COUNTRY() { CountryID = 203, CountryCode = "SZ", CountryName = "Swaziland" });
            countries.Add(new COUNTRY() { CountryID = 204, CountryCode = "SE", CountryName = "Sweden" });
            countries.Add(new COUNTRY() { CountryID = 205, CountryCode = "CH", CountryName = "Switzerland" });
            countries.Add(new COUNTRY() { CountryID = 206, CountryCode = "SY", CountryName = "Syrian Arab Republic" });
            countries.Add(new COUNTRY() { CountryID = 207, CountryCode = "TW", CountryName = "Taiwan, Province of China" });
            countries.Add(new COUNTRY() { CountryID = 208, CountryCode = "TJ", CountryName = "Tajikistan" });
            countries.Add(new COUNTRY() { CountryID = 209, CountryCode = "TZ", CountryName = "Tanzania, United Republic of" });
            countries.Add(new COUNTRY() { CountryID = 210, CountryCode = "TH", CountryName = "Thailand" });
            countries.Add(new COUNTRY() { CountryID = 211, CountryCode = "TL", CountryName = "Timor-Leste" });
            countries.Add(new COUNTRY() { CountryID = 212, CountryCode = "TG", CountryName = "Togo" });
            countries.Add(new COUNTRY() { CountryID = 213, CountryCode = "TK", CountryName = "Tokelau" });
            countries.Add(new COUNTRY() { CountryID = 214, CountryCode = "TO", CountryName = "Tonga" });
            countries.Add(new COUNTRY() { CountryID = 215, CountryCode = "TT", CountryName = "Trinidad and Tobago" });
            countries.Add(new COUNTRY() { CountryID = 216, CountryCode = "TN", CountryName = "Tunisia" });
            countries.Add(new COUNTRY() { CountryID = 217, CountryCode = "TR", CountryName = "Turkey" });
            countries.Add(new COUNTRY() { CountryID = 218, CountryCode = "TM", CountryName = "Turkmenistan" });
            countries.Add(new COUNTRY() { CountryID = 219, CountryCode = "TC", CountryName = "Turks and Caicos Islands" });
            countries.Add(new COUNTRY() { CountryID = 220, CountryCode = "TV", CountryName = "Tuvalu" });
            countries.Add(new COUNTRY() { CountryID = 221, CountryCode = "UG", CountryName = "Uganda" });
            countries.Add(new COUNTRY() { CountryID = 222, CountryCode = "UA", CountryName = "Ukraine" });
            countries.Add(new COUNTRY() { CountryID = 223, CountryCode = "AE", CountryName = "United Arab Emirates" });
            countries.Add(new COUNTRY() { CountryID = 224, CountryCode = "GB", CountryName = "United Kingdom" });
            countries.Add(new COUNTRY() { CountryID = 225, CountryCode = "US", CountryName = "United States" });
            countries.Add(new COUNTRY() { CountryID = 226, CountryCode = "UM", CountryName = "United States Minor Outlying Islands" });
            countries.Add(new COUNTRY() { CountryID = 227, CountryCode = "UY", CountryName = "Uruguay" });
            countries.Add(new COUNTRY() { CountryID = 228, CountryCode = "UZ", CountryName = "Uzbekistan" });
            countries.Add(new COUNTRY() { CountryID = 229, CountryCode = "VU", CountryName = "Vanuatu" });
            countries.Add(new COUNTRY() { CountryID = 230, CountryCode = "VE", CountryName = "Venezuela" });
            countries.Add(new COUNTRY() { CountryID = 231, CountryCode = "VN", CountryName = "Vietnam" });
            countries.Add(new COUNTRY() { CountryID = 232, CountryCode = "VG", CountryName = "Virgin Islands, British" });
            countries.Add(new COUNTRY() { CountryID = 233, CountryCode = "VI", CountryName = "Virgin Islands, U.S." });
            countries.Add(new COUNTRY() { CountryID = 234, CountryCode = "WF", CountryName = "Wallis and Futuna" });
            countries.Add(new COUNTRY() { CountryID = 235, CountryCode = "EH", CountryName = "Western Sahara" });
            countries.Add(new COUNTRY() { CountryID = 236, CountryCode = "YE", CountryName = "Yemen" });
            countries.Add(new COUNTRY() { CountryID = 237, CountryCode = "ZM", CountryName = "Zambia" });
            countries.Add(new COUNTRY() { CountryID = 238, CountryCode = "ZW", CountryName = "Zimbabwe" });
            return countries;
        }

        private List<AddressType> GetAddressTypes() {
            List<AddressType> addressTypes = new List<AddressType>();
            Array values = Enum.GetValues(typeof(DeepBlue.Models.Admin.Enums.AddressType));
            
            foreach (var i in values) {
                int val = (int)i;
                addressTypes.Add(new AddressType {
					AddressTypeName = Enum.GetName(typeof(DeepBlue.Models.Admin.Enums.AddressType), val),
                    AddressTypeID = val
                });
            }

            return addressTypes;
        }

        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

		#region FindInvestors
		[Test]
		public void valid_Find_Investors_sets_json_result_error() {
			Assert.IsTrue((DefaultController.FindInvestors("test")!=null));
		}
		#endregion
    }
}
