using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeSharp.MongoToLINQ
{
    /// <summary>
    /// Represents an object that can create
    /// <see cref="ITypeDescriptor"/>s.
    /// </summary>
    public interface ITypeDescriptorFactory
    {

        /// <summary>
        /// Creates a <see cref="ITypeDescriptor"/>
        /// for the given <param name="type"></param>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ITypeDescriptor Create(Type type);

    }

    /// <summary>
    /// Represents an object that can get 
    /// state properties of given object by its type
    /// </summary>
    public interface ITypeDescriptor
    {

        IStateProperty GetStateProperty(string name);

    }

    /// <summary>
    /// Represents any member of type that can be read.
    /// </summary>
    public interface IStateProperty
    {

        /// <summary>
        /// Creates an expression that reads the current property
        /// from the given <param name="instance"></param>.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        Expression CreateGetExpression(Expression instance);

    }

    public static class ConventionsStringExtensions
    {

        /// <summary>
        /// Returns the string as camelCased
        /// by changing the first letter to lower case.
        /// <example>
        /// InNode will turn to inNode
        /// IPhone will turn to iPhone
        /// </example>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

    }

    public class EmptyNamingConvention : INamingConvention
    {
        public static readonly INamingConvention Default = new EmptyNamingConvention();

        private EmptyNamingConvention()
        {
            
        }

        public string GetName(string name)
        {
            return name;
        }
    }

    public class CamelCaseNamingConvention : INamingConvention
    {
        public static readonly INamingConvention Default = new CamelCaseNamingConvention();

        private CamelCaseNamingConvention()
        {
            
        }

        public string GetName(string name)
        {
            return name.ToCamelCase();
        }
    }

    public class TrivialPropertyTypeDescriptor : ITypeDescriptor
    {
        private readonly IDictionary<string, IStateProperty> _properties;

        public TrivialPropertyTypeDescriptor(Type type, INamingConvention namingConvention)
        {
            _properties = type.GetProperties().ToDictionary(p => namingConvention.GetName(p.Name), PropertyStateProperty.Create);
        }

        public IStateProperty GetStateProperty(string name)
        {
            IStateProperty retVal;
            if (_properties.TryGetValue(name, out retVal))
            {
                return retVal;
            }
            return IdioticStateProperty.Default;
        }
    }

    public interface INamingConvention
    {

        string GetName(string name);

    }

    public class IdioticStateProperty : IStateProperty
    {
        public static readonly IStateProperty Default = new IdioticStateProperty();
        private static readonly ConstantExpression NullExpression = Expression.Constant(null);

        private IdioticStateProperty()
        {
            
        }

        public Expression CreateGetExpression(Expression instance)
        {
            return NullExpression;
        }
    }

    public class PropertyStateProperty : IStateProperty
    {
        private readonly PropertyInfo _property;


        public static IStateProperty Create(PropertyInfo property)
        {
            return new PropertyStateProperty(property);
        }

        public PropertyStateProperty(PropertyInfo property)
        {
            _property = property;
        }

        public Expression CreateGetExpression(Expression instance)
        {
            return Expression.Property(instance, _property);
        }
    }

    public class TypeDescriptorCacheDecorator : ITypeDescriptorFactory
    {
        private readonly ITypeDescriptorFactory _child;
        private readonly object _syncRoot = new object();
        private readonly IDictionary<Type, ITypeDescriptor> _cache = new Dictionary<Type, ITypeDescriptor>();

        public TypeDescriptorCacheDecorator(ITypeDescriptorFactory child)
        {
            _child = child;
        }

        public ITypeDescriptor Create(Type type)
        {
            ITypeDescriptor retVal;
            if (!_cache.TryGetValue(type, out retVal))
            {
                lock (_syncRoot)
                {
                    if (!_cache.TryGetValue(type, out retVal))
                    {
                        _cache[type] = retVal = _child.Create(type);
                    }
                }
                
            }

            return retVal;
        }
    }

    public class AnonymousTypeDescriptorFactory  : ITypeDescriptorFactory
    {
        private readonly Func<Type, ITypeDescriptor> _factoryFunc;

        public AnonymousTypeDescriptorFactory(Func<Type, ITypeDescriptor> factoryFunc)
        {
            _factoryFunc = factoryFunc;
        }

        public ITypeDescriptor Create(Type type)
        {
            return _factoryFunc(type);
        }
    }
}
