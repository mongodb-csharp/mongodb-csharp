namespace MongoDB.Util
{
    internal static class ObjectToDocumentConverter
    {
        public static Document Convert(object obj)
        {
            if(obj == null)
                return null;

            var doc = obj as Document;

            if(doc != null)
                return doc;

            doc = new Document();

            foreach(var prop in obj.GetType().GetProperties())
            {
                if(!prop.CanRead)
                    continue;

                var value = prop.GetValue(obj, null);
                if(!TypeHelper.IsNativeToMongo(prop.PropertyType))
                    value = Convert(value);

                doc[prop.Name] = value;
            }

            return doc;
        }
    }
}