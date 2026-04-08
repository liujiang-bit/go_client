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
    unsafe class Sproto_SprotoTypeDeserialize_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Sproto.SprotoTypeDeserialize);
            args = new Type[]{};
            method = type.GetMethod("read_integer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, read_integer_0);
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
            args = new Type[]{typeof(System.Int64), typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            if (genericMethods.TryGetValue("read_map", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.Dictionary<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>), typeof(Sproto.SprotoTypeDeserialize.gen_key_func<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>), typeof(System.String)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, read_map_1);

                        break;
                    }
                }
            }
            args = new Type[]{};
            method = type.GetMethod("read_integer_list", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, read_integer_list_2);
            args = new Type[]{};
            method = type.GetMethod("read_boolean", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, read_boolean_3);
            args = new Type[]{};
            method = type.GetMethod("read_unknow_data", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, read_unknow_data_4);
            args = new Type[]{};
            method = type.GetMethod("read_tag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, read_tag_5);
            args = new Type[]{typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            if (genericMethods.TryGetValue("read_obj", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor), typeof(System.String)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, read_obj_6);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            if (genericMethods.TryGetValue("read_obj_list", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>), typeof(System.String)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, read_obj_list_7);

                        break;
                    }
                }
            }
            args = new Type[]{};
            method = type.GetMethod("read_string", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, read_string_8);
            args = new Type[]{};
            method = type.GetMethod("read_string_list", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, read_string_list_9);
            args = new Type[]{};
            method = type.GetMethod("read_binary", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, read_binary_10);


        }


        static StackObject* read_integer_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_integer();

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* read_map_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @fullname = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Sproto.SprotoTypeDeserialize.gen_key_func<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor> @func = (Sproto.SprotoTypeDeserialize.gen_key_func<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(Sproto.SprotoTypeDeserialize.gen_key_func<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_map<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>(@func, @fullname);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* read_integer_list_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_integer_list();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* read_boolean_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_boolean();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* read_unknow_data_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.read_unknow_data();

            return __ret;
        }

        static StackObject* read_tag_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_tag();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* read_obj_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @fullname = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_obj<Sproto.SprotoTypeBaseAdaptor.Adaptor>(@fullname);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* read_obj_list_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @fullname = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_obj_list<Sproto.SprotoTypeBaseAdaptor.Adaptor>(@fullname);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* read_string_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_string();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* read_string_list_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_string_list();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* read_binary_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeDeserialize instance_of_this_method = (Sproto.SprotoTypeDeserialize)typeof(Sproto.SprotoTypeDeserialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.read_binary();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
