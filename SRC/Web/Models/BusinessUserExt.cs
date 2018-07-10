using HiLand.Framework.BusinessCore;
using HiLand.Framework.FoundationLayer;

namespace Web.Models
{
    public class BusinessUserExt : BusinessUser
    {
        public string Note
        {
            get { return ((IModelExtensible)this).ExtensiableRepository.GetExtentibleProperty("Note"); }
            set { ((IModelExtensible)this).ExtensiableRepository.SetExtentibleProperty("Note", value); }
        }

        public string FlowUpHistory
        {
            get { return ((IModelExtensible)this).ExtensiableRepository.GetExtentibleProperty("FlowUpHistory"); }
            set { ((IModelExtensible)this).ExtensiableRepository.SetExtentibleProperty("FlowUpHistory", value); }
        }
    }
}