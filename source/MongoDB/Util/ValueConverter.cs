using System;

namespace MongoDB.Util
{
    internal static class ValueConverter
    {
        public static object Convert(object value, Type destinationType)
        {
            var valueType = value != null ? value.GetType() : typeof(object);

            if(value==null)
                return null;

            if(valueType != destinationType)
                try
                {
                    var code = System.Convert.GetTypeCode(value);

                    if(destinationType.IsEnum)
                        if(value is string)
                            value = Enum.Parse(destinationType, (string)value);
                        else
                            value = Enum.ToObject(destinationType, value);
                    else if(destinationType.IsGenericType &&
                            destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        value = System.Convert.ChangeType(value, Nullable.GetUnderlyingType(destinationType));
                    else if(code != TypeCode.Object)
                        value = System.Convert.ChangeType(value, destinationType);
                    else if(valueType==typeof(Binary)&&destinationType==typeof(byte[]))
                        value = (byte[])(Binary)value;
                }
                catch(FormatException exception)
                {
                    throw new MongoException("Can not convert value from " + valueType + " to " + destinationType, exception);
                }
                catch(ArgumentException exception)
                {
                    throw new MongoException("Can not convert value from " + valueType + " to " + destinationType, exception);
                }

            return value;
        }

        public static Array ConvertArray(object[] elements, Type type)
        {
            var array = Array.CreateInstance(type, elements.Length);

            for(var i = 0; i < elements.Length; i++)
                array.SetValue(Convert(elements[i], type), i);

            return array;
        }

        public static string ConvertKey(object key)
        {
            if(key == null)
                throw new ArgumentNullException("key");

            if(key is Enum)
                return System.Convert.ToInt64(key).ToString();

            return key.ToString();
        }
    }
}