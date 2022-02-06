using System;
using System.Linq;
using System.Reflection;

namespace PrismWPF.Common.Infrastructure
{
    public static class TypeUtils
    {
        public static object CreateObject(this Type type, object[] parameters = null)
        {
            object result = null;

            try
            {
                if (parameters == null)
                {
                    ConstructorInfo ctor = type.GetConstructor(new Type[0]);
                    if (ctor != null)
                    {
                        result = ctor.Invoke(new object[0]);
                    }
                    else
                    {
                        ConstructorInfo[] ctors = type.GetConstructors();
                        ParameterInfo[] parameterInfos = ctors[0].GetParameters();

                        object[] newParameters = new object[parameterInfos.Count()];
                        for (int i = 0; i < parameterInfos.Count(); i++)
                        {
                            if (parameterInfos[i].ParameterType.IsClass
                                || parameterInfos[i].ParameterType == typeof(string))
                            {
                                newParameters[i] = null;
                            }
                            else if (parameterInfos[i].ParameterType == typeof(short)
                                || parameterInfos[i].ParameterType == typeof(int)
                                || parameterInfos[i].ParameterType == typeof(long)
                                || parameterInfos[i].ParameterType == typeof(ushort)
                                || parameterInfos[i].ParameterType == typeof(uint)
                                || parameterInfos[i].ParameterType == typeof(ulong)
                                || parameterInfos[i].ParameterType == typeof(float)
                                || parameterInfos[i].ParameterType == typeof(decimal)
                                || parameterInfos[i].ParameterType == typeof(float)
                                || parameterInfos[i].ParameterType == typeof(double)
                                || parameterInfos[i].ParameterType == typeof(byte)
                                || parameterInfos[i].ParameterType == typeof(sbyte)
                                || parameterInfos[i].ParameterType == typeof(char)
                                || parameterInfos[i].ParameterType == typeof(DateTime)
                                || parameterInfos[i].ParameterType == typeof(DateTimeOffset)
                                || parameterInfos[i].ParameterType == typeof(TimeSpan)
                                || parameterInfos[i].ParameterType.IsEnum)
                            {
                                newParameters[i] = 0;
                            }
                            else if (parameterInfos[i].ParameterType == typeof(bool))
                            {
                                newParameters[i] = false;
                            }
                            else
                            {
                                newParameters[i] = null;
                            }
                        }

                        result = ctors[0].Invoke(newParameters);
                    }
                }
                else
                {
                    Type[] types = new Type[parameters.Count()];

                    for (int i = 0; i < parameters.Count(); i++)
                    {
                        types[i] = parameters[i] == null ? typeof(object) : parameters[i].GetType();
                    }

                    ConstructorInfo ctor = type.GetConstructor(types);

                    result = ctor.Invoke(parameters);
                }
            }
            catch { }

            return result;
        }

        public static bool IsCastableTo(this Type from, Type to)
        {
            if (to.IsAssignableFrom(from))
            {
                return true;
            }
            System.Collections.Generic.IEnumerable<MethodInfo> methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.ReturnType == to
                    && (m.Name == "op_Implicit" || m.Name == "op_Explicit"));
            return methods.Any();
        }
    }
}
