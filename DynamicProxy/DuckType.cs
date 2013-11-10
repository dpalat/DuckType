using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prueba.DynamicProxy
{
    public static class DuckType
    {
        public static T As<T>( object target ) where T : class
        {
            var interceptor = new DuckTypingInterceptor(target);
            return new ProxyGenerator(true).CreateInterfaceProxyWithoutTarget<T>(interceptor);
        }

        private class DuckTypingInterceptor : IInterceptor
        {
            private readonly object target;

            public DuckTypingInterceptor(object target)
            {
                this.target = target;
            }

            public void Intercept(IInvocation invocation)
            {
                var methods = target.GetType()
                                    .GetMethods(BindingFlags.Instance | BindingFlags.Public);

                // This should (probably) be cached
                var method = methods.FirstOrDefault(x => IsCompatible(x, invocation.Method));

                if (invocation.GenericArguments != null &&
                    invocation.GenericArguments.Length > 0)
                {
                    if (!method.IsGenericMethod)
                        throw MissingMethodException(invocation);

                    method = method.MakeGenericMethod(invocation.GenericArguments);
                }

                if (method == null)
                    throw MissingMethodException(invocation);

                invocation.ReturnValue = method.Invoke(target, invocation.Arguments);
            }

            private MissingMethodException MissingMethodException(IInvocation invocation)
            {
                return new MissingMethodException(
                             string.Format("Cannot found compatible method {0} on type {1}",
                               invocation.Method.Name, target.GetType().Name));
            }

            private bool IsCompatible(MethodInfo m1, MethodInfo m2)
            {
                // FIXME: handle ref/out parameters and generic arguments restrictions

                if (m1.Name != m2.Name || m1.ReturnType != m2.ReturnType)
                    return false;

                if (!m1.IsGenericMethod)
                {
                    var parameterTypes1 = m1.GetParameters().Select(p => p.ParameterType);
                    var parameterTypes2 = m2.GetParameters().Select(p => p.ParameterType);

                    return parameterTypes1.SequenceEqual(parameterTypes2);
                }

                return true;
            }
        }
    }
}
