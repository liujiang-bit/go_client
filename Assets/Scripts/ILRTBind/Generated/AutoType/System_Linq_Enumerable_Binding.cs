using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class System_Linq_Enumerable_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(System.Linq.Enumerable);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(System.String)};
            if (genericMethods.TryGetValue("ToArray", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.String[]), typeof(System.Collections.Generic.IEnumerable<System.String>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToArray_0);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>)};
            if (genericMethods.TryGetValue("First", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, First_1);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Int32)};
            if (genericMethods.TryGetValue("First", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Int32), typeof(System.Collections.Generic.IEnumerable<System.Int32>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, First_2);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_3);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(UnityEngine.Animation)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<UnityEngine.Animation>), typeof(System.Collections.Generic.IEnumerable<UnityEngine.Animation>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_4);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.String)};
            if (genericMethods.TryGetValue("Contains", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Boolean), typeof(System.Collections.Generic.IEnumerable<System.String>), typeof(System.String)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Contains_5);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>), typeof(System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_6);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Int64)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<System.Int64>), typeof(System.Collections.Generic.IEnumerable<System.Int64>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_7);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(global::ManorItem)};
            if (genericMethods.TryGetValue("Count", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Int32), typeof(System.Collections.Generic.IEnumerable<global::ManorItem>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Count_8);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(global::ManorItem)};
            if (genericMethods.TryGetValue("Except", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.IEnumerable<global::ManorItem>), typeof(System.Collections.Generic.IEnumerable<global::ManorItem>), typeof(System.Collections.Generic.IEnumerable<global::ManorItem>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Except_9);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(global::ManorItem)};
            if (genericMethods.TryGetValue("Any", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Boolean), typeof(System.Collections.Generic.IEnumerable<global::ManorItem>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Any_10);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(UnityEngine.GameObject)};
            if (genericMethods.TryGetValue("ToArray", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(UnityEngine.GameObject[]), typeof(System.Collections.Generic.IEnumerable<UnityEngine.GameObject>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToArray_11);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Int64)};
            if (genericMethods.TryGetValue("Count", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Int32), typeof(System.Collections.Generic.IEnumerable<System.Int64>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Count_12);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Collections.Generic.Dictionary<System.Int64, ILRuntime.Runtime.Intepreter.ILTypeInstance>)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.Int64, ILRuntime.Runtime.Intepreter.ILTypeInstance>>), typeof(System.Collections.Generic.IEnumerable<System.Collections.Generic.Dictionary<System.Int64, ILRuntime.Runtime.Intepreter.ILTypeInstance>>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_13);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>), typeof(System.Collections.Generic.IEnumerable<System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_14);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Collections.Generic.IEnumerable<System.Single>)};
            method = type.GetMethod("Min", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Min_15);
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("ToArray", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance[]), typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToArray_16);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(UnityEngine.GameObject)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<UnityEngine.GameObject>), typeof(System.Collections.Generic.IEnumerable<UnityEngine.GameObject>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_17);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Int32)};
            if (genericMethods.TryGetValue("ToList", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<System.Int32>), typeof(System.Collections.Generic.IEnumerable<System.Int32>)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToList_18);

                        break;
                    }
                }
            }


        }


        static StackObject* ToArray_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.String> @source = (System.Collections.Generic.IEnumerable<System.String>)typeof(System.Collections.Generic.IEnumerable<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToArray<System.String>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* First_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>> @source = (System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>)typeof(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.First<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* First_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.Int32> @source = (System.Collections.Generic.IEnumerable<System.Int32>)typeof(System.Collections.Generic.IEnumerable<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.First<System.Int32>(@source);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* ToList_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToList_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<UnityEngine.Animation> @source = (System.Collections.Generic.IEnumerable<UnityEngine.Animation>)typeof(System.Collections.Generic.IEnumerable<UnityEngine.Animation>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<UnityEngine.Animation>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Contains_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @value = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<System.String> @source = (System.Collections.Generic.IEnumerable<System.String>)typeof(System.Collections.Generic.IEnumerable<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Contains<System.String>(@source, @value);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* ToList_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor> @source = (System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<Sproto.SprotoTypeBaseAdaptor.Adaptor>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToList_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.Int64> @source = (System.Collections.Generic.IEnumerable<System.Int64>)typeof(System.Collections.Generic.IEnumerable<System.Int64>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<System.Int64>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Count_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<global::ManorItem> @source = (System.Collections.Generic.IEnumerable<global::ManorItem>)typeof(System.Collections.Generic.IEnumerable<global::ManorItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Count<global::ManorItem>(@source);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Except_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<global::ManorItem> @second = (System.Collections.Generic.IEnumerable<global::ManorItem>)typeof(System.Collections.Generic.IEnumerable<global::ManorItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.IEnumerable<global::ManorItem> @first = (System.Collections.Generic.IEnumerable<global::ManorItem>)typeof(System.Collections.Generic.IEnumerable<global::ManorItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Except<global::ManorItem>(@first, @second);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Any_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<global::ManorItem> @source = (System.Collections.Generic.IEnumerable<global::ManorItem>)typeof(System.Collections.Generic.IEnumerable<global::ManorItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Any<global::ManorItem>(@source);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* ToArray_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<UnityEngine.GameObject> @source = (System.Collections.Generic.IEnumerable<UnityEngine.GameObject>)typeof(System.Collections.Generic.IEnumerable<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToArray<UnityEngine.GameObject>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Count_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.Int64> @source = (System.Collections.Generic.IEnumerable<System.Int64>)typeof(System.Collections.Generic.IEnumerable<System.Int64>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Count<System.Int64>(@source);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* ToList_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.Collections.Generic.Dictionary<System.Int64, ILRuntime.Runtime.Intepreter.ILTypeInstance>> @source = (System.Collections.Generic.IEnumerable<System.Collections.Generic.Dictionary<System.Int64, ILRuntime.Runtime.Intepreter.ILTypeInstance>>)typeof(System.Collections.Generic.IEnumerable<System.Collections.Generic.Dictionary<System.Int64, ILRuntime.Runtime.Intepreter.ILTypeInstance>>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<System.Collections.Generic.Dictionary<System.Int64, ILRuntime.Runtime.Intepreter.ILTypeInstance>>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToList_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>> @source = (System.Collections.Generic.IEnumerable<System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>)typeof(System.Collections.Generic.IEnumerable<System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Min_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.Single> @source = (System.Collections.Generic.IEnumerable<System.Single>)typeof(System.Collections.Generic.IEnumerable<System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.Min(@source);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* ToArray_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance> @source = (System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>)typeof(System.Collections.Generic.IEnumerable<ILRuntime.Runtime.Intepreter.ILTypeInstance>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToArray<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToList_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<UnityEngine.GameObject> @source = (System.Collections.Generic.IEnumerable<UnityEngine.GameObject>)typeof(System.Collections.Generic.IEnumerable<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<UnityEngine.GameObject>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToList_18(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<System.Int32> @source = (System.Collections.Generic.IEnumerable<System.Int32>)typeof(System.Collections.Generic.IEnumerable<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = System.Linq.Enumerable.ToList<System.Int32>(@source);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
