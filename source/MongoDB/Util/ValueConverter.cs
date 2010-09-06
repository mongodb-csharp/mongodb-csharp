using System;

namespace MongoDB.Util
{
    internal static class ValueConverter
    {
        public static object Convert(object value, Type destinationType)
        {
            if(value == null)
                return null;

            var valueType = value.GetType();

            if(valueType == destinationType)
                return value;

            try
            {
                var code = System.Convert.GetTypeCode(value);

                if(destinationType.IsEnum)
                    if(value is string)
                        return Enum.Parse(destinationType, (string)value);
                    else
                        return Enum.ToObject(destinationType, System.Convert.ToInt64(value));
                
                if(destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return System.Convert.ChangeType(value, Nullable.GetUnderlyingType(destinationType));

                if(valueType == typeof(long) && destinationType == typeof(TimeSpan))
                    return TimeSpan.FromTicks((long)value);

                if(code != TypeCode.Object)
                    return System.Convert.ChangeType(value, destinationType);
                
                if(valueType == typeof(Binary) && destinationType == typeof(byte[]))
                    return (byte[])(Binary)value;

                return value;
            }
            catch(FormatException exception)
            {
                throw new MongoException("Can not convert value from " + valueType + " to " + destinationType, exception);
            }
            catch(ArgumentException exception)
            {
                throw new MongoException("Can not convert value from " + valueType + " to " + destinationType, exception);
            }
        }

        public static Array ConvertArray(object[] elements, Type type)
        {
            var array = Array.CreateInstance(type, elements.Length);

            for(var i = 0; i < elements.Length; i++)
                array.SetValue(Convert(elements[i], type), i);

            return array;
        }

        public static string ConvertBackDictionaryKey(object key)
        {
            if(key == null)
                throw new ArgumentNullException("key");

            if(key is Enum)
                return System.Convert.ToInt64(key).ToString();

            return key.ToString();
        }
    }
}