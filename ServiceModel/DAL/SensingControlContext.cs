using System.Data.Entity;

namespace ServiceModel.DAL
{
    public class SensingControlContext: DbContext
    {
        public SensingControlContext()
            : base("DefaultConnection")
        {
        }
    }
}
