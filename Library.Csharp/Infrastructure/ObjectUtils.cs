using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace PrismWPF.Common.Infrastructure
{
    public static class ObjectUtils
    {
        public static T CopyObject<T>(this object origin) where T : class
        {
            if (origin == null)
            {
                return null;
            }

            Type type = typeof(T);

            if (origin is JArray)
            {
                return (origin as JArray).DeepClone() as T;
            }

            object cloned = type.CreateObject();

            if (cloned != null)
            {
                cloned.CopyPropertiesFrom(origin);
            }

            return cloned as T;
        }

        public static T CopyObject<T>(this T origin) where T : class
        {
            if (origin == null)
            {
                return null;
            }

            Type type = origin.GetType();

            if (origin is JArray)
            {
                return (origin as JArray).DeepClone() as T;
            }

            object cloned = type.CreateObject();

            if (cloned != null)
            {
                cloned.CopyPropertiesFrom(origin);
            }

            return cloned as T;
        }

        public static void CopyPropertiesFrom<T>(this T setter, object getter) where T : class
        {
            if (setter == null || getter == null)
            {
                return;
            }

            Type setterType = setter.GetType();
            Type getterType = getter.GetType();

            if (typeof(IList).IsAssignableFrom(setterType) && typeof(IList).IsAssignableFrom(getterType))
            {
                IList setterList = setter as IList;
                IList getterList = getter as IList;

                setterList.Clear();

                if (setterType == typeof(JArray) && getterType == typeof(JArray))
                {
                    setter = (getterList as JArray).DeepClone() as T;
                }
                else
                {
                    Type setterListsItemType = setterType.GetGenericArguments().Single();
                    Type getterListsItemType = getterType.GetGenericArguments().Single();
                    foreach (object item in getterList)
                    {
                        if (item == null)
                        {
                            setterList.Add(null);
                        }
                        else if (getterListsItemType.IsCastableTo(setterListsItemType))
                        {
                            setterList.Add(item.CopyObject());
                        }
                        else if (setterType.GetGenericArguments().Single().IsClass)
                        {
                            object copiedValue = setterType.GetGenericArguments().Single().CreateObject();

                            copiedValue?.CopyPropertiesFrom(item);

                            setterList.Add(copiedValue);
                        }
                        else
                        {
                            setterList.Add(null);
                        }
                    }
                }
            }

            PropertyInfo[] settersProerties = setterType.GetProperties();
            PropertyInfo[] gettersProerties = getterType.GetProperties();

            foreach (PropertyInfo settersProperty in settersProerties)
            {
                try
                {
                    if (settersProperty.CanWrite)
                    {
                        PropertyInfo gettersProperty = getterType.GetProperty(settersProperty.Name);

                        if (gettersProperty != null)
                        {
                            Type settersPropertyType = settersProperty.PropertyType;
                            Type gettersPropertyType = gettersProperty.PropertyType;

                            if (settersPropertyType == typeof(string)
                                || settersPropertyType == typeof(bool)
                                || settersPropertyType == typeof(short)
                                || settersPropertyType == typeof(int)
                                || settersPropertyType == typeof(long)
                                || settersPropertyType == typeof(ushort)
                                || settersPropertyType == typeof(uint)
                                || settersPropertyType == typeof(ulong)
                                || settersPropertyType == typeof(float)
                                || settersPropertyType == typeof(decimal)
                                || settersPropertyType == typeof(float)
                                || settersPropertyType == typeof(double)
                                || settersPropertyType == typeof(byte)
                                || settersPropertyType == typeof(byte[])
                                || settersPropertyType == typeof(sbyte)
                                || settersPropertyType == typeof(char)
                                || settersPropertyType == typeof(DateTime)
                                || settersPropertyType == typeof(DateTimeOffset)
                                || settersPropertyType == typeof(TimeSpan)
                                || settersPropertyType == typeof(ImageSource))
                            {
                                settersProperty.SetValue(setter, Convert.ChangeType(gettersProperty.GetValue(getter), settersPropertyType), null);
                            }
                            else if (settersPropertyType.IsEnum)
                            {
                                settersProperty.SetValue(setter, Enum.Parse(settersPropertyType, gettersProperty.GetValue(getter)?.ToString()), null);
                            }
                            else if (Nullable.GetUnderlyingType(settersPropertyType) != null)
                            {
                                object getterValue = gettersProperty.GetValue(getter);

                                if (getterValue != null)
                                {
                                    getterValue = Convert.ChangeType(getterValue, Nullable.GetUnderlyingType(settersPropertyType));
                                }

                                settersProperty.SetValue(setter, getterValue, null);
                            }
                            else if (gettersPropertyType.IsCastableTo(settersPropertyType))
                            {
                                object getterValue = gettersProperty.GetValue(getter);

                                if (getterValue == null)
                                {
                                    settersProperty.SetValue(setter, null);
                                }
                                else
                                {
                                    settersProperty.SetValue(setter, getterValue.CopyObject());
                                }
                            }
                            else if (settersPropertyType.IsClass)
                            {
                                object getterValue = gettersProperty.GetValue(getter);

                                if (getterValue == null)
                                {
                                    settersProperty.SetValue(setter, null);
                                }
                                else
                                {
                                    object setterValue = settersProperty.GetValue(setter);

                                    if (setterValue == null)
                                    {
                                        setterValue = settersPropertyType.CreateObject();
                                    }

                                    setterValue?.CopyPropertiesFrom(getterValue);

                                    settersProperty.SetValue(setter, setterValue);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static object GetPropertyObject(this object obj, string propertyName)
        {
            object result = null;

            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName);

            if (property != null)
            {
                result = property.GetValue(obj);
            }

            return result;
        }
    }
}
