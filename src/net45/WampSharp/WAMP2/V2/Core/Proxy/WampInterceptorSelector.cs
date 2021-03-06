﻿using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Proxy;

namespace WampSharp.V2.Core.Proxy
{
    /// <summary>
    /// An <see cref="IInterceptorSelector"/> that chooses between
    /// <see cref="WampOutgoingInterceptor{TMessage}"/> and 
    /// <see cref="WampRawOutgoingInterceptor{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampInterceptorSelector<TMessage> : IInterceptorSelector
    {
        private readonly WampOutgoingInterceptor<TMessage> mInterceptor;
        private readonly WampRawOutgoingInterceptor<TMessage> mRawInterceptor;

        /// <summary>
        /// Creates a new instance of <see cref="WampInterceptorSelector{TMessage}"/>.
        /// </summary>
        /// <param name="interceptor">The given <see cref="WampOutgoingInterceptor{TMessage}"/> used
        /// for WAMP method calls</param>
        public WampInterceptorSelector(WampOutgoingInterceptor<TMessage> interceptor, WampRawOutgoingInterceptor<TMessage> rawInterceptor)
        {
            mInterceptor = interceptor;
            mRawInterceptor = rawInterceptor;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (method.IsDefined(typeof(WampRawHandlerAttribute), true))
            {
                return new IInterceptor[] { mRawInterceptor };
            }
            if (method.IsDefined(typeof(WampHandlerAttribute), true))
            {
                return new IInterceptor[] {mInterceptor};
            }

            return null;
        }
    }
}