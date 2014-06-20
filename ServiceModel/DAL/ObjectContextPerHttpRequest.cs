using System.Web;

namespace ServiceModel.DAL
{
    public static class ObjectContextPerHttpRequest
    {
        public static SensingControlContext Context
        {
            get
            {
                const string key = "SensingControlContext";
                if (!HttpContext.Current.Items.Contains(key))
                    HttpContext.Current.Items.Add(key, new SensingControlContext());

                return HttpContext.Current.Items[key] as SensingControlContext;
            }
        }

    }
}
