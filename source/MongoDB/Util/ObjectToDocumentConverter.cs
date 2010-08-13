using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Util
{
    internal static class ObjectToDocumentConverter
    {

        public static Document Convert(object obj)
        {
            if (obj == null)
                return null;

            Document doc = obj as Document;
            if (doc != null)
                return doc;

            doc = new Document();

            foreach (var prop in obj.GetType().GetProperties())
            {
                if (!prop.CanRead)
                    continue;

                object value = prop.GetValue(obj, null);
                if (!TypeHelper.IsNativeToMongo(prop.PropertyType))
                    value = Convert(value);

                doc[prop.Name] = value;
            }

            return doc;
        }


    }
}