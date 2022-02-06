using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace PrismWPF.Common.Infrastructure
{
    public static class DataUtils
    {
        public static T ToObject<T>(this DataRow dataRow) where T : class
        {
            Type targetType = typeof(T);

            T targetObj = targetType.CreateObject() as T;
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo targetsProperty = targetType.GetProperty(column.ColumnName);

                if (targetsProperty != null && dataRow[column] != DBNull.Value)
                {
                    try
                    {
                        Type targetsPropertyType = targetsProperty.PropertyType;
                        object originData = dataRow[column];

                        if (targetsPropertyType.IsEnum)
                        {
                            targetsProperty.SetValue(targetObj, Enum.Parse(targetsPropertyType, originData.ToString()), null);
                        }
                        else if (Nullable.GetUnderlyingType(targetsPropertyType) != null)
                        {
                            object parsedData = Convert.ChangeType(originData, Nullable.GetUnderlyingType(targetsPropertyType));

                            targetsProperty.SetValue(targetObj, parsedData, null);
                        }
                        else
                        {
                            object parsedData = Convert.ChangeType(originData, targetsPropertyType);
                            targetsProperty.SetValue(targetObj, parsedData, null);
                        }
                    }
                    catch { }
                }
            }

            return targetObj as T;
        }

        public static DataRow GetDataRowFromObject(this DataTable dt, object obj)
        {
            DataRow dr = dt.NewRow();

            if (obj != null)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    PropertyInfo property = obj.GetType().GetProperty(column.ColumnName);

                    if (property != null)
                    {
                        try
                        {
                            object result = Convert.ChangeType(property.GetValue(obj), column.DataType);
                            dr.SetField(column, result);
                        }
                        catch { }
                    }
                }
            }

            return dr;
        }

        public static void AddDataRowFromObject(this DataTable dt, object obj)
        {
            dt.Rows.Add(dt.GetDataRowFromObject(obj));
        }

        public static List<T> ToObjectList<T>(this DataTable dt) where T : class
        {
            List<T> result = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(dt.Rows[i].ToObject<T>());
            }

            return result;
        }

        public static ObservableCollection<T> ToObjectCollection<T>(this DataTable dt) where T : class
        {
            ObservableCollection<T> result = new ObservableCollection<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(dt.Rows[i].ToObject<T>());
            }

            return result;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> obj)
        {
            return new ObservableCollection<T>(obj);
        }

        public static bool CompareJsonData(object compareA, object compareB)
        {
            string strA = JsonConvert.SerializeObject(compareA);
            string strB = JsonConvert.SerializeObject(compareB);

            int result = strA.CompareTo(strB);

            if(result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
