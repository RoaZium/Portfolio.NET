using CommonServiceLocator;
using Prism.Events;
using System;
using System.Reflection;
using Unity;
using Unity.Lifetime;

namespace PrismWPF.Common.Infrastructure
{
    public class PrismUtil
    {
        public static void RegisterInstance(Type instType, object obj)
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.RegisterInstance(instType, obj, new ContainerControlledLifetimeManager());
        }

        public static void ResgisterInstance(Type instType, string name, object obj)
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.RegisterInstance(instType, name, obj, new ContainerControlledLifetimeManager());
        }

        public static object Resolve(Type instType)
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            return container.Resolve(instType);
        }

        public static T Resolve<T>()
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            return (T)container.Resolve(typeof(T));
        }

        public static void Subscribe<T, U>(Action<U> eventhandle)
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            Type eventType = typeof(T);
            MethodInfo method = typeof(IEventAggregator).GetMethod("GetEvent");
            MethodInfo generic = method.MakeGenericMethod(eventType);
            dynamic subscribeEvent = generic.Invoke(container.Resolve<IEventAggregator>(), null);
            subscribeEvent.Subscribe(eventhandle, ThreadOption.UIThread, true);
        }

        public static void Subscribe<T, U>(Action<U> eventhandle, ThreadOption threadOpiton)
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            Type eventType = typeof(T);
            MethodInfo method = typeof(IEventAggregator).GetMethod("GetEvent");
            MethodInfo generic = method.MakeGenericMethod(eventType);
            dynamic subscribeEvent = generic.Invoke(container.Resolve<IEventAggregator>(), null);
            subscribeEvent.Subscribe(eventhandle, threadOpiton, true);
        }

        public static void Subscribe<T, U>(Action<U> eventhandle, ThreadOption threadOpiton, bool keepSubscriberReferenceAlive)
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            Type eventType = typeof(T);
            MethodInfo method = typeof(IEventAggregator).GetMethod("GetEvent");
            MethodInfo generic = method.MakeGenericMethod(eventType);
            dynamic subscribeEvent = generic.Invoke(container.Resolve<IEventAggregator>(), null);
            subscribeEvent.Subscribe(eventhandle, threadOpiton, keepSubscriberReferenceAlive);
        }

        public static void Publish<T, U>(U arg)
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            Type eventType = typeof(T);
            MethodInfo method = typeof(IEventAggregator).GetMethod("GetEvent");
            MethodInfo generic = method.MakeGenericMethod(eventType);
            dynamic subscribeEvent = generic.Invoke(container.Resolve<IEventAggregator>(), null);
            subscribeEvent.Publish(arg);
        }

        public static void Unsubscribe<T, U>(Action<U> eventhandle)
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            Type eventType = typeof(T);
            MethodInfo method = typeof(IEventAggregator).GetMethod("GetEvent");
            MethodInfo generic = method.MakeGenericMethod(eventType);
            dynamic subscribeEvent = generic.Invoke(container.Resolve<IEventAggregator>(), null);
            subscribeEvent.Unsubscribe(eventhandle);
        }
    }
}
