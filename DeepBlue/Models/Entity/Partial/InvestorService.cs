using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
    public interface IInvestorService {
        void SaveInvestor(Investor investor);
    }

    public class InvestorService : IInvestorService {
        public void SaveInvestor(Investor investor) {
            using (DeepBlueEntities context = new DeepBlueEntities()) {
                context.Investors.AddObject(investor);
                context.SaveChanges();
            }
        }
    }
}