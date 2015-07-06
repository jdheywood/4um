using System;
using MongoDB.Bson;

namespace Forum.Domain.Extensions
{
    public static class BsonValueExtension
    {
        public static String TryGetString(this BsonValue value, string key, string defaultValue)
        {
            try
            {
                return value[key].AsString;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public static Int32 TryGetInt32(this BsonValue value, string key, Int32 defaultValue)
        {
            try
            {
                return value[key].AsInt32;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public static DateTime? TryGetNullableDateTime(this BsonValue value, string key, DateTime? defaultValue)
        {
            try
            {
                return value[key].ToUniversalTime();
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public static Boolean TryGetBoolean(this BsonValue value, string key, Boolean defaultValue)
        {
            try
            {
                return value[key].AsBoolean;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }
                
        // alt: Reflection, one method to rule them all

        // alt2: return default BsonValue and convert/cast in consuming code, better! 

    }
}
