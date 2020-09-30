using Matrip.Domain.Models.Entities;
using Matrip.Web.Database;
using Matrip.Web.Repositories.Contracts;
using System.Linq;

namespace Matrip.Web.Repositories
{
    public class ma33UserAddressRepository : BaseRepository<ma33UserAddress>, Ima33UserAddressRepository
    {
        public ma33UserAddressRepository(ApplicationDbContext ApplicationDbContext) : base(ApplicationDbContext)
        {
        }

        public ma33UserAddress GetByUserID(int UserID)
        {
            var query = _DbContext.ma33UserAddress.Where(e => e.FK3301iduser == UserID);
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            return null;
        }
        public void AddOrUpdate(int UserID, ma33UserAddress userAddress)
        {
            var query = _DbContext.ma33UserAddress.Where(e => e.FK3301iduser == UserID);
            if (query.Any())
            {
                ma33UserAddress ma33UserAddress = query.FirstOrDefault();
                if (userAddress.ma33Zipcode != ma33UserAddress.ma33Zipcode || userAddress.ma33Complement != ma33UserAddress.ma33Complement
                    || userAddress.ma33StreetNumber != ma33UserAddress.ma33StreetNumber 
                    || userAddress.ma33CPF != ma33UserAddress.ma33CPF
                    || userAddress.ma33documentNumber != ma33UserAddress.ma33documentNumber 
                    || userAddress.ma33DocumentUF != ma33UserAddress.ma33DocumentUF 
                    || userAddress.ma33DocumentIssuingBody != ma33UserAddress.ma33DocumentIssuingBody)
                {
                    ma33UserAddress.ma33City = userAddress.ma33City;
                    ma33UserAddress.ma33Complement = userAddress.ma33Complement;
                    ma33UserAddress.ma33Country = userAddress.ma33Country;
                    ma33UserAddress.ma33Neighborhood = userAddress.ma33Neighborhood;
                    ma33UserAddress.ma33State = userAddress.ma33State;
                    ma33UserAddress.ma33Street = userAddress.ma33Street;
                    ma33UserAddress.ma33StreetNumber = userAddress.ma33StreetNumber;
                    ma33UserAddress.ma33Zipcode = userAddress.ma33Zipcode;
                    ma33UserAddress.ma33CPF = userAddress.ma33CPF;
                    ma33UserAddress.ma33documentNumber = userAddress.ma33documentNumber;
                    ma33UserAddress.ma33DocumentIssuingBody = userAddress.ma33DocumentIssuingBody;
                    ma33UserAddress.ma33DocumentUF = userAddress.ma33DocumentUF;
                    _DbContext.ma33UserAddress.Update(ma33UserAddress);
                }
            }
            else
            {
                userAddress.FK3301iduser = UserID;
                _DbContext.ma33UserAddress.Add(userAddress);
            }
        }
    }
}
