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
    unsafe class System_Collections_Generic_List_1_Sproto_SprotoTypeBaseAdaptor_Binding_Adaptor_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("get_Item", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Item_0);
            args = new Type[]{};
            method = type.GetMethod("get_Count", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Count_1);
            args = new Type[]{};
            method = type.GetMethod("GetEnumerator", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetEnumerator_2);
            args = new Type[]{typeof(System.Action<Sproto.SprotoTypeBaseAdaptor.Adaptor>)};
            method = type.GetMethod("ForEach", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ForEach_3);
            args = new Type[]{typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            method = type.GetMethod("Add", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Add_4);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("RemoveAt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveAt_5);
            args = new Type[]{typeof(System.Int32), typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            method = type.GetMethod("Insert", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Insert_6);
            args = new Type[]{};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_7);
            args = new Type[]{};
            method = type.GetMethod("Reverse", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Reverse_8);
            args = new Type[]{typeof(System.Comparison<Sproto.SprotoTypeBaseAdaptor.Adaptor>)};
            method = type.GetMethod("Sort", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Sort_9);
            args = new Type[]{typeof(System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>)};
            method = type.GetMethod("AddRange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddRange_10);
            args = new Type[]{typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            method = type.GetMethod("Contains", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Contains_11);
            args = new Type[]{typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            method = type.GetMethod("IndexOf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IndexOf_12);
            args = new Type[]{typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            method = type.GetMethod("Remove", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Remove_13);
            args = new Type[]{typeof(System.Int32), typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor)};
            method = type.GetMethod("set_Item", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_Item_14);
            args = new Type[]{typeof(System.Predicate<Sproto.SprotoTypeBaseAdaptor.Adaptor>)};
            method = type.GetMethod("FindIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FindIndex_15);

            app.RegisterCLRCreateArrayInstance(type, s => new System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>[s]);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);
            args = new Type[]{typeof(System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_1);

        }


        static StackObject* get_Item_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method[index];

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_Count_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Count;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetEnumerator_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetEnumerator();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ForEach_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<Sproto.SprotoTypeBaseAdaptor.Adaptor> @action = (System.Action<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Action<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ForEach(@action);

            return __ret;
        }

        static StackObject* Add_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeBaseAdaptor.Adaptor @item = (Sproto.SprotoTypeBaseAdaptor.Adaptor)typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Add(@item);

            return __ret;
        }

        static StackObject* RemoveAt_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveAt(@index);

            return __ret;
        }

        static StackObject* Insert_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeBaseAdaptor.Adaptor @item = (Sproto.SprotoTypeBaseAdaptor.Adaptor)typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Insert(@index, @item);

            return __ret;
        }

        static StackObject* Clear_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Clear();

            return __ret;
        }

        static StackObject* Reverse_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Reverse();

            return __ret;
        }

        static StackObject* Sort_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Comparison<Sproto.SprotoTypeBaseAdaptor.Adaptor> @comparison = (System.Comparison<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Comparison<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Sort(@comparison);

            return __ret;
        }

        static StackObject* AddRange_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor> @collection = (System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddRange(@collection);

            return __ret;
        }

        static StackObject* Contains_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeBaseAdaptor.Adaptor @item = (Sproto.SprotoTypeBaseAdaptor.Adaptor)typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Contains(@item);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IndexOf_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeBaseAdaptor.Adaptor @item = (Sproto.SprotoTypeBaseAdaptor.Adaptor)typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IndexOf(@item);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Remove_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeBaseAdaptor.Adaptor @item = (Sproto.SprotoTypeBaseAdaptor.Adaptor)typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Remove(@item);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* set_Item_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Sproto.SprotoTypeBaseAdaptor.Adaptor @value = (Sproto.SprotoTypeBaseAdaptor.Adaptor)typeof(Sproto.SprotoTypeBaseAdaptor.Adaptor).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method[index] = value;

            return __ret;
        }

        static StackObject* FindIndex_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Predicate<Sproto.SprotoTypeBaseAdaptor.Adaptor> @match = (System.Predicate<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Predicate<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor> instance_of_this_method = (System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.FindIndex(@match);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Ctor_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor> @collection = (System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>)typeof(System.Collections.Generic.IEnumerable<Sproto.SprotoTypeBaseAdaptor.Adaptor>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new System.Collections.Generic.List<Sproto.SprotoTypeBaseAdaptor.Adaptor>(@collection);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
