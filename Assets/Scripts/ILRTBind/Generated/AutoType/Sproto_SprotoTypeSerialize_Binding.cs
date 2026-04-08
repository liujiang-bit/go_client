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
    unsafe class Sproto_SprotoTypeSerialize_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Sproto.SprotoTypeSerialize);
            args = new Type[]{typeof(Sproto.SprotoStream)};
            method = type.GetMethod("open", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, open_0);
            args = new Type[]{typeof(System.Int64), typeof(System.Int32)};
            method = type.GetMethod("write_integer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, write_integer_1);
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
            if (genericMethods.TryGetValue("write_obj", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(void), typeof(System.Collections.Generic.Dictionary<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>), typeof(System.Int32)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, write_obj_2);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Collections.Generic.List<System.Int64>), typeof(System.Int32)};
            method = type.GetMethod("write_integer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, write_integer_3);
            args = new Type[]{typeof(System.Boolean), typeof(System.Int32)};
            method = type.GetMethod("write_boolean", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, write_boolean_4);
            args = new Type[]{};
            method = type.GetMethod("close", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, close_5);
            args = new Type[]{typeof(Sproto.SprotoTypeBase), typeof(System.Int32)};
            method = type.GetMethod("write_obj", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, write_obj_6);
            args = new Type[]{typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            if (genericMethods.TryGetValue("write_obj", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(void), typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>), typeof(System.Int32)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, write_obj_7);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.String), typeof(System.Int32)};
            method = type.GetMethod("write_string", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, write_string_8);
            args = new Type[]{typeof(System.Collections.Generic.List<System.String>), typeof(System.Int32)};
            method = type.GetMethod("write_string", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, write_string_9);
            args = new Type[]{typeof(System.Byte[]), typeof(System.Int32)};
            method = type.GetMethod("write_binary", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, write_binary_10);


        }


        static StackObject* open_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoStream @stream = (Sproto.SprotoStream)typeof(Sproto.SprotoStream).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.open(@stream);

            return __ret;
        }

        static StackObject* write_integer_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int64 @integer = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_integer(@integer, @tag);

            return __ret;
        }

        static StackObject* write_obj_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.Dictionary<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor> @map = (System.Collections.Generic.Dictionary<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.Dictionary<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_obj<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>(@map, @tag);

            return __ret;
        }

        static StackObject* write_integer_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<System.Int64> @integer_list = (System.Collections.Generic.List<System.Int64>)typeof(System.Collections.Generic.List<System.Int64>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_integer(@integer_list, @tag);

            return __ret;
        }

        static StackObject* write_boolean_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @b = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_boolean(@b, @tag);

            return __ret;
        }

        static StackObject* close_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.close();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* write_obj_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Sproto.SprotoTypeBase @obj = (Sproto.SprotoTypeBase)typeof(Sproto.SprotoTypeBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_obj(@obj, @tag);

            return __ret;
        }

        static StackObject* write_obj_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> @obj_list = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_obj<Sproto.SprotoTypeBaseAdaptor.Adaptor>(@obj_list, @tag);

            return __ret;
        }

        static StackObject* write_string_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_string(@str, @tag);

            return __ret;
        }

        static StackObject* write_string_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<System.String> @str_list = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_string(@str_list, @tag);

            return __ret;
        }

        static StackObject* write_binary_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @tag = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Byte[] @bytes = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Sproto.SprotoTypeSerialize instance_of_this_method = (Sproto.SprotoTypeSerialize)typeof(Sproto.SprotoTypeSerialize).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.write_binary(@bytes, @tag);

            return __ret;
        }



    }
}
